using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public float lifespan = 0.5f;

    [Header("Bonuses")]
    public int expriences;
    public int health;
    public int coin;
 
    protected PlayerStats target; // If the pickup has a target, then fly towards the target

    protected float speed; // the speed at which  the pick up travels.
    Vector2 initialPosition;
    float initialOffset;

    // To represent the bobbingAnimation animation of the object
    [System.Serializable]
    public struct BobbingAnimation
    {
        public float frequency; // speed
        public Vector2 direction;
    }

    public BobbingAnimation bobbingAnimation = new BobbingAnimation { frequency = 2f, direction = new Vector2(0, 0.3f) };

    protected virtual void Start()
    {
        initialPosition = transform.position;
        initialOffset = Random.Range(0,bobbingAnimation.frequency);
    }
    protected virtual void Update()
    {
        if(target)
        {
            // Move it towards the player and check the distance btw
            Vector2 distance = target.transform.position - transform.position;
            if (distance.sqrMagnitude > speed * speed * Time.deltaTime)
            {
                transform.position += (Vector3)distance.normalized * speed * Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // Handle the animation of the object
            transform.position = initialPosition + bobbingAnimation.direction * Mathf.Sin(Time.time + initialOffset) * bobbingAnimation.frequency;
        }
    }
    public virtual bool Collect(PlayerStats target,float speed,float lifespan = 0)
    {
        if(!this.target)
        {
            this.target = target;
            this.speed = speed;
            if(lifespan > 0) { this.lifespan = lifespan; }
            Destroy(gameObject,Mathf.Max(0.01f,this.lifespan));
            return true;
        }
        return false;
    }

    protected virtual void OnDestroy()
    {
        if (!target ) return; 
        target.IncreaseExperience(expriences);
        target.RestoreHealth(health);
       
        GameManager.instance.AddCoinToCurrentMatch(coin);
    }

}
