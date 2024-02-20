using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansServer
{
    public class SessionManager
    {
        public static ConcurrentDictionary<string, SessionFactory> Domains = new ConcurrentDictionary<string, SessionFactory>();

        public static Session CreateSession(string sessionId)
        {
            SessionFactory sessionFactory = null;
            AppDomain domain = null;

            if (Domains.ContainsKey(sessionId))
            {
                sessionFactory = Domains[sessionId];
            }
            else
            {
                domain = AppDomain.CreateDomain(sessionId);
                sessionFactory = (SessionFactory)domain.CreateInstanceAndUnwrap(typeof(SessionFactory).Assembly.FullName, typeof(SessionFactory).FullName);
                sessionFactory.Name = domain;
                Domains.TryAdd(sessionId, sessionFactory);

            }
            return sessionFactory.Session;

        }

        public class SessionFactory : MarshalByRefObject
        {
            public AppDomain Name { get; set; }

            public Session Session
            {
                get
                {
                    return Session.Instance;
                }
            }

        }

        public class Session: MarshalByRefObject
        {

            public static int SessionId = 0;
            public int Count { get; set; }

            private static Session instance = null;
            public static Session Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new Session();
                    }
                    return instance;
                }
            }


        }
    }
}
