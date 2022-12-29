using System.ComponentModel.DataAnnotations.Schema;

namespace lab4
{
    public class Enrollment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EnrollmentId { get; set; }

        public DateTime DateOfEnroll { get; set; }

        public int ClientPHnumber { get; set; } //зовнішній ключ

        public virtual Client? Client { get; set; }
    }
}
