using MARO.Domain;

namespace MARO.Application.Repository.RouteRepo
{
    public interface IRouteRepository
    {
        Task<List<double[]>> BuildRoute(Guid userId);
    }
}
