using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class Invoice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceId { get; set; }

        public int ClientPHnumber { get; set; } //зовнішній ключ

        public string CarVINcode { get; set; } = null!; //зовнішній ключ
        
        public Car? Car { get; set; }

        public Client? Client { get; set; }

        public List<ListOfProvidedServices> ListOfProvidedServices { get; } = new List<ListOfProvidedServices>();

        public List<ListOfSpareParts> ListOfSpareParts { get; } = new List<ListOfSpareParts>();
    }
}
