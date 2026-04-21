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
    public class EfBranchDal : GenericRepocitory<Branch>, IBranchDal
    {
        public EfBranchDal(RentalyContext context) : base(context)
        {
        }

        public Branch GetBranchWithCars(int id)
        {
            using var context = new RentalyContext();

            return context.Branches
                .Include(x => x.Cars)
                .FirstOrDefault(x => x.BranchId == id);
        }
    }
}
