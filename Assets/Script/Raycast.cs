using UnityEngine;

public class HoverEffectMulti : MonoBehaviour
{
    public Shader shader; // Shader à utiliser
    private Material material;
    private Camera mainCamera;

    public Material Omaterial;

    // Combined LayerMask for multiple layers
    int combinedLayerMask;

    void Start()
    {
        mainCamera = Camera.main;

        // Définir le LayerMask combiné
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
                material.EnableKeyword("_EMISSION"); // Active l'émission du shader
            }
            targetObject.GetComponent<Renderer>().material = material;
            anyHovered = true;
        }

        // Désactivation du shader si aucune cible n'est survolée
        if (!anyHovered && material != null)
        {
            material.DisableKeyword("_EMISSION"); // Désactive l'émission du shader
            // Réinitialiser au matériau par défaut ou transparent
            material = Omaterial;
            foreach (var targetObject in GameObject.FindGameObjectsWithTag("Target"))
            {
                targetObject.GetComponent<Renderer>().material = material;
            }
        }
    }
}
