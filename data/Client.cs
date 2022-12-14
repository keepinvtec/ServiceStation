using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class Client
    {
        [Key] //опис простого первинного ключа через анотації даних
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PHnumber { get; set; }

        public string FullName { get; set; } = null!;

        public virtual List<Enrollment> Enrollments { get; } = new List<Enrollment>();

        public virtual List<Invoice> Invoices { get; } = new List<Invoice>();
    }


    public class VIPclient : Client
    {
        public int YearsOfService { get; set; }
    }

    public class ObsoleteClient : VIPclient
    {
        public DateTime DateOfDeletion { get; set; }
    }

    public class Worker
    {
        public int WorkerId { get; set; }

        public string? FullName { get; set; }
    }
}
