using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFly : MonoBehaviour
{
    // Start is called before the first frame update
    public float flyspeed;
    public bool fly = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fly){
            transform.position += new Vector3(0, flyspeed, 0);
        }
    }

    public void SetFly(float fs){
        fly = true;
        flyspeed = fs;
    }
}
