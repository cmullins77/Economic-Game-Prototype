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

    public List<Job> jobs;

    public Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        jobs = new List<Job>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool addJob(Job newJob) {
        if (newJob.quantity > maxCapacity - currentCapacity) {
            Debug.Log("Full");
            return false;
        }
        if (newJob.type != type) {
            Debug.Log("Wrong Car Type");
            return false;
        }
        if (newJob.minQuality > quality) {
            Debug.Log("Not High Enough Quality");
            return false;
        }
        jobs.Add(newJob);
        currentCapacity += newJob.quantity;
        return true;

    }

    public void checkJobs() {
        Debug.Log("Count: " + jobs.Count);
        for (int i = 0; i < jobs.Count; i++) {
            Job job = jobs[i];
            if (job.stops != -1) {
                job.currentCount++;
                if (job.stops == job.currentCount) {
                    FindObjectOfType<GameController>().updateGold(job.reward);
                    FindObjectOfType<GameController>().updateRep(job.quantity/2);
                    jobs.Remove(job);
                    currentCapacity -= job.quantity;
                }
            } else {
                job.currentCount += FindObjectOfType<GameController>().lengthCurrent;
                if (job.distance <= job.currentCount) {
                    FindObjectOfType<GameController>().updateGold(job.reward);
                    FindObjectOfType<GameController>().updateRep(job.quantity/2);
                    jobs.Remove(job);
                    currentCapacity -= job.quantity;
                }
            }
        }
    }
}

public enum CarType
{
    Passenger, Livestock, Freight
}
