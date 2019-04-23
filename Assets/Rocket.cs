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
        Scene currentScene = SceneManager.GetActiveScene();

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;
            case "Finish":
                print("Hit Finish");
                //total levels = 2
                state = State.Transcending;
               Invoke("LoadNextScene", 3f);

                break;
            case "Unfriendly":
                print("Dead");
                SceneManager.LoadScene(currentScene.buildIndex);
                break;
            default:
                print("Dead");
                break;
        }

    }

    private void LoadNextScene()
    {
        //                Invoke("LoadNextScene", 1f);
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
        Thrust();
        Rotate();

    }

    private void Thrust()
    {
        
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);

            if (!audioSource.isPlaying)
            {
                audioSource.Play();

            }
        }
        else
        {
            audioSource.Stop();
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

}