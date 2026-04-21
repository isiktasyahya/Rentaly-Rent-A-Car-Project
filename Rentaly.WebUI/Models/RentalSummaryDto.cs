namespace Rentaly.WebUI.Models
{
    public class RentalSummaryDto
    {
        public int RentalId { get; set; }

        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }

        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public int CarYear { get; set; }
        public string FuelType { get; set; }

        public string PickupBranch { get; set; }
        public string ReturnBranch { get; set; }

        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
