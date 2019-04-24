using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunCollision
{
    public class stage_2 : MonoBehaviour
    {
        public GameObject fireball;
        public GameObject laser;
        public GameObject target;
        public GameObject stage_3;
        public AudioSource audioData;
        public float turnAngle;
        public float height_of_suns;
        public float circling_radius;


        private GameObject chandelier;
        private List<GameObject> fireball_suns;
        private List<GameObject> laser_suns;
        private List<Vector3> fb_suns_locations;
        private List<Vector3> ls_suns_locations;
        private bool stage2_fb_ready;
        private bool stage2_ls_ready;

        private bool laser_not_ready;
        private List<Vector3> laser_new_location;
        private float ls_distance;

        private bool ready;

        void Start()
        {
            ready = false;
            audioData.Play(0);
            prepareElements();
            //StartCoroutine("prepareElements");
            StartCoroutine("shoot_fireball");
            StartCoroutine("shoot_laser");
        }

        void Update()
        {
            int num_of_sun = getAlivedSun().Count;
            if (num_of_sun > 3)  // Still in stage 2
            {
                if (!stage2_fb_ready || !stage2_ls_ready)
                    getToStage2Position();
                if (laser_not_ready)
                    lsToReadyPosition();
                if(stage2_fb_ready && stage2_ls_ready && !ready)
                {   
                    StartCoroutine("removeInvinsibility");
                    ready = true;
                }
                turn_chandelier();
            }
            else // preceeding to next stage
            {
                if (num_of_sun > 0 && stage2_fb_ready && stage2_ls_ready) // clean up suns
                {
                    StopCoroutine("shoot_fireball");
                    StopCoroutine("shoot_laser");
                    endStage();
                }
                else if(num_of_sun == 0)
                { // all suns have been cleaned, shut down stage 2
                    //gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
            cleanUpLaser();
        }


        void turn_chandelier()
        {
            if (stage2_fb_ready && stage2_ls_ready)
            {
                Quaternion newRotation = chandelier.transform.rotation;
                chandelier.transform.eulerAngles = chandelier.transform.eulerAngles + new Vector3(0, turnAngle * Time.deltaTime, 0);
            }
        }

        // Prepare the elements in the stage 1
        private void prepareElements()
        {
            // Setting other variables
            Debug.Log("Stage_2");
            chandelier = GameObject.Find("stage_2_chandelier");
            fireball_suns = new List<GameObject>();
            laser_suns = new List<GameObject>();
            fb_suns_locations = new List<Vector3>();
            ls_suns_locations = new List<Vector3>();
            laser_not_ready = false;
            ls_distance = circling_radius + 10;
            // prepare suns, fill in fb_suns and ls_suns
            List<GameObject> remaining_suns = getAlivedSun();
            
            
            foreach (GameObject sun in remaining_suns)
            {
                
                if (fireball_suns.Count < 4)
                    fireball_suns.Add(sun);
                else
                    laser_suns.Add(sun);
            }
            float x, z, angle;
            float y = height_of_suns;
            for (int i = 0; i < 4; i++)
            {
                angle = (Mathf.PI / 180) * 360.0f / 4 * i;
                x = 0 + circling_radius * Mathf.Cos(angle);
                z = 0 + circling_radius * Mathf.Sin(angle);
                fb_suns_locations.Add(new Vector3(x, y, z));
            }

            ls_suns_locations.Add(new Vector3(ls_distance, height_of_suns - 20, 0));
            ls_suns_locations.Add(new Vector3(0, height_of_suns - 20, ls_distance));

        }
        private IEnumerator removeInvinsibility()
        {
            yield return new WaitForSeconds(0.1f);
            foreach(GameObject sun in getAlivedSun())
                sun.GetComponent<SunColliderHandler>().invinsible = false;

        }
        // Get to ready position before stage 2 starts
        void getToStage2Position()
        {
            if (!stage2_fb_ready)
            {
                bool check = true; // variable that check whether all suns are all set
                for (int i = 0; i < fireball_suns.Count; i++) // move each sun and its expected position
                {
                    GameObject sun = fireball_suns[i];
                    Vector3 dest = fb_suns_locations[i];
                    sun.transform.position = Vector3.MoveTowards(sun.transform.position, dest, Time.deltaTime * 50);
                    check = check && (Vector3.Distance(sun.transform.position, dest) < 1);
                    if (Vector3.Distance(sun.transform.position, dest) < 1) // reached the destination
                        sun.transform.parent = GameObject.Find("stage_2_chandelier").transform;
                }
                stage2_fb_ready = check;
            }

            if (!stage2_ls_ready) // same thing for laser
            {
                bool check = true;
                for (int i = 0; i < laser_suns.Count; i++)
                {
                    GameObject sun = laser_suns[i];
                    Vector3 dest = ls_suns_locations[i];
                    sun.transform.position = Vector3.MoveTowards(sun.transform.position, dest, Time.deltaTime * 50);
                    check = check && (Vector3.Distance(sun.transform.position, dest) < 1);
                }
                stage2_ls_ready = check;
            }
        }


        // check ready position
        bool lsInReadyPosition()
        {
            List<GameObject> alive_suns_laser = getAlivedLSSun();
            bool check = true;
            for (int i = 0; i < alive_suns_laser.Count; i++)
            {
                GameObject sun = alive_suns_laser[i];
                Vector3 dest = ls_suns_locations[i];
                check = check && (Vector3.Distance(sun.transform.position, dest) < 1);
            }
            return check;
        }

        // get to ready position
        void lsToReadyPosition()
        {
            List<GameObject> alive_suns_laser = getAlivedLSSun();
            for (int i = 0; i < alive_suns_laser.Count; i++)
            {
                GameObject sun = alive_suns_laser[i];
                Vector3 dest = ls_suns_locations[i];
                sun.transform.position = Vector3.MoveTowards(sun.transform.position, dest, Time.deltaTime * 30);
            }
        }


        // Shoot fireball
        private IEnumerator shoot_fireball()
        {
            while (true)
            {
                // wait a few seconds before shoot
                float wait_time = 1.0f;
                yield return new WaitForSeconds(wait_time); // wait here until wait_time, then keep executing
                                                            // choose sun to shoot fireball
                if (stage2_fb_ready && stage2_ls_ready)
                {
                    List<GameObject> alive_suns = getAlivedFBSun();
                    int chosen_sun = (int)Random.Range(0, alive_suns.Count);
                    GameObject sun_as_shooter = alive_suns[chosen_sun];
                    // Prepare to shoot
                    sun_as_shooter.transform.LookAt(target.transform.position); // make sun face the target
                    GameObject new_fireball = Instantiate(fireball, sun_as_shooter.transform.position - new Vector3(0, 5, 0), sun_as_shooter.transform.rotation);
                    new_fireball.name = "fireball_clone_";
                    new_fireball.transform.parent = GameObject.Find("collection_fb").transform;
                }
            }
        }


        private IEnumerator shoot_laser()
        {
            while (true)
            {
                // wait a few seconds before shoot
                float wait_time = 1.0f;
                yield return new WaitForSeconds(wait_time); // wait here until wait_time, then keep executing
                                                            // check if suns are in the correct location
                if (stage2_fb_ready && stage2_ls_ready)
                {
                    if (!lsInReadyPosition() && !hasLaser()) // if not in ready position, adjust position
                    {
                        laser_not_ready = true;
                    }
                    else if (lsInReadyPosition() && !hasLaser()) // in ready, no laser, then shoot laser
                    {
                        List<GameObject> alive_suns = getAlivedLSSun();
                        for (int i = 0; i < alive_suns.Count; i++)
                        {
                            GameObject sun_as_shooter = alive_suns[i];
                            sun_as_shooter.transform.LookAt(target.transform.position); // make sun face the target
                            Vector3 rot = sun_as_shooter.transform.rotation.eulerAngles;
                            rot = new Vector3(90, rot.y, rot.z);
                            GameObject new_laser = Instantiate(laser, sun_as_shooter.transform.position, Quaternion.Euler(rot));
                            new_laser.name = "laser_clone_" + i;
                        }
                        // assign new location
                        laser_not_ready = false;
                        for (int i = 0; i < alive_suns.Count; i++)
                        {
                            GameObject sun_as_shooter = alive_suns[i];
                            Vector3 curr_sun_location = sun_as_shooter.transform.position;
                            if (curr_sun_location[0] == ls_distance)
                                ls_suns_locations[i] = new Vector3(ls_distance, height_of_suns - 10, Random.Range(-(circling_radius + 10.0f), (circling_radius + 10.0f)));
                            else if (curr_sun_location[2] == ls_distance)
                                ls_suns_locations[i] = new Vector3(Random.Range(-(circling_radius + 10.0f), (circling_radius + 10.0f)), height_of_suns - 10, ls_distance);
                        }
                    }
                    else if (hasLaser())
                        laser_not_ready = false;

                }
            }
        }

        // Get all alived suns
        List<GameObject> getAlivedSun()
        {
            GameObject[] all_obj = UnityEngine.Object.FindObjectsOfType<GameObject>();
            List<GameObject> alive_suns = new List<GameObject>();
            foreach (GameObject obj in all_obj)
                if(obj!=null)
                    if ((obj.name.Contains("sun2_")) && obj.activeInHierarchy)
                        alive_suns.Add(obj);
            return alive_suns;
        }

        // Get alived suns for fireball
        List<GameObject> getAlivedFBSun()
        {
            List<GameObject> alive_suns = new List<GameObject>();
            foreach (GameObject obj in fireball_suns)
                if(obj!=null)
                    if (obj.activeInHierarchy)
                        alive_suns.Add(obj);
            return alive_suns;
        }

        // Get alived suns for laser
        List<GameObject> getAlivedLSSun()
        {
            List<GameObject> alive_suns = new List<GameObject>();
            foreach (GameObject obj in laser_suns)
                if(obj != null)
                    if (obj.activeInHierarchy)
                        alive_suns.Add(obj);
            return alive_suns;
        }

        // check whether there are lasers on the field
        bool hasLaser()
        {
            GameObject[] all_obj = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in all_obj)
                if ((obj.name.Contains("laser")) && obj.activeInHierarchy)
                    return true;
            return false;
        }

        // clean up laser if sun dies
        void cleanUpLaser()
        {
            GameObject[] all_obj = UnityEngine.Object.FindObjectsOfType<GameObject>();
            List<GameObject> all_laser_suns = getAlivedLSSun();
            if (all_laser_suns.Count == 1)
            {
                GameObject only_laser_sun = all_laser_suns[0];
                if (only_laser_sun.transform.position[0] == ls_distance)
                {
                    foreach (GameObject obj in all_obj)
                        if ((obj.name.Contains("laser")) && obj.activeInHierarchy && obj.transform.position[0] != ls_distance)
                            obj.SetActive(false);
                }
                else if (only_laser_sun.transform.position[0] != ls_distance)
                {
                    foreach (GameObject obj in all_obj)
                        if ((obj.name.Contains("laser")) && obj.activeInHierarchy && obj.transform.position[0] == ls_distance)
                            obj.SetActive(false);
                }
            }
            else if (all_laser_suns.Count == 0)
            {
                foreach (GameObject obj in all_obj)
                    if ((obj.name.Contains("laser")) && obj.activeInHierarchy)
                        obj.SetActive(false);
            }

        }

        // end of stage 2
        void endStage()
        {
            audioData.volume -= 0.05f * audioData.volume;
            stage_3.SetActive(true);
            List<GameObject> rest_of_suns = getAlivedSun();
            foreach (GameObject sun in rest_of_suns)
            {
                sun.GetComponent<SunColliderHandler>().invinsible = true;
                // get close to center
                if (Vector3.Distance(sun.transform.position, new Vector3(0, height_of_suns, 0)) >= 25)
                    sun.transform.position = Vector3.MoveTowards(sun.transform.position, new Vector3(0, height_of_suns, 0), 2f);
                else // closer enough, transfer sun to stage 2 with name changed
                    sun.SetActive(false);
            }
            if (getAlivedSun().Count == 0)
                gameObject.SetActive(false);
        }

    }
}

