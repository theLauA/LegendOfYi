using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stage_3 : MonoBehaviour
{

    public GameObject fireball_float;
    public GameObject fireball_homing;
    public GameObject stage_2;
    public GameObject target;
    public GameObject farmer;
    public GameObject laser;
    public GameObject sun_prefab;
    public AudioSource audioData;

    public int num_of_suns;
    public float fireballs_spin_angle;
    public float max_spin_radius;
    public int height_of_suns;

    private float time;
    private GameObject main_sun;
    private GameObject chandelier;
    private Vector3 new_location;
    private float last_loc_change_time;
    private float[] actions_prob = new float[] {0.0f,6.0f, 8.0f};
    private Vector3 location_spiral;
    private Vector3 location_umbrella;
    // Use this for initialization
    void Start()
    {
        audioData.Play(0);
        prepareElements();
        StartCoroutine("shoot_fireball");
        StartCoroutine("fireball_spiral");
        StartCoroutine("laser_umbrella");

    }

    // Update is called once per frame
    void Update()
    {
        
        int numStage2Suns = numAlivedSun();
        if (main_sun != null) {
            main_sun.transform.localScale = new Vector3(12 - 2 * numStage2Suns, 12 - 2 * numStage2Suns, 12 - 2 * numStage2Suns);
            if (main_sun.transform.localScale[0] == 12)
            {
                turn(); // chandelier
                if (!inReadyPosition() && !hasLaser())
                    getToReadyPosition();
                else if (inReadyPosition() && time - last_loc_change_time > 3)
                {

                    // choose next random actions
                    float action = Random.Range(0, 10);
                    if (actions_prob[0] <= action && action < actions_prob[1]) //fireball
                        new_location = new Vector3(Random.Range(-50.0f, 50.0f), Random.Range(40.0f, 50.0f), Random.Range(-50.0f, 50.0f));
                    else if (actions_prob[1] <= action && action < actions_prob[2])
                        new_location = location_umbrella;
                    else if (actions_prob[2] <= action && action < 10)
                        new_location = location_spiral;
                    //Debug.Log("Changed to: " + new_location.ToString());
                    last_loc_change_time = time;

                }
                time += Time.deltaTime;
            }
        }
        else
        {
            StopAllCoroutines();
        }
        
    }

    // prepare the elements in the stage 1
    void prepareElements()
    {
        createSuns();
        // Setting other variables
        chandelier = GameObject.Find("stage_3_chandelier");
        new_location = main_sun.transform.position;
        time = 0;
        last_loc_change_time = -99;
        location_spiral = new Vector3(0, 7, 0);
        location_umbrella = new Vector3(0, height_of_suns , 0);
    }

    // create suns
    void createSuns()
    {
        main_sun = Instantiate(sun_prefab, new Vector3(0, height_of_suns+1, 0), transform.rotation);
        main_sun.transform.parent = gameObject.transform;
        main_sun.transform.localScale = new Vector3(7, 7, 7);
        main_sun.name = "sun3_0";
    }

    // spiral spin
    void turn()
    {
        Quaternion newRotation = chandelier.transform.rotation;
        chandelier.transform.eulerAngles = chandelier.transform.eulerAngles + new Vector3(0, fireballs_spin_angle * Time.deltaTime, 0);
    }

    // check ready position
    bool inReadyPosition()
    {
        return (Vector3.Distance(main_sun.transform.position, new_location) < 1);
    }

    // get to ready position
    void getToReadyPosition()
    {
        main_sun.transform.position = Vector3.MoveTowards(main_sun.transform.position, new_location, Time.deltaTime * 50);
    }


    private IEnumerator shoot_fireball()
    {
        while (true)
        {   
            
            // wait a few seconds before shoot
            float wait_time = 0.2f;
            yield return new WaitForSeconds(wait_time); // wait here until wait_time, then keep executing
            float size_of_sun = 0f;
            if (main_sun != null)
            {
                size_of_sun = main_sun.transform.localScale[0];
                if (size_of_sun == 12 && numFireballHoming() < 3 && inReadyPosition() && new_location != location_umbrella && new_location != location_spiral)
                {
                    main_sun.transform.LookAt(target.transform.position); // make sun face the target
                    GameObject new_fireball = Instantiate(fireball_homing, main_sun.transform.position + main_sun.transform.forward * Time.deltaTime * 500, main_sun.transform.rotation);
                    new_fireball.name = "fireball_homing_clone_";
                    new_fireball.transform.parent = GameObject.Find("collection_fb").transform;
                }
            }
            else
            {
                break;
            }
            
      
            
        }
    }

    private IEnumerator fireball_spiral()
    {
        while (true)
        {
            // wait a few seconds before shoot
            float wait_time = 1.5f;
            yield return new WaitForSeconds(wait_time); // wait here until wait_time, then keep executing
            float size_of_sun = main_sun.transform.localScale[0];
            if (size_of_sun == 12&&new_location == location_spiral && Vector3.Distance(main_sun.transform.position, new_location) < 1)
            {
                // Prepare to shoot
                List<Vector3> directions = getDirections(8);
                foreach (Vector3 dir in directions)
                {
                    main_sun.transform.LookAt(dir); // make sun face the target
                    Vector3 rot = main_sun.transform.rotation.eulerAngles;
                    rot = new Vector3(rot.x, rot.y, rot.z);
                    GameObject new_fireball_float = Instantiate(fireball_float, main_sun.transform.position + main_sun.transform.forward * Time.deltaTime * 500, Quaternion.Euler(rot));
                    new_fireball_float.name = "fireball_float_clone";
                    new_fireball_float.transform.parent = chandelier.transform;
                }
            }
        }
    }


    private IEnumerator laser_umbrella()
    {
        while (true)
        {
            // wait a few seconds before shoot
            float wait_time = 1f;
            yield return new WaitForSeconds(wait_time); // wait here until wait_time, then keep executing
            float size_of_sun = main_sun.transform.localScale[0];
            if (size_of_sun == 12&& new_location == location_umbrella && !hasLaser() && Vector3.Distance(main_sun.transform.position, new_location) < 1) {
                // Prepare to shoot
                int random_angle = Random.Range(0, 45);
                List<Vector3> directions = getDirections(8);
                foreach (Vector3 dir in directions)
                {
                    main_sun.transform.LookAt(dir); // make sun face the target
                    Vector3 rot = main_sun.transform.rotation.eulerAngles;
                    rot = new Vector3(rot.x, rot.y + random_angle, rot.z);
                    GameObject new_laser = Instantiate(laser, main_sun.transform.position, Quaternion.Euler(rot));
                    new_laser.name = "laser_clone";
                    new_laser.transform.parent = GameObject.Find("collection_ls").transform;
                }
            }
        }
    }

    
    
    
    // get directions
    List<Vector3> getDirections(int total_directions)
    {
        List<Vector3> directions = new List<Vector3>();
        float x, z, angle;
        float y = height_of_suns;
        for (int i = 0; i < total_directions; i++)
        {
            angle = (Mathf.PI / 180) * 360.0f / total_directions * i;
            x = 0 + 1000 * Mathf.Cos(angle);
            z = 0 + 1000 * Mathf.Sin(angle);
            directions.Add(new Vector3(x, y, z));
        }
        return directions;
    }

    // get current alived suns
    int numAlivedSun()
    {
        int count = 0;
        GameObject[] all_obj = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in all_obj)
            if ((obj.name.Contains("sun2_")) && obj.activeInHierarchy)
                count++;
        return count;
    }

    // if fireball float exist
    bool hasFireballFloat()
    {
        GameObject[] all_obj = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in all_obj)
            if ((obj.name.Contains("fireball_float_")) && obj.activeInHierarchy)
                return true;
        return false;
    }

    // if fireball float exist
    int numFireballHoming()
    {
        int count = 0;
        GameObject[] all_obj = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in all_obj)
            if ((obj.name.Contains("fireball_homing_")) && obj.activeInHierarchy)
                count++;
        return count;
    }

    // if laser exist
    bool hasLaser()
    {
        GameObject[] all_obj = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in all_obj)
            if ((obj.name.Contains("laser")) && obj.activeInHierarchy)
                return true;
        return false;
    }

    /*
    // end of stage 1
    void endStage()
    {
        StopAllCoroutines();
        // get close to center
        if (Vector3.Distance(main_sun.transform.position, new Vector3(0, height_of_suns, 0)) >= 20)
            main_sun.transform.position = Vector3.MoveTowards(main_sun.transform.position, new Vector3(0, height_of_suns, 0), Time.deltaTime * 10);
        else // closer enough, change size()
        {
            if(main_sun.transform.localScale[0] != 7)
            {
                
            }
        }

    }
    */
}
