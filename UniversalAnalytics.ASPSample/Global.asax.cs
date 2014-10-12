using Echovoice.UniversalAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace UniversalAnalytics.ASPSample
{
    public class Global : System.Web.HttpApplication
    {
        // initialize a static (thread-safe) tracker
        public static UATracker tracker = new UATracker("UA-XXXXXX-X");

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // track page view
            tracker.TrackPageViewAsync();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}