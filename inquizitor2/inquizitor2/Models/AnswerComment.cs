//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace inquizitor2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class AnswerComment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ACommentId { get; set; }
        public int AnswerId { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> Date { get; set; } = DateTime.Now;
    
        public virtual Answer Answer { get; set; }
        public virtual User User { get; set; }
    }
}
