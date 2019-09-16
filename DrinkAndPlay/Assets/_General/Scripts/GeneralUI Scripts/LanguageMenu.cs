using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LanguageMenu : MonoBehaviour
{
    [SerializeField] private Transform contents;
    [SerializeField] private GameObject languagePrefab;
    [SerializeField] private Language[] languages;

    private void Start()
    {
        List<Language> languageList = languages.ToList();
        languageList.Sort();

        foreach (Language lang in languageList)
        {
            GameObject langButton = Instantiate(languagePrefab, contents);
            langButton.GetComponent<LanguageButton>().Setup(lang);
            langButton.transform.SetSiblingIndex(1);
        }
    }
}
