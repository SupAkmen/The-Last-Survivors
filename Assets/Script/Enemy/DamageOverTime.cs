using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    public float damagePerSecond;
    public float duration;

    private float timer;
    private EnemyStats enemy;

    private void Start()
    {
        timer = duration;
        enemy = GetComponentInParent<Collider2D>().GetComponent<EnemyStats>();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            enemy?.TakeDamage(damagePerSecond * Time.deltaTime, transform.position);
        }
        else
        {
            Destroy(gameObject);
        }    
    }
}
