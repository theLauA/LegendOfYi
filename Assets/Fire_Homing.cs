using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Homing : MonoBehaviour {
    private GameObject target;
	// Use this for initialization
	void Start () {
        target = GameObject.Find("target");	
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {

            transform.LookAt(target.transform.position);
            
                
        }
	}

    
}
