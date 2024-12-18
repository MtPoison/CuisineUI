using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;




public class Inv : MonoBehaviour
{

    [System.Serializable]
    public class NestedList
    {
        public List<ItemData> innerList;
        public void Add(ItemData item) { innerList.Add(item); }
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
    [SerializeField] private Player player;

    private bool update = false;

    private List<Button> buttonList = new List<Button>();
    private List<GameObject> itemsAliment = new List<GameObject>();
    [SerializeField] private List<ItemData> totalItem = new List<ItemData>();

    void Start()
    {
        GenerateUI();
        CreateButton();
    }


    public void CreateButton()
    {
        ClearButtons();
        for (int i = 0; i < outerList.Count; i++)
        {
            Button newButton = Instantiate(buttonPrefab, parentTransform);

            Text buttonLabel = newButton.GetComponentInChildren<Text>();
            if (buttonLabel != null)
            {
                buttonLabel.text = $"bouton_{i}";
            }
            ShelfButton shel = newButton.GetComponent<ShelfButton>();
            shel.SetShel(itemsAliment[i]);
            shel.SetTitleText(titleText);
            shel.SetDescription(descriptionText);
            shel.SetGame(game);
            buttonList.Add(newButton);
        }
    }

    private void Update()
    {
        if (update)
        {
            GenerateUI();
            CreateButton();
            update = false;
        }
    }

    void GenerateUI()
    {
        ClearUI();
        for (int i = 0; i < outerList.Count; i++)
        {
            NestedList nestedList = outerList[i];
            GameObject container = Instantiate(containerPrefab, parentTransformItem);

            foreach (ItemData itemData in nestedList.innerList)
            {
                GameObject buttonGO = Instantiate(buttonPrefabItem, container.transform);
                buttonGO.name = $"Button_{i}_{nestedList.innerList.IndexOf(itemData)}";
                print(buttonGO.name);
                if (!HasDigitInName(buttonGO.name))
                {
                    print("okk");
                    buttonGO.SetActive(false);
                }

                AlimentSlot alimentSlot = buttonGO.GetComponent<AlimentSlot>();
                if (alimentSlot != null)
                {
                    alimentSlot.SetTitleText(titleText);
                    alimentSlot.SetDescription(descriptionText);
                    alimentSlot.SetText(text);
                    alimentSlot.SetGame(game);
                    alimentSlot.SetAlimentData(itemData);
                }
                else
                {
                    Debug.LogWarning($"AlimentSlot script is missing on {buttonGO.name}");
                }
            }
            itemsAliment.Add(container);
            foreach (Transform child in container.transform)
            {
                if (!HasDigitInName(child.name))
                {
                    print($"Child '{child.name}' does not contain a digit. Removing...");
                    Destroy(child.gameObject);
                }
            }
        }
    }

    bool HasDigitInName(string name)
    {
        foreach (char c in name)
        {
            if (char.IsDigit(c))
            {
                return true;
            }
        }
        return false;
    }


    void ClearUI()
    {
        foreach (GameObject container in itemsAliment)
        {
            Destroy(container);
        }
        itemsAliment.Clear();
    }

    public void ClearButtons()
    {
        foreach (Button button in buttonList)
        {
            Destroy(button.gameObject);
        }
        buttonList.Clear();
    }



    public void Drop(GameObject data, string hand)
    {
        if (player.Verrify(hand))
        {
            print(outerList.Count);
            for (int i = 0; i < totalItem.Count; i++)
            {
                /*print(totalItem[i].prefab);
                print(data);*/
                if (data.name == totalItem[i].prefab.name)
                {
                    int rand = Random.Range(0, outerList.Count);

                    outerList[rand].Add(totalItem[i]);
                    player.Remove(hand);
                    update = true;
                    return;
                }
                /*else
                {
                    print("this Object can't store");
                }*/
            }
        }
    }
}
