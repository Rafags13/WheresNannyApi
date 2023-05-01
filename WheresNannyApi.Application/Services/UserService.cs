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

        public async Task<string> RegisterUser(UserRegisterDto userRegisterDto)
        {
            var userExists = await UserExists(userRegisterDto.Username) || await PersonExists(userRegisterDto.Email);

            if (userExists) return "O usuário informado já está cadastrado no sistema. ";

            var user = new User(userRegisterDto.Username, userRegisterDto.Password);
            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            var currentUser = await _repository.GetAsync<User>(x => x.Username == userRegisterDto.Username);

            var person = 
                new Person(
                        userRegisterDto.Fullname,
                        userRegisterDto.Email,
                        userRegisterDto.Cellphone,
                        userRegisterDto.BirthdayDate,
                        userRegisterDto.Cpf,
                        userRegisterDto.IsNanny,
                        currentUser.Id
                    );

            await _repository.AddAsync(person);
            await _repository.SaveChangesAsync();

            var currentPerson = await _repository.GetAsync<Person>(x => x.UserId == currentUser.Id);

            var address = 
                new Address(
                    userRegisterDto.Cep,
                    userRegisterDto.HouseNumber is null ? "" : userRegisterDto.HouseNumber,
                    userRegisterDto.Complement is null ? "" : userRegisterDto.Complement,
                    currentPerson.Id
                );

            await _repository.AddAsync(address);
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
    }
}
