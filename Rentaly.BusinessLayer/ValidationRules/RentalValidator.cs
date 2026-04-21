using FluentValidation;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.ValidationRules
{
    public class RentalValidator:AbstractValidator<Rental>
    {
        public RentalValidator()
        {
            RuleFor(x => x.CarId)
                .GreaterThan(0).WithMessage("Araç seçilmelidir.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Müşteri seçilmelidir.");

            RuleFor(x => x.PickupBranchId)
                .GreaterThan(0).WithMessage("Alış şubesi seçilmelidir.");

            RuleFor(x => x.ReturnBranchId)
                .GreaterThan(0).WithMessage("İade şubesi seçilmelidir.");

            RuleFor(x => x.PickupDate)
                .NotEmpty().WithMessage("Alış tarihi boş bırakılamaz.")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Alış tarihi bugünden önce olamaz.");

            RuleFor(x => x.ReturnDate)
                .NotEmpty().WithMessage("İade tarihi boş bırakılamaz.")
                .GreaterThan(x => x.PickupDate)
                .WithMessage("İade tarihi alış tarihinden sonra olmalıdır.");

            RuleFor(x => x.TotalPrice)
                .GreaterThan(0).WithMessage("Toplam ücret 0'dan büyük olmalıdır.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Durum bilgisi boş bırakılamaz.")
                .MaximumLength(20).WithMessage("Durum bilgisi çok uzun.");
        }
    }
}
