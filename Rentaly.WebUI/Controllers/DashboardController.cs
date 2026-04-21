using Microsoft.AspNetCore.Mvc;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.WebUI.Models;

namespace Rentaly.WebUI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICustomerService _customerService;
        private readonly IRentalService _rentalService;
        private readonly IBranchService _branchService;

        public DashboardController(
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

        public async Task<IActionResult> Index()
        {
            var cars = await _carService.TGetListAsync();
            var customers = await _customerService.TGetListAsync();
            var rentals = await _rentalService.TGetAllRentalsWithDetailsAsync();
            var branches = await _branchService.TGetListAsync();

            var today = DateTime.Today;
            var now = DateTime.Now;

            // ================= KPI =================
            int totalCars = cars.Count;
            int availableCars = cars.Count(x => x.IsAvailable);
            int unavailableCars = cars.Count(x => !x.IsAvailable);

            int totalCustomers = customers.Count;

            int totalRentals = rentals.Count;
            int approvedRentals = rentals.Count(x => x.Status == "Onaylandı");
            int pendingRentals = rentals.Count(x => x.Status == "Beklemede");
            int cancelledRentals = rentals.Count(x => x.Status == "İptal");

            int totalBranches = branches.Count;

            decimal averageDailyPrice = totalCars > 0
                ? cars.Average(x => x.DailyPrice)
                : 0;

            decimal totalRevenue = rentals
                .Where(x => x.Status == "Onaylandı")
                .Sum(x => x.TotalPrice);

            // ================= 2 AYRI KART =================

            // Bugün başlayan rezervasyonlardan gelir
            decimal todayRevenue = rentals
                .Where(x => x.Status == "Onaylandı"
                         && x.PickupDate.Date == today)
                .Sum(x => x.TotalPrice);

            // Bugün aktif devam eden kiralamalar
            decimal activeRevenue = rentals
                .Where(x => x.Status == "Onaylandı"
                         && x.PickupDate.Date <= today
                         && x.ReturnDate.Date >= today)
                .Sum(x => x.TotalPrice);

            int activeRentalCount = rentals
                .Count(x => x.Status == "Onaylandı"
                         && x.PickupDate.Date <= today
                         && x.ReturnDate.Date >= today);

            decimal thisMonthRevenue = rentals
                .Where(x => x.Status == "Onaylandı"
                         && x.PickupDate.Month == now.Month
                         && x.PickupDate.Year == now.Year)
                .Sum(x => x.TotalPrice);

            // ================= ORANLAR =================
            int occupancyRate = totalCars > 0
                ? (int)((double)unavailableCars / totalCars * 100)
                : 0;

            int approvalRate = totalRentals > 0
                ? (int)((double)approvedRentals / totalRentals * 100)
                : 0;

            // ================= LAST 7 DAYS =================
            var labels = new List<string>();
            var revenueList = new List<decimal>();
            var rentalCountList = new List<int>();

            for (int i = 6; i >= 0; i--)
            {
                var date = today.AddDays(-i);

                labels.Add(date.ToString("dd MMM"));

                revenueList.Add(
                    rentals.Where(x =>
                        x.Status == "Onaylandı" &&
                        x.PickupDate.Date == date.Date)
                    .Sum(x => x.TotalPrice));

                rentalCountList.Add(
                    rentals.Count(x =>
                        x.PickupDate.Date == date.Date));
            }

            // ================= RECENT RENTALS =================
            var recentRentals = rentals
                .OrderByDescending(x => x.RentalId)
                .Take(10)
                .Select(x => new RentalSummaryDto
                {
                    RentalId = x.RentalId,
                    CustomerName = x.Customer != null
                        ? x.Customer.Name + " " + x.Customer.Surname
                        : "-",

                    CustomerPhone = x.Customer?.Phone ?? "-",

                    CarBrand = x.Car?.Brand?.BrandName ?? "-",
                    CarModel = x.Car?.CarModel?.ModelName ?? "-",
                    CarYear = x.Car?.Year ?? 0,
                    FuelType = x.Car?.FuelType ?? "-",

                    PickupBranch = x.PickupBranch?.BranchName ?? "-",
                    ReturnBranch = x.ReturnBranch?.BranchName ?? "-",

                    PickupDate = x.PickupDate,
                    ReturnDate = x.ReturnDate,

                    TotalPrice = x.TotalPrice,
                    Status = x.Status
                }).ToList();

            // ================= VIEWMODEL =================
            var model = new DashboardViewModel
            {
                TotalCars = totalCars,
                AvailableCars = availableCars,
                UnavailableCars = unavailableCars,

                TotalCustomers = totalCustomers,

                TotalRentals = totalRentals,
                ApprovedRentals = approvedRentals,
                PendingRentals = pendingRentals,
                CancelledRentals = cancelledRentals,

                TotalBranches = totalBranches,

                AverageDailyPrice = averageDailyPrice,
                TotalRevenue = totalRevenue,

                TodayRevenue = todayRevenue,          // Kart 1
                ActiveRevenue = activeRevenue,        // Kart 2
                ActiveRentalCount = activeRentalCount,

                ThisMonthRevenue = thisMonthRevenue,

                OccupancyRate = occupancyRate,
                ApprovalRate = approvalRate,

                Last7DayLabels = labels,
                Last7DayRevenue = revenueList,
                Last7DayRentalCounts = rentalCountList,

                RecentRentals = recentRentals
            };

            return View(model);
        }
    }
}