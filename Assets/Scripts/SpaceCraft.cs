using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCraft : MonoBehaviour
{
    // move the object right and left by swiping 
    // determine a border for left/right movememnt for player to not exceed the screen limits

    public GameObject player;
    public Camera mainCamera; // gameobject to represent camera
    //public Camera finishCamera; // this activates when the view needs to stop
    //private Vector3 cameraPos; // detects position of mainCamera to transform it to finishCamera
    
    //private int pD = 100; // pixel distance to detect the swipe action
    private Vector2 startPos; // first touch / mouse press
    //[HideInInspector] public bool isLeft; // move the spacecraft left if the variable is true
    //[HideInInspector] public bool isRight; // move the spacecraft right if the variable is true
    //private bool isUp; // force up the spacecraft if the variable is true
    [HideInInspector] public bool isRightLeft; // first up function must be triggered to go left/right
    private bool isRight; // to move towards rigth only during single finger touch 
    private bool isLeft; // to move towards left only during single finger touch
    private bool isUp;  // to move towards rigth only during single finger touch

    private bool isCounterMove; // to return back of the spacecraft when it exceeds borders

    private int screenX = Screen.width; // to determine swipe action on x-axis
    private int screenY = Screen.height; // to determine swipe action on y-axis 
    
    public float moveSpeed; // to determine speed constant of spacecraft
    [HideInInspector] public float currentVelocity; // to check the velocity of the spacecraft
    [HideInInspector] public float speedFading; // to make more realistic stop on x-axis

    [HideInInspector] public Rigidbody2D p_RigidBody; // rigidbody of the player 
    private SpriteRenderer sr; // sprite indicator to disable the spacecraft image

    [HideInInspector] public bool isFinished; // to determine if the game is finished
    public int maxValue; // sets upper border

    void Awake() { maxValue = 32; }

    void Start()
    {
        // if any problem occurs, check isFinished 
        isFinished = true; // the game should not start until play button is pressed when slider is at the end
        currentVelocity = 0f; // velocity of spacecraft
        
        //isLeft = false; isRight = false; 
        isRightLeft = false; isCounterMove = false;

        p_RigidBody = GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        mainCamera = gameObject.GetComponent<Camera>();
    }

    void FixedUpdate()
    { 
        // to adjust the speed of camera on y-axis
        if (!isFinished) // spacecraft is shaking when camera movement is in update() function
            Camera.main.transform.Translate(Vector2.up * currentVelocity * Time.deltaTime);
    }

    void Update()
    {
        if (Input.touchCount > 0 && !isFinished) // to handle index out of bound error
            if (Input.GetTouch(0).phase == TouchPhase.Began) 
                startPos = Input.touches[0].position;
         
        if (Input.touchCount > 0 && !isFinished)
        {
            if (Input.touches[0].position.x - startPos.x >= screenX / 5 && player.transform.position.x < 1.7f && isRightLeft && isRight)
            {
                p_RigidBody.AddForce(transform.right * moveSpeed / 2);
                //transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
                isLeft = false; isUp = false;
            }
            else if (Input.touches[0].position.x - startPos.x <= -screenX / 5 && player.transform.position.x > -1.7f && isRightLeft && isLeft)
            {
                p_RigidBody.AddForce(transform.right * -moveSpeed / 2);
                //transform.Translate(Vector2.right * -moveSpeed * Time.deltaTime);
                isRight = false; isUp = false;
            }
            else if (Input.touches[0].position.y - startPos.y >= screenY / 6 && player.transform.position.y < maxValue && isUp)
            { // force up if swipe is along one sixth the screen
                p_RigidBody.AddForce(transform.up * moveSpeed / 3);

                isRightLeft = true;
                currentVelocity = p_RigidBody.velocity.magnitude;
                isRight = false; isLeft = false;
            }
        }

        if (Input.touchCount == 0 || Mathf.Abs(player.transform.position.x) > 1.7f && !isFinished)
        {
            speedFading = p_RigidBody.velocity.x * 0.96f; // speed decreases cumulatively (multiplied with 0.96  continuously)
            p_RigidBody.velocity = new Vector2(speedFading, currentVelocity); // omitting time.deltatime solves background speed problem
            //transform.Translate(Vector2.up * currentVelocity * Time.deltaTime);
            //isRight = false; isLeft = false;
            if (p_RigidBody.velocity.x < 0.001f) // slightly more than zero since the velocity never be zero (always infinitely small greater)
                isCounterMove = true; // make the counter move available if velocity is zero out of the border
        }

        if (Input.touchCount == 0 && (!isUp || !isRight || !isLeft)) // to make them all true for the next move
        {
            isUp = true; isLeft = true; isRight = true;
        }

        if (isCounterMove && Mathf.Abs(player.transform.position.x) > 1.7f)
        { // if the spacecraft exceeds the border on x-axis, turn back inside of the border
            if (player.transform.position.x < -1.7f) // if left border exceeds
                transform.Translate(Vector2.right * 0.5f * Time.deltaTime);
            else if (player.transform.position.x > 1.7f) // if right border exceeds
                transform.Translate(Vector2.right * -0.5f * Time.deltaTime);

            if (Mathf.Abs(player.transform.position.x) < 1.7f)
                isCounterMove = false; // turning back lasts until coordinate < |1.7f|
        }
        
        if (player.transform.position.y > maxValue && !isFinished) // Y: 32 is the maximum vertical distance for map
        { 
            isFinished = true;

            // this functions were needed where finishcamera replaced the main camera
            // if spacecraft exceeds the border, switch the camera to stop the view
            //cameraPos = mainCamera.transform.position;
            //finishCamera.transform.position = cameraPos;
            //print("f: " + finishCamera.transform.position.ToString());

            //mainCamera.enabled = false;
            //finishCamera.enabled = true;
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
}
