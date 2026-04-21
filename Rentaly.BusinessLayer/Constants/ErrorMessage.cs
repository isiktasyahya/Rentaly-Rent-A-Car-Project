using FluentValidation;
using Rentaly.BusinessLayer.ValidationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.Constants
{
    public class ErrorMessage
    {
        // Bir mesajı ayrı ayrı yerlerde göstermektense tek bir yerde gösterilir ve kontrol edilir.

        // 1. Adım: Mesaj kontrolü yapılmalı

        public const string CustomerNameRequired = "Müşteri adı boş bırakılamaz.";
        public const string CustomerSurnameRequired = "Müşteri soyadı boş bırakılamaz.";
        public const string InvalidEmail = "Geçerli bir email adresi giriniz.";
        public const string InvalidPhone = "Telefon numarası geçersiz.";
        public const string IdentityNumberInvalid = "Kimlik numarası 11 haneli olmalıdır.";
        public const string DrivingLicenseRequired = "Ehliyet numarası boş bırakılamaz.";
        public const string DrivingLicenseDateInvalid = "Ehliyet tarihi gelecekte olamaz.";


        // 2 Adım: FluentValidation ile kullanılır.


        //        using FluentValidation;
        //public class CustomerValidator : AbstractValidator<CreateCustomerDto>
        //    {
        //        public CustomerValidator()
        //        {
        //            RuleFor(x => x.CustomerName)
        //                .NotEmpty()
        //                .WithMessage(ErrorMessages.CustomerNameRequired);

        //            RuleFor(x => x.CustomerSurname)
        //                .NotEmpty()
        //                .WithMessage(ErrorMessages.CustomerSurnameRequired);

        //            RuleFor(x => x.Email)
        //                .EmailAddress()
        //                .WithMessage(ErrorMessages.InvalidEmail);

        //            RuleFor(x => x.Phone)
        //                .Matches(@"^\d{10,11}$")
        //                .WithMessage(ErrorMessages.InvalidPhone);
        //        }
        //    }



        // 3. Adım: Controllarda gösterme

        //[HttpPost]
        //public IActionResult CreateCustomer(CreateCustomerDto dto)
        //{
        //    var validator = new CustomerValidator();
        //    var result = validator.Validate(dto);

        //    if (!result.IsValid)
        //    {
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError("", error.ErrorMessage);
        //        }

        //        return View(dto);
        //    }

        //    return RedirectToAction("Index");
        //}

    }
}
