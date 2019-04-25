using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser_black : MonoBehaviour {
    public GameObject flame;
    private LineRenderer lr;
    // Use this for initialization
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // destroy itself after turning
        
        if (lr.transform.rotation.eulerAngles.x > 85 && lr.transform.rotation.eulerAngles.x < 300)
        {
            //Debug.Log(lr.transform.rotation.eulerAngles.ToString());
            Destroy(gameObject);
        }
        
        turn();
        lr.SetPosition(0, transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                lr.SetPosition(1, hit.point);
                GameObject new_flame = Instantiate(flame, hit.point, Quaternion.Euler(new Vector3(0, 0, 0)));
                new_flame.name = "fire_clone";
                new_flame.transform.parent = GameObject.Find("collection_fr").transform;
            }
        }
        else lr.SetPosition(1, transform.forward * 5000);

    }


    void turn()
    {
        Vector3 rot = lr.transform.rotation.eulerAngles;
        //turning_sppeds[speed_index++]
        rot = new Vector3(rot.x + 30 * Time.deltaTime, rot.y, rot.z);
        lr.transform.rotation = Quaternion.Euler(rot);
    }
}
