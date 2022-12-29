using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab4
{
    public class Client
    {
        [Key] //опис простого первинного ключа через анотації даних
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PHnumber { get; set; }

        public string FullName { get; set; } = null!;

        public virtual List<Enrollment> Enrollments { get; } = new List<Enrollment>();

    }
}
