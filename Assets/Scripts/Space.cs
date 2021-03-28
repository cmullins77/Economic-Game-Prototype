using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{

    public float difficulty;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Train>().isMoving) {
            transform.Translate(0.001f, 0f, 0f);
            if (transform.position.x > 15) {
                Destroy(gameObject);
            }
        }
    }
}
