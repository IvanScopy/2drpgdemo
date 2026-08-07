using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public static UI instance;
    private float elapsedTime;
    private bool isGameOver;

    [SerializeField] private GameObject GameOverUI;
    [Space]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI killCountText;
    
    private int killCount;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
        elapsedTime = 0f;
        isGameOver = false;
    }

    private void Update()
    {
        if (isGameOver)
            return;
        
        elapsedTime += Time.deltaTime;
        timerText.text = elapsedTime.ToString("F1") + "s";
    }

    public void EnableGameOverUI()
    {
        isGameOver = true;
        Time.timeScale = .5f;
        GameOverUI.SetActive(true);
        
    } 

    public void RestartLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void AddKillCount()
    {
        killCount++;
        killCountText.text = killCount.ToString();
    }
}
