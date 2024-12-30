using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundsPanel : MonoBehaviour
{
    private void OnEnable()
    {
        var animator = GetComponent<Animator>();
        var roundText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        if (!RoundsManager.Instance)
        {
            return;
        }
        roundText.text = $"ROUND {RoundsManager.Instance.CurrentRoundNumber}";
        animator.Play("NewRoundAnim");
    }

    public void StartRoundChange()
    {
        GameManager.Instance.DestroyEveryEnemy();
        GameManager.Instance.SetSpawnersState(false);
    }

    public void StopRoundChange()
    {
        GameManager.Instance.SetSpawnersState(true);
        gameObject.SetActive(false);
    }
}
