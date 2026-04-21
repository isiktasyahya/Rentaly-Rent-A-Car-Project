using FluentValidation;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.ValidationRules
{
    public class CarModelValidator:AbstractValidator<CarModel>
    {
        public CarModelValidator()
        {
            RuleFor(x => x.ModelName)
           .NotEmpty().WithMessage("Model adı boş bırakılamaz.")
           .MinimumLength(2).WithMessage("Model adı en az 2 karakter olmalıdır.")
           .MaximumLength(50).WithMessage("Model adı en fazla 50 karakter olabilir.");

            RuleFor(x => x.BrandId)
                .NotEmpty().WithMessage("Marka seçilmelidir.")
                .GreaterThan(0).WithMessage("Geçerli bir marka seçiniz.");

            RuleFor(x => x.ModelName)
                .Matches(@"^[a-zA-Z0-9ğüşöçıİĞÜŞÖÇ\s]+$")
                .WithMessage("Model adı geçersiz karakter içeriyor.");
        }
    }
}
