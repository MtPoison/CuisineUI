using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemData : ScriptableObject
{
    [Header("Informations de l'objet")]
    public string titre;
    public GameObject prefab;
    public Texture2D img;
    [TextArea(3, 10)] 
    public string description;
}
