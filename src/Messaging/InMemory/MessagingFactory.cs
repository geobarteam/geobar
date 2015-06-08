using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoSimple.Messaging.InMemory
{
    public class MessagingFactory : IMessagingFactory
    {
        private MessagingConfiguration messagingConfiguration { get; set; }

        public MessagingFactory(MessagingConfiguration messagingConfiguration)
        {
            this.messagingConfiguration = messagingConfiguration;
        }

        public IReceiver<T> CreateReceiver<T>(string destination)
        {
            throw new NotImplementedException();
        }

        public IReceiver<T> CreateReceiver<T>()
        {
            throw new NotImplementedException();
        }

        public ISender<T> CreateSender<T>(string destination)
        {
            throw new NotImplementedException();
        }

        public IPublisher<T> CreatePublisher<T>(string destination)
        {
            throw new NotImplementedException();
        }

        public ISubscriber<T> CreateSubscriber<T>(string destination)
        {
            throw new NotImplementedException();
        }

        public ISubscriber<T> CreateDurableSubscriber<T>(string destination, string subscription)
        {
            throw new NotImplementedException();
        }

        public IBrowser<T> CreateBrowser<T>(string destination)
        {
            throw new NotImplementedException();
        }

        public ISession Session
        {
            get { throw new NotImplementedException(); }
        }
    }
}
