using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This Script is used for generate cubes in the scene in Edit Mode

[ExecuteInEditMode]
public class CreateNikeCubes : MonoBehaviour
{
    // Start is called before the first frame update
    public bool place_boxes = false;
    public GameObject boxPrefab;
    public int number_boxes;
    private bool cubes_generated = false;
    
    void Awake(){
    }
    
    void Update()
    {
        if (place_boxes){
            place_boxes = false;
            Collider2D mycollider = GetComponent<PolygonCollider2D>();
            Bounds bounds = mycollider.bounds;
            int counter = 0;
            for (int i = 0; i < number_boxes; i++){
                float x = Random.Range(bounds.min.x, bounds.max.x);
                float y = Random.Range(bounds.min.y, bounds.max.y);
                if(mycollider.OverlapPoint(new Vector2(x,y))){
                    Vector3 spawn = new Vector3(x,y,0);
                    GameObject spawnedObject = Instantiate(boxPrefab,spawn,Quaternion.identity, transform);
                    spawnedObject.tag = "NikeCube";
                    counter++;
                }else{
                    continue;
                }
            }
            Debug.Log(counter);
            cubes_generated = true;
        } 
    }
}
