using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    [SerializeField] List<Recette> recettes;
    [SerializeField] PlayerControl playerControl;
    [SerializeField] GameObject canvasPlayer;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject canvasPopUp;


    [Header("TextMeshPro")]
    [Tooltip("TextMeshPro pour afficher le nom de la recette")]
    [SerializeField]  private TMP_Text nomRecetteText;
    [SerializeField]  private TMP_Text nomRecetteTextPop;

    [Tooltip("TextMeshPro pour afficher les ingrédients")]
    [SerializeField] private TMP_Text ingredientsText;
    [SerializeField] private TMP_Text ingredientsTextPop;

    [Tooltip("TextMeshPro pour afficher les instructions")]
    [SerializeField] private TMP_Text preparationText;
    [SerializeField] private TMP_Text preparationTextPop;

    private int indexActuel = 0;

    private void Start()
    {
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
        ingredients.text = string.Join(",\n", recetteActuelle.ingredients);
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
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();

            GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
            if (raycaster != null)
            {
                raycaster.Raycast(pointerData, results);

                if (results.Count > 0)
                {
                    playerControl.IsReading();
                    canvasPlayer.SetActive(false);
                    canvasPopUp.SetActive(true);
                    AfficherRecette(indexActuel, nomRecetteTextPop, ingredientsTextPop, preparationTextPop);
                }
            }
        }
    }
}
