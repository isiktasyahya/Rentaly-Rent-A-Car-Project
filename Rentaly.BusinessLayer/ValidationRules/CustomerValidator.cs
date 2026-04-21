using FluentValidation;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.ValidationRules
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("İsim boş bırakılamaz.")
                .MinimumLength(2).WithMessage("İsim en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("İsim en fazla 50 karakter olabilir.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Soyisim boş bırakılamaz.")
                .MinimumLength(2).WithMessage("Soyisim en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Soyisim en fazla 50 karakter olabilir.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş bırakılamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon numarası boş bırakılamaz.")
                .Matches(@"^05\d{9}$").WithMessage("Telefon numarası 05 ile başlamalı ve 11 haneli olmalıdır.");

            RuleFor(x => x.IdentityNumber)
                .NotEmpty().WithMessage("TC Kimlik numarası boş bırakılamaz.")
                .Length(11).WithMessage("TC Kimlik numarası 11 haneli olmalıdır.")
                .Matches(@"^[0-9]+$").WithMessage("TC Kimlik numarası sadece rakamlardan oluşmalıdır.");

            RuleFor(x => x.DrivingLicenseNumber)
                .NotEmpty().WithMessage("Ehliyet numarası boş bırakılamaz.")
                .MinimumLength(5).WithMessage("Ehliyet numarası çok kısa.")
                .MaximumLength(50).WithMessage("Ehliyet numarası en fazla 50 karakter olabilir.");

            RuleFor(x => x.DrivingLicenseDate)
                .NotEmpty().WithMessage("Ehliyet tarihi boş bırakılamaz.")
                .LessThan(DateTime.Now).WithMessage("Ehliyet tarihi bugünden ileri olamaz.");
        }
    }
}
