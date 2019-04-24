using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flame : MonoBehaviour {

    public float time_before_die = 10;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // destroy itself after turning
        if (time_before_die <= 0)
            Destroy(gameObject);
        time_before_die -= Time.deltaTime;
    }

}
