using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static Enums;

public class GameManager : MonoBehaviour
{
    internal static GameManager Instance;
    internal Player Player;

    private bool _spawnersState = true;

    internal DifficultyParameters DefaultDiffParams;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            FindReferences();
            SceneManager.sceneLoaded += OnChangeScene;
            DefaultDiffParams = new DifficultyParameters()
            {
                EnemiesCount = 5,
                EnemiesSpeed = 3,
                EnemiesLife = 10,
                EnemiesDamage = 3,
                SpawnCoolDown = 1
            };
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnChangeScene(Scene scene, LoadSceneMode loadSceneMode)
    {
        FindReferences();
    }

    private void FindReferences()
    {
        Player = GameObject.Find("Player")?.GetComponent<Player>();
    }

    internal void ChangeScene(Enums.GameScene sceneIndex)
    {
        SceneManager.LoadScene((int)sceneIndex);
    }

    internal Enums.GameScene GetCurrentScene()
    {
        return (Enums.GameScene)SceneManager.GetActiveScene().buildIndex;
    }

    internal void Death()
    {
        UIManager.Instance.ShowDeathPanel();
    }

    internal void Win()
    {
        UIManager.Instance.ShowWonPanel();
    }

    public void SetSpawnersState(bool stateActive)
    {
        _spawnersState = stateActive;
        var spawners = GameManager.FindObjectsOfType<Spawner>();

        spawners.ToList().ForEach(spawner =>
        {
            spawner.IsActive = stateActive;
        });
    }

    public bool GetSpawnersState()
    {
        return _spawnersState;
    }

    internal void DestroyEveryEnemy()
    {
        var enemies = GameManager.FindObjectsOfType<Enemy>();

        enemies.ToList().ForEach(enemy =>
        {
            Destroy(enemy.gameObject);
        });
    }

    internal void ResetState()
    {
        RoundsManager.Instance.CurrentRoundNumber = 1;
        RoundsManager.Instance.SpawnedEnemies = 0;
        RoundsManager.Instance.RoundKilledEnemies = 0;
        Player.Money = Player.StartingMoney;
        Player.Life = Player.StartingLife;
    }

    internal void SetSpawnersCooldown(float newCooldown)
    {
        var spawners = GameManager.FindObjectsOfType<Spawner>();

        spawners.ToList().ForEach(spawner =>
        {
            spawner.SpawnCooldown = newCooldown;
        });
    }
}
