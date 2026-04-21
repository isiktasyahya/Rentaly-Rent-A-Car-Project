namespace Rentaly.WebUI.Models
{
    public class DashboardViewModel
    {
        // ================= KPI =================
        public int TotalCars { get; set; }
        public int AvailableCars { get; set; }
        public int UnavailableCars { get; set; }

        public int TotalCustomers { get; set; }

        public int TotalRentals { get; set; }
        public int ApprovedRentals { get; set; }
        public int PendingRentals { get; set; }
        public int CancelledRentals { get; set; }

        public int TotalBranches { get; set; }

        public decimal AverageDailyPrice { get; set; }
        public decimal TotalRevenue { get; set; }

        // Yeni kartlar
        public decimal TodayRevenue { get; set; }
        public decimal ThisMonthRevenue { get; set; }

        public decimal ActiveRevenue { get; set; }
        public int ActiveRentalCount { get; set; }

        // ================= ORANLAR =================
        public int OccupancyRate { get; set; }
        public int ApprovalRate { get; set; }
        public int CustomerGrowthRate { get; set; }

        // ================= CHART DATA =================
        public List<int> Last7DayRentalCounts { get; set; } = new();
        public List<decimal> Last7DayRevenue { get; set; } = new();
        public List<string> Last7DayLabels { get; set; } = new();

        // ================= TABLES =================
        public List<RentalSummaryDto> RecentRentals { get; set; } = new();

        public List<TopCarModelDto> TopModels { get; set; } = new();

        public List<TopCustomerDto> TopCustomers { get; set; } = new();

        public List<BranchPerformanceDto> Branches { get; set; } = new();
    }
}