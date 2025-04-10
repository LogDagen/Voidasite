using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    private GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    /// <summary>
    /// restarts game
    /// </summary>
    public void Restart()
    {
        gameManager.RestartLevel();
    }
    public void Quit()
    {
        Time.timeScale = 1f;
        gameManager.Quitlevel();
    }
}