using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimalAnimation : MonoBehaviour {
    private Animator anim;
    public GameObject item;
    public GameObject self;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("walk"))
        {
            self.transform.position += Vector3.forward * Time.deltaTime;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Arrow")
        {
            anim.SetBool("dead", true);
            Collider c = GetComponent<Collider>();
            c.enabled = false;
        }
        
    }

    public void Drop()
    {
        item.SetActive(true);
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }
}
