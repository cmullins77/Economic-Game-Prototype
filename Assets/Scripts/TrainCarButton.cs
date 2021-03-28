using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCarButton : MonoBehaviour
{
    public int indexOfCar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectCar() {
        FindObjectOfType<Stop>().addCarToSwap(indexOfCar);
    }
}
