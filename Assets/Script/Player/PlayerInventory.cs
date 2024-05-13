using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [System.Serializable]
    public class Slot
    {
        public Item item;
        public Image image;

        public void Assign(Item assignedItem)
        {
            item = assignedItem;
            if (item is Weapon)
            {
                Weapon w = item as Weapon;
                image.enabled = true;
                image.sprite = w.data.icon;
            }
            else
            {
                Passive p = item as Passive;
                image.enabled = true;
                image.sprite = p.data.icon;
            }
        
        }

        public void Clear()
        {
            item = null;
            image.enabled = false;
            image.sprite=null;
        }

        public bool IsEmpty()
        {
            return item == null;
        }
    }
    public List<Slot> weaponSlots = new List<Slot>(6);
    public List<Slot> passiveSlots = new List<Slot>(6);

    [System.Serializable]
    public class UpgardeUI
    {
        public TextMeshProUGUI upgradeNameDisplay;
        public TextMeshProUGUI upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    [Header("UI Elements")]
    public List<WeaponData> availableWeapons = new List<WeaponData>();// list of upgrade options for weapon
    public List<PassiveData> availablePassives = new List<PassiveData>();
    public List<UpgardeUI> upgradeUIOptions = new List<UpgardeUI>();
   
    PlayerStats player;

    private void Start()
    {
        player = GetComponent<PlayerStats>();
    }


    // Check if the inventory ha an item of a certain type.
    public bool Has(ItemData type) { return Get(type); }

    // Find a weapon of a certain type in the inventory
    public Weapon Get(WeaponData type)
    {
        foreach (Slot s in weaponSlots)
        {
            Weapon w = s.item as Weapon;
            if (w.data == type)
                return w;
        }
        return null;
    }

    // Find a passive of a certain type in the inventory
    public Passive Get(PassiveData type)
    {
        foreach (Slot s in passiveSlots)
        {
            Passive p = s.item as Passive;
            if (p.data == type)
                return p;
        }
        return null;
    }
    public Item Get(ItemData type)
    {
        if (type is WeaponData) return Get(type as WeaponData);
        else if (type is PassiveData) return Get(type as PassiveData);
        return null;
    }
    // Clear a weapon of a particular type, as specified by <data>
    public bool Remove(WeaponData data, bool removeUpgradeAvailality = false)
    {
        // Remove this weapon from the upgrade pool.
        if(removeUpgradeAvailality) availableWeapons.Remove(data);
        for(int i= 0; i < weaponSlots.Count; i++)
        {
            Weapon w = weaponSlots[i].item as Weapon;
            if (w.data == data)
            {
                weaponSlots[i].Clear();
                w.OnUnequip();
                Destroy(w.gameObject);
                return true;
            }
        }

        return false;
    }
    // Clear a passive of a particular type, as specified by <data>
    public bool Remove(PassiveData data, bool removeUpgradeAvailality = false)
    {
        // Remove this weapon from the upgrade pool.
        if (removeUpgradeAvailality) availablePassives.Remove(data);
        for (int i = 0; i < passiveSlots.Count; i++)
        {
            Passive p = passiveSlots[i].item as Passive;
            if (p.data == data)
            {
                passiveSlots[i].Clear();
                p.OnUnequip();
                Destroy(p.gameObject);
                return true;
            }
        }

        return false;
    }
    // if itemdata is passed, determine what type it is and call the respective overload.
    public bool Remove(ItemData data,bool removeUpgradeAvailality = false)
    {
        if(data is PassiveData) return Remove(data as PassiveData, removeUpgradeAvailality);
        if(data is WeaponData) return Remove(data as WeaponData, removeUpgradeAvailality);
        return false;
    }
    // Finds an empty slot and add a weapon of certain type.
    public int Add(WeaponData data)
    {
        int slotNum = -1;

        // Try to find an empty slot
        for(int i = 0;i<weaponSlots.Capacity;i++)
        {
            if (weaponSlots[i].IsEmpty())
            {
                slotNum = i;
                break;
            }
        }

        // if there is no empty slot,exit
        if (slotNum < 0) return slotNum;

        // otherwise crate the weapon int the slot.Get the type of the weapon we want to spwan
        Type weaponType = Type.GetType(data.behaviour);

        if (weaponType != null)
        {
            // spawn the weapon gameobject
            GameObject go = new GameObject(data.baseStats.name + "Controller");
            Weapon spawnedWeapon = (Weapon)go.AddComponent(weaponType);
            
            spawnedWeapon.transform.SetParent(transform); // vk la con cua nguoi choi
            spawnedWeapon.transform.localPosition = Vector2.zero;
            spawnedWeapon.Initialise(data);
            spawnedWeapon.OnEquip();
           

            // Assign the weapon to the slot
            weaponSlots[slotNum].Assign(spawnedWeapon);
          
            if (GameManager.instance != null && GameManager.instance.choosingUpgrade) // sau khi chon nang cap thi game se tiep tuc
            {
                GameManager.instance.EndLevelUp();
            }

            return slotNum;
        }
        else
        {
            Debug.LogWarning(string.Format("Invalid weapon type for {0}.", data.name));
        }

        return -1;
    }
    public int Add(PassiveData data)
    {
        int slotNum = -1;

        // Try to find an empty slot
        for (int i = 0; i < passiveSlots.Capacity; i++)
        {
            if (passiveSlots[i].IsEmpty())
            {
                slotNum = i;
                break;
            }
        }

        // if there is no empty slot,exit
        if (slotNum < 0) return slotNum;

        // otherwise crate the passive in the slot.
        // Get the type of the passive we want to spwan
        GameObject go = new GameObject(data.baseStats.name + "Passive");
        Passive p = go.AddComponent<Passive>();
        p.Initialise(data);
        p.transform.SetParent(transform); // vk la con cua nguoi choi
        p.transform.localPosition = Vector2.zero;

        // Assign the item to the slot
        passiveSlots[slotNum].Assign(p);
      

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
        player.RecalculateStats();

        return slotNum;
    }
    public int Add(ItemData data)
    {
        if(data is WeaponData) return Add(data as WeaponData);
        else if (data is PassiveData) return Add(data as PassiveData);
        return -1;
    }

    public void LevelUpWeapon(int slotIndex, int upgradeIndex)
    {
        if (weaponSlots.Count > slotIndex)
        {
            Weapon weapon = weaponSlots[slotIndex].item as Weapon;

            if (!weapon.DoLevelUp())
            {
                Debug.LogWarning(string.Format("Fail to level up {0}.", weapon.name));
                return;
            }

            // weapon.DoLevelUp();
        }

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }

    }

    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
    {
        if (passiveSlots.Count > slotIndex)
        {
            Passive p = passiveSlots[slotIndex].item as Passive;
            if(!p.DoLevelUp())
            {
                Debug.LogWarning(string.Format("Failed to level up {0} ", p.name));
                return;
            }

            if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
            {
                GameManager.instance.EndLevelUp();
            }
        }
        player.RecalculateStats();
    }

    // Xac dinh loai nang cap nao ne xuaat hien
    void ApplyUpgradeOptions()
    {
        // Make a duplicate of the availble weapon/ passive ungrade lists 
        // so we can iterate through them in the function
        List<WeaponData> availbleWeaponUpgrades = new List<WeaponData>(availableWeapons);
        List<PassiveData> availblePassiveItemUpgrades = new List<PassiveData>(availablePassives); //

        //Iterate through each slot in the upgrade UI
        foreach (UpgardeUI upgradeOption in upgradeUIOptions)
        {
            // if there are no more availble upgrade, then we abort
            if (availbleWeaponUpgrades.Count == 0 && availblePassiveItemUpgrades.Count == 0) // neu chua co vk hoac vat pham nao dc chon
            {
                return;
            }

            // determine wheter this upgrade shoulb be for passive or active weapons.
            int upgradeType;

            if (availbleWeaponUpgrades.Count == 0)
            {
                upgradeType = 2;
            }
            else if (availblePassiveItemUpgrades.Count == 0)
            {
                upgradeType = 1;
            }
            else
            {
                upgradeType = UnityEngine.Random.Range(1, 3);
            }

            // Generates an active weapon upgrade
            if (upgradeType == 1)
            {
                // chon 1 vu khi va xoa no di de trong hang cho ko co 2 vk giong nhau
                WeaponData chosenWeaponUpgrade = availbleWeaponUpgrades[UnityEngine.Random.Range(0, availbleWeaponUpgrades.Count)];
                availbleWeaponUpgrades.Remove(chosenWeaponUpgrade);// xoa di cac vk da dc chon de ds chi gom cac vk khac nhau

                // Ensure that the selected weapon data is valid
                if (chosenWeaponUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption); // goi khi vk dc dua len UI

                    //Loop through all our existing weapons. if we find a match, we will hook an event listener to the button
                    // that will level up the weapon when this upgrade option is clicked.

                    bool isLevelUp = false;
                    for (int i = 0; i < weaponSlots.Count; i++)
                    {
                        Weapon w = weaponSlots[i].item as Weapon;
                        if (w != null && w.data == chosenWeaponUpgrade) // ktra vu khi da ton tai hay chua
                        {

                            if (chosenWeaponUpgrade.maxLevel <= w.currentLevel) // ktra khi vk toi muc toi da se ko con nan cap nua
                            {
                                DisableUpgradeUI(upgradeOption); // neu vk dat max ko can dua vao UI nua
                                isLevelUp = true;
                                //isLevelUp = false;
                                break;
                            }

                            upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, i));
                         
                            // set des and name to be that of the next level
                            Weapon.Stats nextLevel = chosenWeaponUpgrade.GetLevelData(w.currentLevel + 1);
                            upgradeOption.upgradeDescriptionDisplay.text = nextLevel.description;
                            upgradeOption.upgradeNameDisplay.text = nextLevel.name;
                            upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.icon;
                            isLevelUp = true;
                            break;
                        }
                    }

                    // if the code gets here, it means that we will be adding a new weapon , instead of upgrading an existing weapon.
                    if (!isLevelUp)
                    {

                        upgradeOption.upgradeButton.onClick.AddListener(() => Add(chosenWeaponUpgrade)); // apply button function
                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.baseStats.description;
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.baseStats.name;
                        upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.icon;
                    }

                }


            }
            else if (upgradeType == 2)
            {
                PassiveData chosenPassiveUpgrade = availblePassiveItemUpgrades[UnityEngine.Random.Range(0, availblePassiveItemUpgrades.Count)];
                availblePassiveItemUpgrades.Remove(chosenPassiveUpgrade);


                if (chosenPassiveUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);


                    bool isLevelUp = false;

                    for (int i = 0; i < passiveSlots.Count; i++)
                    {
                        Passive p = passiveSlots[i].item as Passive;
                        if (p != null && p.data == chosenPassiveUpgrade) // ktra vu khi da ton tai hay chua
                        {

                            if (chosenPassiveUpgrade.maxLevel <= p.currentLevel) // ktra khi vk toi muc toi da se ko con nan cap nua
                            {
                                DisableUpgradeUI(upgradeOption); // neu vk dat max ko can dua vao UI nua
                                //isLevelUp = false;
                                isLevelUp = true;
                                break;
                               
                            }
                            upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, i));
                            Passive.Modifier nextLevel = chosenPassiveUpgrade.GetLevelData(p.currentLevel + 1);
                            upgradeOption.upgradeDescriptionDisplay.text = nextLevel.description;
                            upgradeOption.upgradeNameDisplay.text = nextLevel.name;
                            upgradeOption.upgradeIcon.sprite = chosenPassiveUpgrade.icon;
                            isLevelUp = true;
                            break;
                        }

                    }


                    if (!isLevelUp)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() =>Add(chosenPassiveUpgrade));
                        Passive.Modifier nextLevel = chosenPassiveUpgrade.baseStats;
                        upgradeOption.upgradeDescriptionDisplay.text = nextLevel.description;
                        upgradeOption.upgradeNameDisplay.text = nextLevel.name;
                        upgradeOption.upgradeIcon.sprite = chosenPassiveUpgrade.icon;
                    }

                }

            }
        }
    }

    void RemoveUpgradeoptions()
    {
        foreach (UpgardeUI upgradeOption in upgradeUIOptions)
        {

            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption); // disable all UI option before applying upgrades to them
        }
    }

    public void RemoveAndApplyUpgrades() // co the su dung sendmessage de goi ham nay 1 cach gian tiep maf ko can truc tiep truy cap
    {
        RemoveUpgradeoptions();
        ApplyUpgradeOptions();
    }

    void DisableUpgradeUI(UpgardeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(false); // truy xuat vao doi tuong cha
    }

    void EnableUpgradeUI(UpgardeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(true); // truy xuat vao doi tuong cha

    }

   
}


