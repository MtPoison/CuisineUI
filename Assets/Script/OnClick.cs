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

    [Header("Store")]
    [SerializeField] private GameManger game;

    private GameObject tmp;

    public void TakeLeft()
    {
        player.AddHand(mouse.GetSelectObject(), "left", Vector3.left + new Vector3(0, 0.5f, 2));
        
        canvasTake.SetActive(false);
    }

    public void TakeRight()
    {
        player.AddHand(mouse.GetSelectObject(), "right", Vector3.right + new Vector3(0, 0.5f, 2));
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
        mouse.SetPause(false);
        playerControl.IsReading();
        book.IsRecast(true);
        game.SetCanvasUp(false);
    }

    public void AddRecipe()
    {
        canvasAdd.SetActive(true);
        canvasPlayer.SetActive(false);
        playerControl.IsReading();
        mouse.SetPause(true);
        book.IsRecast(false);
        game.SetCanvasUp(true);
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
        canvasPlayer.SetActive(false);
        playerControl.IsReading();
        book.IsRecast(false);
        inv.SetOpen(true);
        mouse.SetPause(true);
        game.SetCanvasUp(true);
    }

    public void TakeStoreLeft() 
    {
        inv.Take("left", Vector3.left + new Vector3(0, 0, 3));
    }

    public void TakeStoreRight()
    {
        inv.Take("right", Vector3.right + new Vector3(0, 0, 3));
    }
}
