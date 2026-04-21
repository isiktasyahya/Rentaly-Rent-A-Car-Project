using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface ICarDal : IGenericDal<Car>
    {
        Task<List<Car>> GetAllCarsWithCategoryAsync();

        Task<int> GetCarCountAync();

        Task<List<Car>> GetLast10CarsAsync();

        Task<List<Car>> GetAvailableCarsAsync(
    int categoryId,
    int pickupBranchId,
    DateTime pickupDate,
    DateTime returnDate);

        Task<List<Car>> GetCarsForListAsync();

        Task<List<Car>> GetFilteredCarsAsync(
int? branchId,
int? categoryId,
decimal? minPrice,
decimal? maxPrice,
DateTime? pickupDate,
DateTime? returnDate,
string? sort);

        Task<Car> GetCarDetailAsync(int id);

        Task<List<Car>> SearchAvailableCarsAsync(
            int? categoryId,
            int? pickupBranchId,
            DateTime? pickupDate,
            DateTime? returnDate);


    }

}
