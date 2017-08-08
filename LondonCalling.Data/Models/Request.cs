using System;
using System.Collections.Generic;
using System.Text;

namespace LondonCalling.Data.Models
{
    public partial class Request
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string SessionId { get; set; }
        public string AppId { get; set; }
        public string RequestId { get; set; }
        public string UserId { get; set; }
        public System.DateTime Timestamp { get; set; }
        public string Intent { get; set; }
        public string Slots { get; set; }
        public bool IsNew { get; set; }

        public string consentToken { get; set; }
        public string deviceId { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }
        public string Reason { get; set; }
        public System.DateTime DateCreated { get; set; }

        public virtual Member Member { get; set; }
    }
}
