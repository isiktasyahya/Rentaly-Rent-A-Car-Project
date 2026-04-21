using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface ICarService : IGenericService<Car>
    {
        Task<List<Car>> TGetAllCarsWithCategoryAsync();
        Task<int> TGetCarCountAync();
        Task<List<Car>> TGetLast10CarsAsync();
        Task<List<Car>> TGetAvailableCarsAsync(
    int categoryId,
    int pickupBranchId,
    DateTime pickupDate,
    DateTime returnDate);
        // ICarService.cs
        Task<List<Car>> TGetCarsForListAsync();

        Task<List<Car>> TGetFilteredCarsAsync(
int? branchId,
int? categoryId,
decimal? minPrice,
decimal? maxPrice,
DateTime? pickupDate,
DateTime? returnDate,
string? sort);

        Task<Car> TGetCarDetailAsync(int id);

        Task<List<Car>> TSearchAvailableCarsAsync(
           int? categoryId,
           int? pickupBranchId,
           DateTime? pickupDate,
           DateTime? returnDate);
    }

}
