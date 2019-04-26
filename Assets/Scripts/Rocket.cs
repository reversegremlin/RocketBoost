using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip win;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem explosionEngineParticles;
    [SerializeField] ParticleSystem winParticles;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] bool debugEnabled = true;

    Rigidbody rigidBody;
    AudioSource audioSource;

    int finishScene = 4;
    bool collisionsEnabled = true;

    bool particlesEnabled = false;

    enum State
    {
        PreLaunch = 0,
        Alive = 1,
        Dying = 2,
        Transcending = 3
    }

    State state = State.PreLaunch;

    // Start is called before the first frame update
    void Start()
    {

        if (Debug.isDebugBuild)
        {
            debugEnabled = true;
        }

        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;
            case "Finish":
                StartWinSequence();
                break;
            case "Unfriendly":
                if (collisionsEnabled == true)
                {
                    StartDeathSequence();
                }
                break;
            default:
                break;
        }
    }

    private void StartWinSequence()
    {
        state = State.Transcending;
        audioSource.PlayOneShot(win);
        if (particlesEnabled) { winParticles.Play(); }
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(explosion);
        if (particlesEnabled) { explosionEngineParticles.Play(); }
        Invoke("ReloadCurrentLevelWithDelay", levelLoadDelay);
    }

    void ReloadCurrentLevelWithDelay()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    void LoadNextScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.buildIndex < finishScene)
        {
            SceneManager.LoadScene(currentScene.buildIndex + 1);
        } 
        else
        {
            SceneManager.LoadScene(finishScene);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (state == State.PreLaunch)
        {
            Thrust();
        }
        else if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
        if (debugEnabled == true)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        } else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsEnabled = !collisionsEnabled;
        }
    }

    private void Thrust()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
            if (state == State.PreLaunch) { state = State.Alive; }
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);

        }
        if (particlesEnabled) { mainEngineParticles.Play(); }
         

    }

    private void Rotate()
    {
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        rigidBody.freezeRotation = true; //take manual control of rotation

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //take manual control of rotation
    }
}