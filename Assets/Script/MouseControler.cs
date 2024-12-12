using UnityEngine;

public class MouseControler : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject hoveredObject;
    private GameObject selectObject;
    private Material originalMaterial;
    private Material outlineMaterial;

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject canvasTake;
    [SerializeField] private GameObject canvasDrop;
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
        RaycastHit hit;

        int layer1Mask = 1 << LayerMask.NameToLayer("Food");
        int layer2Mask = 1 << LayerMask.NameToLayer("Recipiant");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer1Mask))
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
                print("c'est bon");
                if (hoveredObject != null)
                {
                    selectObject = hoveredObject;
                    MoveCanva(selectObject, canvasTake);
                }
            }
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer2Mask))
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
                    MoveCanva(selectObject, canvasDrop);
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
        print(_canvas);
        if (_canvas != null && target != null)
        {
            _canvas.SetActive(true);
            Vector3 canvasPosition = new Vector3(
               target.transform.position.x,
               target.transform.position.y + 1f,
               target.transform.position.z
           );

            _canvas.transform.position = canvasPosition;

            Vector3 direction = player.transform.position - _canvas.transform.position;
            direction.y = 0;

            Quaternion rotation = Quaternion.LookRotation(direction);
            _canvas.transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y - 180, 0);
        }
    }

    public GameObject GetSelectObject() { return selectObject; }
}
