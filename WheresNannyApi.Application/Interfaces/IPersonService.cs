using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WheresNannyApi.Domain.Entities;

namespace WheresNannyApi.Application.Interfaces
{
    public interface IPersonService
    {
        Task<List<Nanny>> GetAllNannys();
    }
}
