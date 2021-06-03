using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pomelo.EntityFrameworkCore.MySql;
namespace PlayList.Models
{
    [Table("Custoemr")]
    public class Customer : ModelBase
    {
        public string RealName{get;set;}
        public string NickName{get;set;}
        public string PhoneNumber{get;set;}

    }
}