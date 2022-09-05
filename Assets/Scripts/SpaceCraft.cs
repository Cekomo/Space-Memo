using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCraft : MonoBehaviour
{
    public MovementController movementController; 
    
    // move the object right and left by swiping 
    // determine a border for left/right movememnt for player to not exceed the screen limits

    public GameObject player;
    private GameObject thrusterLeft; // it represents left thruster flame
    private GameObject thrusterMid; // it represents middle thruster flame
    private GameObject thrusterRight; // it represents right thruster flame
    
    private GameObject blastOff; // it represents the blast after spacecraft collision
    private float clock; // timer to deactivate explosion effect

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
    private bool toGo; // allows the spacecraft movement along x/y-axis 

    private int screenX = Screen.width; // to determine swipe action on x-axis
    private int screenY = Screen.height; // to determine swipe action on y-axis 
    
    public float moveSpeed; // to determine speed constant of spacecraft
    [HideInInspector] public float currentVelocity; // to check the velocity of the spacecraft
    [HideInInspector] public float speedFading; // to make more realistic stop on x-axis

    [HideInInspector] public Rigidbody2D p_RigidBody; // rigidbody of the player 
    private SpriteRenderer sr; // sprite indicator to disable the spacecraft image

    [HideInInspector] public bool isFinished; // to determine if the game is finished
    //private bool isThruster; // determine if the thruster launches
    public int maxValue; // sets upper border
    
    private int j; // to adjust the index of movement control arrays
    private int k; // to adjust the index of idle time arrays
    //private float holdDownClock; // to count holding down time for each element of holdDownTime
    //private float idleClock; // to count the idle time of the system in terms of touch input
    private bool idleLoadTransition; // boolean to swap between holddown and idle operations

    void Awake() { maxValue = 32; }


    // spacecraft initial condidition is y: -3.2f
    // references brokes the system but the principle works as a general concept
    // camera does not stop after the game finishes
    void Start()
    {
        idleLoadTransition = false; // false means the system work on load
        toGo = true;
        //isThruster = false; 
        
        j = 0; k = 0;
        //holdDownClock = movementController.holdDownTime[j];
        //idleClock = movementController.idleTime[j];

        // if any problem occurs, check isFinished 
        isFinished = true; // the game should not start until play button is pressed when slider is at the end
        currentVelocity = 0f; // velocity of spacecraft
        
        //isLeft = false; isRight = false; 
        isRightLeft = false; isCounterMove = false;

        p_RigidBody = GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        mainCamera = gameObject.GetComponent<Camera>();

        thrusterLeft = player.transform.GetChild(0).gameObject; // reference to represent the thruster flame left
        thrusterMid = player.transform.GetChild(1).gameObject; // reference to represent the thruster flame mid
        thrusterRight = player.transform.GetChild(2).gameObject; // reference to represent the thruster flame right
        blastOff = player.transform.GetChild(3).gameObject;
    }

    void FixedUpdate()
    { 
        // to adjust the speed of camera on y-axis
        if (!isFinished && player.transform.position.y < maxValue) // spacecraft is shaking when camera movement is in update() function
            Camera.main.transform.Translate(Vector2.up * currentVelocity * Time.deltaTime);   
    }

    // it would be nice if border exceed is expressed with a reference
    void Update()
    {
        //if (Input.touchCount > 0 && !isFinished) // to handle index out of bound error
        //    if (Input.GetTouch(0).phase == TouchPhase.Began) 
        //        startPos = Input.touches[0].position;

        //if (!idleLoadTransition && !isFinished) print("Going " + movementController.movementCatcher[j] + " for " + movementController.holdDownTime[j+1].ToString() + " seconds");
        //else if (idleLoadTransition && !isFinished) print("Being idle for " + idleClock.ToString() + " seconds.");

        //if (!isFinished)
        //    for (int i = 0; i < movementController.holdDownTime.Length; i++)
        //    {
        //        print(movementController.holdDownTime[i]);
        //    }
        //if (!isFinished)
        //{
        //    print("time "+movementController.holdDownTime[j + 1].ToString());
        //    print("movementcatcher "+ (j + 1).ToString() + " " + movementController.movementCatcher[j + 1].ToString());
        //}

        // i will adjust the active position of thruster flames
        if (!isFinished || player.transform.position.y > maxValue)
        { // activate if there is a force
            thrusterLeft.SetActive(true);
            thrusterMid.SetActive(true);
            thrusterRight.SetActive(true);
        }
        else
        { // deactivate if there is no force
            thrusterLeft.SetActive(false);
            thrusterMid.SetActive(false);
            thrusterRight.SetActive(false);
        }

        //if (!isFinished) // lock the thruster if isFinished is true
        //    isThruster = true;
        //else 
        //    isThruster = false;

        if (movementController.holdDownTime[j+1] > 0.01f && !isFinished && Mathf.Abs(player.transform.position.x) < 1.7f)
        {
            //if (Input.touches[0].position.x - startPos.x >= screenX / 5 && player.transform.position.x < 1.7f && isRightLeft && isRight)
            if (movementController.movementCatcher[j+1] == 1 && movementController.holdDownTime[j + 1] > 0 && !idleLoadTransition)
            { // maintain until holding time is greater than zero 
                if (toGo) p_RigidBody.AddForce(transform.right * moveSpeed / 2);
                //transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
                isLeft = false; isUp = false;

                movementController.holdDownTime[j+1] -= Time.deltaTime; // decrase the timer for each force action
                //print("Going rigth for " + movementController.holdDownTime[j+1].ToString() + " seconds");

            }
            else if (movementController.movementCatcher[j+1] == 2 && movementController.holdDownTime[j + 1] > 0 && !idleLoadTransition)
            {
                if (toGo) p_RigidBody.AddForce(transform.right * -moveSpeed / 2);
                //transform.Translate(Vector2.right * -moveSpeed * Time.deltaTime);
                isRight = false; isUp = false;

                movementController.holdDownTime[j+1] -= Time.deltaTime; // decrase the timer for each force action
                //print("Going left for " + movementController.holdDownTime[j+1].ToString() + " seconds");
            }
            //else if (Input.touches[0].position.y - startPos.y >= screenY / 6 && player.transform.position.y < maxValue && isUp)
            else if (movementController.movementCatcher[j+1] == 3 && movementController.holdDownTime[j + 1] > 0 && !idleLoadTransition)
            { // force up if swipe is along one sixth the screen
                p_RigidBody.AddForce(transform.up * moveSpeed / 3);

                isRightLeft = true;
                currentVelocity = p_RigidBody.velocity.magnitude;
                isRight = false; isLeft = false;

                movementController.holdDownTime[j+1] -= Time.deltaTime; // decrase the timer for each force action
                //print("Going up for " + movementController.holdDownTime[j+1].ToString() + " seconds");
            }
        }

        if (player.transform.position.y > maxValue && Mathf.Abs(player.transform.position.x) > 1.7f) // transfer it to below function if possible
        {
            toGo = false; // spacecraft should not move if it exceeds horizontal borders
            //movementController.holdDownTime[j] = 0f; // this can not be implemented since it brokes balance of timing
        }

        if (movementController.holdDownTime[j+1] < 0.05f || Mathf.Abs(player.transform.position.x) > 1.7f && !isFinished) // discard !isFinished by controllin
        {
            speedFading = p_RigidBody.velocity.x * 0.96f; // speed decreases cumulatively (multiplied with 0.96  continuously)
            p_RigidBody.velocity = new Vector2(speedFading, currentVelocity); // omitting time.deltatime solves background speed problem
            //transform.Translate(Vector2.up * currentVelocity * Time.deltaTime);
            //isRight = false; isLeft = false;
            if (p_RigidBody.velocity.x < 0.001f) // slightly more than zero since the velocity never be zero (always infinitely small greater)
                isCounterMove = true; // make the counter move available if velocity is zero out of the border
        }

        if (movementController.holdDownTime[j+1] < 0.01f && (!isUp || !isRight || !isLeft) && !isFinished) // to make them all true for the next move
        {
            isUp = true; isLeft = true; isRight = true;
            j++;
            //movementController.holdDownTime[j+1] = movementController.holdDownTime[j+1];
            // movementController.movementCatcher[j] = movementType; // this can be implemented to have less reference from another class

            toGo = true;
            idleLoadTransition = true;
        }

        if (idleLoadTransition && !isFinished) // wait until idle time in queue is over
            movementController.idleTime[k+1] -= Time.deltaTime;

        if (movementController.idleTime[k+1] < 0.01f && !isFinished) // hand queue over to holding operation
        {
            idleLoadTransition = false;

            k++;
            //idleClock = movementController.idleTime[k]; // check if indexing works
            
        }

        if (!isFinished && Mathf.Abs(player.transform.position.x) > 1.7f) // Mathf.Abs(player.transform.position.x) > 1.7f
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


        if (isFinished) // GetComponent2D<Sprite>()
        {
            clock += Time.deltaTime;
            if (clock > 1.8f)
            {
                blastOff.SetActive(false);
                clock = 0f;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    { // when collision happens, the camera sometimes go out the borders of the background
        if (collision.gameObject.tag == "Obstacle")
        { // if the player hit an object, game over
            isFinished = true;
            sr.enabled = false;
            p_RigidBody.bodyType = RigidbodyType2D.Static; // to stop the object and the camera to stop the view
            blastOff.SetActive(true); // set blast variable true to animate the explosion
        }
    }
}
