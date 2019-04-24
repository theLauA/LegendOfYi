using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_motion : MonoBehaviour {
    private Animator controller;
    private GameObject constructor;
    private float angle;
    private float peak = 2f;
    private float diffWithPeak;

	// Use this for initialization
	void Start () {
        controller = GetComponent<Animator>();
        constructor = GameObject.Find("Constructor");
        StartCoroutine("Spawn");
    }

    // Update is called once per frame
    void Update() {

        bool walk_pressed = Input.GetKey(KeyCode.UpArrow);
        bool run_pressed = Input.GetKey(KeyCode.Z);
        bool jump_pressed = Input.GetKey(KeyCode.Space);
        bool left_pressed = Input.GetKey(KeyCode.LeftArrow);
        bool right_pressed = Input.GetKey(KeyCode.RightArrow);
        angle = constructor.transform.eulerAngles.y * Mathf.Deg2Rad;

        controller.SetBool("isWalking", walk_pressed);
        controller.SetBool("isRunning", run_pressed);
        controller.SetBool("isJumping", jump_pressed);

        // Walk or Run or Idle
        if (walk_pressed && run_pressed)
            controller.SetFloat("Speed", 3);
        else if (walk_pressed)
            controller.SetFloat("Speed", 1);
        else
            controller.SetFloat("Speed", 0);

        // Turn left or right
        if (!controller.GetCurrentAnimatorStateInfo(0).IsName("idle") && left_pressed)
            controller.SetFloat("turnAngle", -3);
        else if (!controller.GetCurrentAnimatorStateInfo(0).IsName("idle") && right_pressed)
            controller.SetFloat("turnAngle", 3);
        else
            controller.SetFloat("turnAngle", 0);

        // Jump
        if (!controller.GetCurrentAnimatorStateInfo(0).IsName("jump_pose") && jump_pressed)
        {
            controller.SetBool("finishJumping", false);
        }

    }

    private void OnAnimatorMove()
    {
        if (controller)
        {
            // position - moving
            Vector3 newPosition = transform.position;
            newPosition.x += Mathf.Sin(angle) * controller.GetFloat("Speed") * Time.deltaTime;
            newPosition.z += Mathf.Cos(angle) * controller.GetFloat("Speed") * Time.deltaTime;
            // position - jumping
            newPosition.y += controller.GetFloat("heightChange") * Time.deltaTime;
            newPosition.y = Mathf.Max(Mathf.Min(newPosition.y, peak),1); // keep y in the range
         
            transform.position = newPosition;

            // if in jumping state and jumping is not finished
            if (controller.GetCurrentAnimatorStateInfo(0).IsName("jump_pose") && !controller.GetBool("finishJumping"))
            {
                if (!controller.GetBool("reachedPeak")) // peak not reached, keep jumping up
                {
                    controller.SetFloat("heightChange", 8);
                    if(newPosition.y == peak)
                        controller.SetBool("reachedPeak", true);
                }
                else if (newPosition.y <= 1 && controller.GetBool("reachedPeak")) // finished jumping, reset
                {
                    controller.SetBool("finishJumping", true);
                    controller.SetBool("reachedPeak", false);
                    controller.SetFloat("heightChange", 0);
                }
                else if(controller.GetBool("reachedPeak"))// peak reached, falling down
                {
                    controller.SetFloat("heightChange", -8);
                }

            }

            // rotation
            Quaternion newRotation = transform.rotation;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + controller.GetFloat("turnAngle"),transform.eulerAngles.z);
        }
    }
}
