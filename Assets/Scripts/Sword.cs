using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Sword : MonoBehaviour
{
    Knight knight;
    private void Awake()
    {
        knight = transform.parent.GetComponent<Knight>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.tag == "Enemy")
        {
            var enemy = collider.transform.GetComponent<Enemy>();

            enemy.TakeDamage(knight.Damage);
        }
    }
}
