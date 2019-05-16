using CML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CML.Utilities
{
    public class Utilities
    {
        private static Utilities m_utilsInstance;

        public static Utilities Instance
        {
            get
            {
                if ( m_utilsInstance == null )
                    m_utilsInstance = new Utilities();

                return m_utilsInstance;
            }
        }
        public Utilities()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public virtual void CreateEmailNotice(string type, int? reqid,int? testid, int userid )
        {
            using ( CMLEntities db = new CMLEntities() )
            {
                Notification n = new Models.Notification();
                n.LoggedOn = DateTime.Now;
                n.MessageType = type;
                n.Recipient = userid;
                n.RequestID = reqid;
                n.Sent = false;
                n.TestID = testid;

                db.Notifications.Add( n );
                db.SaveChanges();
            }



        }


        public virtual string StripDomain(string user )
        {
            string temp;
            if ( user.Contains( "\\" ) )
            {
                temp = user.Substring( user.LastIndexOf( "\\" ) + 1 );
            }
            else
            {
                temp = user;
            }

            return temp;
        }

    }
}