﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireball_homing : MonoBehaviour {

    public Rigidbody homingFireball;
    public float missileVelocity = 100f;
    public float turn = 0f; //20f
    private Transform target;
    private float time;

    // Use this for initialization
    void Start () {
        homingFireball = gameObject.GetComponent<Rigidbody>();
        Fire();
        time = 0;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (gameObject.transform.position[1] <= gameObject.transform.localScale[1] / 2.0f + 0.5)
        {
            Destroy(gameObject);
        }else if(time>5){
            Destroy(gameObject);
        }
    }
    
    // Update the location
    void FixedUpdate()
    {

        if (target == null || homingFireball == null)
            return;
        
        homingFireball.velocity = transform.forward * missileVelocity;

        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);

        homingFireball.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turn));

    }


    void Fire()
    {

        float distance = Mathf.Infinity;
        GameObject[] all_obj = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in all_obj)
        {
            if ((obj.name.Contains("target")) && obj.activeInHierarchy)
            {
                float diff = (obj.transform.position - transform.position).sqrMagnitude;

                if (diff < distance)
                {
                    distance = diff;
                    target = obj.transform;
                }
            }
        }

    }



}
