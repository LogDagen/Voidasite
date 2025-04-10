/*****************************************************************************
// File Name : GameManager.cs
// Author : Logan Dagenais
// Creation Date : March 6, 2025
//
// Brief Description : This code controls the Game itself, freezing the game
for frames when called, setting level, quitting level, Scoring, 
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
public class GameManager : MonoBehaviour
{
    [Header("Score")]
    private TMP_Text ScoreText;
    public int Score;
    [Header("Freeze Frames")]
    private float pendingFreezeDuration;
    [SerializeField] private float duration;
    private GameObject Pausescreen;
    private bool isFrozen;
    private bool isPaused;
    [Header("Restart")]
    private Animator Fade;
    private GameObject FadeObject;
    [Header("Death")]
    private GameObject deathScreen;
    [Header("Victory")]
    private GameObject VictoryScreen;
    private TMP_Text FinalScoreText;
    private TMP_Text FinalTimeText;
    private Timer time;
    /// <summary>
    /// does not destroy object
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// causes freeze frames when called
    /// </summary>
    private void Update()
    {
        if (pendingFreezeDuration > 0 && isFrozen == false)
        {
            StartCoroutine(DoFreeze());
        }
    }
    /// <summary>
    /// decides how long to freeze
    /// </summary>
    public void Freeze()
    {
        pendingFreezeDuration = duration;
    }
    public void RestartLevel()
    {
        FadeObject.SetActive(true);
        Fade.SetTrigger("Fadein");
        StartCoroutine(Restart());
    }
    public void StartLevel()
    {
        Pausescreen = GameObject.FindGameObjectWithTag("PauseScreen");
        Fade = GameObject.FindGameObjectWithTag("FadeScreen").GetComponent<Animator>();
        FadeObject = GameObject.FindGameObjectWithTag("FadeScreen");
        deathScreen = GameObject.FindGameObjectWithTag("DeathScreen");
        VictoryScreen = GameObject.FindGameObjectWithTag("VictoryScreen");
        FinalScoreText = GameObject.Find("FinalScoreText").GetComponent<TMP_Text>();
        FinalTimeText = GameObject.Find("FinalTimeText").GetComponent<TMP_Text>();
        ScoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        time = GameObject.Find("Canvas").GetComponent<Timer>();
        Pausescreen.SetActive(false);
        deathScreen.SetActive(false);
        VictoryScreen.SetActive(false);
        FadeObject.SetActive(true);
        Fade.SetTrigger("Fadeout");
        StartCoroutine(LevelStart());
        Score = 0;
    }
    public void Quitlevel()
    {
        FadeObject.SetActive(true);
        Fade.SetTrigger("Fadein");
        StartCoroutine(QuitLevel());
    }
    public void NextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
    public void UnlockNewLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
    public void CompleteLevel()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        VictoryScreen.SetActive(true);
        FinalScoreText.text = "Score: " + Score;
        int minutes = Mathf.FloorToInt(time.elapsedTime / 60);
        int seconds = Mathf.FloorToInt(time.elapsedTime % 60);
        FinalTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    /// <summary>
    /// Freezes game for specific number of frames
    /// </summary>
    /// <returns></returns>
    IEnumerator DoFreeze()
    {
        isFrozen = true;
        var original = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        if (isPaused == false)
        {
            Time.timeScale = original;
            pendingFreezeDuration = 0;
            isFrozen = false;
        }
    }
    /// <summary>
    /// Restarts level
    /// </summary>
    /// <returns></returns>
    IEnumerator Restart()
    {
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield break;
    }
    /// <summary>
    /// Happens when level starts
    /// </summary>
    /// <returns></returns>
    IEnumerator LevelStart()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        FadeObject.SetActive(false);
        yield break;
    }
    /// <summary>
    /// Quits level
    /// </summary>
    /// <returns></returns>
    IEnumerator QuitLevel()
    {
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        yield break;
    }
    /// <summary>
    /// Pauses the game
    /// </summary>
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        Pausescreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
    /// <summary>
    /// Unpauses game
    /// </summary>
    public void UnpauseGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        Pausescreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    /// <summary>
    /// sets score
    /// </summary>
    /// <param name="points">points given</param>
    public void UpdateScore(int points)
    {
        Score += points;
        ScoreText.text = "Score: " + Score;
    }
    /// <summary>
    /// happens when player dies
    /// </summary>
    public void Death()
    {
        Fade.SetTrigger("Fadeout");
        StartCoroutine(LevelStart());
        Time.timeScale = 0f;
        deathScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}