using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;

public class Inv : MonoBehaviour
{
    [System.Serializable]
    public class NestedList
    {
        public List<ItemData> innerList = new List<ItemData>();
        public void Add(ItemData item) { innerList.Add(item); }
    }

    [SerializeField] private Button buttonPrefab;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private GameObject buttonPrefabItem;
    [SerializeField] private GameObject containerPrefab;
    [SerializeField] private Transform parentTransformItem;
    [SerializeField] private List<NestedList> outerList;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameManger game;
    [SerializeField] private GameObject text;
    [SerializeField] private Player player;
    [SerializeField] private bool random = false;
    private bool update = false;

    private List<Button> buttonList = new List<Button>();
    private List<GameObject> itemsAliment = new List<GameObject>();
    [SerializeField] private List<ItemData> totalItem = new List<ItemData>();
    private List<NestedList> tmpItems = new List<NestedList>();
    private bool isOpen = false;

    void Start()
    {
        GenerateUI(outerList);
        CreateButton(outerList);
        isOpen = true;
        UpdateText();
    }

    public void CreateButton(List<NestedList> _items)
    {
        ClearButtons();
        for (int i = 0; i < _items.Count; i++)
        {
            Button newButton = Instantiate(buttonPrefab, parentTransform);
            TextMeshProUGUI buttonLabel = newButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonLabel != null)
            {
                buttonLabel.text = $"shel {i+1}";
            }
            ShelfButton shel = newButton.GetComponent<ShelfButton>();
            if (shel != null)
            {
                shel.SetShel(itemsAliment[i]);
                shel.SetTitleText(titleText);
                shel.SetDescription(descriptionText);
                shel.SetGame(game);
            }
            buttonList.Add(newButton);
        }
    }

    private void Update()
    {
        if (random)
        {
            if (isOpen)
            {
                UpdateText();
                RandomList();
                isOpen = false;
            }
        }
        else
        {
            if (isOpen)
            {
                NormalList();
                isOpen = false;
            }
        }
    }

    void NormalList()
    {
        UpdateText();
        GenerateUI(outerList);
        CreateButton(outerList);
    }

    void GenerateUI(List<NestedList> _itmes)
    {
        ClearUI();
        for (int i = 0; i < _itmes.Count; i++)
        {
            NestedList nestedList = _itmes[i];
            GameObject container = Instantiate(containerPrefab, parentTransformItem);

            foreach (ItemData itemData in nestedList.innerList)
            {
                GameObject buttonGO = Instantiate(buttonPrefabItem, container.transform);
                buttonGO.name = $"Button_{i}_{nestedList.innerList.IndexOf(itemData)}";
                if (!HasDigitInName(buttonGO.name))
                {
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
                    Destroy(child.gameObject);
                }
            }
        }
    }

    public void SetRandom(bool _bool)
    {
        random = _bool;
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
            for (int i = 0; i < totalItem.Count; i++)
            {
                if (data.name == totalItem[i].prefab.name)
                {
                    int rand = Random.Range(0, outerList.Count);
                    outerList[rand].Add(totalItem[i]);
                    player.Remove(hand);
                    update = true;
                    return;
                }
            }
        }
    }

    private void RandomList()
    {
        tmpItems.Clear();
        int randCount = Random.Range(1, 4);
        for (int i = 0; i < randCount; i++)
        {
            NestedList nested = new NestedList();
            int randSize = Random.Range(1, 8);
            for (int j = 0; j < randSize; j++)
            {
                int rand = Random.Range(0, totalItem.Count);
                nested.Add(totalItem[rand]);
            }
            tmpItems.Add(nested);
        }
        GenerateUI(tmpItems);
        CreateButton(tmpItems);
    }

    public void Take(string hand, Vector3 vec)
    {
        GameObject item = Instantiate(game.GetItem(), player.transform);
        if (item.name.Contains("(Clone)"))
        {
            item.name = item.name.Replace("(Clone)", "").Trim();
            Debug.Log($"Le GameObject a été renommé en : {item.name}");
        }
        player.AddHand(item, hand, vec);
    }

    public void SetOpen(bool _bool)
    {
        isOpen = _bool;
    }

    public void UpdateText()
    {
        titleText.text = "";
        descriptionText.text = "";
    }
}
