using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    protected float currentAttackInterval;
    protected int currentAttackCount; // number of times this attack will happen
    protected override void Update()
    {
        base.Update();
        // otherwise, if the attack iterval goes from above 0 to below, we also call attack.
        if (currentAttackInterval > 0)
        {
            currentAttackInterval -= Time.deltaTime;
            if (currentAttackInterval <= 0)
            {
                Attack(currentAttackCount);
            }
        }
    }

    public override bool CanAttack()
    {
        if (currentAttackCount > 0) { return true; }
        return base.CanAttack();
    }
    protected override bool Attack(int attackCount = 1)
    {
      // If there is no projectile assigned, set the weapon on cooldown
        if (!currentStats.projectilePrefab)
        {
            Debug.LogWarning(string.Format("Projectile prefab ha not been ser for {0}",name));
            ActiveCooldown(true);
            return false;
        }

        // If there is no projectile assigned, set the weapon on cooldown
        if ( !CanAttack())  return false;
       

        // otherwise,calculate the angle and the offset of our spawned projectile
        float spawnedAngle = GetSpawnAngle();

        // and spawn a copy of the projectiles prefab
        Projectile prefab = Instantiate(
                 currentStats.projectilePrefab, owner.transform.position + (Vector3)GetSpawnOffset(spawnedAngle),
                 Quaternion.Euler(0, 0, spawnedAngle)
                 );

        prefab.weapon = this;
        prefab.owner = owner;

        //Reset the cooldown only if this attack was triggered by cooldown
        ActiveCooldown(true);
       
        attackCount--;

        if (attackCount > 0)
        {
            currentAttackCount = attackCount;
            currentAttackInterval = data.baseStats.projectileInterval;
        }

        return true;
    }

    // Get which direction the projectile shold spawn
    protected virtual float GetSpawnAngle()
    {
        return Mathf.Atan2(movement.lastMovedVector.y, movement.lastMovedVector.x) * Mathf.Rad2Deg;
    }

    protected virtual Vector2 GetSpawnOffset( float spawnAngle =0 )
    {
        return Quaternion.Euler(0, 0, GetSpawnAngle()) * new Vector2(
            Random.Range(currentStats.spawnVariance.xMin, currentStats.spawnVariance.xMax),
            Random.Range(currentStats.spawnVariance.yMin, currentStats.spawnVariance.yMax)
            );
    }

    protected virtual void Start() { }
    protected virtual void FixedUpdate() { }
    protected virtual void OnTriggerEnter2D(Collider2D other) { }
}
