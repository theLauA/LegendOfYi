using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // Use this for initialization
    public GameObject pauseMenu;
    public GameObject introMenu;
    public GameObject instructionMenu;
    public GameObject crossHair;
    public GameObject healthBar;

    public GameObject Enemy;
    public GameObject EndCredit;
    public GameObject Ground;
    public Material Green;

    public GameObject Hero;
    public Camera generalCamera;
   
    //private Material Original;
	void Start () {
        pauseMenu.SetActive(false);
        crossHair.SetActive(false);
        healthBar.SetActive(false);
        introMenu.SetActive(true);
        instructionMenu.SetActive(false);
        Enemy.SetActive(false);
        EndCredit.SetActive(false);

        Hero.SetActive(false);
        generalCamera.enabled = true;
        Time.timeScale = 0;
      
    }
	
	// Update is called once per frame
	void Update () {
        if (introMenu.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                introMenu.SetActive(false);
                instructionMenu.SetActive(true);
                crossHair.SetActive(true);
                healthBar.SetActive(true);

                Hero.SetActive(true);
                generalCamera.enabled = false;
            }
        }
        else if (instructionMenu.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                instructionMenu.SetActive(false);
                StartCoroutine("startGame");
                Time.timeScale = 1;


            }
        }
        else if (Enemy == null)
        {
            StartCoroutine("endGame");
        }
        else {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!pauseMenu.activeInHierarchy)
                {
                    PauseGame();
                }
                else
                {
                    ContinueGame();
                }


            }
        }
        
	}
    private IEnumerator startGame()
    {
        yield return new WaitForSeconds(1f);
        Enemy.SetActive(true);
    }
    private void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
    private void ContinueGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    private IEnumerator endGame()
    {

        yield return new WaitForSeconds(1f);
        EndCredit.SetActive(true);
        Hero.SetActive(false);
        generalCamera.enabled = true;
        
        Renderer rend = Ground.GetComponent<Renderer>();
        rend.material.Lerp(rend.material, Green, 9f);
        crossHair.SetActive(false);
        healthBar.SetActive(false);
        yield return new WaitForSeconds(10f);
        Time.timeScale = 0;
    }
}
