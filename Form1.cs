using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Xml;
using System.IO;

namespace howto_weather_forecast
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Enter your API key here.
        // Get an API key by making a free account at:
        //      http://home.openweathermap.org/users/sign_in
        private const string API_KEY = "e1053d1b85007fa1f764226b8401b627";

        // Query URLs. Replace @LOC@ with the location.
        private const string CurrentUrl =
            "http://api.openweathermap.org/data/2.5/weather?" +
            "q=@LOC@&mode=xml&units=imperial&APPID=" + API_KEY;
        private const string ForecastUrl =
            "http://api.openweathermap.org/data/2.5/forecast?" +
            "q=@LOC@&mode=xml&units=imperial&APPID=" + API_KEY;

        // Get current conditions.
        private void btnConditions_Click(object sender, EventArgs e)
        {
            // Compose the query URL.
            string url = CurrentUrl.Replace("@LOC@", txtLocation.Text);
            txtXml.Text = GetFormattedXml(url);
        }

        // Get a forecast.
        private void btnForecast_Click(object sender, EventArgs e)
        {
            // Compose the query URL.
            string url = ForecastUrl.Replace("@LOC@", txtLocation.Text);
            txtXml.Text = GetFormattedXml(url);
        }

        // Return the XML result of the URL.
        private string GetFormattedXml(string url)
        {
            // Create a web client.
            using (WebClient client = new WebClient())
            {
                // Get the response string from the URL.
                string xml = client.DownloadString(url);

                // Load the response into an XML document.
                XmlDocument xml_document = new XmlDocument();
                xml_document.LoadXml(xml);

                string GetTemp=xml_document .DocumentElement .SelectSingleNode("temperature").Attributes["value"].Value;
                double temp = double.Parse (GetTemp)-32 ;
                temp = temp * 5 / 9;
                label3.Text = Convert.ToInt64(temp) + "°C";

                label4.Text = xml_document.DocumentElement.SelectSingleNode("city").Attributes["name"].Value;

                string ImId = xml_document.DocumentElement.SelectSingleNode("weather").Attributes["icon"].Value;
                string Url = "http://openweathermap.org/img/w/" +ImId+".png";
                

                pictureBox1.ImageLocation = Url ;



                XmlNodeList xnList = xml_document.SelectNodes("/Names/Name[@type='M']");
                // Format the XML.
                using (StringWriter string_writer = new StringWriter())
                {
                    XmlTextWriter xml_text_writer = new XmlTextWriter(string_writer);
                    xml_text_writer.Formatting = Formatting.Indented;
                    xml_document.WriteTo(xml_text_writer);

                    // Return the result.
                    return string_writer.ToString();


                    

                    
                }
                
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label3.Text = "";
            label4.Text = "";

        }
    }
}
