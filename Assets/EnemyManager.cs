using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public Transform target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float x = target.transform.position.x;
        float z = target.transform.position.z;
        Vector3 newLocation = transform.position;
        if( (x*x + z*z) < 100f*100f)
        {   
            newLocation.x = x;
            newLocation.z = z;
            transform.position =  Vector3.Lerp(transform.position, newLocation, Time.deltaTime*1000);
        }
	}
}
