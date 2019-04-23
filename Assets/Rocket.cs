using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State
    {
        Alive = 0,
        Dying = 1,
        Transcending = 2
    }

    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
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
                print("Hit Finish");
                state = State.Transcending;
               Invoke("LoadNextScene", 2f);

                break;
            case "Unfriendly":
                print("Dead");
                state = State.Dying;
                //StartCoroutine(AudioController.FadeOut(audioSource, 1f));
                audioSource.Stop();

                Invoke("ReloadCurrentLevelWithDelay", 3f);
                break;
            default:
                print("Dead");
                break;
        }

    }
    void ReloadCurrentLevelWithDelay()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    void LoadNextScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.buildIndex == 1)
        {
            SceneManager.LoadScene(2);
        }
        SceneManager.LoadScene(currentScene.buildIndex + 1);

    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }

    }

    private void Thrust()
    {
        
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            //todo no sound when dying
            if (!audioSource.isPlaying)
            {
                StartCoroutine(AudioController.FadeIn(audioSource, 4f));

                //audioSource.Play();

            }
        }
        else
        {
            StartCoroutine(AudioController.FadeOut(audioSource, 2f));

            //audioSource.Stop();
        }
    }

    private void Rotate()
    {
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        rigidBody.freezeRotation = true; //take manual control of rotation

        if (Input.GetKey(KeyCode.A))
        {
            // time.deltatime = framespeed


            transform.Rotate(Vector3.forward * rotationThisFrame);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //take manual control of rotation

    }

    public static class AudioController
    {
        public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
        {
            float startVolume = audioSource.volume;
            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
                yield return null;
            }
            audioSource.Stop();
        }
        public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
        {
            audioSource.Play();
            audioSource.volume = 0f;
            while (audioSource.volume < 1)
            {
                audioSource.volume += Time.deltaTime / FadeTime;
                yield return null;
            }
        }
    }


}