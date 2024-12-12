using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NouvelleRecette", menuName = "JeuDeCuisine/Recette")]
public class Recette : ScriptableObject
{
    [Header("Informations de la Recette")]
    [Tooltip("Nom de la recette")]
    public string nomDeLaRecette;

    [Tooltip("Liste des ingrédients requis pour la recette")]
    public List<string> ingredients;

    [Tooltip("Instructions à suivre pour préparer la recette")]
    [TextArea(5, 10)]
    public string preparation;
}
