using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Passive Data", menuName = "NS/Passive Data")]
public class PassiveData : ItemData
{
    public Passive.Modifier baseStats;
    public Passive.Modifier[] growth;

    public Passive.Modifier GetLevelData(int level)
    {
        if (level - 2 < growth.Length)
            return growth[level - 2];

        // return an empty value
        Debug.LogWarning(("Passive doesn't have its level up baseStats configured for level {0}!", level.ToString()));
        return new Passive.Modifier();
    }
}
