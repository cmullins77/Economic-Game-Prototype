using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobButton : MonoBehaviour
{

    public Sprite sprite;
    public Job job;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectJob() {
        FindObjectOfType<Stop>().showJobInfo(this);
    }
}
