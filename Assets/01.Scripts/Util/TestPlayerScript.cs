using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TestPlayerScript : MonoBehaviour
{
    private void Start()
    {

    }
    Vector2 moveX;
    private float moveSpeed = 3.0f; 

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {

        }
    } 
}
