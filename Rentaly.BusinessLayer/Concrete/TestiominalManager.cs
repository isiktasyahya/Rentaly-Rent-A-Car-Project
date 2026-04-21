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
    public class TestiominalManager : ITestiominalService
    {
        private readonly ITestiominalDal _testiominalDal;

        public TestiominalManager(ITestiominalDal testiominalDal)
        {
            _testiominalDal = testiominalDal;
        }

        public async Task TDeleteAsync(int id)
        {
            await _testiominalDal.DeleteAsync(id);
        }

        public async Task<Testimonial> TGetByIdAsync(int id)
        {
            return await _testiominalDal.GetByIdAsync(id);
        }

        public async Task<List<Testimonial>> TGetListAsync()
        {
            return await _testiominalDal.GetListAsync();
        }

        public async Task TInsertAsync(Testimonial entity)
        {
            await _testiominalDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(Testimonial entity)
        {
            await _testiominalDal.UpdateAsync(entity);
        }
    }
}
