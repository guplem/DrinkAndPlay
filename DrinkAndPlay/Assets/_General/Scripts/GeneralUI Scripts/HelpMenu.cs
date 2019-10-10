using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class HelpMenu : MonoBehaviour
{
    [SerializeField] private Localizer textContainer;

    public void Setup(Section section)
    {
        textContainer.Localize(section.descriptionId);
    }
}
