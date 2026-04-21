using FluentValidation;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.ValidationRules
{
    public class BranchValidator:AbstractValidator<Branch>
    {
        public BranchValidator()
        {
            RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("Şube adı boş bırakılamaz.")
            .MinimumLength(3).WithMessage("Şube adı en az 3 karakter olmalıdır.")
            .MaximumLength(50).WithMessage("Şube adı en fazla 50 karakter olabilir.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Şehir boş bırakılamaz.")
                .MinimumLength(2).WithMessage("Şehir adı en az 2 karakter olmalıdır.")
                .MaximumLength(40).WithMessage("Şehir adı çok uzun.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres boş bırakılamaz.")
                .MinimumLength(5).WithMessage("Adres çok kısa.")
                .MaximumLength(200).WithMessage("Adres çok uzun.");

            RuleFor(x => x.BranchName)
                .Matches(@"^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$")
                .WithMessage("Şube adı özel karakter içeremez.");

            RuleFor(x => x.City)
                .Matches(@"^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$")
                .WithMessage("Şehir adı sadece harf içermelidir.");
        }
    }
}
