using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunCollision
{
    public class SunColliderHandler : MonoBehaviour
    {
        //private float maxHealth;
        private float currentHealth;
        public bool invinsible;
        // Use this for initialization
        void Start()
        {
            //maxHealth = 100f;
            currentHealth = 100f;
            invinsible = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (currentHealth == 0f)
            {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "Arrow" && !invinsible)
            {
                currentHealth -= 100f;
            }
            else if (collision.collider.tag == "Arrow")
            {
                Debug.Log(invinsible);
            }
        }
    }
}
