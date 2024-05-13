using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    private PlayerStats player;

    [System.Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefabs;
        public bool isRare;
        public float dropRate;
    }

    public List<Drops> drops;

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
    }
    private void OnDestroy() // vat pham dc khoi tao truoc khi quai vat chet
    {
        if(!gameObject.scene.isLoaded) // ham nay chi duoc chay khi trinh chinh sua chay
        {
            return;
        }
        float randomNumber = UnityEngine.Random.Range(0, 100);
        List<Drops> possibleDrops = new List<Drops>();
        foreach (Drops rate in drops)
        {
            if (rate.isRare && player != null)
            {
                float luck = player.GetLuck();
                rate.dropRate = rate.dropRate * luck; 
            }
           
            if(randomNumber < rate.dropRate)
            {
                possibleDrops.Add(rate);
            }
        }
        // kiem tra xem co the roi hay ko khi co nhieu vat pham tren 1 quai random ra 
        if(possibleDrops.Count > 0)
        {
            Drops drops = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
            Instantiate(drops.itemPrefabs, transform.position, Quaternion.identity);
        }
    }
}
