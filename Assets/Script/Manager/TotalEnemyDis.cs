using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TotalEnemyDis : MonoBehaviour
{
    public TextMeshProUGUI totalEnemyKillText;

    void Start()
    {
        int totalEnemyKill = PlayerPrefs.GetInt("TotalEnemyKill", 0);

        totalEnemyKillText.text = totalEnemyKill.ToString();
    }
}
