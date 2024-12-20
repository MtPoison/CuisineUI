using UnityEngine;

public class HoverEffectMulti : MonoBehaviour
{
    public Shader shader; // Shader � utiliser
    private Material material;
    private Camera mainCamera;

    public Material Omaterial;

    // Combined LayerMask for multiple layers
    int combinedLayerMask;

    void Start()
    {
        mainCamera = Camera.main;

        // D�finir le LayerMask combin�
        combinedLayerMask = (1 << LayerMask.NameToLayer("Food")) |
                            (1 << LayerMask.NameToLayer("Recipiant")) |
                            (1 << LayerMask.NameToLayer("Store"));
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        bool anyHovered = false;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, combinedLayerMask))
        {
            GameObject targetObject = hit.collider.gameObject;

            if (material == null || targetObject.GetComponent<Renderer>().material.shader.name != shader.name)
            {
                material = new Material(shader);
                material.EnableKeyword("_EMISSION"); // Active l'�mission du shader
            }
            targetObject.GetComponent<Renderer>().material = material;
            anyHovered = true;
        }

        // D�sactivation du shader si aucune cible n'est survol�e
        if (!anyHovered && material != null)
        {
            material.DisableKeyword("_EMISSION"); // D�sactive l'�mission du shader
            // R�initialiser au mat�riau par d�faut ou transparent
            material = Omaterial;
            foreach (var targetObject in GameObject.FindGameObjectsWithTag("Target"))
            {
                targetObject.GetComponent<Renderer>().material = material;
            }
        }
    }
}
