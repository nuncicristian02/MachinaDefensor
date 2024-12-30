using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();

            GameManager.Instance.Player.Life -= enemy.Damage;

            GameObject.Destroy(collision.gameObject);

            if (!GameManager.Instance.GetSpawnersState() 
                && RoundsManager.Instance.SpawnedEnemies == RoundsManager.Instance.DifficultyParameters.EnemiesCount 
                && GameObject.FindObjectsOfType<Enemy>().Length == 1)
            {
                RoundsManager.Instance.StartNextRound(GameManager.Instance.DefaultDiffParams);
            }
        }
    }
}
