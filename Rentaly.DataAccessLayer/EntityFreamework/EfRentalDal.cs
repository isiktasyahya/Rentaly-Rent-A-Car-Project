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
    public class EfRentalDal : GenericRepocitory<Rental>, IRentalDal
    {
        
        public EfRentalDal(RentalyContext context) : base(context)
        {
        }

        public async Task<List<Rental>> GetAllRentalsWithDetailsAsync()
        {
            using var context = new RentalyContext();
            return await context.Rentals
                .Include(r => r.Car)
                    .ThenInclude(c => c.Brand)
                .Include(r => r.Car)
                    .ThenInclude(c => c.CarModel)
                .Include(r => r.Car)
                    .ThenInclude(c => c.Category)
                .Include(r => r.Customer)
                .Include(r => r.PickupBranch)
                .Include(r => r.ReturnBranch)
                .OrderByDescending(r => r.RentalId)
                .ToListAsync();
        }

        public async Task<int> GetRentalCountAsync()
        {
            using var context = new RentalyContext();
            return await context.Rentals.CountAsync();
        }
    }
}
