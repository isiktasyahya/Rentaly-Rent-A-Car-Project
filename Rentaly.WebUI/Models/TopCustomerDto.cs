namespace Rentaly.WebUI.Models
{
    public class TopCustomerDto
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int RentalCount { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
