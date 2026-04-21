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
    public class EfCustomerDal : GenericRepocitory<Customer>, ICustomerDal
    {
        public EfCustomerDal(RentalyContext context) : base(context)
        {
        }

        public async Task<int> GetCustomerCountAsync()
        {
            using var context = new RentalyContext();
            return await context.Customers.CountAsync();
        }
    }
}
