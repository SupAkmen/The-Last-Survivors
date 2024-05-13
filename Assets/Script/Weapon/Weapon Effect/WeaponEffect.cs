using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponEffect : MonoBehaviour
{
    [HideInInspector] public PlayerStats owner;
    [HideInInspector]  public Weapon weapon;

    // make it possible to access owner using capital letter as well
    // this maintain consistency btw naming convention across diff classes
    public PlayerStats Owner { get { return owner; } }
    public float GetDamage()
    {
        //return weapon.GetDamage() * owner.Stats.might;
        return weapon.GetDamage();
    }
}
