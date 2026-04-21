using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.EntityFreamework;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.Concrete
{
    public class CarModelManager : ICarModelService
    {
        private readonly ICarModelDal _carModelDal;

        public CarModelManager(ICarModelDal carModelDal)
        {
            _carModelDal = carModelDal;
        }

        public async Task TDeleteAsync(int id)
        {
            await _carModelDal.DeleteAsync(id);
        }

        public async Task<CarModel> TGetByIdAsync(int id)
        {
            return await _carModelDal.GetByIdAsync(id);
        }

        public async Task<List<CarModel>> TGetCarsGroupedByModelAsync()
        {
            return await _carModelDal.GetCarsGroupedByModelAsync();
        }

        public async Task<List<CarModel>> TGetListAsync()
        {
            return await _carModelDal.GetListAsync();

        }

        public async Task TInsertAsync(CarModel entity)
        {
            await _carModelDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(CarModel entity)
        {
            await _carModelDal.UpdateAsync(entity);
        }
    }
}
