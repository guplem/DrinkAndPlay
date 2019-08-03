using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationManager
{
    public string language;
    private SavesManager savesManager;

    public ConfigurationManager(SavesManager savesManager)
    {
        this.savesManager = savesManager;
    }


}
