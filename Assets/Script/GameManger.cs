using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameManger : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject canvasHeat;
    [SerializeField] private GameObject player;
    void Start()
    {
        canvas.SetActive(false);
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
    }

}
