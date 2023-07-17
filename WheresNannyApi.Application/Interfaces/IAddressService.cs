using WheresNannyApi.Domain.Entities.Dto;

namespace WheresNannyApi.Application.Interfaces
{
    public interface IAddressService
    {
        public Task<bool> CreateAddress(CreateAddressDto address);
    }
}
