using System;
using System.Xml;

namespace OVD.API.Helpers
{
    public class Calculator
    {

        private const string CONFIG_FILE_LOC = "./ConfigurationFiles/ConnectionConfiguration.xml";


        /// <summary>
        /// Gets the next ip.
        /// </summary>
        /// <returns>The next ip.</returns>
        public string GetNextIp()
        {
            return SetCurrentConfigurationIp(CalculateNextIp());
        }


        /// <summary>
        /// Calculates the next ip given an ip address.
        /// </summary>
        /// <returns>The next ip.</returns>
        private string CalculateNextIp()
        {
            string[] i = GetCurrentConfigurationIp().Split('.');

            long n = Convert.ToInt64(i[0]) * (long)Math.Pow(256, 3)
                    + Convert.ToInt64(i[1]) * (long)Math.Pow(256, 2)
                    + Convert.ToInt64(i[2]) * 256
                    + Convert.ToInt64(i[3]);

            n++;
            n = n % (long)Math.Pow(256, 4);

            return string.Format("{0}.{1}.{2}.{3}", n / (int)Math.Pow(256, 3), 
                (n % (int)Math.Pow(256, 3)) / (int)Math.Pow(256, 2), 
                (n % (int)Math.Pow(256, 2)) / 256, n % 256);
        }


        /// <summary>
        /// Gets the current highest ip in the configuration file.
        /// </summary>
        /// <returns>The current configuration ip.</returns>
        private string GetCurrentConfigurationIp()
        {
            //Read in the xml connection configuration file
            XmlDocument config = new XmlDocument();
            config.Load(CONFIG_FILE_LOC);

            XmlNodeList elemList = config.GetElementsByTagName("current-ip");
            return elemList[0].InnerXml;
        }


        /// <summary>
        /// Sets the current highest ip in the configuration file.
        /// </summary>
        /// <returns>The current configuration ip.</returns>
        /// <param name="newIp">New ip.</param>
        private string SetCurrentConfigurationIp(string newIp)
        {
            //Read in the xml connection configuration file
            XmlDocument config = new XmlDocument();
            config.Load(CONFIG_FILE_LOC);
            config.SelectSingleNode("/connection-configuration/current-ip").InnerText = newIp;
            config.Save(CONFIG_FILE_LOC);
            return newIp;
        }
    }
}
