using System;
using System.Net;
using UnityEditor;
using UnityEngine;

class downloadCSV
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

    [MenuItem("Drink and Play/DownloadCSV")]
    public static void DownloadCSV()
    {
        /*
            1. Your Google SpreadSheet document must be set to 'Anyone with the link' can view it
             
            2. To get URL press SHARE (top right corner) on Google SpreeadSheet and copy "Link to share".
              
            3. Now add "&output=csv" parameter to this link
             
            4. Your link will look like:
                
            https://docs.google.com/spreadsheet/ccc?key=1234abcd1234abcd1234abcd1234abcd1234abcd1234&usp=sharing&output=csv
        */

        //string url = @"https://docs.google.com/spreadsheet/ccc?key=1234abcd1234abcd1234abcd1234abcd1234abcd1234&usp=sharing&output=csv"; // REPLACE THIS WITH YOUR URL --> This is old. the new ones are a bit different

        string url = @"https://docs.google.com/spreadsheets/d/1GgTumAPMS6VVR7dW1MsNItz9XamerYLpDXPInQD3_WQ/edit?usp=sharing&output=csv";

        WebClientEx wc = new WebClientEx(new CookieContainer());
        wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:22.0) Gecko/20100101 Firefox/22.0");
        wc.Headers.Add("DNT", "1");
        wc.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
        wc.Headers.Add("Accept-Encoding", "deflate");
        wc.Headers.Add("Accept-Language", "en-US,en;q=0.5");

        //var outputCSVdata = wc.DownloadString(url);

        byte[] dt = wc.DownloadData(url);
        var outputCSVdata = System.Text.Encoding.UTF8.GetString(dt ?? new byte[] { });

        //Console.Write(outputCSVdata);
        Debug.Log("CSV Output: " + outputCSVdata);
    }
}