using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector instance;
    public CharacterData characterData;

    public Button unLockButton;
    public Button startButton;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    public static CharacterData GetData()
    {

        if (instance && instance.characterData)
            return instance.characterData;
        else
        {
#if UNITY_EDITOR
            // giup lay ngau nhien nhan vat ma ko can tu man hinh chon menu
            // if no character data is assigned, we randomly pick one

            string[] allAssetsPaths = AssetDatabase.GetAllAssetPaths();
            List<CharacterData> characters = new List<CharacterData>();

            foreach (string assetPath in allAssetsPaths)
            {
                if (assetPath.EndsWith(".asset"))
                {
                    CharacterData characterData = AssetDatabase.LoadAssetAtPath<CharacterData>(assetPath);
                    if (characterData != null)
                    {
                        characters.Add(characterData);
                    }
                }
            }

            if (characters.Count > 0) return characters[Random.Range(0, characters.Count)];
#endif
        }

        return null;
    }
    public void SelectCharacter(CharacterData character)
    {
        characterData = character;

        if (characterData.Price == 0)
        {
            characterData.isUnlocked = true;
        }
        else
        {
            characterData.isUnlocked = PlayerPrefs.GetInt(characterData.name, 0) == 0 ? false : true;
        }

        UpdateUI();

    }

    public static void DestroySingleton()
    {
        if (instance) Destroy(instance.gameObject);

    }

    public void UpdateUI()
    {
        if (characterData.isUnlocked == true)
        {
            unLockButton.gameObject.SetActive(false);
            startButton.gameObject.SetActive(true);
        }
        else
        {
            if (PlayerPrefs.GetInt("TotalCoin", 0) < characterData.Price)
            {
                unLockButton.gameObject.SetActive(true);
                startButton.gameObject.SetActive(false);
                unLockButton.interactable = false;
            }
            else
            {
                unLockButton.gameObject.SetActive(true);
                unLockButton.interactable = true;
                startButton.gameObject.SetActive(false);
            }
        }

    }


    public void Unlock()
    {
        int coins = PlayerPrefs.GetInt("TotalCoin", 0);
        int price = characterData.Price;
        PlayerPrefs.SetInt("TotalCoin", coins - price);
        PlayerPrefs.SetInt(characterData.name, 1);
        characterData.isUnlocked = true;
        UpdateUI();
    }    
}
