using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClick : MonoBehaviour
{

    [SerializeField] private Book book;

    [Header("Player")]
    [SerializeField] private MouseControler mouse;
    [SerializeField] private Player player;
    [SerializeField] private PlayerControl playerControl;

    [Header("Canvas")]
    [SerializeField] private GameObject canvasTake;
    [SerializeField] private GameObject canvasDrop;
    [SerializeField] private GameObject canvasPlayer;
    [SerializeField] private GameObject canvasPop;
    [SerializeField] private GameObject canvasAdd;
    [SerializeField] private GameObject canvasStore;


    [Header("Store")]
    [SerializeField] private Inv inv;

    private GameObject tmp;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void TakeLeft()
    {
        player.AddHand(mouse.GetSelectObject(), "left", Vector3.left + new Vector3(0, 0, 3));
        canvasTake.SetActive(false);
    }

    public void TakeRight()
    {
        player.AddHand(mouse.GetSelectObject(), "right", Vector3.right + new Vector3(0, 0, 3));
        canvasTake.SetActive(false);
    }

    public void LaidLeft()
    {
        player.DropHande(mouse.GetSelectObject(), "left", Vector3.up *5);
        canvasDrop.SetActive(false);
    }

    public void LaidRight()
    {
        player.DropHande(mouse.GetSelectObject(), "right", Vector3.up * 5);
        canvasDrop.SetActive(false);
    }

    public void Next()
    {
        book.RecetteSuivante();
    }

    public void Previous()
    {
        book.RecettePrecedente();
    }

    public void Left()
    {
        canvasPop.SetActive(false);
        canvasAdd.SetActive(false);
        canvasStore.SetActive(false);
        canvasPlayer.SetActive(true);
        playerControl.IsReading();
        book.IsRecast(true);
    }

    public void AddRecipe()
    {
        canvasAdd.SetActive(true);
        canvasPlayer.SetActive(false);
        playerControl.IsReading();

        book.IsRecast(false);
    }

    public void SaveRecipe()
    {
        book.AddRecette();
        book.IsRecast(true);
    }

    public void SupRecipe()
    {
        book.RemoveRecette();
    }

    public void DropStoreLeft()
    {
        tmp = player.ItemHand("left");
        inv.Drop(player.ItemHand("left"), "left");
        Destroy(tmp);
    }

    public void DropStoreRight()
    {
        tmp = player.ItemHand("right");
        inv.Drop(player.ItemHand("right"), "right");
        Destroy(tmp);
    }

    public void Open()
    {
        canvasStore.SetActive(true);
        canvasPlayer.SetActive(true);
        playerControl.IsReading();
        book.IsRecast(true);
    }
}
