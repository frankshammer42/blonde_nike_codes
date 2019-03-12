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
    private Transform[,] FaceGrid;
    private int _LowerLeftxOffset; //For Boundary Check
    private int _lowerRightxOffset;
    private int x_offset; //Center x
    private int z_offset; //Center z 

    private void Start(){
        _LowerLeftxOffset = -gridWidth / 2;
        _lowerRightxOffset = _LowerLeftxOffset + gridWidth;
        FaceGrid = new Transform[gridWidth, gridHeight];
        x_offset = (int) transform.position.x;
        z_offset = (int) transform.position.z;
        Generate_tetris();
    }

    private void Update(){
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

        foreach (Transform cubeTransform in currentBlock.transform){
            Vector3 pos = Round(cubeTransform.position);
            if (pos.y < gridHeight){
                int xInGridSystem = (int)Mathf.Round(pos.x - _LowerLeftxOffset - x_offset);
                int yInGridSystem = (int) Mathf.Round(pos.y);
                FaceGrid[xInGridSystem, yInGridSystem] = cubeTransform;
            }
        }
    }


    public Transform GetGridTransform(Vector2 gridPos){
        if (gridPos.y > gridHeight-1){
            return null;
        }
        int xInGridSystem = (int)Mathf.Round(gridPos.x - _LowerLeftxOffset - x_offset); 
        return FaceGrid[xInGridSystem, (int)Mathf.Round(gridPos.y)];
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


    //Only Call this method when the row is full
    public void DeleteCubesAtRow(int row_index){
        for (int i = 1; i < gridWidth-1; i++){
            Destroy(FaceGrid[i, row_index].gameObject);
            FaceGrid[i, row_index] = null;
        }
    }


    public void MoveRowDown(int row_index){
        for (int i = 1; i < gridWidth; i++){
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
                Debug.Log("Can we perform the check?");
                DeleteCubesAtRow(i);
                MoveAllRowsAboveDown(i+1);
                i--;
            }
        }
    }
    
    
    public void Generate_tetris(){
        string next_tetris_type = GeneratePrefabNameString();
        int random_x = Random.Range(-2,2);
        GameObject new_tetris_block = (GameObject) Instantiate(Resources.Load(next_tetris_type, typeof(GameObject)),
            new Vector3(random_x + x_offset, gridHeight+10, z_offset), Quaternion.identity);
        //Set Which Face Controls which
        new_tetris_block.GetComponent<Tetris>().SetFaceReference(this);
        new_tetris_block.GetComponent<Tetris>().SetZoffset(z_offset);
    }


    public bool InGrid(Vector3 pos){
        //TODO: Based on lower left side position, adjust the checking method
        return pos.x >= _LowerLeftxOffset + x_offset + 0.5 && pos.x <= _lowerRightxOffset + x_offset- 0.5 && pos.y >= 0.5;
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