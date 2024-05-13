using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public int currentLevel = 1,maxLevel = 1;
    protected ItemData.Evolution[] evolutionData;
    protected PlayerInventory inventory;
    protected PlayerStats owner;

    public PlayerStats Owner { get { return owner; } }

    public virtual void Initialise(ItemData data)
    {
        maxLevel = data.maxLevel;

        //store the evolution data as we have to track whether all the catalysts are in the inventory so we can evolve
        evolutionData = data.evolutionData;

        // we have to find a better way to refernce the player inventory in the future as thid is ineffecient
        inventory = FindObjectOfType<PlayerInventory>();
        owner = FindObjectOfType<PlayerStats>();
    }

    // Call this function to get all the evolutions that the weapon can currently evolve to.
    public virtual ItemData.Evolution[] CanEvolve()
    {
        List<ItemData.Evolution> possibleEvolutions = new List<ItemData.Evolution>();
        // check aech listed evolution and whether it is in the inventory
        
        foreach(ItemData.Evolution e in evolutionData)
        {
           
            if(CanEvolve(e)) { possibleEvolutions.Add(e); }
        }

        return possibleEvolutions.ToArray();
    }

    // Check if a specific evolution is possible
    public virtual bool CanEvolve(ItemData.Evolution evolution, int levelUpAmount = 1)
    {
        // Can't evolve if the item hasn't reached the level to evolve.
        if (evolution.evolutionLevel > currentLevel + levelUpAmount) return false;

        // Check to see if all the catalysts are in the inventory
        foreach (ItemData.Evolution.Config c in evolution.catalysts)
        {
            Item item = inventory.Get(c.itemType);
            if (!item || item.currentLevel < c.level)
            {
                return false;
            }
        }

        return true;
    }

    // AttemptEvolution will spawn a new weapon for the charcter, and remove all the weapons that are  suspposed to be consumed.

    public virtual bool AttemptEvolution(ItemData.Evolution evolutionData, int levelUpAmount=1)
    {
        if(!CanEvolve(evolutionData,levelUpAmount)) return false;
        // should we consume pass/wea?
        bool consumesPassives = (evolutionData.consumes & ItemData.Evolution.Consumption.passives) > 0;
        bool consumesWeapons = (evolutionData.consumes & ItemData.Evolution.Consumption.weapons) > 0;

        // Loop through all the catalysst and check if we should consume them.
        foreach(ItemData.Evolution.Config c in evolutionData.catalysts)
        {
            if (c.itemType is PassiveData && consumesPassives) inventory.Remove(c.itemType);
            if (c.itemType is WeaponData && consumesWeapons) inventory.Remove(c.itemType);
        }


        // Should we consume ourselves as well ?
        if (this is Passive && consumesPassives)
        {
            inventory.Remove((this as Passive).data,true);
        }
        else if (this is Weapon && consumesWeapons)
        {
            inventory.Remove((this as Weapon).data,true);
        }

        //Add the new weapon onto our inventory
        inventory.Add(evolutionData.outcome.itemType);

        return true;
    }
    public virtual bool CanLevelUp()
    {
        return currentLevel <= maxLevel;
    }

    // Whenever an item levels up, attemp to make it evolve.
    public virtual bool DoLevelUp()
    {
        if(evolutionData == null ) return true;
        //Tries to evolve into every listed evolution.
        foreach (ItemData.Evolution e in evolutionData)
        {
            if(e.codition == ItemData.Evolution.Codition.auto)
            {
                AttemptEvolution(e);
            }
        }
            
        return true;
    }

    public virtual void OnEquip()
    {
        // try to replace the aura the weapon has with a new one
    }

    // what effect are removed on unequipping a weapon
    public virtual void OnUnequip() { }
}
