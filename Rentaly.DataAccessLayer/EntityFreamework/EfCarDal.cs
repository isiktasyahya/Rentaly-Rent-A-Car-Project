using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DataAccessLayer.EntityFreamework
{
    public class EfCarDal : GenericRepocitory<Car>, ICarDal
    {
        public EfCarDal(RentalyContext context) : base(context)
        {
        }

        public async Task<List<Car>> GetAllCarsWithCategoryAsync()
        {
            var context = new RentalyContext();
            var values = await context.Cars.Include(x => x.Category).ToListAsync();
            return values;
        }

        public async Task<List<Car>> GetAvailableCarsAsync(int categoryId, int pickupBranchId, DateTime pickupDate, DateTime returnDate)
        {
            using var context = new RentalyContext();

            var values = await context.Cars
                .Where(x => x.CategoryId == categoryId)
                .Where(x => x.BranchId == pickupBranchId)
                .Where(car => !context.Rentals.Any(r =>
                    r.CarId == car.CarId &&
                    r.Status != "İptal" &&
                    pickupDate < r.ReturnDate &&
                    returnDate > r.PickupDate))
                .Include(x => x.Brand)
                .Include(x => x.CarModel)
                .Include(x => x.Branch)
                .ToListAsync();

            return values;
        }

        public async Task<int> GetCarCountAync()
        {
            var context = new RentalyContext();
            return await context.Cars.CountAsync();
        }

        public async Task<Car> GetCarDetailAsync(int id)
        {
            using var context = new RentalyContext();
            return await context.Cars
                .Include(x => x.Brand)
                .Include(x => x.CarModel)
                .Include(x => x.Category)
                .Include(x => x.Branch)
                .FirstOrDefaultAsync(x => x.CarId == id);
        }

        public async Task<List<Car>> GetCarsForListAsync()
        {
            using var context = new RentalyContext();

            var values = await context.Cars
                .Include(x => x.Brand)
                .Include(x => x.CarModel)
                .Include(x => x.Category)
                .Include(x => x.Branch)
                .Where(x => x.IsActive == true)
                .ToListAsync();

            return values;
        }



        public async Task<List<Car>> GetFilteredCarsAsync(int? branchId, int? categoryId, decimal? minPrice, decimal? maxPrice, DateTime? pickupDate, DateTime? returnDate, string? sort)
        {
            using var context = new RentalyContext();

            var query = context.Cars
                .Include(x => x.Brand)
                .Include(x => x.CarModel)
                .Include(x => x.Category)
                .Include(x => x.Branch)
                .Where(x => x.IsActive == true)
                .AsQueryable();

            if (branchId.HasValue)
                query = query.Where(x => x.BranchId == branchId);

            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId);

            if (minPrice.HasValue)
                query = query.Where(x => x.DailyPrice >= minPrice);

            if (maxPrice.HasValue)
                query = query.Where(x => x.DailyPrice <= maxPrice);

            if (pickupDate.HasValue && returnDate.HasValue)
            {
                var pickup = pickupDate.Value;
                var drop = returnDate.Value;

                query = query.Where(car => !context.Rentals.Any(r =>
                    r.CarId == car.CarId &&
                    r.Status != "İptal" &&
                    pickup < r.ReturnDate &&
                    drop > r.PickupDate));
            }

            query = sort switch
            {
                "priceAsc" => query.OrderBy(x => x.DailyPrice),
                "priceDesc" => query.OrderByDescending(x => x.DailyPrice),
                "yearDesc" => query.OrderByDescending(x => x.Year),
                _ => query.OrderByDescending(x => x.CarId)
            };

            return await query.ToListAsync();
        }

        public async Task<List<Car>> GetLast10CarsAsync()
        {
            var context = new RentalyContext();

            return await context.Cars
       .Include(x => x.Brand)
       .Include(x => x.CarModel)
       .Include(x => x.Category)
       .Where(x => x.IsActive == true && x.IsAvailable == true)
       .OrderByDescending(x => x.CarId)
       .Take(10)
       .ToListAsync();
        }

        public async Task<List<Car>> SearchAvailableCarsAsync(int? categoryId, int? pickupBranchId, DateTime? pickupDate, DateTime? returnDate)
        {
            using var context = new RentalyContext();

            var values = context.Cars
                .Include(x => x.Brand)
                .Include(x => x.CarModel)
                .Include(x => x.Category)
                .Include(x => x.Branch)
                .Where(x => x.IsActive == true && x.IsAvailable == true)
                .AsQueryable();

            if (categoryId.HasValue)
                values = values.Where(x => x.CategoryId == categoryId.Value);

            if (pickupBranchId.HasValue)
                values = values.Where(x => x.BranchId == pickupBranchId.Value);

            if (pickupDate.HasValue && returnDate.HasValue)
            {
                values = values.Where(car =>
                    !context.Rentals.Any(r =>
                        r.CarId == car.CarId &&
                        r.Status != "İptal" &&
                        pickupDate.Value < r.ReturnDate &&
                        returnDate.Value > r.PickupDate));
            }

            return await values
                .OrderBy(x => x.DailyPrice)
                .ToListAsync();
        }
    }
}
