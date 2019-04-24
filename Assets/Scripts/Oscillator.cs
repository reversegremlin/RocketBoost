using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;


    // todo remove from inspector later
    [Range(0,1)] [SerializeField] float movementFactor; //0 for not moved, 1 for fully moved.

    Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //todo protect against period is zero

        // todo set movememnt factor automatically
        if (period <= Mathf.Epsilon)
        {
            return;
        }

        float cycles = Time.time / period; //grows continually from zero

        const float tau = Mathf.PI * 2f; //about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau); //goes from -1 to +1

        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
         
    }
}
