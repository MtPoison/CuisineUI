using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class GameManger : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject canvasHeat;
    [SerializeField] private MouseControler mouse;
    [SerializeField] private Player player;
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private Book book;

    [Header("Canvas")]
    [SerializeField] private GameObject canvasTake;
    [SerializeField] private GameObject canvasDrop;
    [SerializeField] private GameObject canvasPlayer;
    [SerializeField] private GameObject canvasPop;
    [SerializeField] private GameObject canvasAdd;
    [SerializeField] private GameObject canvasStore;
    [SerializeField] private GameObject text;

    private GameObject item;
    private bool canvasUp;
    void Start()
    {
        canvas.SetActive(false);
        text.SetActive(false);
    }

    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, canvasHeat.transform.position);
        if (distance < 5)
        {
            canvasHeat.SetActive(true);
        }
        else
        {
            canvasHeat.SetActive(false);
        }

        if(canvasUp)
        {
            if(Input.GetMouseButtonDown(0) && !IsMouseOverUI())
            {
                canvasPop.SetActive(false);
                canvasAdd.SetActive(false);
                canvasStore.SetActive(false);
                mouse.SetPause(false);
                playerControl.IsReading(false);
                book.IsRecast(true);
                text.SetActive(false);
                canvasUp = false;
            }
        }
    }

    public void SetItem(GameObject _item)
    {
        item = _item;
    }

    public GameObject GetItem() { return item; }

    public void SetItemNull()
    {
        item = null;
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

    public void SetCanvasUp(bool _bool)
    {
        canvasUp = _bool;
    }
}
