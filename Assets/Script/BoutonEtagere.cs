using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShelfButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject shelfToActivate;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameManger game;
    private static GameObject currentlyActiveShelf;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentlyActiveShelf != null)
        {
            currentlyActiveShelf.SetActive(false);
            titleText.text = "";
            descriptionText.text = "";
            game.SetItemNull();
        }

        if (shelfToActivate != null)
        {
            shelfToActivate.SetActive(true);
            currentlyActiveShelf = shelfToActivate;
        }
    }

    public void SetShel(GameObject data)
    {
        shelfToActivate = data;
    }
    public void SetTitleText(TextMeshProUGUI _titleText)
    {
        titleText = _titleText;
    }

    public void SetDescription(TextMeshProUGUI _des)
    {
        descriptionText = _des;
    }

    public void SetGame(GameManger game)
    { 
        this.game = game; 
    }

}
