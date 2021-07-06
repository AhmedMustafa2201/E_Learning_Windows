//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace E_Learning.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserQuestion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserQuestion()
        {
            this.UserReplies = new HashSet<UserReply>();
        }
    
        public int Id { get; set; }
        public string Content { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<int> Likes { get; set; }
        public Nullable<int> Dislikes { get; set; }
        public Nullable<int> AnswersCount { get; set; }
        public Nullable<int> Views { get; set; }
        public Nullable<int> UserId { get; set; }
    
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserReply> UserReplies { get; set; }
    }
}