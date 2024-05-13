using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Use for PassiveData and WeaponData
/// </summary>
public abstract class ItemData : ScriptableObject
{
    
    public Sprite icon;
    public int maxLevel;

    [System.Serializable]
    public struct Evolution
    {
        public string name;

        public enum Codition {  auto, truasureChest}
        public Codition codition;

        [System.Flags]
        public enum Consumption { passives = 1, weapons = 2 }
        public Consumption consumes;

        public int evolutionLevel;
        public Config[] catalysts;
        public Config outcome;

        [System.Serializable]
        public struct Config
        {
            public ItemData itemType;
            public int level;
        }
    }

    public Evolution[] evolutionData;
}
