using AutoMapper;
using MARO.Application.Aggregate.Models.DTOs;
using MARO.Application.Common.Exceptions;
using MARO.Application.Interfaces;
using MARO.Domain;
using Microsoft.EntityFrameworkCore;

namespace MARO.Application.Repository.RateRepo
{
    public class RateRepository : IRateRepository
    {
        private readonly IMARODbContext _dbContext;
        private readonly IMapper _mapper;

        public RateRepository(IMARODbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task AddRate(AddRateDto model)
        {
            var rate = _mapper.Map<PlaceRating>(model);

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == model.UserId);

            if (user == null) throw new NotFoundException(nameof(User), model.UserId);

            if (model.Rate == 0) throw new ArgumentException("Оценка не может быть ниже 1 и выше 5");

            await _dbContext.PlaceRatings.AddAsync(rate);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }
    }
}
