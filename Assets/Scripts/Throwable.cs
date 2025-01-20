using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Throwable : MonoBehaviour
{
    private Transform target;
    private float speed;
    protected float damage;

    public void Initialize(Transform target, float speed, float damage)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += speed * Time.deltaTime * direction;

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            var enemy = target.GetComponent<Enemy>();

            if (enemy != null)
            {
                DoEffect(enemy);
            }

            Destroy(gameObject);
        }
    }
    protected abstract void DoEffect(Enemy enemy);
}
