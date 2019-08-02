using System;
using System.IO;
using System.Net;
using UnityEditor;
using UnityEngine;

class Downloader
{
    public class WebClientEx : WebClient
    {
        public WebClientEx(CookieContainer container)
        {
            this.container = container;
        }

        private readonly CookieContainer container = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest r = base.GetWebRequest(address);
            var request = r as HttpWebRequest;
            if (request != null)
            {
                request.CookieContainer = container;
            }
            return r;
        }

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            WebResponse response = base.GetWebResponse(request, result);
            ReadCookies(response);
            return response;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            ReadCookies(response);
            return response;
        }

        private void ReadCookies(WebResponse r)
        {
            var response = r as HttpWebResponse;
            if (response != null)
            {
                CookieCollection cookies = response.Cookies;
                container.Add(cookies);
            }
        }
    }

    [MenuItem("Drink and Play/Download localization file")]
    public static void DownloadLocalizationFileAsCSV()
    {
        /*
            INSTRUCTIONS:
            In your Google Spread, go to: File > Publish to the Web > Link > CSV
            You'll be given a link. Put that link into a WWW request and the text you get back will be your data in CSV form.
        */

        string url = @"https://docs.google.com/spreadsheets/d/e/2PACX-1vQGs31fwKF9vuUg9uUOvgN8Jr7bVSQvDILQEMPk6xiKkzk3PDYosuOPMhd0FjrnKPzLkMA998tnZfGN/pub?output=csv"; //Published to the web

        WebClientEx wc = new WebClientEx(new CookieContainer());
        wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:22.0) Gecko/20100101 Firefox/22.0");
        wc.Headers.Add("DNT", "1");
        wc.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
        wc.Headers.Add("Accept-Encoding", "deflate");
        wc.Headers.Add("Accept-Language", "en-US,en;q=0.5");

        byte[] dt = wc.DownloadData(url);
        File.WriteAllBytes("Assets/Resources/LocalizationFile.csv", dt);

        //to convert it to string
        //var outputCSVdata = System.Text.Encoding.UTF8.GetString(dt ?? new byte[] { });

        Debug.Log("Localization file downloaded.");
    }
}