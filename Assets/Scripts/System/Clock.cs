using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Clock : MonoBehaviour
{

    float sec;

    public bool isPassing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPassing)
        {
            sec += Time.deltaTime;
            transform.eulerAngles = new Vector3(0, 0, sec / 60 * -360 + sec / 60 / 1000 * -360);
        }
    }
}
