using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using WheresNannyApi.Application.Interfaces;
using WheresNannyApi.Application.Util;
using WheresNannyApi.Domain.Entities;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.Application.Services
{
    public class UserService: IUserService
    {
        private readonly IRepository _repository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IRepository repository, IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork) 
        {
            _repository = repository;
            _httpClientFactory = httpClientFactory;
            _unitOfWork = unitOfWork;
        }
        #region Register User

        public async Task<string> RegisterUser(UserRegisterDto userRegisterDto)
        {
            var userExists = await UserExists(userRegisterDto.Username) || await PersonExists(userRegisterDto.Email);

            if (userExists) return "O usuário informado já está cadastrado no sistema.";

            var passwordEncrypted = Functions.Sha1Encrypt(userRegisterDto.Password);

            var user = new User(userRegisterDto.Username, passwordEncrypted);
            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            var addressExists = await AddressExists(userRegisterDto.Cep);
            if (!addressExists)
            {
                var httpRequestNannyUserCep = CreateRequestMessageForSpecificCep(userRegisterDto.Cep);
                var httpClient = _httpClientFactory.CreateClient();

                var requestNannyUser = await httpClient.SendAsync(httpRequestNannyUserCep);

                if (requestNannyUser.IsSuccessStatusCode)
                {
                    var contentStreamNanny = await requestNannyUser.Content.ReadAsStreamAsync();

                    var deserializeNanny = await JsonSerializer.DeserializeAsync<CepRequestDto>(contentStreamNanny) ?? new CepRequestDto();
                    var address = new Address(
                        userRegisterDto.Cep,
                        userRegisterDto.HouseNumber is null ? "" : userRegisterDto.HouseNumber,
                        userRegisterDto.Complement is null ? "" : userRegisterDto.Complement,
                        float.Parse(deserializeNanny.latitude),
                        float.Parse(deserializeNanny.longitude)
                    );
                    await _repository.AddAsync(address);
                    await _repository.SaveChangesAsync();

                    var nannyCepRequest = deserializeNanny;
                }
                else
                {
                    throw new Exception("Não foi possível encontrar o cep informado");
                }
            }

            var currentUser = await _repository.GetAsync<User>(x => x.Username == userRegisterDto.Username);
            var currentAddress = await _repository.GetAsync<Address>(x => x.Cep == userRegisterDto.Cep);
            var person = 
                new Person(
                        userRegisterDto.Fullname,
                        userRegisterDto.Email,
                        userRegisterDto.Cellphone,
                        userRegisterDto.BirthdayDate,
                        userRegisterDto.Cpf,
                        userRegisterDto.ImageUri,
                        currentUser.Id,
                        currentAddress.Id
                    );

            await _repository.AddAsync(person);
            await _repository.SaveChangesAsync();

            return "";
        }

        public async Task<bool> UserExists(string username)
        {
            var user = await _repository.GetAsync<User>(x => x.Username == username);

            return user != null;
        }

        public async Task<bool> PersonExists(string email)
        {
            var person = await _repository.GetAsync<Person>(x => x.Email == email);

            return person != null;
        }

        public async Task<bool> AddressExists(string cep)
        {
            var address = await _repository.GetAsync<Address>(x => x.Cep == cep);

            return address != null;
        }

        private static HttpRequestMessage CreateRequestMessageForSpecificCep(string cep)
        {
            var httpRequestCurrentCep = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://www.cepaberto.com/api/v3/cep?cep={cep}")
            {
                Headers =
                {
                   {HeaderNames.Authorization, "Token token=e8da46eb3abc34cd1f0ad500174abcd1" }
                }
            };

            return httpRequestCurrentCep;
        }
        #endregion

        #region Register Nanny
        public async Task<string> RegisterNanny(NannyRegisterDto nannyRegisterDto)
        {
            var errorMessageFromUserRegister = await RegisterUser(nannyRegisterDto.UserDataToRegister);
            if (errorMessageFromUserRegister != "") { return errorMessageFromUserRegister; }

            var currentPerson = await _repository.GetAsync<Person>(x => x.Email == nannyRegisterDto.UserDataToRegister.Email);

            if( currentPerson is null ) { return "Algo ocorreu de errado durante o seu registro. Por favor, contate um administrador do sistema."; }

            var newNanny = new Nanny(nannyRegisterDto.ServicePrice, currentPerson.Id);

            await _repository.AddAsync(newNanny);
            await _repository.SaveChangesAsync();

            var registerNannyDocuments = new NannyRegisterDocumentDto(currentPerson.Id, nannyRegisterDto.Base64CriminalRecord, nannyRegisterDto.Base64ProofOfAddress);
            await AddDocumentsFromNannyPerson(registerNannyDocuments);

            await AddDefaultCommentToFirstRegisterNanny(currentPerson.Id);

            return "";
        }

        public async Task AddDocumentsFromNannyPerson(NannyRegisterDocumentDto nannyRegisterDocumentDto)
        {
            var criminalRecordDocumentType = await _repository.GetAsync<DocumentType>(x => x.Name == "Antecedente Criminal");
            var proofOfAddressDocumentType = await _repository.GetAsync<DocumentType>(x => x.Name == "Comprovante de residencia");

            var documents = new List<Document>()
            {
                new Document(nannyRegisterDocumentDto.PersonId, criminalRecordDocumentType.Id, nannyRegisterDocumentDto.Base64CriminalRecord),
                new Document(nannyRegisterDocumentDto.PersonId, proofOfAddressDocumentType.Id, nannyRegisterDocumentDto.Base64ProofOfAddress),
            };

            foreach (var document in documents)
            {
                await _repository.AddAsync(document);
            }
            await _repository.SaveChangesAsync();
        }

        public async Task AddDefaultCommentToFirstRegisterNanny(int personId)
        {
            var defaultFirstComment = new CommentRank();
            defaultFirstComment.RankStarsCounting = 5.0f;
            defaultFirstComment.NannyWhoRecieveTheCommentId = (await _repository.GetAsync<Nanny>(x => x.PersonId == personId)).Id;

            await _repository.AddAsync(defaultFirstComment);
            await _repository.SaveChangesAsync();
        }
        #endregion

        #region UpdatePassword
        public async Task<string> UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            var personReference = _unitOfWork.GetRepository<User>().GetFirstOrDefault(predicate: x => x.Id == updatePasswordDto.UserId);

            if (personReference == null) return "Pessoa informada não foi encontrada no sistema. Tente Novamente";
            var currentPasswordEncrypted = Functions.Sha1Encrypt(updatePasswordDto.OldPassword);

            var passwords = new { OldPassword = personReference.Password, Password = currentPasswordEncrypted };
            if (PasswordsDontMatch(passwords)) return "A senha informada não confere com a antiga. Por favor, tente novamente.";

            var newPasswordEncrypted = Functions.Sha1Encrypt(updatePasswordDto.NewPassword);
            personReference.Password = newPasswordEncrypted;
            _repository.Update(personReference);
            await _repository.SaveChangesAsync();
            
            return "";
        }

        public static bool PasswordsDontMatch(dynamic password)
        {
            return password.Password != password.OldPassword;
        }

        #endregion
    }
}
