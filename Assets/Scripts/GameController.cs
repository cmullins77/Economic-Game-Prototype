using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public Text distanceText;
    public int Distance;
    public Text goldText;
    public float Gold;
    public int Reputation;
    public Text reputationText;

    public int nextStop;
    public int lengthCurrent;

    public Text endButtonText;

    public GameObject stopUI;

    int reputationCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        Gold = 600;
        goldText.text = "$" + Gold;
        updateRep(20);
        nextStop = Random.Range(5, 15);
        lengthCurrent = nextStop;
        stopUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Distance >= nextStop) {
            FindObjectOfType<Train>().isMoving = false;
            lengthCurrent = Random.Range(25, 50);
            nextStop += lengthCurrent;
            FindObjectOfType<Train>().checkJobs();
            stopUI.SetActive(true);
            stopUI.GetComponent<Stop>().populateBuy();
            stopUI.GetComponent<Stop>().generateJobs();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(0);
        } else if (Input.GetKeyDown(KeyCode.Q)) {
            Application.Quit();
        }
    }

    public void showEndButton() {
        endButtonText.transform.parent.gameObject.SetActive(true);
        Train train = FindObjectOfType<Train>();
        float goldReq = ((nextStop - Distance) * 10 + train.cars.Count*10)/2;
        endButtonText.text = Gold >= goldReq ? "Get Emergency Lift: $" + goldReq : train.cars.Count > 0 ? "Scrap Last Car for $" + (train.cars[train.cars.Count - 1].sellPrice / 2) : "End Game";
    }

    public void pressEndButton() {
        Train train = FindObjectOfType<Train>();
        float goldReq = ((nextStop - Distance) * 10 + train.cars.Count * 10) / 2;
        if (Gold >= goldReq) {
            updateGold(-1 * goldReq);
            train.updateCoal(train.cars.Count/2 + 1);
            endButtonText.transform.parent.gameObject.SetActive(false);
            Distance = nextStop;
            distanceText.text = Distance + "M";
            lengthCurrent = Random.Range(25, 50);
            nextStop += lengthCurrent;
            FindObjectOfType<Train>().checkJobs();
            stopUI.SetActive(true);
            stopUI.GetComponent<Stop>().populateBuy();
            stopUI.GetComponent<Stop>().generateJobs();
        } else if (train.cars.Count > 0) {
            int numJobs = (int)train.cars[train.cars.Count - 1].currentCapacity;
            updateRep(((numJobs) + 5)/-1.5f);
            updateGold(train.cars[train.cars.Count - 1].sellPrice / 2);
            train.detachLastCar();
            showEndButton();
        } else {
            Debug.Log("Game Over");
            SceneManager.LoadScene(0);
        }
    }

    public void continueJourney() {
        FindObjectOfType<Train>().isMoving = true;
        stopUI.SetActive(false);
    }

    public void addDistance() {
        Distance++;
        distanceText.text = Distance + "M";
        reputationCounter++;
        if (reputationCounter > 10) {
            updateRep(1);
            reputationCounter = 0;
        }
    }

    public void updateGold(float change) {
        Gold += change;
        goldText.text = "$" + Gold;
    }
    public void updateRep(float change) {
        Reputation += (int)change;
        Reputation = Reputation > 100 ? 100 : Reputation < 0 ? 0 : Reputation;
        reputationText.text = Reputation + "%";
    }
}
