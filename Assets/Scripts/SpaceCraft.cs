using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCraft : MonoBehaviour
{
    // move the object right and left by swiping 
    // determine a border for left/right movememnt for player to not exceed the screen limits

    public GameObject player;
    public Camera mainCamera; // gameobject to represent camera
    public Camera finishCamera; // this activates when the view needs to stop
    private Vector3 cameraPos; // detects position of mainCamera to transform it to finishCamera
    
    //private int pD = 100; // pixel distance to detect the swipe action
    private Vector2 startPos; // first touch / mouse press
    [HideInInspector] public bool isLeft; // move the spacecraft left if the variable is true
    [HideInInspector] public bool isRight; // move the spacecraft right if the variable is true
    //private bool isUp; // force up the spacecraft if the variable is true
    private bool isRightLeft; // first up function must be triggered to go left/right

    private int screenX = Screen.width; // to determine swipe action on x-axis
    private int screenY = Screen.height; // to determine swipe action on y-axis 
    //private float moveClock; // determines the flight time for left/right 
    public float moveSpeed; // to determine speed constant of spacecraft
    [HideInInspector] public float currentVelocity; // to check 

    Rigidbody2D p_RigidBody; // rigidbody of the player 
    SpriteRenderer sr; // sprite indicator to disable the spacecraft image

    [HideInInspector] public bool isFinished; // to determine if the game is finished

    void Start()
    {
        isLeft = false; isRight = false; isRightLeft = false;
        //moveClock = 0f;

        p_RigidBody = GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        if (Input.touchCount > 0 && !isFinished) // to handle index out of bound error
            if (Input.GetTouch(0).phase == TouchPhase.Began) 
                startPos = Input.touches[0].position;
         
        if (Input.touchCount > 0 && !isFinished)
        {
            if (Input.touches[0].position.x - startPos.x >= screenX / 3 && player.transform.position.x < 1.75f && isRightLeft)
            {
                transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
                isRight = true;
            }
            else if (Input.touches[0].position.x - startPos.x <= -screenX / 3 && player.transform.position.x > -1.75f && isRightLeft)
            {
                transform.Translate(Vector2.right * -moveSpeed * Time.deltaTime);
                isLeft = true;
            }
            else if (Input.touches[0].position.y - startPos.y >= screenY / 4 && player.transform.position.y < 32f) // force up if swipe is along one sixth the screen
            {
                p_RigidBody.AddForce(transform.up * moveSpeed / 10);

                isRightLeft = true;
                currentVelocity = p_RigidBody.velocity.magnitude;
            }
        }

        if (Input.touchCount == 0 || Mathf.Abs(player.transform.position.x) > 1.75f)
        {
            isRight = false; isLeft = false;
        }

        if (player.transform.position.y > 32 && !isFinished) // Y: 32 is the maximum vertical distance for map
        { // if spacecraft exceeds the border, switch the camera to stop the view

            cameraPos = mainCamera.transform.position;
            finishCamera.transform.position = cameraPos;
            //print("f: " + finishCamera.transform.position.ToString());
            
            mainCamera.enabled = false;
            finishCamera.enabled = true;
            isFinished = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    { // when collision happens, the camera sometimes go out the borders of the background
        if (collision.gameObject.tag == "Obstacle")
        { // if the player hit an object, game over
            isFinished = true;
            sr.enabled = false;
            p_RigidBody.bodyType = RigidbodyType2D.Static; // to stop the object and the camera to stop the view
        }
    }

    //void Update()
    //{
    //    if (isRight)
    //    {
    //        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    //        moveClock -= Time.deltaTime;
    //        if (moveClock <= 0f)
    //            isRight = false;
    //    }
    //    else if (isLeft)
    //    {
    //        transform.Translate(Vector2.right * -moveSpeed * Time.deltaTime);
    //        moveClock -= Time.deltaTime;
    //        if (moveClock <= 0f)
    //            isLeft = false;
    //    }
    //    else if (isUp) // be careful, it is FORCE, not just move operation
    //    {
    //        p_RigidBody.AddForce(transform.up * moveSpeed / 10);
    //        moveClock -= Time.deltaTime;
    //        if (moveClock <= 0f)
    //            isUp = false;

    //        isRightLeft = true;
    //        currentVelocity = p_RigidBody.velocity.magnitude;
    //    }

    //    // can be tried on mobile and desktop mobile simulation
    //    if (Input.touchCount > 0 && !isFinished) // to handle index out of bound error
    //        if (Input.GetTouch(0).phase == TouchPhase.Began) startPos = Input.touches[0].position;

    //    // if statements to adjust direction and flight time
    //    if (Input.touchCount > 0 && !isRight && !isLeft && !isUp && Input.GetTouch(0).phase == TouchPhase.Ended && !isFinished)
    //    { // transform.position in the if statement is to limit the movement boundary of the player
    //        if (Input.touches[0].position.x - startPos.x >= screenX / 1.75f && player.transform.position.x < 1.75f && isRightLeft) // go right statement
    //        {
    //            moveClock = 1f;
    //            isRight = true;
    //        }
    //        else if (Input.touches[0].position.x - startPos.x >= screenX / 6 & player.transform.position.x < 1.75f && isRightLeft) // go right statement
    //        {
    //            moveClock = 0.5f;
    //            isRight = true;
    //        }
    //        else if (Input.touches[0].position.x - startPos.x <= -screenX / 1.75f && player.transform.position.x > -1.75f && isRightLeft) // go left statement
    //        {
    //            moveClock = 1f;
    //            isLeft = true;
    //        }
    //        else if (Input.touches[0].position.x - startPos.x <= -screenX / 6 && player.transform.position.x > -1.75f && isRightLeft) // go left statement
    //        {
    //            moveClock = 0.5f;
    //            isLeft = true;
    //        }
    //        else if (Input.touches[0].position.y - startPos.y >= -screenY / 1.75f && player.transform.position.y < 32f) // force up if swipe is along half of the screen
    //        {
    //            moveClock = 1f;
    //            isUp = true;
    //        }
    //        else if (Input.touches[0].position.y - startPos.y >= -screenY / 6 && player.transform.position.y < 32f) // force up if swipe is along one sixth the screen
    //        {
    //            moveClock = 0.5f;
    //            isUp = true;
    //        }
    //    }

    //    if (player.transform.position.y > 32 && !isFinished) // Y: 32 is the maximum vertical distance for map
    //    { // if spacecraft exceeds the border, switch the camera to stop the view

    //        cameraPos = mainCamera.transform.position;
    //        finishCamera.transform.position = cameraPos;
    //        //print("f: " + finishCamera.transform.position.ToString());

    //        mainCamera.enabled = false;
    //        finishCamera.enabled = true;
    //        isFinished = true;
    //    }
    //}

    //void OnCollisionEnter2D(Collision2D collision)
    //{ // when collision happens, the camera sometimes go out the borders of the background
    //    if (collision.gameObject.tag == "Obstacle")
    //    { // if the player hit an object, game over
    //        isFinished = true;
    //        sr.enabled = false;
    //        p_RigidBody.bodyType = RigidbodyType2D.Static; // to stop the object and the camera to stop the view
    //    }
    //}
}
