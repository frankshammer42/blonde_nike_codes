using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NikeFly : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] nikecubes;
    public float flyspeed;
    
    void Awake()
    {
        nikecubes = GameObject.FindGameObjectsWithTag("NikeCube");
        Debug.Log(nikecubes.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void StartFly(){
        float offset = Random.Range(-0.5f, 0.5f);
        foreach (var nikecube in nikecubes){
            nikecube.GetComponent<BasicFly>().SetFly(flyspeed + offset);
        }
    }
    
}
