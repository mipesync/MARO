using MARO.Application.Common.Exceptions;
using MARO.Application.Interfaces;
using MARO.Domain;
using Microsoft.EntityFrameworkCore;

namespace MARO.Application.Repository.RouteRepo
{
    public class RouteRepository : IRouteRepository
    {
        private readonly IMARODbContext _dbContext;

        public RouteRepository(IMARODbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<double[]>> BuildRoute(Guid userId)
        {
            var items = await _dbContext.UserItems.Where(u => u.UserId == userId.ToString()).ToListAsync();

            if (items.Count() <= 0) throw new NotFoundException(nameof(UserItem), userId);

            //TODO: Вшить ИИ

            var route = new List<double[]>
            {
                new double[] { 37.637236, 55.826507 },
                new double[] { 37.637059, 55.826616 },
                new double[] { 37.636401, 55.826273 },
                new double[] { 37.635477, 55.826835 },
                new double[] { 37.635427, 55.82681 },
                new double[] { 37.635477, 55.826835 },
                new double[] { 37.634543, 55.827395 },
                new double[] { 37.635195, 55.827742 },
                new double[] { 37.637061, 55.826617 },
                new double[] { 37.637713, 55.82696 },
                new double[] { 37.633209, 55.829697 },
                new double[] { 37.633303, 55.830244 },
                new double[] { 37.632702, 55.830609 },
                new double[] { 37.631305, 55.830565 },
                new double[] { 37.630495, 55.830161 },
                new double[] { 37.630334, 55.829382 },
                new double[] { 37.630949, 55.829018 },
                new double[] { 37.632326, 55.82906 },
                new double[] { 37.63272, 55.829263 },
                new double[] { 37.631815, 55.8298 }
            };

            return route;
        }
    }
}
