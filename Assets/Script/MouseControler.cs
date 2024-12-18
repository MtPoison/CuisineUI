using System;
using UnityEngine;

public class MouseControler : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject hoveredObject;
    private GameObject selectObject;
    private Material originalMaterial;
    private Material outlineMaterial;
    RaycastHit hit;

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject canvasTake;
    [SerializeField] private GameObject canvasDrop;
    [SerializeField] private GameObject canvasStore;

    [SerializeField] private float distanceAhead = 2.0f;
    [SerializeField] private float heightOffset = 150;
    [SerializeField] private float distanceLeft = 2.0f;
    [SerializeField] private LayerMask layer1;
    [SerializeField] private LayerMask layer2;

    public float offsetX = 5f;


    Mesh mesh;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        

        int layer1Mask = 1 << LayerMask.NameToLayer("Food");
        int layer2Mask = 1 << LayerMask.NameToLayer("Recipiant");
        int layer3Mask = 1 << LayerMask.NameToLayer("Store");
        Raycast(ray, hit, layer1Mask, MoveCanva, canvasTake);
        Raycast(ray, hit, layer2Mask, MoveCanva, canvasDrop);
        Raycast(ray, hit, layer3Mask, MoveCanva, canvasStore);
    }

    private void Raycast(Ray _ray, RaycastHit hit, int layerMask, Action<GameObject, GameObject> canvas, GameObject type)
    {
        if (Physics.Raycast(_ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hoveredObject != hit.collider.gameObject)
            {
                if (hoveredObject != null)
                {
                    RemoveOutline(hoveredObject);
                }

                hoveredObject = hit.collider.gameObject;
                ApplyOutline(hoveredObject);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (hoveredObject != null)
                {
                    selectObject = hoveredObject;
                    canvas(selectObject, type);
                }
            }
        }
        else
        {
            if (hoveredObject != null)
            {
                RemoveOutline(hoveredObject);
                hoveredObject = null;
            }
        }
    }
    void ApplyOutline(GameObject obj)
    {
        originalMaterial = obj.GetComponent<Renderer>().material;

        obj.GetComponent<Renderer>().material = outlineMaterial;
    }

    void RemoveOutline(GameObject obj)
    {
        obj.GetComponent<Renderer>().material = originalMaterial;
    }

    void MoveCanva(GameObject target, GameObject _canvas)
    {
        if (_canvas != null && target != null)
        {
            _canvas.SetActive(true);
            Vector3 canvasPosition = target.transform.position - target.transform.right * 1.5f;
            canvasPosition.y += 1f;

            _canvas.transform.position = canvasPosition;

            Vector3 direction = player.transform.position - _canvas.transform.position;
            direction.y = 0;

            Quaternion rotation = Quaternion.LookRotation(direction);
            _canvas.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y - 180, 0);
        }
    }

    public GameObject GetSelectObject() { return selectObject; }
}
