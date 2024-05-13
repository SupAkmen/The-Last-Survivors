using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MapSelection : MonoBehaviour
{
    [System.Serializable]
    public class Map
    {
        public string name;
        public Button btnMap;
        public bool isUlocked;
        public int range;
    }    

    public List<Map> maps;


    private void Update()
    {
        Unlock();
    }

    void Unlock()
    {
        foreach (Map map in maps)
        {
            if(map.range == 0)
            {
                map.isUlocked = true;
            }
            else
            {
                map.isUlocked = PlayerPrefs.GetInt(map.name, 0) == 0 ? false : true;
            }
        }

        UpdateUI();
    }
    void UpdateUI()
    {
        foreach(Map map in maps)
        {
            if(map.isUlocked == true)
            {
                map.btnMap.gameObject.SetActive(true);
            }    
            else
            {
               if(PlayerPrefs.GetInt("TotalEnemyKill",0) < map.range)
                {
                    map.btnMap.gameObject.SetActive(true);
                    map.btnMap.interactable = false;
                }
                else
                {
                    map.btnMap.gameObject.SetActive(true);
                    map.btnMap.interactable = true;
                }
            }    
        }
       
    }    
}
