using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class RoundsManager : MonoBehaviour
{
    internal static RoundsManager Instance;

    public float TotRoundsNumber;
    private float _currentRoundNumber = 1;

    public DifficultyParameters DifficultyParameters;

    public DifficultyParameters InternalDiffParams;

    private int _roundKilledEnemies = 0;

    [Range(0, 1)] public float IncrementPercentage = 0.1f;
    [Range(0, 1)] public float DecrementPercentage = 0.05f;

    internal int RoundKilledEnemies
    {
        get { return _roundKilledEnemies; }
        set { RoundKilledEnemiesSetter(value); }
    }

    public float CurrentRoundNumber
    {
        get { return _currentRoundNumber; }
        set
        {
            UIManager.Instance.SetRoundText(value, TotRoundsNumber);
            _currentRoundNumber = value;
        }
    }

    private int _spawnedEnemies = 0;
    internal int SpawnedEnemies
    {
        get { return _spawnedEnemies; }
        set { SpawnedEnemiesSetter(value); }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DifficultyParameters = new DifficultyParameters()
            {
                EnemiesCount = 5,
                EnemiesSpeed = 3,
                EnemiesLife = 10,
                EnemiesDamage = 3,
                SpawnCoolDown = 1
            };

            InternalDiffParams = new DifficultyParameters()
            {
                EnemiesCount = DifficultyParameters.EnemiesCount,
                EnemiesSpeed = DifficultyParameters.EnemiesSpeed,
                EnemiesLife = DifficultyParameters.EnemiesLife,
                EnemiesDamage = DifficultyParameters.EnemiesDamage,
                SpawnCoolDown = DifficultyParameters.SpawnCoolDown
            };
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void SpawnedEnemiesSetter(int newValue)
    {
        _spawnedEnemies = newValue;

        if (_spawnedEnemies == DifficultyParameters.EnemiesCount)
        {
            GameManager.Instance.SetSpawnersState(false);
        }
    }

    internal void StartNextRound(DifficultyParameters diffParams)
    {
        CurrentRoundNumber += 1;

        IncrementDifficulty();

        GameManager.Instance.SetSpawnersCooldown(DifficultyParameters.SpawnCoolDown);

        _spawnedEnemies = 0;
        _roundKilledEnemies = 0;

        UIManager.Instance.ShowNewRoundPanel();
    }


    private void RoundKilledEnemiesSetter(int newValue)
    {
        _roundKilledEnemies = newValue;

        if (_roundKilledEnemies < DifficultyParameters.EnemiesCount)
        {
            return;
        }

        _spawnedEnemies = 0;
        _roundKilledEnemies = 0;

        if (CurrentRoundNumber == TotRoundsNumber)
        {
            GameManager.Instance.Win();
            return;
        }

        StartNextRound(GameManager.Instance.DefaultDiffParams);
    }

    private void IncrementDifficulty()
    {
        float percentage = true ? IncrementPercentage : -DecrementPercentage;

        InternalDiffParams.EnemiesCount = Mathf.Max(1, InternalDiffParams.EnemiesCount * (1 + percentage));
        InternalDiffParams.EnemiesSpeed = Mathf.Max(0.1f, InternalDiffParams.EnemiesSpeed * (1 + percentage));
        InternalDiffParams.EnemiesLife = Mathf.Max(1, InternalDiffParams.EnemiesLife * (1 + percentage));
        InternalDiffParams.EnemiesDamage = Mathf.Max(1, InternalDiffParams.EnemiesDamage * (1 + percentage));
        InternalDiffParams.SpawnCoolDown = Mathf.Max(0.2f, InternalDiffParams.SpawnCoolDown * (1 - percentage));

        UpdateCurrentParameters();
    }

    private void UpdateCurrentParameters()
    {
        DifficultyParameters.EnemiesCount = Mathf.RoundToInt(InternalDiffParams.EnemiesCount);
        DifficultyParameters.EnemiesSpeed = InternalDiffParams.EnemiesSpeed;
        DifficultyParameters.EnemiesLife = Mathf.RoundToInt(InternalDiffParams.EnemiesLife);
        DifficultyParameters.EnemiesDamage = Mathf.RoundToInt(InternalDiffParams.EnemiesDamage);
        DifficultyParameters.SpawnCoolDown = InternalDiffParams.SpawnCoolDown;
    }
}
