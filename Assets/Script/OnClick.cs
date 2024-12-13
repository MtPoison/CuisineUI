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

    [Header("Input Field")]
    [SerializeField] private GameObject canvasTake;
    [SerializeField] private GameObject canvasDrop;
    [SerializeField] private GameObject canvasPlayer;
    [SerializeField] private GameObject canvasPop;
    [SerializeField] private GameObject canvasAdd;

    
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
        player.DropHande(mouse.GetSelectObject(), "left", new Vector3(0,5,0));
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
}
