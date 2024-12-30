using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Enums;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private TextMeshProUGUI moneyText;
    private Image lifeImage;
    private TextMeshProUGUI roundText;

    private bool IsGamePaused = false;

    private GameObject GamePanel;
    private GameObject MainMenuPanel;
    private GameObject PausePanel;
    private GameObject DeathPanel;
    private GameObject WonPanel;
    private GameObject NewRoundPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            FindRefereces();
            SceneManager.sceneLoaded += OnSceneChangedChangeUI;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneChangedChangeUI(Scene scene, LoadSceneMode loadSceneMode)
    {
        GamePanel.SetActive(scene.buildIndex == (int)GameScene.GameScene);
        MainMenuPanel.SetActive(scene.buildIndex == (int)GameScene.MainMenu);
        PausePanel.SetActive(false);
        DeathPanel.SetActive(false);
        WonPanel.SetActive(false);
        NewRoundPanel.SetActive(false);

        if (GameManager.Instance && GameManager.Instance.Player && scene.buildIndex == (int)GameScene.GameScene)
        {
            SetLifeBar(GameManager.Instance.Player.Life);
            SetMoneyText(GameManager.Instance.Player.Money);
            SetRoundText(RoundsManager.Instance.CurrentRoundNumber, RoundsManager.Instance.TotRoundsNumber);
        }
    }

    private void FindRefereces()
    {
        moneyText = GameObject.Find("CoinsNumber")?.GetComponent<TextMeshProUGUI>();
        lifeImage = GameObject.Find("PlayerLifeImage")?.GetComponent<Image>();
        roundText = GameObject.Find("RoundNumber")?.GetComponent<TextMeshProUGUI>();
        GamePanel = GameObject.Find("GamePanel");
        MainMenuPanel = GameObject.Find("MainMenuPanel");
        PausePanel = GameObject.Find("PausePanel");
        DeathPanel = GameObject.Find("DeathPanel");
        WonPanel = GameObject.Find("WonPanel");
        NewRoundPanel = GameObject.Find("NewRoundPanel");
    }

    public void SetMoneyText(float moneyValue)
    {
        moneyText.text = Convert.ToString(moneyValue);
    }

    internal void SetLifeBar(float lifeValue) 
    {
        if (lifeValue <= 0)
        {
            return;
        }

        var currentImageScale = lifeImage.transform.localScale;

        var playerRef = GameManager.Instance?.Player;

        var newScaleX = lifeValue/playerRef.StartingLife;

        var newScale = new Vector3(newScaleX, currentImageScale.y, currentImageScale.z);

        lifeImage.transform.localScale = newScale;
    }

    public void OnPlayButtonClick()
    {
        GameManager.Instance.ChangeScene((GameScene)1);
    }
    public void OnExitGameButtonClick()
    {
        Time.timeScale = 1f;
        GameManager.Instance.ResetState();
        GameManager.Instance.ChangeScene((GameScene)0);
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }

    public void OnRetryButtonClick()
    {
        Time.timeScale = 1f;
        GameManager.Instance.ResetState();
        GameManager.Instance.ChangeScene(GameManager.Instance.GetCurrentScene());
    }

    public void OnPauseButtonClick()
    {
        PauseGame(!IsGamePaused);
    }

    private void PauseGame(bool pause)
    {
        IsGamePaused = pause;
        PausePanel.SetActive(pause);
        Time.timeScale = pause ? 0f : 1f;
    }

    internal void ShowDeathPanel()
    {
        DeathPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    internal void ShowWonPanel()
    {
        WonPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    internal void ShowNewRoundPanel()
    {  
        NewRoundPanel.SetActive(true);    
    }

    internal void SetRoundText(float currentRoundNumber, float totRoundsNumber)
    {
        roundText.text = $"ROUND {currentRoundNumber}/{totRoundsNumber}";
    }
}