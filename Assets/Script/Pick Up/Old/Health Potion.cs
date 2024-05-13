using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : PickUp
{
    public int healthToRestore;

    //    public override bool Collect(Transform target, float speed, float lifespan = 0)
    //    {
    //        if (base.Collect(target, speed, lifespan))
    //        {
    //            PlayerStats player = target.GetComponentInParent<PlayerStats>() ?? target.GetComponentInChildren<PlayerStats>();
    //            if (player != null)
    //            {
    //                player.RestoreHealth(healthToRestore);
    //            }
    //            return true;
    //        }
    //        return false;
    //    }
}
