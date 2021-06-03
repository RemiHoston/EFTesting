using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PlayList.Models
{
    public class ModelBase
    {
        /// <summary>
        /// primary key
        /// </summary>
        /// <value></value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id",TypeName ="int")]
        [Display(Name="The table's primary key.")]
        public int Id{get;set;}
        [Required()]
        [Column("Status",TypeName ="int")]
        [Display(Name ="This record's Status")]
        public CommonStatusEnum Status{get;set;}
        [Required()]
        [Column("CreateTime",TypeName ="DateTime")]
        public DateTime CreateTime{get;set;}
        [Column("UpdateTime",TypeName ="DateTime")]
        public DateTime UpdateTime{get;set;}
        [Column("CreateUser",TypeName ="varchar(50)")]
        public int CreateUser{get;set;}
        [Column("UpdateUser",TypeName ="varchar(50)")]
        public int UpdateUser{get;set;}
    }
}