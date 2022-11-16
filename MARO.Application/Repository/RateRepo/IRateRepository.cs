using MARO.Application.Aggregate.Models.DTOs;

namespace MARO.Application.Repository.RateRepo
{
    public interface IRateRepository
    {
        Task AddRate(AddRateDto model);
    }
}
