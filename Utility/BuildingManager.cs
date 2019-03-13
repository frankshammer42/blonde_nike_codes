using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int number_of_faces;
    public int gridWidth;
    public int gridHeight;
    public float rotation; //Set the rotation for the face 
    public int scale_dist;
    private List<GameObject> tetris_faces;
    
    void Awake(){
        tetris_faces = new List<GameObject>();
        int x_position = (int) transform.position.x;
        int z_position = (int) transform.position.z;
        for (int i = 0; i < number_of_faces; i++){
            int x_offset = 0;
            int z_offset = 0;
            if (rotation == 0){
                x_offset = x_position;
                z_offset = z_position + i * scale_dist;
            }
            else if (rotation == 90){
                x_offset = x_position + i * scale_dist;
                z_offset = z_position;
            }
            GameObject new_tetris_face = (GameObject) Instantiate(Resources.Load("Prefabs/TetrisFace", typeof(GameObject)),
                new Vector3(x_offset, 0,  z_offset), Quaternion.Euler(0, rotation,  0));
            new_tetris_face.GetComponent<TetrisFace>().gridWidth = gridWidth;
            new_tetris_face.GetComponent<TetrisFace>().gridHeight = gridHeight;
            tetris_faces.Add(new_tetris_face);
        }
    }

    // Update is called once per frame
    void Update(){
   
        
    }
}
