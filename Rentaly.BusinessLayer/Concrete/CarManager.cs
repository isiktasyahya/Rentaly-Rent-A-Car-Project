using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.Concrete
{
    public class CarManager : ICarService
    {
        private readonly ICarDal _carDal;

        public CarManager(ICarDal carDal)
        {
            _carDal = carDal;
        }

        public async Task TDeleteAsync(int id)
        {
            await _carDal.DeleteAsync(id);
        }

        public async Task<List<Car>> TGetAllCarsWithCategoryAsync()
        {
            return await _carDal.GetAllCarsWithCategoryAsync();
        }

        public async Task<List<Car>> TGetAvailableCarsAsync(int categoryId, int pickupBranchId, DateTime pickupDate, DateTime returnDate)
        {
            return await _carDal.GetAvailableCarsAsync(
        categoryId,
        pickupBranchId,
        pickupDate,
        returnDate);
        }

        public async Task<Car> TGetByIdAsync(int id)
        {
            return await _carDal.GetByIdAsync(id);
        }

        public async Task<int> TGetCarCountAync()
        {
            return await _carDal.GetCarCountAync();
        }

        public async Task<Car> TGetCarDetailAsync(int id)
        {
            return await _carDal.GetCarDetailAsync(id);
        }

        public async Task<List<Car>> TGetCarsForListAsync()
        {
            return await _carDal.GetCarsForListAsync();
        }

       

        public async Task<List<Car>> TGetFilteredCarsAsync(int? branchId, int? categoryId, decimal? minPrice, decimal? maxPrice, DateTime? pickupDate, DateTime? returnDate, string? sort)
        {
            return await _carDal.GetFilteredCarsAsync(
        branchId,
        categoryId,
        minPrice,
        maxPrice,
        pickupDate,
        returnDate,
        sort);
        }

        public async Task<List<Car>> TGetLast10CarsAsync()
        {
            return await _carDal.GetLast10CarsAsync();
        }

        public async Task<List<Car>> TGetListAsync()
        {
            return await _carDal.GetListAsync();
        }

        public async Task TInsertAsync(Car entity)
        {
            await _carDal.InsertAsync(entity);
        }

        public async Task<List<Car>> TSearchAvailableCarsAsync(int? categoryId, int? pickupBranchId, DateTime? pickupDate, DateTime? returnDate)
        {
            return await _carDal.SearchAvailableCarsAsync(
               categoryId,
               pickupBranchId,
               pickupDate,
               returnDate);
        }

        public async Task TUpdateAsync(Car entity)
        {
            await _carDal.UpdateAsync(entity);
        }
    }
}
