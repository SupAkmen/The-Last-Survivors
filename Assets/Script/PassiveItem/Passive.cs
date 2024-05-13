using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// quan ly viec nang cap cac cat pham tang stat cho nguoi choi
/// </summary>
public class Passive : Item
{
    public PassiveData data;
    [SerializeField] CharacterData.Stats currentBoosts;
    

    [System.Serializable]
    public struct Modifier
    {
        public string name, description;
        public CharacterData.Stats boosts;
    }

    public virtual void Initialise(PassiveData data)
    {
        base.Initialise(data);
        this.data = data;
        currentBoosts = data.baseStats.boosts;
    }
    public virtual CharacterData.Stats GetBoosts()
    {
        return currentBoosts;
    }

    //public virtual bool CanLevelUp()
    //{
    //    return currentLevel <= data.maxLevel;
    //}

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
        currentBoosts += data.GetLevelData(++currentLevel).boosts;

        return true;
    }
   
}
