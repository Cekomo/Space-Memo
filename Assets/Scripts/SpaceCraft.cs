using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCraft : MonoBehaviour
{
    // move the object right and left by swiping 
    // determine a border for left/right movememnt for player to not exceed the screen limits

    public GameObject player;
    
    //private int pD = 100; // pixel distance to detect the swipe action
    private Vector2 startPos; // first touch / mouse press
    private float moveX; // slide operation on x-axis
    private float moveY; // slide operation on y-axis
    private bool isLeft; // move the spacecraft left if the variable is true
    private bool isRight; // move the spacecraft right if the variable is true

    private int screenX = Screen.width; // to determine swipe action on x-axis
    private int screenY = Screen.height; // to determine swipe action on y-axis 
    private float moveClock; // determines the flight time for left/right 
    public float moveSpeed;

    void Start()
    {
        isLeft = false; isRight = false;
        moveClock = 0f;
    }

    
    void Update()
    {
        if (isRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            moveClock -= Time.deltaTime;
            if (moveClock <= 0f)
                isRight = false;
        }
        else if (isLeft)
        {
            transform.Translate(Vector2.right * -moveSpeed * Time.deltaTime);
            moveClock -= Time.deltaTime;
            if (moveClock <= 0f)
                isLeft = false;
        }

        // can be tried on mobile and desktop mobile simulation
        if (Input.touchCount > 0) // to handle index out of bound error
            if (Input.GetTouch(0).phase == TouchPhase.Began)  startPos = Input.touches[0].position; 

        //if (Input.touchCount > 0) // if there are multiple touches on the screen
        //{ // determine their direction and magnitude
        //    moveX = Input.GetTouch(0).deltaPosition.x;
        //    //moveY = Input.GetTouch(0).deltaPosition.y;
        //}

        // if statements to adjust direction and flight time
        if (Input.touchCount > 0 && !isRight && !isLeft) // to handle index out of bound error
        {
            if (Input.touches[0].position.x - startPos.x >= screenX / 2) // go right statement
            {
                moveClock = 1f;
                isRight = true;
            }
            else if (Input.touches[0].position.x - startPos.x >= screenX / 4) // go right statement
            {
                moveClock = 0.5f;
                isRight = true;
            }
            else if (Input.touches[0].position.x - startPos.x <= -screenX / 2) // go left statement
            {
                moveClock = 1f;
                isLeft = true;
            }
            else if (Input.touches[0].position.x - startPos.x <= -screenX / 4) // go left statement
            {
                moveClock = 0.5f;
                isLeft = true;
            }
            //print(Input.touches[0].position.x - startPos.x);
        }
    }
}
