using UnityEngine;
using System;

[CreateAssetMenu(fileName = "CharacterData", menuName = "NS/Character Data")]
public class CharacterData : ScriptableObject
{
    [SerializeField]
    Sprite icon;
    public Sprite Icon { get => icon; private set => icon = value; }
    [SerializeField]
    new string name;
    public string Name { get => name; private set => name = value; }
    [SerializeField]
    string description;
    public string Description { get => description; private set => description = value; }
    [SerializeField]
    int price;
    public int Price { get => price; private set => price = value; }
    [SerializeField]
    public bool isUnlocked;
    [SerializeField]
    private Sprite characterSprite;
    public Sprite CharacterSprite { get => characterSprite; private set => characterSprite = value; }

    [SerializeField]
    private Sprite startingWeaponSprite;
    public Sprite StartingWeaponSprite { get => startingWeaponSprite; private set => startingWeaponSprite = value; }

    [SerializeField]
    WeaponData startingWeapon;
    public WeaponData StartingWeapon { get => startingWeapon; private set => startingWeapon = value; }
    [SerializeField]
    private RuntimeAnimatorController animatorController;
    public RuntimeAnimatorController AnimatorController { get => animatorController; }

    [System.Serializable]
    public struct Stats
    {
        public float maxHealth, recovery, armor;
        [Range(-1, 10)] public float moveSpeed, might, area;
        [Range(-1, 5)] public float speed, duration;
        [Range(-1, 10)] public int amount;
        [Range(-1, 1)] public float cooldown;
        [Min(-1)]public float luck, growth;
        public float magnet;

        public static Stats operator +(Stats s1, Stats s2)
        {
            s1.maxHealth += s2.maxHealth;
            s1.recovery += s2.recovery;
            s1.armor += s2.armor;
            s1.moveSpeed += s2.moveSpeed;
            s1.might += s2.might;
            s1.speed += s2.speed;
            s1.duration += s2.duration;
            s1.amount += s2.amount;
            s1.cooldown += s2.cooldown;
            s1.luck += s2.luck;
            s1.growth += s2.growth;
           
            s1.magnet += s2.magnet;
            return s1;

        }
    }


    public Stats stats = new Stats
    {
        maxHealth = 100,recovery =0,armor=0, moveSpeed = 1, might = 1, area = 1,
        speed=1,duration=1,amount = 0,cooldown=1,luck = 1,growth = 1,
    };
}