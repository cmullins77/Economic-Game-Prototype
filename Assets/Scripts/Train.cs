using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Train : MonoBehaviour
{
    public bool isMoving;

    public int amountCoal;
    public float currentSteam;
    public Text coalText;

    public float trainWeight;

    public List<TrainCar> cars;
    public List<GameObject> startingCars;
    // Start is called before the first frame update
    void Start()
    {
        cars = new List<TrainCar>();
        coalText.text = amountCoal + " Coal";
        isMoving = true;
        foreach (GameObject car in startingCars) {
            GameObject obj = Instantiate(car, transform);
            addCar(obj.GetComponent<TrainCar>());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateCoal(int num) {
         amountCoal+= num;
         coalText.text = amountCoal + " Coal";
    }

    public void useCoal() {
        if (amountCoal == 0) {
            isMoving = false;
        } else {
            amountCoal--;
            currentSteam++;
            coalText.text = amountCoal + " Coal";
        }
    }

    public void travelSpace(float difficulty) {
        difficulty = difficulty + .1f;
        difficulty = difficulty + (trainWeight / 200f);
        difficulty = difficulty / 2f;
        Debug.Log("Difficulty " + difficulty);
        currentSteam -= difficulty;
        if (currentSteam <= 0) {
            useCoal();
        }
    }

    public void addCar(TrainCar newCar) {
        if (cars.Count > 0) {
            newCar.transform.position = cars[cars.Count - 1].transform.position + new Vector3(1.68f, 0, 0);
        }
        cars.Add(newCar);
        trainWeight += newCar.weight;
    }

    public void detachLastCar() {
        if (cars.Count > 0) {
            TrainCar car = cars[cars.Count - 1];
            cars.RemoveAt(cars.Count - 1);
            trainWeight -= car.weight;
            Destroy(car.gameObject);
        }
    }

    public void removeCar(int i) {
        if (cars.Count - 1 > i) {
            TrainCar car = cars[i];
            float carSpacing = 1.68f;
            for (int j = i; j < cars.Count - 1; j++) {
                cars[j].transform.Translate(carSpacing * -1f, 0, 0);
            }
            cars.RemoveAt(i);
            trainWeight -= car.weight;
            Destroy(car.gameObject);
        }
    }

    public void swapCars(int i, int j) {
        if (cars.Count - 1 >= i && cars.Count - 1 >= j) {
            TrainCar car1= cars[i];
            TrainCar car2 = cars[j];
            Vector3 spot1 = car1.transform.position;
            Vector3 spot2 = car2.transform.position;
            car1.transform.position = spot2;
            car2.transform.position = spot1;
            cars[i] = car2;
            cars[j] = car1;
        }
    }

    public void checkJobs() {
        foreach (TrainCar car in cars) {
            car.checkJobs();
        }
    }
}
