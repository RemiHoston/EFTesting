using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PlayList.Models
{
    [Table("Vote")]
    public class Vote : ModelBase
    {
        [Required()]
        [Column("PlayId", TypeName = "int")]
        public int PlayId { get; set; }
        [Required]
        [Column("CustomerId", TypeName = "int")]
        public int CustomerId { get; set; }
    }
}