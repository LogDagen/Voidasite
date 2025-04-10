using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScript : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void Continue()
    {
        gameManager.NextLevel();
    }
    public void Quit()
    {
        gameManager.Quitlevel();
    }
}
