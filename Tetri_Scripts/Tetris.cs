using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
//TODO: Probably need to check the errors, cuz we are moving by one
//TODO: Need to redo this with event 

public class Tetris : MonoBehaviour {

    public float fall_speed; //Default fall speed
    public bool allowRotation = true;
    public bool limitRotation = false;
    private int wholeNumberCounter = 0; //I'm like goddannnnnng
    private int toDivide;
    private TetrisFace Face;  
    private int z_offset;

    
    public void SetFaceReference(TetrisFace FaceRef){
        Face = FaceRef;
    }

    public void SetZoffset(int zOffset){
        z_offset = zOffset;
    } 
     

    //TODO: Maybe need to use glow material in here
    private void Start (){
        toDivide = (int)(1 / fall_speed);
    }
	
	// Update is called once per frame
    private void Update () {
        CheckUserInput();
        transform.position += new Vector3(0, -fall_speed, 0);
        if (IsValidPosition()){
            if (wholeNumberCounter % toDivide == 0){
                if (wholeNumberCounter != 0){
                    Face.UpdateGrid(this);
                }
            }
            wholeNumberCounter++;
        }
        else{
            transform.position += new Vector3(0, +fall_speed, 0);
            transform.position = new Vector3(transform.position.x, (float)Math.Round(transform.position.y, 1), z_offset);
            Face.DeleteRowsIfFull();
            enabled = false;
            Face.Generate_tetris();;
        }
    } 

    //Helper function to switch limited rotation
   private void switchRotation(){
        if (transform.rotation.eulerAngles.z >= 90)
        {
            transform.Rotate(0, 0, -90);
        }
        else
        {
            transform.Rotate(0, 0, 90);
        }
    }

   
    private void CheckUserInput(){
        if (Input.GetKeyDown(KeyCode.RightArrow)){
            transform.position += new Vector3(1, 0, 0);
            if (IsValidPosition()){
//                FindObjectOfType<TetrisFace>().UpdateGrid(this);
                Face.UpdateGrid(this);
            }
            else{
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow)){
            transform.position += new Vector3(-1, 0, 0);
            if (IsValidPosition()){
//                FindObjectOfType<TetrisFace>().UpdateGrid(this);
                Face.UpdateGrid(this);
            }
            else{
                transform.position += new Vector3(1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (allowRotation)
            {
                if (limitRotation)
                {
                    switchRotation();
                }
                else{
                    transform.Rotate(0, 0, 90);
                }
            }
            if (IsValidPosition()){
//                FindObjectOfType<TetrisFace>().UpdateGrid(this);
                Face.UpdateGrid(this);
            }
            else{
                if (limitRotation)
                {
                    switchRotation();
                }
                else{
                    transform.Rotate(0, 0, -90);
                }
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, -1, 0);
            if (IsValidPosition()){
//                FindObjectOfType<TetrisFace>().UpdateGrid(this);
                Face.UpdateGrid(this);
            }
            else{
                transform.position += new Vector3(0, +1, 0);
            }
        }
    }

    private bool IsValidPosition(){
        foreach (Transform tetrisBlock in transform){
            Vector3 rounded_pos = Face.Round(tetrisBlock.position);
            if (!Face.InGrid(rounded_pos)){
                return false;
            }
            Transform currentInGrid = Face.GetGridTransform(new Vector2(rounded_pos.x, rounded_pos.y));
            if (currentInGrid != null && currentInGrid.parent != transform){
                return false;
            }
        }
        return true;
    } 
    

}
