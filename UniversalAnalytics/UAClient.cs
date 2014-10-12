using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Threading;
using System.Security.Cryptography;

namespace Echovoice.UniversalAnalytics
{
    /// <summary>
    /// User/Client class for Universal Analytics
    /// </summary>
    public class UAClient
    {
        /// <summary>
        /// Client ID
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid
        /// Required for all hit types.
        /// This anonymously identifies a particular user, device, or browser instance. For the web, this is generally stored as a first-party cookie with a two-year expiration. For mobile apps, this is randomly generated for each particular instance of an application install. The value of this field should be a random UUID (version 4) as described in http://www.ietf.org/rfc/rfc4122.txt
        /// </summary>
        public string cid
        {
            get
            {
                if (_cid == Guid.Empty)
                    _cid = Guid.NewGuid();

                return _cid.ToString();
            }
        }
        private Guid _cid = Guid.Empty;

        /// <summary>
        /// User ID
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#uid
        /// Optional.
        /// This is intended to be a known identifier for a user provided by the site owner/tracking library user. It may not itself be PII (personally identifiable information). The value should never be persisted in GA cookies or other Analytics provided storage.
        /// </summary>
        public string uid
        {
            get
            {
                return _uid;
            }
        }
        private string _uid = null;

        /// <summary>
        /// IP Override
        /// https://https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#uip
        /// Optional.
        /// The IP address of the user. This should be a valid IP address. It will always be anonymized just as though aip (anonymize IP) had been used.
        /// </summary>
        public string uip
        {
            get
            {
                if (_uip != null)
                    return _uip.ToString();
                return null;
            }
        }
        private IPAddress _uip = null;

        /// <summary>
        /// User Agent Override
        /// https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#ua
        /// Optional.
        /// The User Agent of the browser. Note that Google has libraries to identify real user agents. Hand crafting your own agent could break at any time.
        /// </summary>
        public string ua
        {
            get
            {
                return _ua;
            }
        }
        private string _ua = null;
            
        /// <summary>
        /// Initialize a client for our universal tracker, client id is automatically generated
        /// </summary>
        public UAClient() {  }

        /// <summary>
        /// Initialize a client for our universal tracker
        /// </summary>
        /// <param name="Client_ID">The value will be converted to a UUID (version 4) as described in RFC4122</param>
        public UAClient(string Client_ID)
        {
            // convert a string client id to a guid via MD5
            _cid = Guid.ParseExact(UUID(Client_ID), "N");
        }

        /// <summary>
        /// Initialize a client for our universal tracker
        /// </summary>
        /// <param name="Client_ID">The value will be converted to a UUID (version 4) as described in RFC4122</param>
        /// <param name="User_ID">This is intended to be a known identifier for a user provided by the site owner/tracking library user.</param>
        public UAClient(string Client_ID, string User_ID)
        {
            // convert a string client id to a guid via MD5
            _cid = Guid.ParseExact(UUID(Client_ID), "N");

            // set user id
            _uid = User_ID;
        }

        /// <summary>
        /// Initialize a client for our universal tracker
        /// </summary>
        /// <param name="Client_ID">The value of this field should be a random UUID (version 4) as described in RFC4122</param>
        public UAClient(Guid Client_ID)
        {
            // save the guid
            _cid = Client_ID;
        }

        /// <summary>
        /// Initialize a client for our universal tracker
        /// </summary>
        /// <param name="Client_ID">The value of this field should be a random UUID (version 4) as described in RFC4122</param>
        /// <param name="User_ID">This is intended to be a known identifier for a user provided by the site owner/tracking library user.</param>
        public UAClient(Guid Client_ID, string User_ID)
        {
            // save the guid
            _cid = Client_ID;

            // set user id
            _uid = User_ID;
        }

        /// <summary>
        /// Override the IP sent to GA
        /// </summary>
        /// <param name="IP_Override">The IP address of the user. This should be a valid IP address. It will always be anonymized just as though aip (anonymize IP) had been used.</param>
        public void IPOverride(IPAddress IP_Override)
        {
            // set property
            _uip = IP_Override;
        }

        /// <summary>
        /// Override the IP sent to GA
        /// </summary>
        /// <param name="IP_Override">The IP address of the user. This should be a valid IP address. It will always be anonymized just as though aip (anonymize IP) had been used.</param>
        public void IPOverride(string IP_Override)
        {
            // try to set the property
            IPAddress.TryParse(IP_Override, out _uip);
        }

        /// <summary>
        /// Override the User Agent sent to GA
        /// </summary>
        /// <param name="User_Agent_Override">The User Agent of the browser. Note that Google has libraries to identify real user agents. Hand crafting your own agent could break at any time.</param>
        public void UserAgentOverride(string User_Agent_Override)
        {
            // set property
            _ua = User_Agent_Override;
        }

        /// <summary>
        /// Override the User Agent sent to GA
        /// </summary>
        /// <param name="User_ID">This is intended to be a known identifier for a user provided by the site owner/tracking library user.</param>
        public void SetUserID(string User_ID)
        {
            // set user id
            _uid = User_ID;
        }

        /// <summary>
        /// convert the client id string into a 'uuid' (of sorts)
        /// </summary>
        /// <param name="input">client/session id for this user</param>
        /// <returns>valid uuid format for ga</returns>
        public static string UUID(string input)
        {
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] result = md5.ComputeHash(Encoding.Unicode.GetBytes(input));

                StringBuilder sb = new StringBuilder();
                for (int a = 0; a < result.Length; a++)
                {
                    sb.Append(result[a].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
