using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//Class for building' faces
// Shit, static keyword really fuck me up
public class TetrisFace : MonoBehaviour{
    // TODO: need to change the orientation to build structures face 
    public int gridHeight;
    public int gridWidth;
    public float flyspeed;
    public bool flyup;
        
    private Transform[,] FaceGrid;
    private int _LowerLeftxOffset; //For Boundary Check
    private int _lowerRightxOffset;
    private int x_offset; //Center x
    private int z_offset; //Center z 
    private int y_offset; //Center Y 
    private float y_rotation;
    private List<GameObject> tetris_blocks;

    private void Start(){
        _LowerLeftxOffset = -gridWidth / 2;
        _lowerRightxOffset = _LowerLeftxOffset + gridWidth;
        FaceGrid = new Transform[gridWidth, gridHeight];
        x_offset = (int) transform.position.x;
        z_offset = (int) transform.position.z;
        y_offset = (int) transform.position.y;
        y_rotation = transform.rotation.eulerAngles.y; 
        tetris_blocks = new List<GameObject>();
        Generate_tetris();
    }

    private void Update(){
        if (CheckReset()){
            ResetFace();
            Generate_tetris();
        }
    }

    public void UpdateGrid(Tetris currentBlock){
        for (int i = 0; i < gridWidth; i++){
            for (int j = 0; j < gridHeight; j++){
                if (FaceGrid[i, j] != null){
                    if (FaceGrid[i, j].parent == currentBlock.transform){
                        FaceGrid[i, j] = null;
                    }
                }
            }
        }

        if (y_rotation == 0){
            foreach (Transform cubeTransform in currentBlock.transform){
                Vector3 pos = Round(cubeTransform.position);
                if (pos.y - y_offset < gridHeight){
                    int xInGridSystem = (int)Mathf.Round(pos.x - _LowerLeftxOffset - x_offset);
                    int yInGridSystem = (int) Mathf.Round(pos.y - y_offset);
                    FaceGrid[xInGridSystem, yInGridSystem] = cubeTransform;
                }
            }
        }
        else if (y_rotation == 90){
            foreach (Transform cubeTransform in currentBlock.transform){
                Vector3 pos = Round(cubeTransform.position);
                if (pos.y - y_offset < gridHeight){
                    //if rotation is 90 we need to check z
                    int xInGridSystem = (int)Mathf.Round(pos.z - _LowerLeftxOffset - z_offset);
                    int yInGridSystem = (int) Mathf.Round(pos.y - y_offset);
                    FaceGrid[xInGridSystem, yInGridSystem] = cubeTransform;
                }
            }
        }
    }


    public Transform GetGridTransform(Vector3 gridPos){
        if (gridPos.y - y_offset > gridHeight-1){
            return null;
        }

        int xInGridSystem = 0;
        if (y_rotation == 0){
            xInGridSystem = (int)Mathf.Round(gridPos.x - _LowerLeftxOffset - x_offset); 
        }
        else if (y_rotation == 90){
            xInGridSystem = (int)Mathf.Round(gridPos.z - _LowerLeftxOffset - z_offset); 
        }
        return FaceGrid[xInGridSystem, (int)Mathf.Round(gridPos.y - y_offset)];
    }
    
    
    //Helper Methods for moving around the rows
    public bool RowFullAt(int row_index){
        for (int i = 1; i < gridWidth-1; i++){
            if (FaceGrid[i, row_index] != null){
            }
            if (FaceGrid[i, row_index] == null){
                return false;
            }
        }
        return true;
    }

    public bool CheckReset(){
        bool result = false;
        int counter = 0;
        for (int j = gridHeight-1; j > gridHeight-5 ; j--){
            for (int i = 0; i < gridWidth; i++){
                if (FaceGrid[i,j] != null){
                    counter++;
                    break;
                }
            }
        }
        if (counter == 4){
            result = true;
        }
        return result;
    }


    public void ResetFace(){
        for (int i = 0; i < tetris_blocks.Count; i++){
            Destroy(tetris_blocks[i]);
        }
        for (int i = 1; i < gridWidth-1; i++){
            for (int j = 1; j < gridHeight - 1; j++){
                FaceGrid[i, j] = null;
            }
        }
    }

    //Only Call this method when the row is full
    public void DeleteCubesAtRow(int row_index){
        for (int i = 1; i < gridWidth-1; i++){
            Destroy(FaceGrid[i, row_index].gameObject);
            FaceGrid[i, row_index] = null;
        }
    }


    public void MoveRowDown(int row_index){
        for (int i = 0; i < gridWidth; i++){
            if (FaceGrid[i, row_index] != null){
                FaceGrid[i, row_index - 1] = FaceGrid[i, row_index];
                FaceGrid[i, row_index] = null;
                FaceGrid[i, row_index-1].position += new Vector3(0, -1, 0);
            } 
        }
    }


    public void MoveAllRowsAboveDown(int row_index){
        for (int i = row_index; i < gridHeight; i++){
            MoveRowDown(i);
        }
    }


    public void DeleteRowsIfFull(){
        for (int i = 0; i < gridHeight; i++){
            if (RowFullAt(i)){
                DeleteCubesAtRow(i);
                MoveAllRowsAboveDown(i+1);
                i--;
            }
        }
    }
    
    
    public void Generate_tetris(){
        string next_tetris_type = GeneratePrefabNameString();
        int halfgrid = gridWidth / 2;
        
        int random_offset = Random.Range(-3, 3);
        if (y_rotation == 0){
            GameObject new_tetris_block = (GameObject) Instantiate(Resources.Load(next_tetris_type, typeof(GameObject)),
                new Vector3(random_offset + x_offset, gridHeight+10+y_offset, z_offset), Quaternion.identity);
            //Set Which Face Controls which
            new_tetris_block.GetComponent<Tetris>().SetFaceReference(this);
            new_tetris_block.GetComponent<Tetris>().SetZoffset(z_offset);
            new_tetris_block.GetComponent<Tetris>().SetYrotation(y_rotation);
            new_tetris_block.GetComponent<Tetris>().SetYoffset(y_offset);
            tetris_blocks.Add(new_tetris_block);
        }
        else if (y_rotation == 90){
            GameObject new_tetris_block = (GameObject) Instantiate(Resources.Load(next_tetris_type, typeof(GameObject)),
                new Vector3(x_offset, gridHeight+10+y_offset, z_offset+random_offset), Quaternion.Euler(0, 90, 0));
            //Set Which Face Controls which
            new_tetris_block.GetComponent<Tetris>().SetFaceReference(this);
            new_tetris_block.GetComponent<Tetris>().SetZoffset(z_offset);
            new_tetris_block.GetComponent<Tetris>().SetYrotation(y_rotation);
            new_tetris_block.GetComponent<Tetris>().SetYoffset(y_offset);
            tetris_blocks.Add(new_tetris_block);
        }
    }


    public bool InGrid(Vector3 pos){
        //TODO: Based on lower left side position, adjust the checking method
        if (y_rotation == 0){
            return pos.x >= _LowerLeftxOffset + x_offset + 0.5 && pos.x <= _lowerRightxOffset + x_offset- 0.5 && pos.y - y_offset >= 0.5;
        }
        if(y_rotation == 90){
            return pos.z >= _LowerLeftxOffset + z_offset + 0.5 && pos.z <= _lowerRightxOffset + z_offset- 0.5 && pos.y - y_offset >= 0.5;
        }
        return false;
//        return pos.x >= 0.5 && pos.x <= gridWidth-0.5 && pos.y >= 0.5;
    }


    public Vector3 Round(Vector3 pos){
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
    }


    private string GeneratePrefabNameString(){
        string[] letters = {"I", "J", "L", "O", "S", "T", "Z"};
        int random_num = Random.Range(0, 7);
        string result = "Prefabs/tetri_mino/Tetris_" + letters[random_num];
        return result;
    }
}