using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using WheresNannyApi.Application.Interfaces;
using WheresNannyApi.Domain.Entities;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.Application.Services
{
    public class UserService: IUserService
    {
        private readonly IRepository _repository;
        public UserService(IRepository repository) 
        {
            _repository = repository;
        }
        #region Register User

        public async Task<string> RegisterUser(UserRegisterDto userRegisterDto)
        {
            var userExists = await UserExists(userRegisterDto.Username) || await PersonExists(userRegisterDto.Email);

            if (userExists) return "O usuário informado já está cadastrado no sistema.";

            var user = new User(userRegisterDto.Username, userRegisterDto.Password);
            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            var addressExists = await AddressExists(userRegisterDto.Cep);
            if (!addressExists)
            {
                var address = new Address(
                   userRegisterDto.Cep,
                   userRegisterDto.HouseNumber is null ? "" : userRegisterDto.HouseNumber,
                   userRegisterDto.Complement is null ? "" : userRegisterDto.Complement
               );

                await _repository.AddAsync(address);
                await _repository.SaveChangesAsync();
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
            AddDocumentsFromNannyPerson(registerNannyDocuments);

            AddDefaultCommentToFirstRegisterNanny(currentPerson.Id);

            return "";
        }

        public async void AddDocumentsFromNannyPerson(NannyRegisterDocumentDto nannyRegisterDocumentDto)
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

        public async void AddDefaultCommentToFirstRegisterNanny(int personId)
        {
            var defaultFirstComment = new CommentRank();
            defaultFirstComment.RankStarsCounting = 5.0f;
            defaultFirstComment.NannyWhoRecieveTheCommentId = (await _repository.GetAsync<Nanny>(x => x.PersonId == personId)).Id;

            await _repository.AddAsync(defaultFirstComment);
            await _repository.SaveChangesAsync();
        }
        #endregion
    }
}
