using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class ListOfServices
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceID { get; set; }

        public string NameOfService { get; set; } = null!;

        public double Complexity { get; set; }

        public double CostOfAnHour { get; set; }

        public List<ListOfProvidedServices> ListOfProvidedServices { get; } = new List<ListOfProvidedServices>();
    }
}
