using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoSimple.Messaging
{
    public class Message<T> 
    {
        public Message()
        {
            this.Id = Guid.NewGuid();
        } 

        public Guid Id { get; private set; }

        public string CorrelationId { get; set; }

        public DateTime Timestamp { get; set; }

        public string Sender { get; set; }

        public T Body { get; set; }

        public string Context { get; set; }

        public bool Redelivered { get; set; }

        public int DeliveryCount { get; set; }

    }
}
