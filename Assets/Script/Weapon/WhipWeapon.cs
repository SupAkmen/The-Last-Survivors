using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipWeapon : ProjectileWeapon
{
    int currentSpawnCount; // How many time the whip ha been attacking in this iteration.
    float currentSpawnYOffset; // If there are more than 2 whips, we will start offseting it upward.
    protected override bool Attack(int attackCount = 1)
    {
        //  // If there is no projectile assigned, set the weapon on cooldown
        if (!currentStats.projectilePrefab)
        {
            Debug.LogWarning(string.Format("Projectile prefab ha not been ser for {0}", name));
            ActiveCooldown(true);
            return false;
        }


        // If there is no projectile assigned, set the weapon on cooldown
        if (!CanAttack())
        { 
            return false;
        }
        // if this is the first time the attack has been fired, we reset the currentSpawnCount
        if(currentCooldown <= 0)
        {
            currentSpawnCount = 0;
            currentSpawnYOffset = 0f;
        }

        // otherwise,calculate the angle and the offset of our spawned projectile
        // then if currentspawncount is even (i.e more than 1 projectile)
        //  we flip the directionof the spawn
        float spawnDir = Mathf.Sign(movement.lastMovedVector.x) * (currentSpawnCount%2 != 0 ? -1 : 1);
        Vector2 spawnedOffset =  new Vector2(spawnDir * Random.Range(currentStats.spawnVariance.xMin, currentStats.spawnVariance.xMax),currentSpawnYOffset );

        // and spawn a copy of the projectiles prefab
        Projectile prefab = Instantiate(
                 currentStats.projectilePrefab, owner.transform.position + (Vector3)spawnedOffset,
                 Quaternion.identity
                 );

        prefab.owner = owner; // Set ourselves to be the owner ( lien quan den whipweapon)
        // Flip the projectile's  sprite
       
        if(spawnDir < 0)
        {
               prefab.transform.localScale = new Vector3(
                    -Mathf.Abs(prefab.transform.localScale.x),
                    prefab.transform.localScale.y,
                    prefab.transform.localScale.z
                    );
        }


        // Assign the baseStats
        prefab.weapon = this;
        ActiveCooldown(true);
        attackCount--;

        // deternime where the next projectile should spawn
        currentSpawnCount++;
        if(currentSpawnCount > 1 && currentSpawnCount %2 !=0)
        {
            currentSpawnYOffset += 1;
        }

        if (attackCount > 0)
        {
            currentAttackCount = attackCount;
            currentAttackInterval = data.baseStats.projectileInterval;
        }

        return true;
    }
}
