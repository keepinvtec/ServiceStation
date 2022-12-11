using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class Enrollment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EnrollmentId { get; set; }

        public DateTime DateOfEnroll { get; set; }

        public int ClientPHnumber { get; set; } //зовнішній ключ

        public Client? Client { get; set; }
    }
}
