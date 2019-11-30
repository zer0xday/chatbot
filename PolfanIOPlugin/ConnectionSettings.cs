using System;
using System.Collections.Generic;
using System.Text;

namespace PolfanIOPlugin
{
    public class ConnectionSettings
    {
        public string Nick { get; set; }
        public string Password { get; set; }
        public string UriString { get; set; }
        public Uri Uri
        {
            get
            {
                return new Uri(UriString);
            }
        }
    }
}
