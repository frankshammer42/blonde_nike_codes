using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera mycam;
    public float stable_threshold;
    public float sensitivity;
    public float cam_move_offset; 
    private Vector3 prev_mouse_pos;
    
    
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        if (Input.GetKey(KeyCode.W)){
            Vector3 temp = new Vector3(transform.forward.x, 0, transform.forward.z);
            transform.position += temp * cam_move_offset;
        }
        if (Input.GetKey(KeyCode.S)){
            Vector3 temp = new Vector3(transform.forward.x, 0, transform.forward.z);
            transform.position += -1* temp * cam_move_offset;
        }
        if (Input.GetKey(KeyCode.A)){
            transform.position += new Vector3(0, 0, cam_move_offset);
        }
        if (Input.GetKey(KeyCode.D)){
            transform.position += new Vector3(0, 0, -cam_move_offset);
        }
    }
}
