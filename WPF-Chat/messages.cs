//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WPF_Chat
{
    using System;
    using System.Collections.Generic;
    
    public partial class messages
    {
        public int id { get; set; }
        public string content { get; set; }
        public int user_id { get; set; }
        public int conversation_id { get; set; }
        public System.DateTime created_at { get; set; }
    
        public virtual conversations conversations { get; set; }
        public virtual users users { get; set; }
    }
}
