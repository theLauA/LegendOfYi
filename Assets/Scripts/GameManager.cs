using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

    // Use this for initialization
    public GameObject pauseMenu;
    public GameObject introMenu;
    public GameObject instructionMenu;
    public GameObject crossHair;
    public GameObject healthBar;

    public GameObject Boss;
    public GameObject EndCredit;
    public GameObject Ground;
    public Material Green;

    public GameObject Character;
    public GameObject generalCamera;
    public SimpleHealthBar healthBarValue;
    public GameObject deathMenu;
    //private Material Original;
    private GameObject Hero;
    private GameObject Enemy;
    void Start () {
        pauseMenu.SetActive(false);
        crossHair.SetActive(false);
        healthBar.SetActive(false);
        introMenu.SetActive(true);
        instructionMenu.SetActive(false);
        EndCredit.SetActive(false);
        deathMenu.SetActive(false);
        generalCamera.SetActive(true);
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

                
            }
        }
        else if (instructionMenu.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                instructionMenu.SetActive(false);
                StartCoroutine("startGame");
                //startGame();
                Time.timeScale = 1;


            }
        }
        else if (Enemy == null)
        {
            StartCoroutine("endGame");

        }
        else if(!deathMenu.activeSelf){
            if (healthBarValue.GetCurrentFraction == 0)
            {
                deathMenu.SetActive(true);
                //Hero.SetActive(false);
                StartCoroutine("stopGame");
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
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
        }else if (deathMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                deathMenu.SetActive(false);
                Debug.Log(deathMenu.activeSelf);
                StartCoroutine("startGame");
            }
        }
        
	}
    private IEnumerator startGame()
    {
        Hero = GameObject.Instantiate(Character);
        Hero.transform.position = new Vector3(-84.4f, 10f, 0f);
        Hero.name = "target";
        Hero.SetActive(true);
        generalCamera.SetActive(false);
        foreach(GameObject arrow in GameObject.FindGameObjectsWithTag("Arrow"))
        {
            Destroy(arrow);
        }
        //Enemy.SetActive(true);
        Enemy = GameObject.Instantiate(Boss);
        Enemy.transform.position = new Vector3(34.17299f, 99f, -26.12205f);
        Enemy.SetActive(false);
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
        generalCamera.SetActive(true);

        Renderer rend = Ground.GetComponent<Renderer>();
        rend.material.Lerp(rend.material, Green, 9f);
        crossHair.SetActive(false);
        healthBar.SetActive(false);
        yield return new WaitForSeconds(10f);
        Time.timeScale = 0;
    }

    private IEnumerator stopGame()
    {   
        Destroy(Hero);
        generalCamera.SetActive(true);
        Destroy(Enemy, 3f);

        yield return new WaitForSeconds(3.2f);
        Time.timeScale = 0;
    }

    
}
