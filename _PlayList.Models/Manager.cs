using System.Xml.Serialization;
using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Pomelo.EntityFrameworkCore.MySql;
using System.ComponentModel.DataAnnotations;

namespace PlayList.Models
{
    /// <summary>
    /// Manager of the System
    /// </summary>
    [Table("Manager")]
    public class Manager : ModelBase
    {
        
        [Required]
        [Display(Name = "The user's LoginName")]
        [Column("UserName", TypeName = "varchar(50)")]
        public string UserName { get; set; }
#warning 本次设计这里使用了明文密码
        [Required]
        [Display(Name = "The user's Password")]
        [Column("Password", TypeName = "varchar(50)")]
        public string Password { get; set; }
        [Column("RealName", TypeName = "nvarchar(50)")]
        [Display(Name = "The user's RealName")]
        public string RealName { get; set; }
        [Column("Address", TypeName = "nvarchar(50)")]
        [Display(Name = "The user's Address")]
        public string Address { get; set; }
    }
}
