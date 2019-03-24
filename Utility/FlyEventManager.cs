using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlyEventManager : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEvent starttofly;
    private AudioSource myAudioSource;
    private bool flied = false;
    
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (179 - myAudioSource.time <= 2 && !flied){
            starttofly.Invoke();
            flied = true;
        }
    }
}
