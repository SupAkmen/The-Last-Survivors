using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D detector;
    public float pullSpeed;

    AudioManager audioManager;
    private void Start()
    {
        player = GetComponentInParent<PlayerStats>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }

   public void SetRadius(float r)
    {
        if(!detector) detector = GetComponent<CircleCollider2D>();
        detector.radius = r;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PickUp p))
        {
            p.Collect(player,pullSpeed);
            audioManager.PlaySFX(audioManager.pickUp);
        }
    }

    //private void Update()
    //{
    //   // playerCollector.radius = player.Magnet;

    //    //FInd all collider withim the radius of the collector objects

    //    Collider2D[] allPossibleTragets = Physics2D.OverlapCircleAll(transform.position, player.Magnet);
    //    foreach (Collider2D col in allPossibleTragets)
    //    {
    //        PickUp p = col.GetComponent<PickUp>();
    //        if (p != null)
    //        {
    //            p.Collect(player, pullSpeed);
    //        }
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{

    //    if (collision.gameObject.TryGetComponent(out ICollectable collectable))
    //    {
    //        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
    //        Vector2 forceDirection = ((transform.position - collision.transform.position)).normalized;
    //        rb.AddForce(forceDirection * pullSpeed);
    //        collectable.Collect();
    //    }
    //}

}
