using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    public Camera cam;
    //public GameObject arrowPrefab;
    //public Transform arrowSpawn;
    //public float shootForce = 20f;

    public GameObject arrowInPlace;
    // Use this for initialization
    private Animator animator;
	void Start () {
        animator = arrowInPlace.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        
        
        if (Input.GetMouseButtonDown(0))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Stance"))
            {
                animator.SetBool("draw", true);
            }
            else
            {
                animator.SetBool("draw", false);
            }
            
        }

        
        
        
	}
 
}
