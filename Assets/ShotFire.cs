using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.PyroParticles
{
    public class ShotFire : MonoBehaviour
    {

        public GameObject fireballPrefab;
        public GameObject player;
        // Use this for initialization
        void Start()
        {
            StartCoroutine("Fire");
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator Fire()
        {
            while (true)
            {
                GameObject fireball = GameObject.Instantiate(fireballPrefab, transform.position, Quaternion.LookRotation(player.transform.position - transform.position));
                MeteorSwarmScript currentscript = fireball.GetComponent<MeteorSwarmScript>();
                if (currentscript != null)
                {
                    currentscript.Source = transform.position;
                    currentscript.SourceRadius = 50f;
                }
               
                fireball.transform.position = transform.position;
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
