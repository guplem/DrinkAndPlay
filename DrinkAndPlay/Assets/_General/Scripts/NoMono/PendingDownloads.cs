using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendingDownloads : List<LocalizationFile>
{
    
    public new void Add(LocalizationFile item)
    {
        base.Add(item);
        Downloader.downloading = this.Count > 0;
    }
    
    public new void Remove(LocalizationFile item)
    {
        base.Remove(item);
        Downloader.downloading = this.Count > 0;
    }
}
