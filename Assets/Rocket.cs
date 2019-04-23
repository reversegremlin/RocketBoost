using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource audioSource;

    int currentLevel = 0;

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
                if (currentScene.buildIndex == 1)
                {
                    SceneManager.LoadScene(2);
                } else
                {
                    SceneManager.LoadScene(currentScene.buildIndex + 1);
                }
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