using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Stop : MonoBehaviour
{

    public GameObject trainCarButtonPrefab;
    public GameObject trainCarsPage;
    List<TrainCarButton> trainCarButtons;
    int carSwapIndex1;
    int carSwapIndex2;
    public GameObject swapButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void purchaseCurrentItem() {


    }

    public void sellCurrentItem() {

    }

    public void populateTrainCars() {
        Train train = FindObjectOfType<Train>();
        List<TrainCar> cars = train.cars;
        foreach (Transform child in trainCarsPage.transform) {
            GameObject.Destroy(child.gameObject);
        }
        trainCarButtons = new List<TrainCarButton>();
        for (int i = 0; i < cars.Count; i++) {
            GameObject newButton = Instantiate(trainCarButtonPrefab, trainCarsPage.transform);
            newButton.GetComponent<Image>().sprite = cars[i].GetComponent<SpriteRenderer>().sprite;
            newButton.GetComponent<TrainCarButton>().indexOfCar = i;
            trainCarButtons.Add(newButton.GetComponent<TrainCarButton>());
        }
        carSwapIndex1 = -1;
        carSwapIndex2 = -1;
    }

    public void addCarToSwap(int i) {
        carSwapIndex1 = carSwapIndex2;
        carSwapIndex2 = i;
        if (carSwapIndex1 != -1 && carSwapIndex2 != -1) {
            swapButton.SetActive(true);
        }
    }

    public void swapCars() {
        FindObjectOfType<Train>().swapCars(carSwapIndex1, carSwapIndex2);
    }

    public void continueJourney() {

    }
}
