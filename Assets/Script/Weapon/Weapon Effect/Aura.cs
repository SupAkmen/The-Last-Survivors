using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// Tạo ra vung gay sat thuong xung quanh
/// </summary>
public class Aura : WeaponEffect
{
  
    Dictionary<EnemyStats, float> affectedTargets = new Dictionary<EnemyStats, float>();
    List<EnemyStats> targetToUnaffect = new List<EnemyStats>();

    private void Update()
    {
        Dictionary<EnemyStats, float> affectedTargsCopy = new Dictionary<EnemyStats, float>(affectedTargets);
        // Loop through every target affected by the aura and reduce the cooldown of the auura for this if the cooldown reaches 0, deal damage to it.
        foreach (KeyValuePair<EnemyStats, float> pair in affectedTargsCopy)
        {
            affectedTargets[pair.Key] -= Time.deltaTime;
            if (pair.Value <= 0)
            {
                if (targetToUnaffect.Contains(pair.Key))
                {
                    affectedTargets.Remove(pair.Key);
                    targetToUnaffect.Remove(pair.Key);
                }
                else
                {
                    // Reset the cooldown and deal damage
                    Weapon.Stats stats = weapon.GetStats();
                    affectedTargets[pair.Key] = stats.cooldown * Owner.Stats.cooldown;
                    pair.Key.TakeDamage(GetDamage(), transform.position,stats.knockback);
                }

            }

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyStats es))
        {
            // if the target is not yet affected by this aura,add it to our list of affected targets
            if (!affectedTargets.ContainsKey(es))
            {
                // Always starts with an interval of 0, so that it will get damaged in the next UPdate() tick
                affectedTargets.Add(es, 0);
            }
            else
            {
                if(targetToUnaffect.Contains(es))
                {
                    targetToUnaffect.Remove(es);
                }
            }

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyStats es))
        {
            // do not directly remove the target upon leaving, because we still have to track their coolfown
            if (affectedTargets.ContainsKey(es))
            {

                targetToUnaffect.Add(es);
            }

        }
    }

}
