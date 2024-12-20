using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseControler : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject hoveredObject;
    private GameObject selectObject;
    private Material hoverMaterial;
    private Shader outlineShader;
    private Shader originalShader;

    [SerializeField] private Shader hoverShader;
    [SerializeField] private Material defaultMaterial;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject canvasTake;
    [SerializeField] private GameObject canvasDrop;
    [SerializeField] private GameObject canvasStore;

    
    [SerializeField] private Inv inv;

    private int combinedLayerMask;
    private bool pause = false;

    void Start()
    {
        mainCamera = Camera.main;
        combinedLayerMask = (1 << LayerMask.NameToLayer("Food")) |
                            (1 << LayerMask.NameToLayer("Recipiant")) |
                            (1 << LayerMask.NameToLayer("Store"));

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
        bool anyHovered = false;

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, combinedLayerMask))
        {
            GameObject targetObject = hit.collider.gameObject;

            if (hoveredObject != targetObject)
            {
                ResetHoveredObject();
                hoveredObject = targetObject;

                if (hoverShader != null)
                {
                    hoverMaterial = new Material(hoverShader);
                    hoverMaterial.EnableKeyword("_EMISSION");
                    hoveredObject.GetComponent<Renderer>().material = hoverMaterial;
                }
            }
            anyHovered = true;

            if (Input.GetMouseButtonDown(0))
            {
                selectObject = hoveredObject;
                if (selectObject.name == "refrigerator_2")
                {
                    inv.SetRandom(true);
                }
                else
                {
                    inv.SetRandom(false);
                }

                int hitLayer = hit.collider.gameObject.layer;
                if (hitLayer == LayerMask.NameToLayer("Food"))
                {
                    HandleCanvas(canvasTake, selectObject, 0, 0.75f);
                }
                else if (hitLayer == LayerMask.NameToLayer("Recipiant"))
                {
                    HandleCanvas(canvasDrop, selectObject, 0, 1.5f);
                }
                else if (hitLayer == LayerMask.NameToLayer("Store"))
                {
                    HandleCanvas(canvasStore, selectObject, 3f, 3f);
                }
            }
        }
        else
        {
            ResetHoveredObject();

            if (Input.GetMouseButtonDown(0) && !IsMouseOverUI())
            {
                DeactivateAllCanvases();
            }
        }

        if (!anyHovered && hoverMaterial != null)
        {
            hoverMaterial.DisableKeyword("_EMISSION");
        }
    }


    void ResetHoveredObject()
    {
        if (hoveredObject != null && defaultMaterial != null)
        {
            hoveredObject.GetComponent<Renderer>().material = defaultMaterial;
        }
        hoveredObject = null;
    }

    void HandleCanvas(GameObject canvas, GameObject target, float offsetX, float offsetY)
    {
        if (!IsMouseOverUI())
        {
            DeactivateAllCanvases();

            if (canvas != null && !canvas.activeSelf)
            {
                MoveCanvas(target, canvas, offsetX, offsetY);
            }
        }
    }

    private void DeactivateAllCanvases()
    {
        if (canvasTake != null) canvasTake.SetActive(false);
        if (canvasDrop != null) canvasDrop.SetActive(false);
        if (canvasStore != null) canvasStore.SetActive(false);
    }

    void MoveCanvas(GameObject target, GameObject _canvas, float _x, float _y)
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
