using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInfoDisplay : MonoBehaviour
{
   
    public GameObject playerInfoPanel; 
    public TextMeshProUGUI nameText; 
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI maxHealth;
    public TextMeshProUGUI recovery;
    public TextMeshProUGUI armor;
    public TextMeshProUGUI moveSpeed;
    public TextMeshProUGUI might;
    public TextMeshProUGUI area;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI duration;
    public TextMeshProUGUI amount;
    public TextMeshProUGUI cooldown;
    public TextMeshProUGUI luck;
    public TextMeshProUGUI growth;
    public TextMeshProUGUI greed;
    public TextMeshProUGUI revival;
    public TextMeshProUGUI magnet;
    public Image characterImage; 
    public Image startingWeaponImage;
    public Image uiStatDisplay;
    public TextMeshProUGUI price;

    public CharacterData characterData;
    public CharacterData.Stats baseStats;

    void Start()
    {
        // Ẩn panel khi game bắt đầu
        playerInfoPanel.SetActive(false);
      
    }

    public void ShowPlayerInfo()
    {
        characterData = CharacterSelector.GetData();
        baseStats  = characterData.stats;
        // Hiển thị panel thông tin người chơi
        playerInfoPanel.SetActive(true);


        // Hiển thị tên, mô tả và hình ảnh của người chơi tương ứng với button
        nameText.text = characterData.Name; 
        descriptionText.text = characterData.Description;
        characterImage.sprite = characterData.Icon;
        startingWeaponImage.sprite = characterData.StartingWeaponSprite;
        price.text = characterData.Price.ToString();
        maxHealth.text =  baseStats.maxHealth.ToString();
        recovery.text =  baseStats.recovery.ToString();
        armor.text =   baseStats.armor.ToString();
        moveSpeed.text = baseStats.moveSpeed.ToString();
        might.text =     baseStats.might.ToString();
        area.text =      baseStats.area.ToString();
        speed.text =     baseStats.speed.ToString();
        duration.text =  baseStats.duration.ToString();
        amount.text =    baseStats.amount.ToString();
        luck.text =      baseStats.luck.ToString();
        cooldown.text =  baseStats.cooldown.ToString();
        growth.text =    baseStats.growth.ToString();
        magnet.text =    baseStats.magnet.ToString();
    }

    public void HidePlayerInfo()
    {
        // Ẩn panel thông tin người chơi
        playerInfoPanel.SetActive(false);
    }
}
