using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace PlayList.Models
{
    [TableAttribute("PlayInfo")]
    public class PlayInfo:ModelBase
    {
        [Column("PlayName",TypeName ="nvarchar(50)")]
        [Display(Name ="The name of this play")]
        public string PlayName{get;set;}
        [Column("Note",TypeName ="nvarchar(50)")]
        [Display(Name ="The description of this play")]
        public string Note{get;set;}

        [NotMapped]
        public int VoteCount{get;set;}
    }
}