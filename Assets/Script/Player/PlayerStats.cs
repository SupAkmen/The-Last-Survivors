using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerStats : MonoBehaviour
{
    public CharacterData characterData;
    public CharacterData.Stats baseStats;
    [SerializeField] CharacterData.Stats actualStats;

    public CharacterData.Stats Stats
    {
        get { return actualStats; }
        set { actualStats = value; }

    }

     SpriteRenderer spriteRenderer; // Khai báo biến spriteRenderer để truy cập Sprite Renderer của Player
     Animator anim; // Khai báo biến anim để truy cập Animator của Player
    

    float health;

    #region Current Stats Properties
    public float CurrentHealth
    {
        get { return health; }
        set
        {
            if (health != value)
            {
                health = value;
                UpdateHealthBar();
                
            }
        }
    }
   
    #endregion

    [Header("Visuals")]
    public ParticleSystem damageEffect;

    // Experience and level of the player
    [Header("Experience/Level")]
    public float experience;
    public int level = 1;
    public int experienceCap;


    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    // I-Frame : tranh viec nguoi choi nhan sat thuong lien tuc se co thoi gian bat tu
    [Header("I-Frame")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvicible;

    PlayerInventory inventory;
    PlayerCollector collector;
    public int weaponIndex;
    public int passiveItemIndex;

    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public TextMeshProUGUI levelText;


    private void Awake()
    {
        characterData = CharacterSelector.GetData();
        if (CharacterSelector.instance)
            CharacterSelector.DestroySingleton();

        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        inventory = GetComponent<PlayerInventory>();
        collector = GetComponentInChildren<PlayerCollector>();

        baseStats = actualStats = characterData.stats;
        collector.SetRadius(actualStats.magnet);
        
        health = actualStats.maxHealth;

        spriteRenderer.sprite = characterData.CharacterSprite;
        anim.runtimeAnimatorController = characterData.AnimatorController;

    }

    public List<LevelRange> levelRanges;
    private void Start()
    {

        inventory.Add(characterData.StartingWeapon);


        //SpawnWeapon(characterData.StartingWeapon);

        experienceCap = levelRanges[0].experienceCapIncrease;

     

        GameManager.instance.AssignChosenCharacter(characterData);

        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevelText();
    }

    void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvicible)
        {
            isInvicible = false;
        }

        Recover();
    }

    //private RuntimeAnimatorController GenerateRuntimeAnimatorController(AnimationClip clip)
    //{
    //    AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController();
    //    animatorOverrideController.runtimeAnimatorController = anim.runtimeAnimatorController;
    //    animatorOverrideController["MoveAnimationClip"] = clip;
    //    return animatorOverrideController;
    //}
    public void RecalculateStats()
    {
        actualStats = baseStats;
        foreach (PlayerInventory.Slot s in inventory.passiveSlots)
        {
            Passive p = s.item as Passive;
            if (p)
            {
                actualStats += p.GetBoosts();
            }
        }

        // Update the player radius
        collector.SetRadius(actualStats.magnet);
    }
    public void IncreaseExperience(int amount)
    {
        experience += amount * actualStats.growth;
        UpdateExpBar();
        LevelUpChecker();
    }

    public void RestoreHealth(float amount)
    {
        if (CurrentHealth < baseStats.maxHealth)
        {
            CurrentHealth += amount;
            if (CurrentHealth > baseStats.maxHealth)
            {
                CurrentHealth = baseStats.maxHealth;
            }

            UpdateHealthBar();
        }
    }


    void LevelUpChecker()
    {
        if (experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }

            }

            experienceCap += experienceCapIncrease;

            UpdateLevelText();

            GameManager.instance.StartLevelUp();
        }
    }


    public void TakeDamage(float dmg)
    {
        if (!isInvicible)
        {
            // giam sat thuong (amor) trc khi nhan sat thuong
            dmg -= actualStats.armor;

            if(dmg >0)
            {
                // nhan dam
                CurrentHealth -= dmg;

                if (damageEffect)
                {
                    Instantiate(damageEffect, transform.position, Quaternion.identity);
                }
                if (CurrentHealth <= 0)
                {
                    Kill();
                }
            }


            invincibilityTimer = invincibilityDuration;
            isInvicible = true;
        }

    }
    void UpdateExpBar()
    {
        expBar.fillAmount = (float)experience / experienceCap;
    }
    void UpdateLevelText()
    {
        levelText.text = level.ToString();
    }
    void UpdateHealthBar()
    {
        healthBar.fillAmount = CurrentHealth / baseStats.maxHealth;
    }
    public void Kill()
    {
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReachedUI(level);

           
            List<Image> weaponImages = new List<Image>();
            List<Image> passiveItemImages = new List<Image>();

            foreach (var slot in inventory.weaponSlots)
            {
                weaponImages.Add(slot.image);
            }

            foreach (var slot in inventory.passiveSlots)
            {
                passiveItemImages.Add(slot.image);
            }

            GameManager.instance.AssignChosenWeaponAndPassiveItemUI(weaponImages, passiveItemImages);

            GameManager.instance.GameOver();
        }
    }




    void Recover()
    {
        if (CurrentHealth < baseStats.maxHealth)
        {
            CurrentHealth += Stats.recovery * Time.deltaTime;
            if (CurrentHealth > baseStats.maxHealth)
            {
                CurrentHealth = baseStats.maxHealth;
            }
            UpdateHealthBar();

        }
    }

    public float GetLuck()
    {
        return Stats.luck;
    }

}
