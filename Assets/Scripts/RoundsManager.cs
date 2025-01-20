using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class RoundsManager : MonoBehaviour
{
    internal static RoundsManager Instance;

    public float TotRoundsNumber;
    private float _currentRoundNumber = 1;

    public SpawnersParameters RoundSpawnersParameters;

    public SpawnersParameters InternalSpawnersParams;

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

    internal int SpawnedBaseEnemies;
    internal int SpawnedIntermediateEnemies;
    internal int SpawnedBossEnemies;

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

            RoundSpawnersParameters = new SpawnersParameters()
            {
                BaseEnemiesCount = 5,
                IntermediateEnemiesCount = 0.8f,
                BossEnemiesCount = 0.45f,
                SpawnCoolDown = 1

            };

            SceneManager.sceneLoaded += OnSceneLoaded;

            InternalSpawnersParams = new SpawnersParameters()
            {
                BaseEnemiesCount = 5,
                IntermediateEnemiesCount = 0.8f,
                BossEnemiesCount = 0.45f,
                SpawnCoolDown = 1
            };
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        ResetRoundCounters();
    }

    private void SpawnedEnemiesSetter(int newValue)
    {
        _spawnedEnemies = newValue;

        var level = SpawnedEnemies / RoundSpawnersParameters.TotEnemiesCount;

        UIManager.Instance.UpdateProgressBarLevel(level);

        if (_spawnedEnemies == RoundSpawnersParameters.TotEnemiesCount)
        {
            GameManager.Instance.SetSpawnersState(false);
        }
    }

    internal void StartNextRound()
    {
        CurrentRoundNumber += 1;

        ResetRoundCounters();

        IncrementDifficulty();

        GameManager.Instance.SetSpawnersCooldown(RoundSpawnersParameters.SpawnCoolDown);

        _spawnedEnemies = 0;
        _roundKilledEnemies = 0;

        UIManager.Instance.ShowNewRoundPanel();
    }

    private void ResetRoundCounters()
    {
        SpawnedBaseEnemies = 0;
        SpawnedIntermediateEnemies = 0;
        SpawnedBossEnemies = 0;

        SpawnedEnemies = 0;
        RoundKilledEnemies = 0;
    }


    private void RoundKilledEnemiesSetter(int newValue)
    {
        _roundKilledEnemies = newValue;

        if (_roundKilledEnemies < RoundSpawnersParameters.TotEnemiesCount)
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

        StartNextRound();
    }

    private void IncrementDifficulty()
    {
        float percentage = true ? IncrementPercentage : -DecrementPercentage;

        InternalSpawnersParams.BaseEnemiesCount = Mathf.Max(1, InternalSpawnersParams.TotEnemiesCount * (1 + percentage));
        InternalSpawnersParams.IntermediateEnemiesCount = InternalSpawnersParams.IntermediateEnemiesCount * (1 + percentage);
        InternalSpawnersParams.BossEnemiesCount = InternalSpawnersParams.BossEnemiesCount * (1 + percentage);
        InternalSpawnersParams.SpawnCoolDown = Mathf.Max(0.2f, InternalSpawnersParams.SpawnCoolDown * (1 - percentage));

        UpdateCurrentParameters();
    }

    private void UpdateCurrentParameters()
    {
        RoundSpawnersParameters.BaseEnemiesCount = Mathf.RoundToInt(InternalSpawnersParams.BaseEnemiesCount);
        RoundSpawnersParameters.IntermediateEnemiesCount = Mathf.RoundToInt(InternalSpawnersParams.IntermediateEnemiesCount);
        RoundSpawnersParameters.BossEnemiesCount = Mathf.RoundToInt(InternalSpawnersParams.BossEnemiesCount);
        RoundSpawnersParameters.SpawnCoolDown = InternalSpawnersParams.SpawnCoolDown;
    }
}
