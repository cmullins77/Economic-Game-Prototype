using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSpawner : MonoBehaviour
{

    public GameObject space;

    public GameObject currentSpace;

    public List<Space> spaces;
    // Start is called before the first frame update
    void Start()
    {
        spawnSpace(0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpace.transform.position.x > transform.position.x + 0.95f) {
            spawnSpace(0.1f);
        }
    }

    public void spawnSpace(float difficulty) {
        difficulty = difficulty - (Random.Range(-10, 10) / 100f);
        Color spaceColor = new Color(difficulty, 1f - difficulty, 0);
        GameObject newSpace = Instantiate(space, transform);
        newSpace.transform.position = transform.position;
        newSpace.GetComponent<SpriteRenderer>().color = spaceColor;
        currentSpace = newSpace;

        FindObjectOfType<GameController>().addDistance();
        Space trainSpace = spaces[0];
        spaces.RemoveAt(0);
        spaces.Add(newSpace.GetComponent<Space>());
        FindObjectOfType<Train>().travelSpace(trainSpace.difficulty);
    }
}
