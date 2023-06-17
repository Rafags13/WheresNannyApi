﻿using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using WheresNannyApi.Application.Interfaces;
using WheresNannyApi.Domain.Entities;
using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository _repository;
        private readonly IHttpClientFactory _httpClientFactory;
        public AddressService(IUnitOfWork unitOfWork, IRepository repository, IHttpClientFactory httpClientFactory) 
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _httpClientFactory = httpClientFactory;
        }
        public async void CreateAddress(CreateAddressDto address)
        {
            var addressExists = await AddressExists(address.Cep);

            if (!addressExists)
            {
                var httpRequestNannyUserCep = CreateRequestMessageForSpecificCep(address.Cep);
                var httpClient = _httpClientFactory.CreateClient();

                var requestNannyUser = await httpClient.SendAsync(httpRequestNannyUserCep);

                if (requestNannyUser.IsSuccessStatusCode)
                {
                    var contentStreamNanny = await requestNannyUser.Content.ReadAsStreamAsync();

                    var deserializeNanny = await JsonSerializer.DeserializeAsync<CepRequestDto>(contentStreamNanny) ?? new CepRequestDto();
                    var newAddress = new Address(
                        address.Cep,
                        address.Number ?? "",
                        address.Complement ?? "",
                        float.Parse(deserializeNanny.latitude),
                        float.Parse(deserializeNanny.longitude)
                    );

                    await _repository.AddAsync(newAddress);
                    await _repository.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Não foi possível encontrar o cep informado");
                }
            }

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

        private async Task<bool> AddressExists(string cep)
        {
            var address = await _repository.GetAsync<Address>(x => x.Cep == cep);

            return address != null;
        }
    }
}
