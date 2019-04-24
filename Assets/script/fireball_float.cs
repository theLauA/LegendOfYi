using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireball_float : MonoBehaviour {

    public Rigidbody floatFireball;
    public float velocity;
    private Transform target;
    private float time;

    // Use this for initialization
    void Start()
    {
        floatFireball = gameObject.GetComponent<Rigidbody>();
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= 5)
        {
            Destroy(gameObject);
        }

        time += Time.deltaTime;
    }

    // Update the location
    void FixedUpdate()
    {
        floatFireball.velocity = transform.forward * velocity;
    }

}
