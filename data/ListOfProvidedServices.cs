using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class ListOfProvidedServices
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListOfProvidedServicesId { get; set; }

        public int InvoiceOrderId { get; set; } //зовнішній ключ

        public int ListOfServicesServiceID { get; set; } //зовнішній ключ

        public Invoice? Invoice { get; set; }

        public ListOfServices? ListOfServices { get; set; }
    }
}
