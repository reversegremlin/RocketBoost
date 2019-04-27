using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{

    [SerializeField] AudioClip backgroundMusic;

    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(backgroundMusic);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
