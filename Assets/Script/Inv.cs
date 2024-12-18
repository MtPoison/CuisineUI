using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using static Inv;




public class Inv : MonoBehaviour
{

    [System.Serializable]
    public class NestedList
    {
        public List<ItemData> innerList;
    }

    [SerializeField] private Button buttonPrefab;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private Dictionary<int, ItemData> items;
    [SerializeField] private GameObject buttonPrefabItem;
    [SerializeField] private GameObject containerPrefab;
    [SerializeField] private Transform parentTransformItem;
    [SerializeField] private List<NestedList> outerList;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameManger game;
    [SerializeField] private GameObject text;

    private List<Button> buttonList = new List<Button>();
    private List<GameObject> itemsAliment = new List<GameObject>();

    void Start()
    {
        GenerateUI();
        for (int i = 0; i < outerList.Count; i++)
        {
            CreateButton($"Button {i + 1}", i);
        }
    }
    

    public void CreateButton(string buttonText, int i)
    {
        Button newButton = Instantiate(buttonPrefab, parentTransform);

        Text buttonLabel = newButton.GetComponentInChildren<Text>();
        if (buttonLabel != null)
        {
            buttonLabel.text = buttonText;
        }
        ShelfButton shel = newButton.GetComponent<ShelfButton>();
        shel.SetShel(itemsAliment[i]);
        shel.SetTitleText(titleText);
        shel.SetDescription(descriptionText);
        shel.SetGame(game);
        buttonList.Add(newButton);
    }

    void GenerateUI()
    {
        for (int i = 0; i < outerList.Count; i++)
        {
            NestedList nestedList = outerList[i];
            GameObject container = Instantiate(containerPrefab, parentTransformItem);
            container.SetActive(false);
            for (int j = 0; j < nestedList.innerList.Count; j++)
            {
                GameObject buttonGO = Instantiate(buttonPrefabItem, container.transform);
                buttonGO.name = $"Button_{i}_{j}";
                

                AlimentSlot alimentSlot = buttonGO.GetComponent<AlimentSlot>();
                alimentSlot.SetTitleText(titleText);
                alimentSlot.SetDescription(descriptionText);
                alimentSlot.SetText(text);
                alimentSlot.SetGame(game);
                if (alimentSlot != null)
                {
                    ItemData itemData = nestedList.innerList[j];
                    alimentSlot.SetAlimentData(itemData);
                    
                }
                else
                {
                    Debug.LogWarning($"AlimentSlot script is missing on {buttonGO.name}");
                }
            }
            itemsAliment.Add(container);
        }
    }

    GameObject CreateContainer(string name, Transform parent)
    {
        GameObject container = new GameObject(name);
        container.transform.SetParent(parent);


        RectTransform rectTransform = container.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(240,210);
        rectTransform.localPosition = new Vector3(120, -305, 0);
        rectTransform.localScale = Vector3.one;

        GridLayoutGroup gridLayout = container.AddComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(100, 100);
        gridLayout.spacing = new Vector2(10, 10);
        gridLayout.padding = new RectOffset(30, 0, 0, 0);

        ContentSizeFitter fitter = container.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        return container;
    }
}
