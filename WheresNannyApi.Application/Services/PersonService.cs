using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using WheresNannyApi.Application.Interfaces;
using WheresNannyApi.Domain.Entities;

namespace WheresNannyApi.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IRepository _repository;
        public PersonService(IRepository repository) 
        { 
            _repository = repository;
        }
        public async Task<List<Nanny>> GetAllNannys()
        {
            var nannys = await _repository.GetListAsync<Nanny>();

            return nannys;
        }
    }
}
