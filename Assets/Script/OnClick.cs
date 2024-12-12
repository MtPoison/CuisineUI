using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClick : MonoBehaviour
{
    [SerializeField] private MouseControler mouse;
    [SerializeField] private GameObject canvasTake;
    [SerializeField] private GameObject canvasDrop;
    [SerializeField] private Player player;
    [SerializeField] private Book book;
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private GameObject canvasPlayer;
    [SerializeField] private GameObject canvasPop;
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
        canvasPlayer.SetActive(true);
        playerControl.IsReading();
    }
}
