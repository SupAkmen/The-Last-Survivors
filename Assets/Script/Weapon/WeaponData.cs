using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 /// <summary>
 /// Thay the cho WSO de co the luu tru tat ca du lieu vu khi vao 1 object thay vi nhieu object cho 1 vu khi
 /// </summary>
[CreateAssetMenu(fileName = "Weapon Data", menuName = "NS/Weapon Data")]
public class WeaponData : ItemData
{
    [HideInInspector]public string behaviour;
   
    public Weapon.Stats baseStats;
    
    public Weapon.Stats[] linearGrowth;
    public Weapon.Stats[] randomGrowth;
   

    // Give us the stat  growth/ description of the next level
    
    public Weapon.Stats GetLevelData(int level)
    {
        if(level - 2 < linearGrowth.Length)
            return linearGrowth[level - 2];
        if(randomGrowth.Length > 0)
            return randomGrowth[Random.Range(0,randomGrowth.Length)];

        // return an empty value
        Debug.LogWarning(("Weapon doesn't have its level up baseStats configured for level {0}!",level.ToString()));
        return new Weapon.Stats();
    }

}
