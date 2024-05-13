using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    [SerializeField] private float timer;
    private TextMeshProUGUI text;
    private float startTimer;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        startTimer = 0;
        
    }

    private void Update()
    {
        if (startTimer < timer)
        {
            startTimer += Time.deltaTime;
            text.fontSize += .15f;
        }
        else if (startTimer > timer)
        {
            startTimer += Time.deltaTime;
            text.fontSize -= .15f;
            if (startTimer >= timer * 2)
            {
                startTimer = 0;
            }
        }
    }
}
