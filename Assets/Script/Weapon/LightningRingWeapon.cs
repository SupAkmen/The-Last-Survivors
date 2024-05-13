using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningRingWeapon : ProjectileWeapon
{
    List<EnemyStats> allSelectedEnemies = new List<EnemyStats>();

    protected override bool Attack(int attackCount = 1)
    {
        if (!currentStats.hitEffect)
        {
            Debug.LogWarning(string.Format("Hit effect prefab ha not been ser for {0}", name));
            ActiveCooldown(true);
            return false;
        }

        if (!CanAttack())
        {
            return false;
        }

        // if the cooldown is less than 0, this is the first firing of the weapon.
        if (currentCooldown <= 0)
        {
            
            allSelectedEnemies = new List<EnemyStats>(FindObjectsOfType<EnemyStats>());
            ActiveCooldown(false);
            currentAttackCount = attackCount;
        }


        //  Find an enemy in the map to strike with lightning
        EnemyStats target = PickEnemy();
        if(target)
        {
            
            allSelectedEnemies.Remove(target);
            DamageArea(target.transform.position, GetArea(),GetDamage());
            // target.TakeDamage(currentStats.damage + Random.Range(0, currentStats.damageVariance), transform.position);
            Instantiate(currentStats.hitEffect, target.transform.position, Quaternion.identity);
        }
      
        // if we have more than 1 attack count
        if(attackCount > 0)
        {
            currentAttackCount = attackCount - 1;
            currentAttackInterval = currentStats.projectileInterval;
        }
     
        return true;
    }

    // Randomly picks an enemy on screen
    EnemyStats PickEnemy()
    {
        EnemyStats target = null;
        while (!target && allSelectedEnemies.Count > 0)
        {
            target = allSelectedEnemies[Random.Range(0, allSelectedEnemies.Count)];

            // Kiểm tra nếu mục tiêu là null hoặc đã bị hủy
            if (target == null || target.Equals(null))
            {
                allSelectedEnemies.Remove(target);
                continue;
            }

            // Kiểm tra xem kẻ địch có trên màn hình không
            Renderer r = target.GetComponent<Renderer>();
            if (r && !r.isVisible)
            {
                allSelectedEnemies.Remove(target);
                continue;
            }
        }

        // Loại bỏ mục tiêu nếu nó là null hoặc đã bị hủy
        if (target == null || target.Equals(null))
        {
            allSelectedEnemies.Remove(target);
        }

        return target;
    }


    // Deals damage in an area
    void DamageArea(Vector2 position,float radius,float damage)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(position,radius);
        foreach(Collider2D target in targets)
        {
            EnemyStats es = target.GetComponent<EnemyStats>();
            if (es != null)
            {
                es.TakeDamage(damage,transform.position);
            }
        }
    }
}
