using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCar : MonoBehaviour
{
    public CarType type;
    public float maxCapacity;
    public float currentCapacity;
    public float quality;

    public float buyPrice;
    public float sellPrice;

    public float weight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum CarType
{
    Passenger, Livestock, Freight
}
