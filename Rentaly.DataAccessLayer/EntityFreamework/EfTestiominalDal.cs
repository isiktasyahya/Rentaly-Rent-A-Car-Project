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
    public class EfTestiominalDal : GenericRepocitory<Testimonial>, ITestiominalDal
    {
        public EfTestiominalDal(RentalyContext context) : base(context)
        {
        }
    }
}
