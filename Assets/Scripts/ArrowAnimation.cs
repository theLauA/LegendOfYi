using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimation : MonoBehaviour {
    public GameObject arrowPrefab;
    public Transform arrowSpawn;
    public float shootForce = 1000f;
    public Camera cam;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //Vector3 current_angle = transform.localEulerAngles;
        //current_angle.x = cam.transform.localEulerAngles.x;
        //transform.rotation = Quaternion.Euler(current_angle);
    }

    public void shoot(){
        GameObject go = Instantiate(arrowPrefab, arrowSpawn.position, transform.rotation);
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.velocity = cam.transform.forward* shootForce;
    }

    public void hide()
    {
        transform.localScale = new Vector3(0, 0, 0);
    }

    public void show()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }


}
