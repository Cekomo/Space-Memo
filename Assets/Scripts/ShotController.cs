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


    void Start()
    {

    }

    // double click rocket initialization works little bit problematic 
    void Update()
    {

        if (Input.touchCount > 0)
            if (shotCaller >= 2 && !isFired && spaceCraft.isRightLeft)
            { // if another click occurs at the short time interval
              // Instantiate at position (0, 0, 0) and zero rotation.
                if (flyTime == 0f)
                    rocketClone = Instantiate(rocket, transform.position + (transform.up), Quaternion.identity);

                isFired = true;
                flyTime += Time.deltaTime;
            }

        clickClock += Time.deltaTime;

        if (flyTime > 0)
        {
            flyTime += Time.deltaTime;
            if (flyTime < 0.5f) // for more realistic acceleration and speed, adjust time and constant below
                rocketClone.GetComponent<Rigidbody2D>().AddForce(transform.up * 3); // add the current velocity of spacecraft
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
            Destroy(rocket);
        }
    }
}
