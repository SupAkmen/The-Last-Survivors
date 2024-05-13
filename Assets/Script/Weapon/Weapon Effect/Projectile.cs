using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : WeaponEffect
{
    public enum DamageSource { projectile, owner };
    public DamageSource damageSource = DamageSource.owner;
    public bool hasAutoAim = false;
    public Vector3 rotationSpeed = new Vector3(0, 0, 0);
   
    protected Rigidbody2D rb;
    protected int piercing;

    AudioManager audioManager;
    protected virtual void Start()
    {
      
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        Weapon.Stats stats = weapon.GetStats();
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            rb.angularVelocity = rotationSpeed.z;
            rb.velocity = transform.right * weapon.GetSpeed();

        }

        //prevent the area from being 0, as it hides the projectile.
        //float area = stats.area == 0 ? 1 : stats.area;

        float area = weapon.GetArea();
        if (area <= 0) area = 1;
        transform.localScale = new Vector3(
            area * Mathf.Sign(transform.localScale.x),
            area * Mathf.Sign(transform.localScale.y),
            1f);

        // Set how much piercing this object has
        piercing = stats.piercing;

        // destroy the projectile after its lifesapn expires.
        if (stats.lifespan > 0) Destroy(gameObject, weapon.GetDuration());

        // If the projectile is auto-aiming, automatically dind a suitable enemy
        if (hasAutoAim)
        {
            AcquireAutoAimFacing();
        }
    }

    // If the projectile is auto-aiming, it will automatically dind a suitable target to move towards.

    public virtual void AcquireAutoAimFacing()
    {
        float aimAngle = 0; // determine where to aim

        // Find all enemy on the screen
        EnemyStats[] targets = FindObjectsOfType<EnemyStats>();

        // Select a random enemy ( if there is at least 1) otherwise pick a random angle
        if (targets.Length > 0)
        {
            // thiet lap gtri goc ban toi khi co doi tuong enemy
            EnemyStats selectedTarget = targets[Random.Range(0, targets.Length)];
            Vector2 difference = selectedTarget.transform.position - transform.position;
            aimAngle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }
        else
        {
            aimAngle = Random.Range(0f, 360f);
        }

        // Point the projectile toward where we are aming at.
        transform.rotation = Quaternion.Euler(0, 0, aimAngle);

    }

    protected virtual void FixedUpdate()
    {
        // only drive movement ourselves if this is a kinematic
        
        if (rb.bodyType == RigidbodyType2D.Kinematic)
        {
                transform.position += transform.right * weapon.GetSpeed() * Time.fixedDeltaTime;
                transform.Rotate(rotationSpeed * Time.fixedDeltaTime);
                rb.MovePosition(transform.position);
        }
         
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {

        EnemyStats es = other.GetComponent<EnemyStats>();
        BreakableProps p = other.GetComponent<BreakableProps>();

        
        if (es)
        {
            // if there is an owner, and the damage source is set to owner
            // we will calculate knockback using the owner instead of the projectile.
            Vector3 source = damageSource == DamageSource.owner && owner ? owner.transform.position : transform.position;
            Weapon.Stats stats = weapon.GetStats();
            // Deals the damage and destroys the projectile.
            es.TakeDamage(GetDamage(), source);
            audioManager.PlaySFX(audioManager.attack);
            piercing--;
            if (stats.hitEffect)
            {
                Destroy(Instantiate(stats.hitEffect, transform.position, Quaternion.identity), 5f);
            }
        }
        else if (p != null)
        {
            p.TakeDamage(GetDamage());
            audioManager.PlaySFX(audioManager.attack);
            piercing--;

            Weapon.Stats stats = weapon.GetStats();
            if (stats.hitEffect)
            {
                Destroy(Instantiate(stats.hitEffect, transform.position, Quaternion.identity), 5f);
            }
        }

        if (piercing <= 0)
        {
            Destroy(gameObject);
        }
    }
}