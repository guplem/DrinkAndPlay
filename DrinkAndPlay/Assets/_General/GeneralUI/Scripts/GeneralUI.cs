using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralUI : MonoBehaviour
{
    [Header("Top Bar")]
    [SerializeField] public GameObject topBar;
    [SerializeField] public GameObject backButton;
    [SerializeField] public GameObject sectionTitle;
    [SerializeField] public GameObject configButton;

    [Header("Bottom Bar")]
    [SerializeField] public GameObject bottomBar;
    [SerializeField] public GameObject likeButton;
    [SerializeField] public GameObject addButton;
    [SerializeField] public GameObject shareButton;

    public void SetupFor(Section section)
    {
        topBar.SetActive(section.topBar);
        backButton.SetActive(section.backButton);
        sectionTitle.SetActive(section.sectionTitle);
        configButton.SetActive(section.configButton);

        bottomBar.SetActive(section.bottomBar);
        likeButton.SetActive(section.likeButton);
        addButton.SetActive(section.addButton);
        shareButton.SetActive(section.shareButton);

        if (section.sectionTitle)
            sectionTitle.GetComponent<Localizer>().Localize(section.nameId);

    }

    public void ClickBackButton()
    {
        Debug.Log("Clicked 'Back' button.");
    }

    public void ClickConfigurationButton()
    {
        Debug.Log("Clicked 'Configuration' button.");
    }

    public void ClickLikeButton()
    {
        Debug.Log("Clicked 'Like' button.");
    }

    public void ClickAddButton()
    {
        Debug.Log("Clicked 'Add' button.");
    }

    public void ClickShareButton()
    {
        Debug.Log("Clicked 'Share' button.");
    }

    

}
