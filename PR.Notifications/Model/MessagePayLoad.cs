using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PR.Notifications.Model
{
    public class MessagePayLoad
    {
        public string EventName { get; set; }
        public string Recipents { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
