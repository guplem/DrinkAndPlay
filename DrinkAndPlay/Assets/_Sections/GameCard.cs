using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCard : MonoBehaviour
{
    [SerializeField] private Localizer sentenceText;
    [SerializeField] public ImageSwitcher likeButton;
    [SerializeField] private TextMeshProUGUI author;


    public void Display(TextInTurnsGame textInCard)
    {
        sentenceText.Localize(textInCard.localizedText.id, textInCard.localizationFile);
        author.text = textInCard.localizedText.author;
    }
}
