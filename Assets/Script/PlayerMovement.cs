using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerControl : MonoBehaviour
{
    private PlayerControler playerControler;
    private InputAction deplacements;

    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float movementSpeed = 5f;

    private bool gamePose = false;
    private bool readRecette = false;

    private void Awake()
    {
        if (playerControler == null)
        {
            playerControler = new PlayerControler();
        }

        if (playerControler == null)
        {
            Debug.LogError("PlayerControler n'a pas été initialisé !");
        }

        if (playerControler.Player.Move == null)
        {
            Debug.LogError("L'action de déplacement est nulle !");
        }
    }

    private void OnEnable()
    {
        deplacements = playerControler.Player.Move; 
        deplacements.Enable();
    }

    private void OnDisable()
    {
        deplacements.Disable();
    }

    private void Update()
    {
        if (gamePose) return;
        if (!readRecette)
        {
            Move();
        }
        
        
    }

    public void IsReading(bool _bool) 
    { 
        readRecette = !readRecette; 
    }

    private void Move()
    {
        Vector2 input = deplacements.ReadValue<Vector2>();

        float rotationY = input.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotationY, 0);

        Vector3 movement = transform.forward * input.y * movementSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }

    public void PoseGame() { gamePose = true; }
    public void IsPoseGame() { gamePose = false; }
}
