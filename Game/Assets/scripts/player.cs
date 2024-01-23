using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;


public class player : MonoBehaviour {
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject[] lives;

    public Camera thirdPersonCamera; // Reference to the third-person camera
    public Camera firstPersonCamera; // Reference to the first-person camera
    private bool isThirdPerson = true;

    public Animator anim;
    public Rigidbody rb;

    public AudioSource audioSource;
    public AudioClip collectSound;
    public AudioClip mushroomcollectSound;
    public AudioClip btnSound;
    public AudioClip checkPointSound;
    public AudioClip roundEndSound;
    public AudioClip fallSound;

    public float jumpForce;
    private float inputH;
    private float inputV;
    private bool isGrounded;
    public float speed;
    private Vector3 spawnPos;
    private Quaternion spawnRot;
    public int score;
    private int livesCount;
    public Text scoreText;
    private bool isGameOver = false; // Flag to track game over state




    private void Awake()
    {
        Time.timeScale = 1;
    }
    void Start () {

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        jumpForce = 1f;
        speed = 1f;
        isGrounded = true;
        spawnPos = transform.position;
        spawnRot = transform.rotation;
        score = 0;
        livesCount = 2;
        setScoreText();
        // Initialize camera states
        thirdPersonCamera.enabled = true;
        firstPersonCamera.enabled = false;
    }

    void Update()
    {
        if (isGameOver)
        {
            anim.SetFloat("inputH", 0);
            anim.SetFloat("inputV", 0);
            return; // Skip the rest of the update if the game is over
        }
        //handling movement 
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        // Update animations
        anim.SetFloat("inputH", inputH); //running animations working only when grounded
        anim.SetFloat("inputV", inputV);


        float moveY = inputH * 150f * Time.deltaTime * speed;
        float moveZ = inputV * 5f * Time.deltaTime * speed;

        transform.Translate(0, 0, moveZ);
        transform.Rotate(0, moveY, 0);
        
        //only able to jump when grounded
        if (isGrounded == true)
        {
            anim.SetFloat("inputH", inputH);    //running animations working only when grounded
            anim.SetFloat("inputV", inputV);

            //jumping
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                anim.SetBool("jump", true); //telling animator that a jump has been initiated
                rb.AddForce(new Vector3(0, 500 * jumpForce, 0), ForceMode.Impulse); //actual jump
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseBtn();
        }

        // Toggle camera view when 'V' is pressed
        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleCameraView();
        }
    }

    //progressing levels
    void nextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Assuming "LoadingScene" is not directly after the last gameplay level in the build index order
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings - 1)
        {
            nextSceneIndex = 0; // Loop back to the first scene (menu) or set to a specific scene that you want to load after the last level
        }

        string nextScenePath = SceneUtility.GetScenePathByBuildIndex(nextSceneIndex);
        string nextSceneName = Path.GetFileNameWithoutExtension(nextScenePath);

        Debug.Log("Current scene index: " + currentSceneIndex);
        Debug.Log("Next scene index: " + nextSceneIndex);
        Debug.Log("Setting next scene to: " + nextSceneName);

        // Set the PlayerPrefs for the next gameplay scene
        PlayerPrefs.SetString("NextScene", nextSceneName);
        // Load the LoadingScene
        SceneManager.LoadScene("LoadingScene");
    }



    void OnCollisionEnter(Collision col)
    {
        //checking if on ground
        if (col.gameObject.tag == "Ground")
        { 
            isGrounded = true;
            anim.SetBool("isGrounded", isGrounded);
            anim.SetBool("jump", false);    //jump will always be reset to false once grounded
        }

        //dying & respawning
        else if (col.gameObject.tag == "Death")
        {
            

            StartCoroutine(deathSoundPlay());
            
            lives[livesCount].SetActive(false);
            if (livesCount >= 0)
            {
                livesCount -= 1;
                if (livesCount < 0)
                {
                    Debug.Log("Lives : " + livesCount);
                    gameOverPanel.SetActive(true);
                    isGameOver = true; // Set the game over flag
                }
            }
            

        }   
    }

    //checking if left ground
    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isGrounded = false;
            anim.SetBool("isGrounded", isGrounded);
        }
    }

    void OnTriggerEnter(Collider trig)
    {
        if (trig.gameObject.tag == "Score")
        {
            Destroy(trig.gameObject);
            audioSource.PlayOneShot(collectSound);
            score++;
            setScoreText();
        }

        else if (trig.gameObject.tag == "JumpPowerup")
        {
            Destroy(trig.gameObject);
            audioSource.PlayOneShot(mushroomcollectSound);
            jumpForce = 1.5f;
        }

        else if (trig.gameObject.tag == "Checkpoint")
        {
            StartCoroutine(checkPointSoundPlay());
            spawnPos = trig.transform.position;  //set new spawn points
            spawnRot = trig.transform.rotation;
            Destroy(trig.gameObject);    //get rid of checkpoint
        }

        else if (trig.gameObject.tag == "Goal" && score >= 5)    //requires all score pickups
        {
            Destroy(trig.gameObject);
            StartCoroutine(TransitionToEndOfLevel());
        }
    }

    IEnumerator TransitionToEndOfLevel()
    {
        audioSource.PlayOneShot(roundEndSound);
        anim.Play("Win", -1, 0f);
        yield return new WaitForSeconds(3);

        // Transition to the next level through the loading screen
        nextLevel();
    }



    void setScoreText()
    {
        if (score < 5)
        {
            scoreText.text = "Score: " + score.ToString();
        }
        else
        {
            scoreText.text = "Go to the goal!";
        }
    }

    public void PauseBtn()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0.3f;
    }
    public void ResumeBtn()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }
    public void RestartBtn()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MenuBtn()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitBtn()
    {
        Application.Quit();
    }

    public void SoundOnBtn()
    {
        audioSource.enabled = false;
    }
    public void SoundOffBtn()
    {
        audioSource.enabled = true;
    }

    public void btnSoundPlay()
    {
        audioSource.PlayOneShot(btnSound);
    }

    IEnumerator roundEnded()
    {
        audioSource.PlayOneShot(roundEndSound);
        yield return new WaitForSeconds(1); // Wait for the end round sound to play
        anim.Play("Win", -1, 0f);
        yield return new WaitForSeconds(3); // Wait for the win animation to play
        nextLevel(); // Call the nextLevel method directly
    }


    IEnumerator checkPointSoundPlay()
    {
        yield return new WaitForSeconds(1);
        audioSource.PlayOneShot(checkPointSound);
    }

    IEnumerator deathSoundPlay()
    {
        audioSource.PlayOneShot(fallSound);
        yield return new WaitForSeconds(0.5f);
        transform.position = spawnPos;
        transform.rotation = spawnRot;
        rb.velocity = new Vector3(0f, 1f, 0f); //resetting velocity
        anim.Play("Lose", -1, 0f);  //play lose animation
    }

    private void ToggleCameraView()
    {
        isThirdPerson = !isThirdPerson;
        thirdPersonCamera.enabled = isThirdPerson;
        firstPersonCamera.enabled = !isThirdPerson;
    }
}