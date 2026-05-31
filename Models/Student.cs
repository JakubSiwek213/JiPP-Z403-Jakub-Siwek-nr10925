using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projekt_Jakub_Siwek_Z403_AV.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        
        [Column("imię")]
        public string Name { get; set; } = string.Empty;

        [Column("nazwisko")]
        public string Surname { get; set; } = string.Empty;

        [Column("grupa")]
        public string GroupName { get; set; } = string.Empty;
    }
}