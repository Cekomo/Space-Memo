using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// in record session ship goes less in x/y-axis

public class SpaceCraft : MonoBehaviour
{
    public MovementController movementController; 
    
    // move the object right and left by swiping 
    // determine a border for left/right movememnt for player to not exceed the screen limits

    public GameObject player;
    private GameObject thrusterLeft; // it represents left thruster flame
    private GameObject thrusterMid; // it represents middle thruster flame
    private GameObject thrusterRight; // it represents right thruster flame
    [HideInInspector] public GameObject[] obstacles; // represents all obstacles in the map
    
    private GameObject blastOff; // it represents the blast after spacecraft collision
    [HideInInspector] public float clock; // timer to deactivate explosion effect
    private float waitClock; // timer to wait the ship until it comes its position

    public Camera mainCamera; // gameobject to represent camera
    
    //private int pD = 100; // pixel distance to detect the swipe action
    private Vector2 startPos; // first touch / mouse press
    
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
    [HideInInspector] public bool preStart; // represents the time after play button is activated
    [HideInInspector] public bool isRecording; // to record the actions for spacecraft
    //private bool isThruster; // determine if the thruster launches
    public int maxValue; // sets upper border
    
    private int j; // to adjust the index of movement control arrays
    private int k; // to adjust the index of idle time arrays
    //private float holdDownClock; // to count holding down time for each element of holdDownTime
    //private float idleClock; // to count the idle time of the system in terms of touch input
    private bool idleLoadTransition; // boolean to swap between holddown and idle operations
    
    private float timer; // to count time to destroy the spacecraft when collision happens
    
    void Awake() { maxValue = 32; }

    // spacecraft initial condidition is y: -3.2f
    // references brokes the system but the principle works as a general concept
    void Start()
    {
        isRecording = false; // initially false (until start button is triggered)

        idleLoadTransition = false; // false means the system work on load
        toGo = true;
        preStart = false;
        //isThruster = false;  

        timer = 0;
        j = 0; k = 0;
        //holdDownClock = movementController.holdDownTime[j];
        //idleClock = movementController.idleTime[j];

        // if any problem occurs, check isFinished 
        isFinished = true; // the game should not start until play button is pressed when slider is at the end
        currentVelocity = 0f; // velocity of spacecraft
        
        //isLeft = false; isRight = false; 
        isRightLeft = false; 
        isCounterMove = false; // it blocks the horizontal movement ability when the borders (x-axis) are excededed

        // determine the obstacles
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        p_RigidBody = GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        mainCamera = gameObject.GetComponent<Camera>();

        thrusterLeft = player.transform.GetChild(0).gameObject; // reference to represent the thruster flame left
        thrusterMid = player.transform.GetChild(1).gameObject; // reference to represent the thruster flame mid
        thrusterRight = player.transform.GetChild(2).gameObject; // reference to represent the thruster flame right
        blastOff = player.transform.GetChild(3).gameObject;
    }

    void FixedUpdate() // spacecraft is shaking when camera movement is in update() function
    { 
        // statement the follow the player in action
        if (player.transform.position.y < maxValue) 
            Camera.main.transform.Translate(Vector2.up * p_RigidBody.velocity.y * Time.deltaTime);   
    }
    // problem occurs if the vertical speed is too low 
    // check speedfading variable to syncronize the slowing speed of both recording and playin session
    // it would be nice if border exceed is expressed with a reference
    void Update()
    {
        //print("currentvelocity: " + currentVelocity.ToString() + " velocity.y: " + p_RigidBody.velocity.y.ToString()); // !!!when forcing up merges with border exceed, vertical speed is boosted!!!
        
        //if (Input.touchCount > 0 && !isFinished) // to handle index out of bound error
        //    if (Input.GetTouch(0).phase == TouchPhase.Began) 
        //        startPos = Input.touches[0].position;

        //if (!idleLoadTransition && !isFinished) print("Going " + movementController.movementCatcher[j+1] + " for " + movementController.holdDownTime[j+1].ToString() + " seconds");
        //else if (idleLoadTransition && !isFinished) print("Being idle for " + movementController.holdDownTime[j+1].ToString() + " seconds.");

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
        
        // ---------------- part after recording finishes ----------------
        if (currentVelocity > 0f) // flames are active if the velocity is greater than 0 (ship is moving)
        {
            //thrusterLeft.SetActive(true);
            thrusterMid.SetActive(true);
            //thrusterRight.SetActive(true);
        }
        else if (currentVelocity == 0f && player.transform.position.y >= -3.25f)
        {
            thrusterLeft.SetActive(false);
            thrusterMid.SetActive(false);
            thrusterRight.SetActive(false);
        }

        if (player.transform.position.y < -3.5f && preStart) 
        {
            transform.Translate(Vector2.up * clock * Time.deltaTime);
            //thrusterLeft.SetActive(true);
            thrusterMid.SetActive(true);
            //thrusterRight.SetActive(true);
        }
        else if (currentVelocity == 0f && clock > 0.01f && preStart)// stop the flame if it reaches -3.2 for beginning
        {
            //print("zort");
            clock = clock * 0.97f;
            transform.Translate(Vector2.up * clock * Time.deltaTime); // if the border is exceeded, decrease the speed
            thrusterLeft.SetActive(false);
            thrusterMid.SetActive(false);
            thrusterRight.SetActive(false);
        }
        //print("isright: " + isRight.ToString() + " " + "isleft: " + isLeft.ToString());
        if (isRight && !isLeft)
            thrusterLeft.SetActive(true);
        else if (isLeft && !isRight)
            thrusterRight.SetActive(true);
        
        if ((!isRight || !isLeft) && Input.touchCount == 0 && isRecording)
        {
            // deactivate the thrusters when there is no horizontal movement
            thrusterLeft.SetActive(false);
            thrusterRight.SetActive(false);
        }
        else if ((!isLeft || !isRight) && movementController.holdDownTime[j + 1] < 0.05f && !isRecording)
        {
            thrusterLeft.SetActive(false);
            thrusterRight.SetActive(false);
        }

        // adjust clock properly
        if (clock < 0.01f) // to block the system to go inside statements above unnecessarily
            preStart = false;
        // ----------------------------------------------------------------

        // ---------------- part functioning while isRecording is true ----------------
        if (isRecording)
        {
            // isrigthleft does not work properly

            if (Input.touchCount > 0 && !preStart) 
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                    startPos = Input.touches[0].position;

                if (Input.touches[0].position.x - startPos.x >= screenX / 5 && isRightLeft && isRight && !isCounterMove)
                {
                    p_RigidBody.AddForce(transform.right * moveSpeed / 2);
                    isUp = false; isLeft = false;
                    //timer += Time.deltaTime;
                }
                else if (Input.touches[0].position.x - startPos.x <= -screenX / 5 && isRightLeft && isLeft && !isCounterMove)
                {
                    p_RigidBody.AddForce(transform.right * -moveSpeed / 2);
                    isUp = false; isRight = false;
                    //timer += Time.deltaTime;
                    
                }
                else if (Input.touches[0].position.y - startPos.y >= screenY / 6 && isUp)
                {
                    currentVelocity = p_RigidBody.velocity.magnitude;
                    p_RigidBody.AddForce(transform.up * moveSpeed / 3);
                    isLeft = false; isRight = false;
                    if (currentVelocity > 0) isRightLeft = true;
                    //timer += Time.deltaTime;
                    
                }
            }

            if ((!isUp || !isRight || !isLeft) && Input.touchCount == 0)
            {
                //if (isRight)
                //    print("Going 1 for " + timer.ToString() + " seconds");
                //else if (isLeft)
                //    print("Going 2 for " + timer.ToString() + " seconds");
                //else if (isUp)
                //    print("Going 3 for " + timer.ToString() + " seconds");

                isUp = true; isLeft = true; isRight = true;
                timer = 0;
            }

            if (Mathf.Abs(player.transform.position.x) > 1.7f || Input.touchCount == 0) // discard !isFinished by controllin
            {
                speedFading = p_RigidBody.velocity.x * 0.94f;
                p_RigidBody.velocity = new Vector2(0, currentVelocity); // y-variable was currentVelocity

                //if (p_RigidBody.velocity.x < 0.001f) // slightly more than zero since the velocity never be zero (always infinitely small greater)
                //    isCounterMove = true; // make the counter move available if velocity is zero out of the border
                if (speedFading < 0.03f) speedFading = 0;
            }

            if (Mathf.Abs(player.transform.position.x) > 1.7f) // if the spacecraft exceeds the border on x-axis, turn back inside of the border
            {
                isCounterMove = true; // to prevent forcing to exceed borders while pressin down to move left or right
                
                if (player.transform.position.x < -1.7f) // if left border exceeds
                    transform.Translate(Vector2.right * 0.5f * Time.deltaTime);
                else if (player.transform.position.x > 1.7f) // if right border exceeds
                    transform.Translate(Vector2.right * -0.5f * Time.deltaTime);

                if (Mathf.Abs(player.transform.position.x) < 1.7f)
                    isCounterMove = false; // turning back lasts until coordinate < |1.7f|
            }
        }
        // --------------------------------------------------------------------------

        if (movementController.holdDownTime[j+1] > 0.01f && !isFinished && Mathf.Abs(player.transform.position.x) < 1.7f && !isRecording && !preStart)
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

        if ((movementController.holdDownTime[j + 1] < 0.05f || Mathf.Abs(player.transform.position.x) > 1.7f) && !isFinished && !isRecording) // discard !isFinished by controllin
        {
            speedFading = p_RigidBody.velocity.x * 0.94f; // speed decreases cumulatively (multiplied with 0.96  continuously)
            p_RigidBody.velocity = new Vector2(0, p_RigidBody.velocity.y); // omitting time.deltatime solves background speed problem                                                                             

            if (speedFading < 0.03f) speedFading = 0;

            //if (p_RigidBody.velocity.x < 0.001f) // slightly more than zero since the velocity never be zero (always infinitely small greater)
            //isCounterMove = true; // make the counter move available if velocity is zero out of the border
        }

        if (movementController.holdDownTime[j+1] < 0.01f && (!isUp || !isRight || !isLeft) && !isFinished && !isRecording) // to make them all true for the next move
        {
            isUp = true; isLeft = true; isRight = true;
            j++;
            //movementController.holdDownTime[j+1] = movementController.holdDownTime[j+1];
            // movementController.movementCatcher[j] = movementType; // this can be implemented to have less reference from another class

            toGo = true;
            idleLoadTransition = true;
        }

        try
        {
            if (idleLoadTransition && !isFinished) // wait until idle time in queue is over
                movementController.idleTime[k + 1] -= Time.deltaTime;
        }
        catch { }

        try // this function gives error when idle time finishes
        {
            if (movementController.idleTime[k + 1] < 0.01f && !isFinished && !isRecording) // hand queue over to holding operation
            {
                idleLoadTransition = false;
                k++;
                //idleClock = movementController.idleTime[k]; // check if indexing works
            }
        }
        catch { }

        if (!isFinished && !isRecording && Mathf.Abs(player.transform.position.x) > 1.7f) // Mathf.Abs(player.transform.position.x) > 1.7f
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
            isRecording = false; // when map is finished for the first time, complete recording period

            // this functions were needed where finishcamera replaced the main camera
            // if spacecraft exceeds the border, switch the camera to stop the view
            //cameraPos = mainCamera.transform.position;
            //finishCamera.transform.position = cameraPos;
            //print("f: " + finishCamera.transform.position.ToString());

            //mainCamera.enabled = false;
            //finishCamera.enabled = true;
        }

        if (player.transform.position.y > maxValue) // reset horizontal speed when the maxvalue is exceeded
            p_RigidBody.velocity = new Vector2(0f, p_RigidBody.velocity.y);

        if (timer > 0)
            timer += Time.deltaTime;

        if (timer > 1)
        {
            Destroy(player); // destroy the spacecraft when animation ends
            timer = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    { // when collision happens, the camera sometimes go out the borders of the background
        if (collision.gameObject.tag == "Obstacle")
        { // if the player hit an object, game over
            isFinished = true;
            sr.enabled = false;

            timer += Time.deltaTime;

            p_RigidBody.bodyType = RigidbodyType2D.Static; // to stop the object and the camera to stop the view
            blastOff.SetActive(true); // set blast variable true to animate the explosion
            currentVelocity = 0; // velocity resets if the ships collides
        }
    }
}
