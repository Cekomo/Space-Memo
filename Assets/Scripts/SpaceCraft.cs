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
    [HideInInspector] public bool isLeft; // move the spacecraft left if the variable is true
    [HideInInspector] public bool isRight; // move the spacecraft right if the variable is true
    private bool isUp; // force up the spacecraft if the variable is true

    private int screenX = Screen.width; // to determine swipe action on x-axis
    private int screenY = Screen.height; // to determine swipe action on y-axis 
    private float moveClock; // determines the flight time for left/right 
    public float moveSpeed; // to determine speed constant of spacecraft
    [HideInInspector] public float currentVelocity; // to check 

    Rigidbody2D p_RigidBody; // rigidbody of the player 

    void Start()
    {
        isLeft = false; isRight = false;
        moveClock = 0f;

        p_RigidBody = GetComponent<Rigidbody2D>();
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
        else if (isUp) // be careful, it is FORCE, not just move operation
        {
            p_RigidBody.AddForce(transform.up * moveSpeed / 10);
            moveClock -= Time.deltaTime;
            if (moveClock <= 0f)
                isUp = false;

            currentVelocity = p_RigidBody.velocity.magnitude;
        }

        // can be tried on mobile and desktop mobile simulation
        if (Input.touchCount > 0) // to handle index out of bound error
            if (Input.GetTouch(0).phase == TouchPhase.Began)  startPos = Input.touches[0].position; 

        // if statements to adjust direction and flight time
        if (Input.touchCount > 0 && !isRight && !isLeft && !isUp && Input.GetTouch(0).phase == TouchPhase.Ended) // to handle index out of bound error
        { // transform.position in the if statement is to limit the movement boundary of the player
            if (Input.touches[0].position.x - startPos.x >= screenX / 1.75f && player.transform.position.x < 1.75f) // go right statement
            {
                moveClock = 1f;
                isRight = true;
            }
            else if (Input.touches[0].position.x - startPos.x >= screenX / 6 & player.transform.position.x < 1.75f) // go right statement
            {
                moveClock = 0.5f;
                isRight = true;
            }
            else if (Input.touches[0].position.x - startPos.x <= -screenX / 1.75f && player.transform.position.x > -1.75f) // go left statement
            {
                moveClock = 1f;
                isLeft = true;
            }
            else if (Input.touches[0].position.x - startPos.x <= -screenX / 6 && player.transform.position.x > -1.75f) // go left statement
            {
                moveClock = 0.5f;
                isLeft = true;
            }
            else if (Input.touches[0].position.y - startPos.y >= -screenY / 1.75f) // force up if swipe is along half of the screen
            {
                moveClock = 1f;
                isUp = true;
            }
            else if (Input.touches[0].position.y - startPos.y >= -screenY / 6) // force up if swipe is along one sixth the screen
            {
                moveClock = 0.5f;
                isUp = true;
            }
            print(player.transform.position.x);
        }
    }
}
