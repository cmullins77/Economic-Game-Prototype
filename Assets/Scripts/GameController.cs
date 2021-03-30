using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public Text distanceText;
    public int Distance;
    public Text goldText;
    public float Gold;


    public int nextStop;
    public int lengthCurrent;

    public GameObject stopUI;
    // Start is called before the first frame update
    void Start()
    {
        Gold = 600;
        goldText.text = "$" + Gold;
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
    }

    public void continueJourney() {
        FindObjectOfType<Train>().isMoving = true;
        stopUI.SetActive(false);
    }

    public void addDistance() {
        Distance++;
        distanceText.text = Distance + "M";
    }

    public void updateGold(float change) {
        Gold += change;
        goldText.text = "$" + Gold;
    }
}
