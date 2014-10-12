using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Echovoice.UniversalAnalytics
{
    /// <summary>
    /// Tracker class for Universal Analytics
    /// </summary>
    public class UATracker
    {
        /// <summary>
        /// Protocol Version
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#v
        /// Required for all hit types.
        /// The Protocol version. The current value is '1'. This will only change when there are changes made that are not backwards compatible.
        /// </summary>
        public string v = "1";

        /// <summary>
        /// Tracking ID / Web Property ID
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#tid
        /// Required for all hit types.
        /// The tracking ID / web property ID. The format is UA-XXXX-Y. All collected data is associated by this ID.
        /// </summary>
        public string tid;

        /// <summary>
        /// Anonymize IP
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#aip
        /// Optional.
        /// When present, the IP address of the sender will be anonymized. For example, the IP will be anonymized if any of the following parameters are present in the payload: aip=, aip=0, or aip=1
        /// </summary>
        public bool aip = true;

        /// <summary>
        /// Use SSL for sending tracking data
        /// </summary>
        public bool useSSL = true;

        /// <summary>
        /// User agent to use to send GA data from, not the users useragent and built internally and then cached
        /// </summary>
        private string userAgent
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_userAgent))
                    return _userAgent;

                else
                {
                    try
                    {
                        _userAgent = string.Format("Tracker/1.0 ({0}; {1}; {2})", Environment.OSVersion.Platform.ToString(), Environment.OSVersion.Version.ToString(), Environment.OSVersion.VersionString);
                    }
                    catch
                    {
                        _userAgent = "Tracker/1.0 (+https://github.com/proctorio/UniversalAnalyticsTracker)";
                    }

                    return _userAgent;
                }

            }
        }
        private string _userAgent = null;

        /// <summary>
        /// Initialize a universal tracker for sending ga data
        /// </summary>
        /// <param name="Tracking_ID">The tracking ID / web property ID. The format is UA-XXXX-Y.</param>
        public UATracker(string Tracking_ID)
        {
            // set the tracker id
            tid = Tracking_ID;
        }

        /// <summary>
        /// Initialize a universal tracker for sending ga data
        /// </summary>
        /// <param name="Tracking_ID">The tracking ID / web property ID. The format is UA-XXXX-Y.</param>
        /// <param name="Anonymize_IP">Anonymize IP (True or False)</param>
        public UATracker(string Tracking_ID, bool Anonymize_IP)
        {
            // set the tracker id
            tid = Tracking_ID;

            // set the anon flag
            aip = Anonymize_IP;
        }

        /// <summary>
        /// Check if the http request (current) is set
        /// </summary>
        /// <returns></returns>
        private bool IsHttpRequestAvailable
        {
            get
            {
                try
                {
                    if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    {
                        return true;
                    }
                }
                catch { }

                return false;
            }
        }

        /// <summary>
        /// Track an event
        /// </summary>
        /// <param name="eventCategory">Specifies the event category. Must not be empty</param>
        /// <param name="eventAction">Specifies the event action. Must not be empty</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackEvent(string eventCategory, string eventAction, UAClient uaclient = null)
        {
            // upfill and build
            TrackEvent((IsHttpRequestAvailable) ? new HttpContextWrapper(HttpContext.Current) : null, eventCategory, eventAction, null, uaclient);
        }

        /// <summary>
        /// Track an event
        /// </summary>
        /// <param name="httpContext">Current http context to extract payload from</param>
        /// <param name="eventCategory">Specifies the event category. Must not be empty</param>
        /// <param name="eventAction">Specifies the event action. Must not be empty</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackEvent(HttpContextBase httpContext, string eventCategory, string eventAction, UAClient uaclient = null)
        {
            // upfill and build
            TrackEvent(httpContext, eventCategory, eventAction, null, uaclient);
        }

        /// <summary>
        /// Track an event
        /// </summary>
        /// <param name="eventCategory">Specifies the event category. Must not be empty</param>
        /// <param name="eventAction">Specifies the event action. Must not be empty</param>
        /// <param name="eventLabel">Specifies the event label</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackEvent(string eventCategory, string eventAction, string eventLabel, UAClient uaclient = null)
        {
            // upfill and build
            TrackEvent((IsHttpRequestAvailable) ? new HttpContextWrapper(HttpContext.Current) : null, eventCategory, eventAction, eventLabel, -1, uaclient);
        }

        /// <summary>
        /// Track an event
        /// </summary>
        /// <param name="httpContext">Current http context to extract payload from</param>
        /// <param name="eventCategory">Specifies the event category. Must not be empty</param>
        /// <param name="eventAction">Specifies the event action. Must not be empty</param>
        /// <param name="eventLabel">Specifies the event label</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackEvent(HttpContextBase httpContext, string eventCategory, string eventAction, string eventLabel, UAClient uaclient = null)
        {
            // upfill and build
            TrackEvent(httpContext, eventCategory, eventAction, eventLabel, -1, uaclient);
        }

        /// <summary>
        /// Track an event
        /// </summary>
        /// <param name="eventCategory">Specifies the event category. Must not be empty</param>
        /// <param name="eventAction">Specifies the event action. Must not be empty</param>
        /// <param name="eventLabel">Specifies the event label</param>
        /// <param name="eventValue">Specifies the event value. Values must be non-negative</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackEvent(string eventCategory, string eventAction, string eventLabel, int eventValue, UAClient uaclient = null)
        {
            // upfill and build
            TrackEvent((IsHttpRequestAvailable) ? new HttpContextWrapper(HttpContext.Current) : null, eventCategory, eventAction, eventLabel, eventValue, uaclient);
        }

        /// <summary>
        /// Track an event
        /// </summary>
        /// <param name="httpContext">Current http context to extract payload from</param>
        /// <param name="eventCategory">Specifies the event category. Must not be empty</param>
        /// <param name="eventAction">Specifies the event action. Must not be empty</param>
        /// <param name="eventLabel">Specifies the event label</param>
        /// <param name="eventValue">Specifies the event value. Values must be non-negative</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackEvent(HttpContextBase httpContext, string eventCategory, string eventAction, string eventLabel, int eventValue, UAClient uaclient = null)
        {
            // initialize the uaclient if null
            if (uaclient == null) uaclient = new UAClient();

            // build the payload
            StringBuilder data = BuildPayload(BuildBasePayload(httpContext, uaclient), BuildEventTrackPayload(eventCategory, eventAction, eventLabel, eventValue));

            // build the http request
            HttpWebRequest request = BuildRequest(data);

            // send sync and fail silently
            try
            {
                using (WebResponse response = request.GetResponse()) { }
            }
            catch { }
        }

        /// <summary>
        /// Async track an event (non-blocking)
        /// </summary>
        /// <param name="eventCategory">Specifies the event category. Must not be empty</param>
        /// <param name="eventAction">Specifies the event action. Must not be empty</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackEventAsync(string eventCategory, string eventAction, UAClient uaclient = null)
        {
            // upfill and build
            TrackEventAsync((IsHttpRequestAvailable) ? new HttpContextWrapper(HttpContext.Current) : null, eventCategory, eventAction, null, uaclient);
        }

        /// <summary>
        /// Async track an event (non-blocking)
        /// </summary>
        /// <param name="httpContext">Current http context to extract payload from</param>
        /// <param name="eventCategory">Specifies the event category. Must not be empty</param>
        /// <param name="eventAction">Specifies the event action. Must not be empty</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackEventAsync(HttpContextBase httpContext, string eventCategory, string eventAction, UAClient uaclient = null)
        {
            // upfill and build
            TrackEventAsync(httpContext, eventCategory, eventAction, null, uaclient);
        }

        /// <summary>
        /// Async track an event (non-blocking)
        /// </summary>
        /// <param name="eventCategory">Specifies the event category. Must not be empty</param>
        /// <param name="eventAction">Specifies the event action. Must not be empty</param>
        /// <param name="eventLabel">Specifies the event label</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackEventAsync(string eventCategory, string eventAction, string eventLabel, UAClient uaclient = null)
        {
            // upfill and build
            TrackEventAsync((IsHttpRequestAvailable) ? new HttpContextWrapper(HttpContext.Current) : null, eventCategory, eventAction, eventLabel, -1, uaclient);
        }

        /// <summary>
        /// Async track an event (non-blocking)
        /// </summary>
        /// <param name="httpContext">Current http context to extract payload from</param>
        /// <param name="eventCategory">Specifies the event category. Must not be empty</param>
        /// <param name="eventAction">Specifies the event action. Must not be empty</param>
        /// <param name="eventLabel">Specifies the event label</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackEventAsync(HttpContextBase httpContext, string eventCategory, string eventAction, string eventLabel, UAClient uaclient = null)
        {
            // upfill and build
            TrackEventAsync(httpContext, eventCategory, eventAction, eventLabel, -1, uaclient);
        }

        /// <summary>
        /// Async track an event (non-blocking)
        /// </summary>
        /// <param name="eventCategory">Specifies the event category. Must not be empty</param>
        /// <param name="eventAction">Specifies the event action. Must not be empty</param>
        /// <param name="eventLabel">Specifies the event label</param>
        /// <param name="eventValue">Specifies the event value. Values must be non-negative</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackEventAsync(string eventCategory, string eventAction, string eventLabel, int eventValue, UAClient uaclient = null)
        {
            // upfill and build
            TrackEventAsync((IsHttpRequestAvailable) ? new HttpContextWrapper(HttpContext.Current) : null, eventCategory, eventAction, eventLabel, eventValue, uaclient);
        }

        /// <summary>
        /// Async track an event (non-blocking)
        /// </summary>
        /// <param name="httpContext">Current http context to extract payload from</param>
        /// <param name="eventCategory">Specifies the event category. Must not be empty</param>
        /// <param name="eventAction">Specifies the event action. Must not be empty</param>
        /// <param name="eventLabel">Specifies the event label</param>
        /// <param name="eventValue">Specifies the event value. Values must be non-negative</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackEventAsync(HttpContextBase httpContext, string eventCategory, string eventAction, string eventLabel, int eventValue, UAClient uaclient = null)
        {
            // initialize the uaclient if null
            if (uaclient == null) uaclient = new UAClient();

            // build the payload
            StringBuilder data = BuildPayload(BuildBasePayload(httpContext, uaclient), BuildEventTrackPayload(eventCategory, eventAction, eventLabel, eventValue));

            // build the http request
            HttpWebRequest request = BuildRequest(data);

            // send it async
            ProcessAsync(request);
        }

        /// <summary>
        /// Track a page view
        /// </summary>
        /// <param name="uaclient">Client override object</param>
        public void TrackPageView(UAClient uaclient = null)
        {
            // make sure we don't need it passed in
            if (IsHttpRequestAvailable)
            {
                // upfill and build
                TrackPageView(new HttpContextWrapper(HttpContext.Current), null, uaclient);
            }
            else
            {
                // google doesnt accept page views without a url
                throw new InvalidOperationException("Unable to find a valid HttpContext.Current, Google requires a valid URL to process page views.");
            }
        }

        /// <summary>
        /// Track a page view
        /// </summary>
        /// <param name="httpContext">Current http context to extract payload from</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackPageView(HttpContextBase httpContext, UAClient uaclient = null)
        {
            // check for null context
            if (httpContext == null) throw new ArgumentNullException("httpContext", "Null HttpContextBase, Google requires a valid URL to process page views.");

            // upfill and build
            TrackPageView(httpContext, null, uaclient);
        }

        /// <summary>
        /// Track a page view
        /// </summary>
        /// <param name="pageTitle">The title of the page / document</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackPageView(string pageTitle, UAClient uaclient = null)
        {
            // make sure we don't need it passed in
            if (IsHttpRequestAvailable)
            {
                // extract the context
                HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);

                // upfill and build
                TrackPageView(httpContext, pageTitle, (httpContext != null && httpContext.Request != null && httpContext.Request.Url != null) ? httpContext.Request.Url.PathAndQuery : null, uaclient);
            }
            else
            {
                // google doesnt accept page views without a url
                throw new InvalidOperationException("Unable to find a valid HttpContext.Current, Google requires a valid URL to process page views.");
            }
        }

        /// <summary>
        /// Track a page view
        /// </summary>
        /// <param name="httpContext">Current http context to extract payload from</param>
        /// <param name="pageTitle">The title of the page / document</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackPageView(HttpContextBase httpContext, string pageTitle, UAClient uaclient = null)
        {
            // check for null context
            if (httpContext == null) throw new ArgumentNullException("httpContext", "Null HttpContextBase, Google requires a valid URL to process page views.");

            // upfill and build
            TrackPageView(httpContext, pageTitle, (httpContext != null && httpContext.Request != null && httpContext.Request.Url != null) ? httpContext.Request.Url.PathAndQuery : null, uaclient);
        }

        /// <summary>
        /// Track a page view
        /// </summary>
        /// <param name="pageTitle">The title of the page / document</param>
        /// <param name="pageUrl">The path portion of the page URL. Should begin with '/'</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackPageView(string pageTitle, string pageUrl, UAClient uaclient = null)
        {
            // check for empty page url
            if (string.IsNullOrWhiteSpace(pageUrl)) throw new ArgumentNullException("pageUrl", "Google requires a valid URL to process page views.");

            // extract the context
            HttpContextBase httpContext = (IsHttpRequestAvailable) ? new HttpContextWrapper(HttpContext.Current) : null;

            // upfill and build
            TrackPageView(httpContext, pageTitle, pageUrl, (httpContext != null && httpContext.Request != null && httpContext.Request.Url != null) ? httpContext.Request.Url.Host : null, uaclient);
        }

        /// <summary>
        /// Track a page view
        /// </summary>
        /// <param name="httpContext">Current http context to extract payload from</param>
        /// <param name="pageTitle">The title of the page / document</param>
        /// <param name="pageUrl">The path portion of the page URL. Should begin with '/'</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackPageView(HttpContextBase httpContext, string pageTitle, string pageUrl, UAClient uaclient = null)
        {
            // check for empty page url
            if (string.IsNullOrWhiteSpace(pageUrl)) throw new ArgumentNullException("pageUrl", "Google requires a valid URL to process page views.");

            // upfill and build
            TrackPageView(httpContext, pageTitle, pageUrl, (httpContext != null && httpContext.Request != null && httpContext.Request.Url != null) ? httpContext.Request.Url.Host : null, uaclient);
        }

        /// <summary>
        /// Track a page view
        /// </summary>
        /// <param name="pageTitle">The title of the page / document</param>
        /// <param name="pageUrl">The path portion of the page URL. Should begin with '/'</param>
        /// <param name="hostName">Specifies the hostname from which content was hosted</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackPageView(string pageTitle, string pageUrl, string hostName, UAClient uaclient = null)
        {
            // check for empty page url
            if (string.IsNullOrWhiteSpace(pageUrl)) throw new ArgumentNullException("pageUrl", "Google requires a valid URL to process page views.");

            // upfill and build
            TrackPageView((IsHttpRequestAvailable) ? new HttpContextWrapper(HttpContext.Current) : null, pageTitle, pageUrl, hostName, uaclient);
        }
        
        /// <summary>
        /// Track a page view
        /// </summary>
        /// <param name="httpContext">Current http context to extract payload from</param>
        /// <param name="pageTitle">The title of the page / document</param>
        /// <param name="pageUrl">The path portion of the page URL. Should begin with '/'</param>
        /// <param name="hostName">Specifies the hostname from which content was hosted</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackPageView(HttpContextBase httpContext, string pageTitle, string pageUrl, string hostName, UAClient uaclient = null)
        {
            // check for empty page url
            if (string.IsNullOrWhiteSpace(pageUrl)) throw new ArgumentNullException("pageUrl", "Google requires a valid URL to process page views.");

            // initialize the uaclient if null
            if (uaclient == null) uaclient = new UAClient();

            // build the payload
            StringBuilder data = BuildPayload(BuildBasePayload(httpContext, uaclient), BuildPageTrackPayload(pageTitle, pageUrl, hostName));

            // build the http request
            HttpWebRequest request = BuildRequest(data);

            // send sync and fail silently
            try
            {
                using (WebResponse response = request.GetResponse()) { }
            }
            catch { }
        }

        /// <summary>
        /// Async track a page view (non-blocking)
        /// </summary>
        /// <param name="uaclient">Client override object</param>
        public void TrackPageViewAsync(UAClient uaclient = null)
        {
            // make sure we don't need it passed in
            if (IsHttpRequestAvailable)
            {
                // upfill and build
                TrackPageViewAsync(new HttpContextWrapper(HttpContext.Current), null, uaclient);
            }
            else
            {
                // google doesnt accept page views without a url
                throw new InvalidOperationException("Unable to find a valid HttpContext.Current, Google requires a valid URL to process page views.");
            }
        }

        /// <summary>
        /// Async track a page view (non-blocking)
        /// </summary>
        /// <param name="httpContext">Current http context to extract payload from</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackPageViewAsync(HttpContextBase httpContext, UAClient uaclient = null)
        {
            // check for null context
            if (httpContext == null) throw new ArgumentNullException("httpContext", "Null HttpContextBase, Google requires a valid URL to process page views.");

            // upfill and build
            TrackPageViewAsync(httpContext, null, uaclient);
        }

        /// <summary>
        /// Async track a page view (non-blocking)
        /// </summary>
        /// <param name="pageTitle">The title of the page / document</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackPageViewAsync(string pageTitle, UAClient uaclient = null)
        {
            // make sure we don't need it passed in
            if (IsHttpRequestAvailable)
            {
                // extract the context
                HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);

                // upfill and build
                TrackPageViewAsync(httpContext, pageTitle, (httpContext != null && httpContext.Request != null && httpContext.Request.Url != null) ? httpContext.Request.Url.PathAndQuery : null, uaclient);
            }
            else
            {
                // google doesnt accept page views without a url
                throw new InvalidOperationException("Unable to find a valid HttpContext.Current, Google requires a valid URL to process page views.");
            }
        }

        /// <summary>
        /// Async track a page view (non-blocking)
        /// </summary>
        /// <param name="httpContext">Current http context to extract payload from</param>
        /// <param name="pageTitle">The title of the page / document</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackPageViewAsync(HttpContextBase httpContext, string pageTitle, UAClient uaclient = null)
        {
            // check for null context
            if (httpContext == null) throw new ArgumentNullException("httpContext", "Null HttpContextBase, Google requires a valid URL to process page views.");

            // upfill and build
            TrackPageViewAsync(httpContext, pageTitle, (httpContext != null && httpContext.Request != null && httpContext.Request.Url != null) ? httpContext.Request.Url.PathAndQuery : null, uaclient);
        }

        /// <summary>
        /// Async track a page view (non-blocking)
        /// </summary>
        /// <param name="pageTitle">The title of the page / document</param>
        /// <param name="pageUrl">The path portion of the page URL. Should begin with '/'</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackPageViewAsync(string pageTitle, string pageUrl, UAClient uaclient = null)
        {
            // check for empty page url
            if (string.IsNullOrWhiteSpace(pageUrl)) throw new ArgumentNullException("pageUrl", "Google requires a valid URL to process page views.");

            // extract the context
            HttpContextBase httpContext = (IsHttpRequestAvailable) ? new HttpContextWrapper(HttpContext.Current) : null;

            // upfill and build
            TrackPageViewAsync(httpContext, pageTitle, pageUrl, (httpContext != null && httpContext.Request != null && httpContext.Request.Url != null) ? httpContext.Request.Url.Host : null, uaclient);
        }

        /// <summary>
        /// Async track a page view (non-blocking)
        /// </summary>
        /// <param name="httpContext">Current http context to extract payload from</param>
        /// <param name="pageTitle">The title of the page / document</param>
        /// <param name="pageUrl">The path portion of the page URL. Should begin with '/'</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackPageViewAsync(HttpContextBase httpContext, string pageTitle, string pageUrl, UAClient uaclient = null)
        {
            // check for empty page url
            if (string.IsNullOrWhiteSpace(pageUrl)) throw new ArgumentNullException("pageUrl", "Google requires a valid URL to process page views.");

            // upfill and build
            TrackPageViewAsync(httpContext, pageTitle, pageUrl, (httpContext != null && httpContext.Request != null && httpContext.Request.Url != null) ? httpContext.Request.Url.Host : null, uaclient);
        }

        /// <summary>
        /// Async track a page view (non-blocking)
        /// </summary>
        /// <param name="pageTitle">The title of the page / document</param>
        /// <param name="pageUrl">The path portion of the page URL. Should begin with '/'</param>
        /// <param name="hostName">Specifies the hostname from which content was hosted</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackPageViewAsync(string pageTitle, string pageUrl, string hostName, UAClient uaclient = null)
        {
            // check for empty page url
            if (string.IsNullOrWhiteSpace(pageUrl)) throw new ArgumentNullException("pageUrl", "Google requires a valid URL to process page views.");

            // upfill and build
            TrackPageViewAsync((IsHttpRequestAvailable) ? new HttpContextWrapper(HttpContext.Current) : null, pageTitle, pageUrl, hostName, uaclient);
        }

        /// <summary>
        /// Async track a page view (non-blocking)
        /// </summary>
        /// <param name="httpContext">Current http context to extract payload from</param>
        /// <param name="pageTitle">The title of the page / document</param>
        /// <param name="pageUrl">The path portion of the page URL. Should begin with '/'</param>
        /// <param name="hostName">Specifies the hostname from which content was hosted</param>
        /// <param name="uaclient">Client override object</param>
        public void TrackPageViewAsync(HttpContextBase httpContext, string pageTitle, string pageUrl, string hostName, UAClient uaclient = null)
        {
            // check for empty page url
            if (string.IsNullOrWhiteSpace(pageUrl)) throw new ArgumentNullException("pageUrl", "Google requires a valid URL to process page views.");

            // initialize the uaclient if null
            if (uaclient == null) uaclient = new UAClient();

            // build the payload
            StringBuilder data = BuildPayload(BuildBasePayload(httpContext, uaclient), BuildPageTrackPayload(pageTitle, pageUrl, hostName));

            // build the http request
            HttpWebRequest request = BuildRequest(data);

            // send it async
            ProcessAsync(request);
        }

        /// <summary>
        /// Send the request async depending on the environment found
        /// </summary>
        /// <param name="request">the ga request to process</param>
        private void ProcessAsync(HttpWebRequest request)
        {
            
            // send async and fail silently
            bool queued_job = false;

            // check for an ASP.NET application
            if (HostingEnvironment.ApplicationHost != null)
            {
                // The HostingEnvironment.QueueBackgroundWorkItem method lets you schedule small background work items.
                // ASP.NET tracks these items and prevents IIS from abruptly terminating the worker process until all background work items have completed.
                try
                {
                    HostingEnvironment.QueueBackgroundWorkItem(ct =>
                    {
                        try
                        {
                            using (WebResponse response = request.GetResponse()) { }
                        }
                        catch { }
                    });

                    // set the flag
                    queued_job = true;
                }
                catch (InvalidOperationException)
                {
                    // override fail
                    queued_job = false;
                }
            }

            // check for either non-iis or queued fail
            if (!queued_job)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        using (WebResponse response = request.GetResponse()) { }
                    }
                    catch { }
                });
            }
        }

        /// <summary>
        /// build the simple request object
        /// </summary>
        /// <param name="data">serialized data for transmission</param>
        /// <returns>the request to use in either async or sync</returns>
        private HttpWebRequest BuildRequest(StringBuilder data)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}?{1}", ((useSSL) ? "https://ssl.google-analytics.com/collect" : "http://www.google-analytics.com/collect"), data));
            request.UserAgent = userAgent;
            return request;
        }

        /// <summary>
        /// builds the ga page track payload
        /// </summary>
        /// <param name="pageTitle">The title of the page / document</param>
        /// <param name="pageUrl">The path portion of the page URL. Should begin with '/'</param>
        /// <param name="hostName">Specifies the hostname from which content was hosted</param>
        /// <returns></returns>
        private Dictionary<string, string> BuildPageTrackPayload(string pageTitle, string pageUrl, string hostName)
        {
            // dictionary to make it look cleaner below
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            // page tracking parameters
            parameters.Add("t", "pageview");

            // hostname
            if (!string.IsNullOrWhiteSpace(hostName))
                parameters.Add("dh", hostName);

            // page url
            if (!string.IsNullOrWhiteSpace(pageUrl))
                parameters.Add("dp", pageUrl);

            // page title
            if (!string.IsNullOrWhiteSpace(pageTitle))
                parameters.Add("dt", pageTitle);

            // end
            return parameters;
        }

        /// <summary>
        /// builds the ga event track payload
        /// </summary>
        /// <param name="eventCategory">Specifies the event category. Must not be empty</param>
        /// <param name="eventAction">Specifies the event action. Must not be empty</param>
        /// <param name="eventLabel">Specifies the event label</param>
        /// <param name="eventValue">Specifies the event value. Values must be non-negative</param>
        /// <returns></returns>
        private Dictionary<string, string> BuildEventTrackPayload(string eventCategory, string eventAction, string eventLabel, int eventValue)
        {
            // dictionary to make it look cleaner below
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            // event tracking parameters
            parameters.Add("t", "event");

            // category
            if (!string.IsNullOrWhiteSpace(eventCategory))
                parameters.Add("ec", eventCategory);

            // action
            if (!string.IsNullOrWhiteSpace(eventAction))
                parameters.Add("ea", eventAction);

            // label
            if (!string.IsNullOrWhiteSpace(eventLabel))
                parameters.Add("el", eventLabel);

            // value, must be non-negative
            if (eventValue >= 0)
                parameters.Add("ev", eventValue.ToString());

            // end
            return parameters;
        }

        /// <summary>
        /// build the base payload for all requests regardless of type
        /// </summary>
        /// <param name="httpContext">the http context to extract data from</param>
        /// <param name="uaclient">the client to override data in payload</param>
        /// <returns></returns>
        private Dictionary<string, string> BuildBasePayload(HttpContextBase httpContext, UAClient uaclient)
        {
            // extract the request
            HttpRequestBase request = (httpContext != null) ? httpContext.Request : null;

            // dictionary to make it look cleaner below
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            // always required parameters
            parameters.Add("v", v);
            parameters.Add("tid", tid);
            parameters.Add("cid", uaclient.cid);

            // get anonymize value
            if (aip)
                parameters.Add("aip", "1");

            // user agent
            // user agent override first
            if (!string.IsNullOrWhiteSpace(uaclient.ua))
                parameters.Add("ua", uaclient.ua);

            // user agent extracted from the request
            else if (request != null && !string.IsNullOrWhiteSpace(request.UserAgent))
                parameters.Add("ua", request.UserAgent);

            // ip address
            // ip address override first
            if (!string.IsNullOrWhiteSpace(uaclient.uip))
                parameters.Add("uip", uaclient.uip);

            // ip extracted from the request, make sure this isnt a local request as well
            else if (request != null && !request.IsLocal && !string.IsNullOrWhiteSpace(request.UserHostAddress))
                parameters.Add("uip", request.UserHostAddress);

            // document encoding, skip default de=UTF-8
            if (request != null && request.ContentEncoding != null && !string.IsNullOrWhiteSpace(request.ContentEncoding.HeaderName) && request.ContentEncoding.HeaderName.ToUpper() != "UTF-8")
                parameters.Add("de", request.ContentEncoding.HeaderName.ToUpper());

            // user language
            if (request != null && request.UserLanguages != null && request.UserLanguages.Length > 0)
                parameters.Add("ul", string.Join(";", request.UserLanguages));

            // end
            return parameters;
        }

        /// <summary>
        /// combines the two payloads into a single http query string
        /// </summary>
        /// <param name="basePayload">request base data</param>
        /// <param name="actionPayload">the action specific data</param>
        /// <returns></returns>
        private StringBuilder BuildPayload(Dictionary<string, string> basePayload, Dictionary<string, string> actionPayload)
        {
            // string  builder
            StringBuilder data = new StringBuilder();

            // convert and encode the base payload
            data.Append(string.Join("&", basePayload.Select((x) => x.Key + "=" + Uri.EscapeDataString(x.Value))));

            // add the seperator
            data.Append('&');

            // convert and encode the action payload
            data.Append(string.Join("&", actionPayload.Select((x) => x.Key + "=" + Uri.EscapeDataString(x.Value))));

            // end
            return data;
        }
    }
}
