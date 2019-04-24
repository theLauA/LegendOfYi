using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    Rigidbody mybody;
    private float lifeTimer = 2f;
    private float timer;
    private bool hitSomething = false;
    // Use this for initialization
    private Quaternion correction;
    void Start () {
        
        
        mybody = GetComponent<Rigidbody>();
        
        //pointToward();
    }
	
	// Update is called once per frame
	void Update () {

        //pointToward();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.tag);
        if (collision.collider.tag == "Animal")
        {
            hitSomething = true;

            Stick(collision);
        }
        else if(collision.collider.tag != "Fire")
        {
            mybody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void Stick(Collision collision)
    {
        gameObject.transform.parent = collision.gameObject.transform;
        //FixedJoint fc = gameObject.AddComponent<FixedJoint>();
        //fc.anchor = collision.contacts[0].point;
        //fc.connectedBody = collision.rigidbody; 
        mybody.constraints = RigidbodyConstraints.FreezeAll;
        //StartCoroutine("fade");
    }

    IEnumerator fade()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void pointToward()
    {   
       
        float direction = (mybody.velocity / mybody.velocity.magnitude).y;
        if (transform.forward.z > 0)
            correction = Quaternion.Euler(0, -90, -90);
        else
            correction = Quaternion.Euler(0, 90, -90);
        correction = Quaternion.Euler(new Vector3(90*(direction-1),0,0));
        //correction = Quaternion.Euler()
        transform.rotation = Quaternion.LookRotation(mybody.velocity) * correction;
        //transform.localEulerAngles = new Vector3(0, 90 * direction, -90);
        //Debug.Log(transform.rotation);
        //transform.localEulerAngles = new Vector3(0, -90, 0);
        
    }
}
    