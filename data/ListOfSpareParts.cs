using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class ListOfSpareParts
    {
        public int SPnumber { get; set; }

        public string? NameOfPart { get; set; }

        public int Amount { get; set; }

        public double Price { get; set; }

        public int InvoiceOrderId { get; set; } //зовнішній ключ

        public virtual Invoice? Invoice { get; set; }
    }
}
