using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NouvelleRecette", menuName = "JeuDeCuisine/Recette")]
public class Recette : ScriptableObject
{
    [Header("Informations de la Recette")]
    [Tooltip("Nom de la recette")]
    public string nomDeLaRecette;

    [Tooltip("Liste des ingr�dients requis pour la recette")]
    public List<string> ingredients;

    [Tooltip("Instructions � suivre pour pr�parer la recette")]
    [TextArea(5, 10)]
    public string preparation;
}
