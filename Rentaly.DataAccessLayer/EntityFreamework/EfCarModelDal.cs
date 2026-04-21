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
    public class EfCarModelDal : GenericRepocitory<CarModel>, ICarModelDal
    {
        public EfCarModelDal(RentalyContext context) : base(context)
        {
        }

        public async Task<List<CarModel>> GetCarsGroupedByModelAsync()
        {
            using var context = new RentalyContext();

            var values = await context.Cars
                .Where(x => x.IsActive == true)
                .Include(x => x.Brand)
                .Include(x => x.CarModel)
                .GroupBy(x => new
                {
                    x.CarModelId,
                    x.CarModel.ModelName,
                    x.BrandId,
                    x.Brand.BrandName
                })
                .Select(g => new CarModel
                {
                    CarModelId = g.Key.CarModelId,
                    ModelName = g.Key.ModelName,
                    BrandId = g.Key.BrandId,
                    Brand = new Brand
                    {
                        BrandId = g.Key.BrandId,
                        BrandName = g.Key.BrandName
                    },
                    Cars = g.ToList()
                })
                .ToListAsync();

            return values;
        }
    }
}
