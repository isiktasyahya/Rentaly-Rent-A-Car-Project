using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Controllers
{
    public class BookingController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICustomerService _customerService;
        private readonly IRentalService _rentalService;
        private readonly IBranchService _branchService;

        public BookingController(
            ICarService carService,
            ICustomerService customerService,
            IRentalService rentalService,
            IBranchService branchService)
        {
            _carService = carService;
            _customerService = customerService;
            _rentalService = rentalService;
            _branchService = branchService;
        }

        // ── Booking Sayfası GET ──
        [HttpGet]
        public async Task<IActionResult> CreateBooking(
            int carId,
            int pickupBranchId,
            int returnBranchId,
            string pickupDate,
            string pickupTime,
            string returnDate,
            string returnTime)
        {
            var car = await _carService.TGetCarDetailAsync(carId);
            if (car == null) return NotFound();

            var branches = await _branchService.TGetListAsync();
            var pickupBranch = branches.FirstOrDefault(b => b.BranchId == pickupBranchId);
            var returnBranch = branches.FirstOrDefault(b => b.BranchId == returnBranchId);

            var pickup = DateTime.Parse(pickupDate + " " + (pickupTime ?? "00:00"));
            var ret = DateTime.Parse(returnDate + " " + (returnTime ?? "00:00"));

            var diffHours = (ret - pickup).TotalHours;
            var days = (int)Math.Ceiling(diffHours / 24);
            if (days < 1) days = 1;
            var totalPrice = car.DailyPrice * days;

            ViewBag.Car = car;
            ViewBag.PickupBranch = pickupBranch?.BranchName ?? "-";
            ViewBag.ReturnBranch = returnBranch?.BranchName ?? "-";
            ViewBag.PickupDate = pickup;
            ViewBag.ReturnDate = ret;
            ViewBag.Days = days;
            ViewBag.TotalPrice = totalPrice;
            ViewBag.PickupBranchId = pickupBranchId;
            ViewBag.ReturnBranchId = returnBranchId;

            return View();
        }

        // ── Booking POST ──
        [HttpPost]
        public async Task<IActionResult> CreateBooking(
            int carId,
            int pickupBranchId,
            int returnBranchId,
            DateTime pickupDate,
            DateTime returnDate,
            decimal totalPrice,
            string name,
            string surname,
            string email,
            string phone,
            string identityNumber,
            string drivingLicenseNumber,
            DateTime drivingLicenseDate)
        {
            // 1. Müşteriyi kaydet
            var customer = new Customer
            {
                Name = name ?? "Belirtilmedi",
                Surname = surname ?? "Belirtilmedi",
                Email = email ?? "Belirtilmedi",
                Phone = phone ?? "Belirtilmedi",
                IdentityNumber = identityNumber ?? "Belirtilmedi",
                DrivingLicenseNumber = drivingLicenseNumber ?? "Belirtilmedi",
                DrivingLicenseDate = drivingLicenseDate == default ? DateTime.Today : drivingLicenseDate
            };

            await _customerService.TInsertCustomerDirectAsync(customer);

            // 2. Rental'ı kaydet
            var rental = new Rental
            {
                CarId = carId,
                CustomerId = customer.CustomerId,
                PickupBranchId = pickupBranchId,
                ReturnBranchId = returnBranchId,
                PickupDate = pickupDate,
                ReturnDate = returnDate,
                TotalPrice = totalPrice,
                Status = "Beklemede"
            };

            await _rentalService.TInsertAsync(rental);

            TempData["Success"] = "Rezervasyon talebiniz alındı! En kısa sürede onaylanacaktır.";
            return RedirectToAction("BookingSuccess");
        }

        // ── Başarı Sayfası ──
        public IActionResult BookingSuccess()
        {
            return View();
        }

        
    }
}
