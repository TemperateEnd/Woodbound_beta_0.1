﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watertrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D log)
    {
        log.GetComponent<logsPhysics>().Water();
    }
    void OnTriggerExit2D(Collider2D log)
    {
        log.GetComponent<logsPhysics>().Water();
    }


}