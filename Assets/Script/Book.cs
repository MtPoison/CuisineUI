using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO; // Importation correcte de System.IO

public class Book : MonoBehaviour
{
    public string saveFolder = "Assets/Scriptable";

    private bool cast = true;

    [SerializeField] List<Recette> recettes;
    [SerializeField] PlayerControl playerControl;
    [SerializeField] GameManger game;

    [Header("Input Field")]
    [SerializeField] public TMP_InputField titleInputField;
    [SerializeField] public TMP_InputField descriptionInputField;
    [SerializeField] public TMP_InputField authorInputField;

    [Header("Canvas")]
    [SerializeField] GameObject canvasPlayer;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject canvasPopUp;
    [SerializeField] GameObject canvasAdd;

    [Header("TextMeshPro")]
    [Tooltip("TextMeshPro pour afficher le nom de la recette")]
    [SerializeField] private TMP_Text nomRecetteText;
    [SerializeField] private TMP_Text nomRecetteTextPop;

    [Tooltip("TextMeshPro pour afficher les ingrédients")]
    [SerializeField] private TMP_Text ingredientsText;
    [SerializeField] private TMP_Text ingredientsTextPop;

    [Tooltip("TextMeshPro pour afficher les instructions")]
    [SerializeField] private TMP_Text preparationText;
    [SerializeField] private TMP_Text preparationTextPop;

    private int indexActuel = 0;

    private void Start()
    {
        LoadRecettes();
        AfficherRecette(indexActuel, nomRecetteText, ingredientsText, preparationText);
    }

    public void AfficherRecette(int index, TMP_Text nomRecette, TMP_Text ingredients, TMP_Text preparation)
    {
        if (recettes == null || recettes.Count == 0)
        {
            Debug.LogWarning("La liste des recettes est vide !");
            return;
        }

        if (index < 0 || index >= recettes.Count)
        {
            Debug.LogWarning("Index de recette invalide !");
            return;
        }

        Recette recetteActuelle = recettes[index];

        nomRecette.text = recetteActuelle.nomDeLaRecette;
        string[] mots = recetteActuelle.ingredients.Split(' ');
        ingredients.text = string.Join("\n", mots);
        preparation.text = recetteActuelle.preparation;
    }

    private void Update()
    {
        SelectRecette();
    }

    public void RecetteSuivante()
    {
        indexActuel = (indexActuel + 1) % recettes.Count;
        AfficherRecette(indexActuel, nomRecetteText, ingredientsText, preparationText);
    }

    public void RecettePrecedente()
    {
        indexActuel = (indexActuel - 1 + recettes.Count) % recettes.Count;
        AfficherRecette(indexActuel, nomRecetteText, ingredientsText, preparationText);
    }

    private void SelectRecette()
    {
        if (cast)
        {
            int layerMask = 1 << LayerMask.NameToLayer("Tv");
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = UnityEngine.Input.mousePosition
                };

                List<RaycastResult> results = new List<RaycastResult>();

                GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
                if (raycaster != null)
                {
                    raycaster.Raycast(pointerData, results);

                    bool isButtonClicked = results.Exists(result => result.gameObject.GetComponent<Button>() != null);
                    bool isLayerCorrect = results.Exists(result => result.gameObject.layer == layerMask);

                    if (!isButtonClicked && !isLayerCorrect && results.Count > 0)
                    {
                        playerControl.IsReading();
                        canvasPlayer.SetActive(false);
                        canvasPopUp.SetActive(true);
                        game.SetCanvasUp(true);
                        AfficherRecette(indexActuel, nomRecetteTextPop, ingredientsTextPop, preparationTextPop);
                    }
                }
            }
        }
    }

    public void RemoveRecette()
    {
        recettes.RemoveAt(indexActuel);
        PlayerPrefs.DeleteKey(saveFolder + "/Recette" + indexActuel + "_nom");
        PlayerPrefs.DeleteKey(saveFolder + "/Recette" + indexActuel + "_ingredients");
        PlayerPrefs.DeleteKey(saveFolder + "/Recette" + indexActuel + "_preparation");

        SaveRecettes();
        UpdateCanvas();
    }


    public void IsRecast(bool active)
    {
        cast = active;
    }

    public void AddRecette()
    {
        cast = false;
        Recette newTextData = ScriptableObject.CreateInstance<Recette>();

        newTextData.nomDeLaRecette = titleInputField.text;
        newTextData.ingredients = descriptionInputField.text;
        newTextData.preparation = authorInputField.text;

        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }
        recettes.Add(newTextData);
        SaveRecettes();

        ClearInputFields();
    }

    private void SaveRecettes()
    {
        for (int i = 0; i < recettes.Count; i++)
        {
            PlayerPrefs.SetString(saveFolder + "/Recette" + i + "_nom", recettes[i].nomDeLaRecette);
            PlayerPrefs.SetString(saveFolder + "/Recette" + i + "_ingredients", recettes[i].ingredients);
            PlayerPrefs.SetString(saveFolder + "/Recette" + i + "_preparation", recettes[i].preparation);
        }
        PlayerPrefs.Save();
    }

    private void LoadRecettes()
    {
        int index = 0;
        while (PlayerPrefs.HasKey(saveFolder + "/Recette" + index + "_nom"))
        {
            Recette loadedRecette = ScriptableObject.CreateInstance<Recette>();

            loadedRecette.nomDeLaRecette = PlayerPrefs.GetString(saveFolder + "/Recette" + index + "_nom");
            loadedRecette.ingredients = PlayerPrefs.GetString(saveFolder + "/Recette" + index + "_ingredients");
            loadedRecette.preparation = PlayerPrefs.GetString(saveFolder + "/Recette" + index + "_preparation");

            recettes.Add(loadedRecette);
            index++;
        }
    }

    private void UpdateCanvas()
    {
        if (recettes.Count > 0)
        {
            indexActuel = Mathf.Clamp(indexActuel, 0, recettes.Count - 1);
            AfficherRecette(indexActuel, nomRecetteText, ingredientsText, preparationText);
        }
        else
        {
            nomRecetteText.text = string.Empty;
            ingredientsText.text = string.Empty;
            preparationText.text = string.Empty;
        }
    }


    private void ClearInputFields()
    {
        titleInputField.text = string.Empty;
        descriptionInputField.text = string.Empty;
        authorInputField.text = string.Empty;
    }
}
