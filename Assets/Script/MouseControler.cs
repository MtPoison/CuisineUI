using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseControler : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject hoveredObject;
    private GameObject selectObject;
    private Shader outlineShader;
    private Shader originalShader;
    private RaycastHit hit;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject canvasTake;
    [SerializeField] private GameObject canvasDrop;
    [SerializeField] private GameObject canvasStore;

    [SerializeField] private float distanceAhead = 2.0f;
    [SerializeField] private float heightOffset = 150;
    [SerializeField] private float distanceLeft = 2.0f;
    [SerializeField] private Inv inv;

    private bool pause = false;

    void Start()
    {
        mainCamera = Camera.main;

        outlineShader = Shader.Find("Custom/HighlightShader");
        if (outlineShader == null)
        {
            Debug.LogError("Outline shader introuvable !");
        }
    }

    private bool IsMouseOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                return true;
            }
        }

        return false;
    }

    void Update()
    {
        if (pause) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        /*int combinedLayerMask = (1 << LayerMask.NameToLayer("Food")) |
                                (1 << LayerMask.NameToLayer("Recipiant")) |
                                (1 << LayerMask.NameToLayer("Store"));*/
        int layer1Mask = 1 << LayerMask.NameToLayer("Food");
        int layer2Mask = 1 << LayerMask.NameToLayer("Recipiant");
        int layer3Mask = 1 << LayerMask.NameToLayer("Store");

        // Vérifiez les objets sous le pointeur
        Raycast(ray, layer1Mask, canvasTake, 0, 0.75f) ;
        Raycast(ray, layer3Mask, canvasStore, 3f, 3f);
        Raycast(ray, layer2Mask, canvasDrop, 0, 1.5f);
        
    }

    private void Raycast(Ray _ray, int layerMask, GameObject canvas, float _x, float _y)
    {
        if (Physics.Raycast(_ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            if (hoveredObject != hit.collider.gameObject)
            {
                if (hoveredObject != null)
                {
                }

                hoveredObject = hit.collider.gameObject;
                
            }

            if (Input.GetMouseButtonDown(0))
            {
                selectObject = hoveredObject;

                // Vérifiez les interactions
                if (selectObject.name == "refrigerator_2")
                {
                    Debug.Log("L'objet sélectionné est refrigerator_2.");
                    inv.SetRandom(true);
                }
                else
                {
                    Debug.Log("Autre objet sélectionné.");
                    inv.SetRandom(false);
                }

                if (!IsMouseOverUI())
                {
                    if (canvas != null)
                    {
                        if (canvas.activeSelf)
                            canvas.SetActive(false);
                        else
                            MoveCanva(selectObject, canvas, _x, _y);
                    }
                }
            }
        }
        else
        {
            if (hoveredObject != null)
            {
                hoveredObject = null;
            }

            if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
            {
                if (canvas != null) canvas.SetActive(false);
            }
        }
    }

    void ApplyOutline(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            originalShader = renderer.material.shader;
            renderer.material.shader = outlineShader;
        }
        else
        {
            Debug.LogWarning($"L'objet {obj.name} n'a pas de Renderer !");
        }
    }

    void RemoveOutline(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null && originalShader != null)
        {
            renderer.material.shader = originalShader;
        }
    }


    void MoveCanva(GameObject target, GameObject _canvas, float _x, float _y)
    {
        if (_canvas != null && target != null)
        {
            _canvas.SetActive(true);

            Vector3 directionToPlayer = (player.transform.position - target.transform.position).normalized;
            Vector3 leftOffset = Vector3.Cross(Vector3.up, directionToPlayer) * -_x;
            Vector3 canvasPosition = target.transform.position + leftOffset;
            canvasPosition.y += _y;

            _canvas.transform.position = canvasPosition;

            Vector3 lookDirection = player.transform.position - _canvas.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection.normalized);

            _canvas.transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y + 180, 0);
        }
    }

    public void SetPause(bool _bool)
    {
        pause = _bool;
    }

    public GameObject GetSelectObject()
    {
        return selectObject;
    }
}
