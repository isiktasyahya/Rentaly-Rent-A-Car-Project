using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.DtoLayer.CarDtos
{
    public class SearchDto
    {
        public int CategoryId { get; set; }          // Car / Van / Minibus / Prestige

        public int PickupBranchId { get; set; }      // Pick Up Location

        public int ReturnBranchId { get; set; }      // Drop Off Location

        public DateTime PickupDate { get; set; }     // Alış Tarihi

        public string PickupTime { get; set; }       // Alış Saati

        public DateTime ReturnDate { get; set; }     // Dönüş Tarihi

        public string ReturnTime { get; set; }       // Dönüş Saati
    }
}
