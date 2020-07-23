using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEditor;
using UnityEngine;

public static class Downloader
{
    private static readonly PendingDownloads pendingDownloads = new PendingDownloads();
    public static bool downloading
    {
        get => _downloading;
        set
        {
            _downloading = value;
            GameManager.instance.generalUi.downloading = downloading;
        }
    }
    private static bool _downloading = false;

    private class WebClientEx : WebClient
    {
        public WebClientEx(CookieContainer container)
        {
            this.container = container;
        }

        private readonly CookieContainer container = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest r = base.GetWebRequest(address);
            HttpWebRequest request = r as HttpWebRequest;
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
            HttpWebResponse response = r as HttpWebResponse;
            
            if (response == null) 
                return;
            
            CookieCollection cookies = response.Cookies;
            container.Add(cookies);
        }
    }

    private static void DownloadLocalizationFileAsCsv(LocalizationFile localizationFile)
    {
        Debug.Log($"Starting the download process for '{localizationFile}' as csv.");
        
        if (localizationFile == null)
        {
            Debug.LogError("Trying to download the localization file of a 'null' localizationFile");
            return;
        }

        /*
            INSTRUCTIONS:
            In your Google Spread, go to: File > Publish to the Web > Link > CSV
            You'll be given a link. Put that link into a WWW request and the text you get back will be your data in CSV form.
            // Example URL
            //string url = @"https://docs.google.com/spreadsheets/d/e/2PACX-1vQGs31fwKF9vuUg9uUOvgN8Jr7bVSQvDILQEMPk6xiKkzk3PDYosuOPMhd0FjrnKPzLkMA998tnZfGN/pub?output=csv"; //Published to the web
        */

        WebClientEx wc = new WebClientEx(new CookieContainer());
        wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:22.0) Gecko/20100101 Firefox/22.0");
        wc.Headers.Add("DNT", "1");
        wc.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
        wc.Headers.Add("Accept-Encoding", "deflate");
        wc.Headers.Add("Accept-Language", "en-US,en;q=0.5");
        wc.Headers.Add("Cache-Control", "no-cache");
        wc.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
        
        if (string.IsNullOrEmpty(localizationFile.localizationUrl))
        {
            Debug.LogError("The localization file for the localizationFile " + localizationFile  + " can not be downloaded because the 'LocalizationURL' is not valid.");
            return;
        }

        Uri uri = new Uri(localizationFile.localizationUrl);
        System.Threading.AutoResetEvent waiter = new System.Threading.AutoResetEvent (false);

        // Specify that the DownloadDataCallback method gets called
        // when the download completes.
        wc.DownloadDataCompleted += new DownloadDataCompletedEventHandler (DownloadDataCallback);
        // wc.DownloadDataAsync (uri, waiter);
        pendingDownloads.Add(localizationFile);
        wc.DownloadDataAsync (uri, localizationFile);

        // Block the main application thread. Real applications
        // can perform other tasks while waiting for the download to complete.
        // waiter.WaitOne ();
    }

    static void DownloadDataCallback(object sender, DownloadDataCompletedEventArgs e)
    {
        //System.Threading.AutoResetEvent waiter = (System.Threading.AutoResetEvent)e.UserState;
        LocalizationFile localizationFile = (LocalizationFile) e.UserState;
        try
        {
            // If the request was not canceled and did not throw
            // an exception, display the resource.
            if (!e.Cancelled && e.Error == null)
            {
                byte[] dt = (byte[])e.Result;
                if (dt.Length <= 0)
                {
                    Debug.LogError("The downloaded data for the localizationFile " + localizationFile  + " is empty.");
                }

                File.WriteAllBytes("Assets/_General/Resources/" + localizationFile + ".csv", dt);

                //To convert it to string...
                //var outputCSVdata = System.Text.Encoding.UTF8.GetString(dt ?? new byte[] { });

                pendingDownloads.Remove(localizationFile);
                Debug.Log($"'{localizationFile}' localization file has been downloaded successfully. Pending downloads: {pendingDownloads.Count}");
            }
        }
        finally
        {
            // Let the main application thread resume.
            // waiter.Set ();
        }
    }

    #if UNITY_EDITOR
    [MenuItem("Drink and Play/Localization files/Download all")]
    #endif
    public static bool DownloadAllLocalizationFilesAsCsv()
    {
        //Search all sections (including UI)
        LocalizationFile[] localizationFiles = GetAllLocalizationFiles();

        //Download every localizationFile (including UI)
        foreach (LocalizationFile localizationFile in localizationFiles)
            DownloadLocalizationFileAsCsv(localizationFile);
        
        #if UNITY_EDITOR
        AssetDatabase.Refresh();
        #endif
        

        Debug.Log("All Localization files have been started downloading.");
        return true;
    }
    
    #if UNITY_EDITOR
    [MenuItem("Drink and Play/Localization files/Download and check all")]
    #endif
    public static void DownloadAllLocalizationFilesAsCsvAndCheck()
    {
        if (DownloadAllLocalizationFilesAsCsv())
            LocalizationFilesChecker.CheckLocalizationFiles();
    }
    
    #if UNITY_EDITOR
    [MenuItem("Drink and Play/Localization files/Delete all")]
    public static void DeleteAllLocalizationFileAsCsv()
    {
        //Search all sections (including UI)
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(LocalizationFile).Name);
        LocalizationFile[] localizationFiles = new LocalizationFile[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            localizationFiles[i] = AssetDatabase.LoadAssetAtPath<LocalizationFile>(path);
        }

        //Download every localizationFile (including UI)
        foreach (LocalizationFile localizationFile in localizationFiles)
            File.Delete("Assets/_General/Resources/" + localizationFile + ".csv");

        AssetDatabase.Refresh();

        Debug.Log("All Localization files have been deleted.");
    }
    #endif
    
    public static LocalizationFile[] GetAllLocalizationFiles()
    {
        LocalizationFile[] localizationFiles = Resources.LoadAll<LocalizationFile>("LocalizationFiles - Models");
        
        return localizationFiles;
    }
    
    
}