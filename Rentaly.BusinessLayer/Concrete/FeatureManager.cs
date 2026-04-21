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
    public class FeatureManager : IFeatureService
    {
        private readonly IFeatureDal _featureDal;

        public FeatureManager(IFeatureDal featureDal)
        {
            _featureDal = featureDal;
        }

        public async Task TDeleteAsync(int id)
        {
            await _featureDal.DeleteAsync(id);
        }

        public async Task<Feature> TGetByIdAsync(int id)
        {
            return await _featureDal.GetByIdAsync(id);
        }

        public async Task<List<Feature>> TGetListAsync()
        {
            return await _featureDal.GetListAsync();
        }

        public async Task TInsertAsync(Feature entity)
        {
            await _featureDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(Feature entity)
        {
            await _featureDal.UpdateAsync(entity);
        }
    }
}
