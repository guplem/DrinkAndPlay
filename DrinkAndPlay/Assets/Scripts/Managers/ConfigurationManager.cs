using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationManager
{
    public string language { get { return "en-us"; } set { Debug.LogWarning("Set language not implemented yet"); }  }
    private SavesManager savesManager;

    public ConfigurationManager(SavesManager savesManager)
    {
        this.savesManager = savesManager;
    }


}
