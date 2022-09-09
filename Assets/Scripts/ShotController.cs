using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public SpaceCraft spaceCraft; 

    public GameObject rocket; // rocket template to create prefab
    [HideInInspector] public GameObject rocketClone; // instantiated rocket

    private float clickClock; // clock that counts for next click after initial click
    private float flyTime; // represents force fly time of instantiated rocked
    //private int shotCaller; // int to instantiate rocket if 2nd click occurs at the time

    private Vector3 rocketVelocity; // to determine the speed of rocket 
    //[HideInInspector] public bool isRocket; // to determine if the rocket is present or not
    private bool isButton = false; // determines if the button is active
    public CanvasGroup rocketButton; // to manipulate the button during play-time

    // try to launch the rocket inside of the spacecraft trigger off/on can be the trick
    void Start()
    {      
         
        // i am going to fix the small bugs 
        // rocket should move with some kind of animation
    }

    // double click rocket initialization works little bit problematic 
    void Update()
    {
        if (Input.touchCount > 0)
            if (isButton && spaceCraft.isRightLeft) // (shotCaller >= 2 || isButton) && spaceCraft.isRightLeft
            { // if another click occurs at the short time interval
              // Instantiate at position (0, 0, 0) and zero rotation.
                if (flyTime == 0f)
                {
                    rocketClone = Instantiate(rocket, transform.position, Quaternion.identity); // + (transform.up)
                    isButton = false;
                }

                flyTime += Time.deltaTime; // to break the equality of flytime with zero
            }

        clickClock += Time.deltaTime;

        if (flyTime > 0)
        {
            flyTime += Time.deltaTime;
            if (flyTime < 0.5f) // for more realistic acceleration and speed, adjust time and constant below
            { // add the current velocity of spacecraft and launching speed 
                rocketVelocity = new Vector3(0f, (spaceCraft.currentVelocity + 10) * (Time.deltaTime * 100), 0f); // seems fine
                rocketClone.GetComponent<Rigidbody2D>().AddForce(rocketVelocity); // (Mathf.Log(spaceCraft.currentVelocity, 1.5f)               
            }
        }

        // below two if statements is for double-click rocket launcher property
        //if (Input.touchCount > 0)
        //    if (Input.GetTouch(0).phase == TouchPhase.Began)
        //    {  
        //        shotCaller++;
        //    }      

        //if (clickClock > 0.35f) // reset clock for next possibility
        //{
        //    shotCaller = 0;
        //    clickClock = 0f;
        //}

        if (flyTime > 5f) // clock for rockets to fire them only once in a five seconds
        {
            Destroy(rocketClone); // here it destroys the clone and in collision method it destroys the rocket itself
            flyTime = 0f;
            rocketButton.alpha = 1f;
        }
    }

    public void RocketLauncher()
    {
        if (flyTime == 0f && spaceCraft.isRightLeft && !spaceCraft.isFinished)
        {
            isButton = true; // make the button available after 5 seconds
            rocketButton.alpha = 0.4f;
        }
    }

    //void RockSetter()
    //{
    //    rockinTime += Time.deltaTime; // 

    //    while (rockinTime < 0.4f) // unless timer is sooner than .4 seconds
    //    {
    //        for (int i = 0; i < rocks.Length; i++) // gives nullreference error in collisionenter
    //        {
    //            if (rockCoordinates[i] != rocks[i].transform.position.y)
    //            {
    //                if (isRocket) // destroy the rocket if there is any
    //                {
    //                    Destroy(rocketClone); // destroy if rocket changes position of any rock
    //                    isRocket = false;
    //                }

    //                Destroy(rocks[i]); // destroy the rock that is moving
    //                rocks = GameObject.FindGameObjectsWithTag("Obstacle"); // rearrange the rock array
    //            }
    //        }
    //    }

    //    rockinTime = 0f; // reset the timer
    //}

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    rockinTime += Time.deltaTime; // 

    //    while (rockinTime < 0.4f) // unless timer is sooner than .4 seconds
    //    {
    //            for (int i = 0; i < rocks.Length; i++) // gives nullreference error in collisionenter
    //            {
    //                if (rockCoordinates[i] != rocks[i].transform.position.y)
    //                {
    //                    if (isRocket) // destroy the rocket if there is any
    //                    {
    //                        Destroy(rocketClone); // destroy if rocket changes position of any rock
    //                        isRocket = false;
    //                    }

    //                    Destroy(rocks[i]); // destroy the rock that is moving
    //                    rocks = GameObject.FindGameObjectsWithTag("Obstacle"); // rearrange the rock array
    //                }
    //            }
    //    }

    //    rockinTime = 0f; // reset the timer
    //}

    void OnCollisionEnter2D(Collision2D col)
    {    
        //if (rocketClone.gameObject.tag == "Obstacle")
        //    Destroy(rocketClone);

        // if the rocket hits a rock, destroy the rocket and the object collided
        //Destroy(collision.collider.gameObject);

        //print(collision.collider.gameObject);       
    }
}
