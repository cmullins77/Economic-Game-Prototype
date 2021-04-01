using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{

    public float difficulty;
    public GameObject coalButton;
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas.worldCamera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Train>().isMoving) {
            transform.Translate(0.05f, 0f, 0f);
            if (transform.position.x > 15) {
                Destroy(gameObject);
            }
        }
    }

    public void pressCoal() {
        coalButton.SetActive(false);
        FindObjectOfType<Train>().updateCoal(Random.Range(1,4));
    }
}
