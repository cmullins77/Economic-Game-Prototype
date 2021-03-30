using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellItem : MonoBehaviour
{
    public Sprite sprite;
    public string info;
    public int index;
    public float price;
    public int quantity;
    public string nameString;

    public GameObject prefab;
    public bool isTrainCar = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectItem() {
        FindObjectOfType<Stop>().selectSellItem(this);
    }
}
