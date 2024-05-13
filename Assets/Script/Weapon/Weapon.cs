using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// dung de quan li va chay cac hanh vi cua vk trong game
/// </summary>
public abstract class Weapon : Item
{
    [System.Serializable]
    public struct Stats
    {
        public string name, description;
        [Header("Visuals")]
        public Projectile projectilePrefab;// If attached, a projectile will spawn every time the weapon cool down
        public Aura auraPrefab; // If attached, an aura will spawn when weapon is equiped
        public ParticleSystem hitEffect;
        public Rect spawnVariance;

        [Header("Values")]
        public float lifespan;
        public float damage, damageVariance, area, speed, cooldown, projectileInterval, knockback;
        public int number, piercing;

        // Allows us to use the + operator to add 2 Stats together.
        public static Stats operator +(Stats s1 , Stats s2)
        {
            Stats result = new Stats();
            result.name = s2.name ?? s1.name;
            result.description = s2.description ?? s1.description;
            result.projectilePrefab = s2.projectilePrefab ?? s1.projectilePrefab;
            result.auraPrefab = s2.auraPrefab ?? s1.auraPrefab;
            result.lifespan = s1.lifespan + s2.lifespan;
            result.hitEffect = s2.hitEffect == null ? s1.hitEffect : s2.hitEffect ;
            result.spawnVariance = s2.spawnVariance;
            result.damage = s1.damage + s2.damage;
            result.damageVariance = s1.damageVariance + s2.damageVariance;
            result.area = s1.area + s2.area;
            result.speed = s1.speed + s2.speed;
            result.cooldown = s1.cooldown + s2.cooldown;
            result.number = s1.number + s2.number;
            result.piercing = s1.piercing + s2.piercing;
            result.projectileInterval = s1 .projectileInterval + s2.projectileInterval;
            result.knockback = s1 .knockback + s2.knockback;
            return result;
        }

        public float GetDamage()
        {
            return damage + Random.Range(0, damageVariance);
        }

    }
    protected Stats currentStats;
    public WeaponData data;
    protected float currentCooldown;
    protected PlayerMovement movement;

    public virtual void Initialise(WeaponData data)
    {
       
        base.Initialise(data);
        this.data = data;
        currentStats = data.baseStats;
        movement = GetComponentInParent<PlayerMovement>();
        ActiveCooldown(false);
    }


    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)
        {
            Attack(currentStats.number + owner.Stats.amount);
        }
    }


    // Level up by 1, and calcualates the corresponding baseStats
    public override bool DoLevelUp()
    {
        base.DoLevelUp();
        //Prevent level up if we are already at max level
        if (!CanLevelUp())
        {
            Debug.LogWarning(string.Format("Can't level up {0}, max level already reached.", name));
            return false;
        }
        //otherwise, add baseStats of the next level to our weapon
        currentStats += data.GetLevelData(++currentLevel);

        return true;
    }

    // ckeck cooldown to weapon can attack at this current moment
    public virtual bool CanAttack()
    {
        return currentCooldown <= 0;
    }

    // Perform an attack with the weapon return true if the attack was successful
    protected virtual bool Attack(int attackCount = 1)
    {
        if(CanAttack())
        {
            ActiveCooldown();
            return true;
        }
        return false;
    }

    // Get the amount of damage that the weapon is supposed to deal.Factoring in the weapon's stats(including damage variance),as well as the character's Might stats
    public virtual float GetDamage()
    {
        return currentStats.GetDamage() * owner.Stats.might;
    }

    // Get the area, including modifications from the player's stats
    public virtual float GetArea()
    {
        return currentStats.area * owner.Stats.area;
    }

    public virtual float GetSpeed()
    {
        return currentStats.speed * owner.Stats.moveSpeed;
    }
    public virtual float GetDuration()
    {
        return currentStats.lifespan * owner.Stats.duration;
    }    
    // for retrieving the weapon's stats
    public virtual Stats GetStats() { return currentStats; }

    // refresh the cooldown of the weapon. if <strict> is true,refreshes only when currentCooldown <0
    public virtual bool ActiveCooldown(bool strict = false)
    {
        // When <strict> is enabled and the cooldown is not yet finished,
        // do not refresh the cooldown
        if(strict && currentCooldown > 0) return false;
        
        // calculate what the cooldown is going to be, factoring in the cooldown reduce stat in the player character
        float actualCooldown = currentStats.cooldown * Owner.Stats.cooldown;

        // Limit the maximum cooldown to the actual cooldown so we can not increase
        // the cd above the cd stat if we accidentally call this function multiple times.
        //currentCooldown += currentStats.cooldown * Owner.Stats.cooldown;

        currentCooldown = Mathf.Min(actualCooldown, currentCooldown + actualCooldown);
        return true;
    }
}
