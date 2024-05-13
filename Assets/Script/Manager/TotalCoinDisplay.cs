using UnityEngine;
using TMPro;

public class TotalCoinDisplay : MonoBehaviour
{
    public TextMeshProUGUI totalCoinText;

    void Start()
    {
        int totalCoin = PlayerPrefs.GetInt("TotalCoin",0);

        totalCoinText.text = totalCoin.ToString();
    }
}
