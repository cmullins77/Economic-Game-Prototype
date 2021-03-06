using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Stop : MonoBehaviour
{
    public List<GameObject> buyCarPrefabs;
    public List<GameObject> resourceTypePrefabs;
    public GameObject buyButtonPrefab;
    public GameObject buyPage;
    public GameObject buyUI;
    public GameObject buyInfoUI;
    public List<BuyItem> buyItems;
    public Image buyItemImage;
    public Text buyInfoText;
    int currentItem = -1;

    public GameObject sellPage;
    public List<SellItem> sellItems;
    public GameObject sellButtonPrefab;
    public float minCoalPrice = 10;
    public GameObject sellUI;
    public GameObject sellInfoUI;
    public Image sellItemImage;
    public Text sellInfoText;

    public List<Job> jobs;
    public List<JobButton> jobButtons;
    public List<GameObject> jobPrefabs;
    public GameObject jobPage;
    public GameObject jobButtonPrefab;
    public Text jobInfoText;
    public Image jobInfoImage;
    public GameObject jobsUI;
    public GameObject jobsInfoUI;
    public GameObject jobsPlacementUI;
    public JobButton currentJob;
    public GameObject trainCarPlacementButtonPrefab;
    public GameObject trainCarsPlacementPage;
    List<TrainCarPlacementButton> trainCarPlacementButtons;
    int currentPlacementIndex;
    public GameObject placeButton;
    public Text carPlacementInfo;
    public Text jobPlacementInfo;

    public GameObject trainCarButtonPrefab;
    public GameObject trainCarsPage;
    List<TrainCarButton> trainCarButtons;
    int carSwapIndex1;
    int carSwapIndex2;
    public GameObject swapButton;

    public Text carInfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void purchaseCurrentItem() {
        BuyItem item = buyItems[currentItem];
        if (FindObjectOfType<GameController>().Gold >= item.price && item.quantity > 0) {
            FindObjectOfType<GameController>().updateGold(-1 * item.price);
            buyItems[currentItem].quantity--;
            selectBuyItem(item);
            if (item.isTrainCar) {
                FindObjectOfType<Train>().addCar(item.prefab.GetComponent<TrainCar>());
            } else {
                if (item.prefab.GetComponent<Resource>().type == ResourceType.Coal) {
                    FindObjectOfType<Train>().updateCoal(1);
                }
            }
        }
    }

    public void sellCurrentItem() {
        SellItem item = sellItems[currentItem];
        FindObjectOfType<GameController>().updateGold(item.price);
        sellItems[currentItem].quantity--;
        selectSellItem(item);
        if (item.isTrainCar) {
           FindObjectOfType<Train>().removeCar(item.index);
         }
         else {
           if (item.prefab.GetComponent<Resource>().type == ResourceType.Coal) {
              FindObjectOfType<Train>().updateCoal(-1);
            }
         }
        if (item.quantity == 0) {
            sellInfoUI.SetActive(false);
            sellUI.SetActive(true);
            populateSell();
        }
       
    }

    public void generateJobs() {
        jobs = new List<Job>();
        for (int i = 0; i < Random.Range(2,5 + (FindObjectOfType<GameController>().Reputation/5)); i++) {
            GameObject jobObj = Instantiate(jobPrefabs[Random.Range(0, jobPrefabs.Count)]);
            jobObj.transform.position = new Vector3(-1000f, -1000f, 0);
            Job job = jobObj.GetComponent<Job>();
            Debug.Log(job);
            int quant = Random.Range(1, 11);
            int distance = -1;
            int stops = -1;
            int qual = Random.Range(0, 6);
            job.minQuality = qual;
            job.quantity = quant;
            if (Random.Range(0,2) == 0) {
                distance = Random.Range(20, 200);
                job.distance = distance;
            } else {
                stops = Random.Range(1, 4);
                job.stops = stops;
            }
            float distanceReward = stops == -1 ? distance : stops * 100;
            float reward = (distanceReward + 30*(qual + quant) + (int)job.type*2)/ 5f;
            job.reward = (int) reward;

            job.sprite = jobObj.GetComponent<SpriteRenderer>().sprite;
            Debug.Log(job.type + " " + quant + " " + qual + " " + distance + " " + stops + " " + reward);

            jobs.Add(job);
        }
    }

    public void populateJobs() {
        float currentX = -240f;
        float currentY = 130f;
        foreach (Transform child in jobPage.transform) {
            GameObject.Destroy(child.gameObject);
        }
        jobButtons = new List<JobButton>();
        for (int i = 0; i < jobs.Count; i++) {
            Job job = jobs[i];
            Debug.Log("job " + job);
            GameObject newButton = Instantiate(jobButtonPrefab, jobPage.transform);
            newButton.GetComponent<Image>().sprite = job.sprite;
            newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentX, currentY);
            newButton.GetComponent<JobButton>().sprite = job.sprite;
            newButton.GetComponent<JobButton>().job = job;
            newButton.GetComponent<JobButton>().index = i;
            jobButtons.Add(newButton.GetComponent<JobButton>());

            currentX += 120f;
            if (currentX > 240f) {
                currentY -= 100f;
                currentX = -240f;
            }
        }

    }
    public void showJobInfo(JobButton jobB) {
        Debug.Log("Distance: " + jobB.job.distance + " Stops: " + jobB.job.stops);
        string info = "Name: " + jobB.job.type + "\nQuantity: " + jobB.job.quantity + "\nMinimum Quality: " + jobB.job.minQuality;
        info = info + (jobB.job.stops != -1 ? "\nStops: " + jobB.job.stops : "\nDistance: " + jobB.job.distance);
        info = info + "\nReward: $" + jobB.job.reward;
        jobInfoText.text = info;
        jobInfoImage.sprite = jobB.sprite;
        jobsUI.SetActive(false);
        jobsInfoUI.SetActive(true);
        currentJob = jobB;
    }
    
    public void acceptJob() {
        populateTrainCarJobPlacement();
        jobPlacementInfo.text = "Type: " + currentJob.job.type + "\nQuantity: " + currentJob.job.quantity + "\nMin Quality: " + currentJob.job.minQuality;
    }

    public void selectPlacementCar(TrainCarPlacementButton button) {
        currentPlacementIndex = button.indexOfCar;
        TrainCar car = FindObjectOfType<Train>().cars[currentPlacementIndex];
        carPlacementInfo.text = "Type: " + car.type + "\nCurrent Capacity: " + (car.maxCapacity - car.currentCapacity) + "\nQuality: " + car.quality;
        placeButton.SetActive(true);
        carPlacementInfo.gameObject.SetActive(true);
    }

    public void placeJob() {
        Debug.Log(currentPlacementIndex + " " + currentJob + " " + currentJob.job);
        bool isPlaced = FindObjectOfType<Train>().cars[currentPlacementIndex].addJob(currentJob.job);
        if (isPlaced) {
            jobs.RemoveAt(currentJob.index);
            populateJobs();
            jobsUI.SetActive(true);
            jobsPlacementUI.SetActive(false);
        }
    }

    void populateTrainCarJobPlacement() {
        Debug.Log("Populate Train Cars for Job Placement");
        Train train = FindObjectOfType<Train>();
        List<TrainCar> cars = train.cars;
        foreach (Transform child in trainCarsPlacementPage.transform) {
            GameObject.Destroy(child.gameObject);
        }
        trainCarPlacementButtons = new List<TrainCarPlacementButton>();
        float currentX = -240;
        float currentY = 130f;
        for (int i = 0; i < cars.Count; i++) {
            GameObject newButton = Instantiate(trainCarPlacementButtonPrefab, trainCarsPlacementPage.transform);
            newButton.GetComponent<Image>().sprite = cars[i].GetComponent<SpriteRenderer>().sprite;
            newButton.GetComponent<TrainCarPlacementButton>().indexOfCar = i;
            newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentX, currentY);
            trainCarPlacementButtons.Add(newButton.GetComponent<TrainCarPlacementButton>());

            currentX += 120f;
            if (currentX > 240f) {
                currentY -= 100f;
                currentX = -240f;
            }
        }
        currentPlacementIndex = -1;
        placeButton.SetActive(false);
        carPlacementInfo.gameObject.SetActive(false);
    }

    public void populateBuy() {
        int options = buyCarPrefabs.Count + resourceTypePrefabs.Count;
        int numAvailable = Random.Range(2, 3 + (FindObjectOfType<GameController>().Reputation/5));

        float currentX = -240f;
        float currentY = 130f;

        foreach (Transform child in buyPage.transform) {
            GameObject.Destroy(child.gameObject);
        }
        buyItems = new List<BuyItem>();
        for (int i = 0; i < numAvailable; i++) {
            int type = Random.Range(0, options);
            GameObject prefab;
            string info = "";
            string name = "";
            float price = 0;
            int Quantity = 0;
            bool isTrainCar = false;
            if (type < buyCarPrefabs.Count) {
                prefab = Instantiate(buyCarPrefabs[type]);
                prefab.transform.position = new Vector3(-1000f, -1000f, 0);
                Quantity = Random.Range(1, 3);
                int Quality = Random.Range(1, 11);
                int maxCapacity = Random.Range(1, 21); ;
                float weight = (maxCapacity * Quality) / 12f;
                price = 100 + 10 * maxCapacity + 20 * Quality + 50 * (int)prefab.GetComponent<TrainCar>().type;
                prefab.GetComponent<TrainCar>().quality = Quality;
                prefab.GetComponent<TrainCar>().maxCapacity = maxCapacity;
                prefab.GetComponent<TrainCar>().weight = weight;
                prefab.GetComponent<TrainCar>().sellPrice = price/Quality;
                name = "Name: " + prefab.GetComponent<TrainCar>().type;
                isTrainCar = true;
                info = "\nPrice: $" + price + "\nQuality: " + Quality + "\nMax Capacity: " + maxCapacity + "\nWeight: " + weight;
            } else {
                prefab = Instantiate(resourceTypePrefabs[type - buyCarPrefabs.Count]);
                prefab.transform.position = new Vector3(-1000f, -1000f, 0);
                Quantity = Random.Range(5, 36);
                price = Random.Range(2, 21) + (int)prefab.GetComponent<Resource>().type * Random.Range(1, 5);
                info = "\nPrice: $" + price;
                name = "Name: " + prefab.GetComponent<Resource>().type;
                if (prefab.GetComponent<Resource>().type == ResourceType.Coal) {
                    minCoalPrice = minCoalPrice > price ? price : minCoalPrice;
                }
            }
            GameObject newButton = Instantiate(buyButtonPrefab, buyPage.transform);
            newButton.GetComponent<Image>().sprite = prefab.GetComponent<SpriteRenderer>().sprite;
            newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentX, currentY);
            newButton.GetComponent<BuyItem>().sprite = newButton.GetComponent<Image>().sprite;
            newButton.GetComponent<BuyItem>().info = info;
            newButton.GetComponent<BuyItem>().price = price;
            newButton.GetComponent<BuyItem>().nameString = name;
            newButton.GetComponent<BuyItem>().quantity = Quantity;
            newButton.GetComponent<BuyItem>().index = i;
            newButton.GetComponent<BuyItem>().isTrainCar = isTrainCar;
            newButton.GetComponent<BuyItem>().prefab = prefab;
            buyItems.Add(newButton.GetComponent<BuyItem>());

            currentX += 120f;
            if (currentX > 240f) {
                currentY -= 100f;
                currentX = -240;
            }

        }
    }
    
    public void populateSell() {
        float currentX = -240f;
        float currentY = 130f;

        foreach (Transform child in sellPage.transform) {
            GameObject.Destroy(child.gameObject);
        }
        int numAvailable = FindObjectOfType<Train>().cars.Count;
        sellItems = new List<SellItem>();
        for (int i = 0; i < numAvailable; i++) {
            GameObject car = FindObjectOfType<Train>().cars[i].gameObject;
            int Quality = (int)car.GetComponent<TrainCar>().quality;
            int maxCapacity = (int)car.GetComponent<TrainCar>().maxCapacity;
            float weight = (int)car.GetComponent<TrainCar>().weight;
            float price = car.GetComponent<TrainCar>().sellPrice;
            string name = "Name: " + car.GetComponent<TrainCar>().type;
            string info = "\nPrice: $" + price + "\nQuality: " + Quality + "\nMax Capacity: " + maxCapacity + "\nWeight: " + weight;
            bool isTrainCar = true;
           
            GameObject newButton = Instantiate(sellButtonPrefab, sellPage.transform);
            newButton.GetComponent<Image>().sprite = car.GetComponent<SpriteRenderer>().sprite;
            newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentX, currentY);
            newButton.GetComponent<SellItem>().sprite = newButton.GetComponent<Image>().sprite;
            newButton.GetComponent<SellItem>().info = info;
            newButton.GetComponent<SellItem>().price = price;
            newButton.GetComponent<SellItem>().nameString = name;
            newButton.GetComponent<SellItem>().quantity = 1;
            newButton.GetComponent<SellItem>().index = i;
            newButton.GetComponent<SellItem>().isTrainCar = isTrainCar;
            newButton.GetComponent<SellItem>().prefab = car;
            sellItems.Add(newButton.GetComponent<SellItem>());

            currentX += 120f;
            if (currentX > 240f) {
                currentY -= 100f;
                currentX = -240f;
            }
        }

        //Coal
        if (FindObjectOfType<Train>().amountCoal > 0) {
            GameObject newButton = Instantiate(sellButtonPrefab, sellPage.transform);
            newButton.GetComponent<Image>().sprite = resourceTypePrefabs[0].GetComponent<SpriteRenderer>().sprite;
            newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentX, currentY);
            newButton.GetComponent<SellItem>().sprite = newButton.GetComponent<Image>().sprite;
            newButton.GetComponent<SellItem>().info = "\nPrice: $" + (minCoalPrice/2f);
            newButton.GetComponent<SellItem>().price = (minCoalPrice / 2f);
            newButton.GetComponent<SellItem>().nameString = "Name: Coal";
            newButton.GetComponent<SellItem>().quantity = FindObjectOfType<Train>().amountCoal;
            newButton.GetComponent<SellItem>().index = sellItems.Count;
            newButton.GetComponent<SellItem>().isTrainCar = false;
            newButton.GetComponent<SellItem>().prefab = resourceTypePrefabs[0];
            sellItems.Add(newButton.GetComponent<SellItem>());
        }
    }

    public void selectBuyItem(BuyItem item) {
        buyItemImage.GetComponent<Image>().sprite = item.sprite;
        buyInfoText.text = item.nameString + "\nQuantity: " + item.quantity + item.info;
        currentItem = item.index;
        buyUI.SetActive(false);
        buyInfoUI.SetActive(true);
    }

    public void selectSellItem(SellItem item) {
        sellItemImage.GetComponent<Image>().sprite = item.sprite;
        sellInfoText.text = item.nameString + "\nQuantity: " + item.quantity + item.info;
        currentItem = item.index;
        sellUI.SetActive(false);
        sellInfoUI.SetActive(true);
    }

    public void populateTrainCars() {
        Debug.Log("Populate Train Cars");
        Train train = FindObjectOfType<Train>();
        List<TrainCar> cars = train.cars;
        foreach (Transform child in trainCarsPage.transform) {
            GameObject.Destroy(child.gameObject);
        }
        trainCarButtons = new List<TrainCarButton>();
        float currentX = -240f;
        float currentY = 130f;
        for (int i = 0; i < cars.Count; i++) {
            GameObject newButton = Instantiate(trainCarButtonPrefab, trainCarsPage.transform);
            newButton.GetComponent<Image>().sprite = cars[i].GetComponent<SpriteRenderer>().sprite;
            newButton.GetComponent<TrainCarButton>().indexOfCar = i;
            newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentX, currentY);
            trainCarButtons.Add(newButton.GetComponent<TrainCarButton>());

            currentX += 120f;
            if (currentX > 240f) {
                currentY -= 100f;
                currentX = -240f;
            }
        }
        carSwapIndex1 = -1;
        carSwapIndex2 = -1;
        carInfo.gameObject.SetActive(false);
    }

    public void addCarToSwap(int i) {
        if (carSwapIndex1 != -1) {
            trainCarButtons[carSwapIndex1].GetComponent<Button>().interactable = true;
        }
        carSwapIndex1 = carSwapIndex2;
        carSwapIndex2 = i;
        if (carSwapIndex1 != -1 && carSwapIndex2 != -1) {
            swapButton.SetActive(true);
        }
        carInfo.gameObject.SetActive(true);
        TrainCar c = FindObjectOfType<Train>().cars[i];
        carInfo.text = "Max Capacity: " + c.maxCapacity + ", Current Quantity: " + c.currentCapacity + ", Quality: " + c.quality;
    }

    public void swapCars() {
        Debug.Log(carSwapIndex1 + " " +  carSwapIndex2);
        FindObjectOfType<Train>().swapCars(carSwapIndex1, carSwapIndex2);
        populateTrainCars();
    }

    public void continueJourney() {
        FindObjectOfType<GameController>().continueJourney();
    }
}
