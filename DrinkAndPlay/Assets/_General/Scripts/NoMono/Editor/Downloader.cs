﻿ using System;
using System.IO;
using System.Net;
 using System.Net.Cache;
 using UnityEditor;
using UnityEngine;

class Downloader
{
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

    private static bool DownloadLocalizationFileAsCsv(LocalizationFile localizationFile)
    {
        Debug.Log("Downloading " + localizationFile + " as csv.");
        
        if (localizationFile == null)
        {
            Debug.LogError("Trying to download the localization file of a 'null' localizationFile");
            return false;
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
            return false;
        }
        
        byte[] dt = wc.DownloadData(localizationFile.localizationUrl);

        if (dt.Length <= 0)
        {
            Debug.LogError("The downloaded data for the localizationFile " + localizationFile  + " is empty.");
            return false;
        }

        File.WriteAllBytes("Assets/_General/Resources/" + localizationFile + ".csv", dt);

        //To convert it to string...
        //var outputCSVdata = System.Text.Encoding.UTF8.GetString(dt ?? new byte[] { });

        Debug.Log(localizationFile + " localization file has been downloaded successfully.");
        
        return true;
    }

    [MenuItem("Drink and Play/Localization files/Download all")]
    public static bool DownloadAllLocalizationFilesAsCsv()
    {
        //Search all sections (including UI)
        LocalizationFile[] localizationFiles = GetAllLocalizationFiles();

        //Download every localizationFile (including UI)
        foreach (LocalizationFile localizationFile in localizationFiles)
            if (!DownloadLocalizationFileAsCsv(localizationFile))
                return false;
        
        AssetDatabase.Refresh();

        Debug.Log("All Localization files have been downloaded successfully.");
        return true;
    }
    
    [MenuItem("Drink and Play/Localization files/Download and check all")]
    public static void DownloadAllLocalizationFilesAsCsvAndCheck()
    {
        if (DownloadAllLocalizationFilesAsCsv())
            LocalizationFilesChecker.CheckLocalizationFiles();
    }
    
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
    
    public static LocalizationFile[] GetAllLocalizationFiles()
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(LocalizationFile).Name);
        LocalizationFile[] localizationFiles = new LocalizationFile[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            localizationFiles[i] = AssetDatabase.LoadAssetAtPath<LocalizationFile>(path);
        }

        return localizationFiles;
    }
    
    
}