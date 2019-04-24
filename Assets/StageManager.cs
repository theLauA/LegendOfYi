using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunCollision
{   
    public class StageManager : MonoBehaviour
    {
        public GameObject fireball_homing;
        public GameObject sun;
        //public GameObject stage_2;
        public GameObject target;
        public float turnAngle;
        public int num_of_suns;
        public float circling_radius;
        public int height_of_suns;

        private float time;
        private bool harder;
        private float harder_duration; // hard level duration
        private float turn_harder;
        private float shoot_faster;
        public AudioSource audioData;

        // Use this for initialization
        void Start()
        {
            audioData.Play(0);
            prepareElements();
            StartCoroutine("shoot_fireball");
        }

        // Update is called once per frame
        void Update()
        {
            turn();
            int num_of_sun = getAlivedSun().Count;

            if (num_of_sun > 6)  // Still in stage 1
            {
                if ((int)time % 10 == 0 && harder_duration <= 0)  //make it harder sometimes
                    makeItHarder();
                if (harder_duration <= 0)
                    backToNormal();
                harder_duration -= Time.deltaTime;
            }
            else // Preceeding to next stage
            {
                if (num_of_sun > 0) // transfer the suns to next stage
                {
                    StopCoroutine("shoot_fireball");
                    endStage();
                }
                else
                { // all suns have been transfered, shut down stage 1
                    //gameObject.SetActive(false);
                    Destroy(gameObject);

                }
            }

            time += Time.deltaTime;
        }

        void prepareElements()
        {
            createSuns();
            // initialize other variables
            time = 1;
            harder = false;
        }

        // create suns
        void createSuns()
        {
            float x, z, angle;
            float y = height_of_suns;
            for (int i = 0; i < num_of_suns; i++)
            {
                angle = (Mathf.PI / 180) * 360.0f / num_of_suns * i;
                x = 0 + circling_radius * Mathf.Cos(angle);
                z = 0 + circling_radius * Mathf.Sin(angle);
                GameObject new_sun = Instantiate(sun, new Vector3(x, y, z), transform.rotation);
                new_sun.transform.parent = gameObject.transform;
                new_sun.name = "sun1_" + (i + 1);
            }
        }

        // spin the suns
        void turn()
        {
            Quaternion newRotation = transform.rotation;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (turnAngle + turn_harder) * Time.deltaTime, transform.eulerAngles.z);
        }

        // make the suns shoot fireballs
        private IEnumerator shoot_fireball()
        {
            while (true)
            {
                // wait a few seconds before shoot
                float wait_time = 1.0f;
                yield return new WaitForSeconds(wait_time - shoot_faster); // wait here until wait_time, then keep executing
                                                                           // choose sun to shoot fireball
                List<GameObject> alive_suns = getAlivedSun();
                int chosen_sun = (int)Random.Range(0, alive_suns.Count);
                GameObject sun_as_shooter = alive_suns[chosen_sun];
                // Prepare to shoot
                sun_as_shooter.transform.LookAt(target.transform.position); // make sun face the target
                GameObject new_fireball = Instantiate(fireball_homing, sun_as_shooter.transform.position + sun_as_shooter.transform.forward * Time.deltaTime * 500, Quaternion.identity); //sun_as_shooter.transform.rotation);

                new_fireball.name = "fireball_clone_";
                new_fireball.transform.parent = GameObject.Find("collection_fb").transform;
            }
        }

        // get current alived suns
        List<GameObject> getAlivedSun()
        {
            GameObject[] all_obj = UnityEngine.Object.FindObjectsOfType<GameObject>();
            List<GameObject> alive_suns = new List<GameObject>();
            foreach (GameObject obj in all_obj)
                if ((obj.name.Contains("sun1_")) && obj.activeInHierarchy)
                    alive_suns.Add(obj);
            return alive_suns;
        }

        // make the stage harder
        void makeItHarder()
        {
            harder_duration = 5.0f;
            turn_harder = 100.0f;
            shoot_faster = 0.7f;
        }

        // make the stage harder
        void backToNormal()
        {
            turn_harder = 0.0f;
            shoot_faster = 0.0f;
        }

        // end of stage 1
        void endStage()
        {
            audioData.volume -= 0.05f * audioData.volume;
            List<GameObject> rest_of_suns = getAlivedSun();

            foreach (GameObject sun in rest_of_suns)
            {
                // get close to center
                SunColliderHandler script = sun.GetComponent<SunColliderHandler>();
                script.invinsible = true;

                if (Vector3.Distance(sun.transform.position, new Vector3(0, height_of_suns, 0)) >= 20)
                    sun.transform.position = Vector3.MoveTowards(sun.transform.position, new Vector3(0, height_of_suns, 0), Time.deltaTime * 10);
                
            }

        }
    }
}


