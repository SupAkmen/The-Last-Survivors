using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float frequency; // speed
    public float magnitude; // range
    public Vector3 direction;
    Vector3 initialPos;
    PickUp pickUp;

    private void Start()
    {
        pickUp = GetComponent<PickUp>();
        initialPos = transform.position;
    }

    private void Update()
    {
        //if(pickUp && !pickUp.hasBeenCollected)
        //{
        transform.position = initialPos + direction * Mathf.Sin(Time.time * frequency) * magnitude;
        //}
    }
}
