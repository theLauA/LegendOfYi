using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

    private SimpleHealthBar healthBar;
    public Camera mainCam;

    private float health;
    private float currentHleath;
    private float shakeDuration;
    private float decreaseFactor;
    private float shakeAmount;
    private Vector3 originalCamPosition;
    void Start () {
        healthBar = GameObject.Find("HP").GetComponent<SimpleHealthBar>();
        health = 100f;
        currentHleath = 100f;
        shakeDuration = 0f;
        shakeAmount = 0.9f;
        decreaseFactor = 1.0f;

        originalCamPosition = mainCam.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
	    if(shakeDuration > 0)
        {
            mainCam.transform.localPosition = originalCamPosition + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            mainCam.transform.localPosition = originalCamPosition;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Fire")
        {
            Debug.Log(collision.collider.name);
            shakeDuration = 2f;
            currentHleath -= 50f;
            healthBar.UpdateBar(currentHleath, health);
        }
        
    }
}
