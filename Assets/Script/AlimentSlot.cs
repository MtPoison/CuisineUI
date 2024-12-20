using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class AlimentSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private ItemData alimentData;
    [SerializeField] private GameObject text;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] GameManger gameManger;
    private GameObject item;

    private void Start()
    {
        Button button = GetComponent<Button>();
        Sprite newSprite = Sprite.Create(
            alimentData.img,
            new Rect(0, 0, alimentData.img.width, alimentData.img.height),
            new Vector2(0.5f, 0.5f)
            
        );
        button.image.sprite = newSprite;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (titleText != null && descriptionText != null)
        {
            
            titleText.text = alimentData.titre;
            descriptionText.text = alimentData.description;
            gameManger.SetItem(alimentData.prefab);
            text.SetActive(true);
            
        }
        else
        {
            Debug.LogWarning("TitleText ou DescriptionText n'est pas assigné dans l'inspecteur !");
        }
    }

    public void SetAlimentData(ItemData data)
    {
        alimentData = data;

        if (titleText != null)
            titleText.text = data.titre;

        if (descriptionText != null)
            descriptionText.text = data.description;
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
        this.gameManger = game;
    }

    public void SetText(GameObject _text) 
    {
        text = _text;
    }

    public void setNullItem()
    {
        item = null;
    }

    public GameObject getItem() { return item; }
}

