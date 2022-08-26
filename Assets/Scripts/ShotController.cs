using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public SpaceCraft spaceCraft; 

    public GameObject rocket; // rocket template to create prefab
    private GameObject rocketClone; // instantiated rocket

    private float clickClock; // clock that counts for next click after initial click
    private float flyTime; // represents force fly time of instantiated rocked
    private int shotCaller; // int to instantiate rocket if 2nd click occurs at the time
    private bool isFired; // to check if rocket is fired in the interval

    private Collider2D rocketCollider; // to adjust isTrigger of the object
    private Vector3 rocketVelocity; // to determine the speed of rocket 

    private GameObject[] rocks; // rocks in the map
    private float[] rockCoordinates; // rock coordinates for x-axis

    // try to launch the rocket inside of the spacecraft trigger off/on can be the trick
    void Start()
    {
        rocks = GameObject.FindGameObjectsWithTag("Obstacle");
        rockCoordinates = new float[rocks.Length]; // set the length of the array

        for (int i = 0; i < rocks.Length; i++)
            rockCoordinates[i] = 0f;

        for (int i = 0; i < rocks.Length; i++) // to assign initial coordinates of y-axis
            rockCoordinates[i] = rocks[i].transform.position.y;
    }

    // double click rocket initialization works little bit problematic 
    void Update()
    {
        if (Input.touchCount > 0)
            if (shotCaller >= 2 && !isFired && spaceCraft.isRightLeft)
            { // if another click occurs at the short time interval
              // Instantiate at position (0, 0, 0) and zero rotation.
                if (flyTime == 0f)
                {
                    rocketClone = Instantiate(rocket, transform.position, Quaternion.identity); // + (transform.up)
                    rocketCollider = rocketClone.GetComponent<Collider2D>();
                    rocketCollider.isTrigger = true; // for rocket to not stuck inside of spacecraft
                }

                isFired = true;
                flyTime += Time.deltaTime;
            }

        clickClock += Time.deltaTime;

        if (flyTime > 0)
        {
            flyTime += Time.deltaTime;
            if (flyTime < 0.5f) // for more realistic acceleration and speed, adjust time and constant below
            { // add the current velocity of spacecraft and launching speed 
                rocketVelocity = new Vector3(0f, (spaceCraft.currentVelocity + 10) * (Time.deltaTime*100), -0.5f); // seems fine
                rocketClone.GetComponent<Rigidbody2D>().AddForce(rocketVelocity); // (Mathf.Log(spaceCraft.currentVelocity, 1.5f)
            }
            print(transform.up);
            if (flyTime > 0.5f && rocketCollider != null)
                rocketCollider.isTrigger = false; // trigger off when rocket leaves the spacecraft entirely
        }

        if (Input.touchCount > 0)
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {  
                shotCaller++;
            }      

        if (clickClock > 0.35f) // reset clock for next possibility
        {
            shotCaller = 0;
            clickClock = 0f;
            isFired = false;
        }

        if (flyTime > 5f) // clock for rockets to fire them only once in a five seconds
            flyTime = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Destroy(GameObject.FindWithTag("Obstacle"));
        }

        for (int i = 0; i < rocks.Length; i++) // gives nulreference error
        {
            print(rocks[i]);
            if (rockCoordinates[i] != rocks[i].transform.position.y)
                Destroy(rocks[i]);
        }
    }
}
