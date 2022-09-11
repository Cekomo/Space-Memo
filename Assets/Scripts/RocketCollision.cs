using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCollision : MonoBehaviour
{
    // script to control collision of prefab rockets
    [SerializeField] private GameObject rocketClon;
    private GameObject blastOff;
    private GameObject flame; // represents first child of the missile
    private SpriteRenderer sr; // sprite indicator to disable the missile image
    private Rigidbody2D m_Rigidbody; // to freeze the position of missile

    private float timer; // it waits for explosion animation to occur to destroy 

    void Update()
    {
        if (timer > 0) // timer 
            timer += Time.deltaTime;

        if (timer > 1f) // 1 second is suitable for that explosion animation setup
        {
            Destroy(rocketClon);
            timer = 0; // reset it for the next rocket actions
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        blastOff = rocketClon.transform.GetChild(1).gameObject;
        flame = rocketClon.transform.GetChild(0).gameObject;
        sr = gameObject.GetComponent<SpriteRenderer>();
        m_Rigidbody = GetComponent<Rigidbody2D>(); // sometimes explosion effect moves

        timer += Time.deltaTime;

        // if the rocket hits a rock, destroy the rocket 
        //if (col.gameObject.tag == "Obstacle") // adjust it just as rocket
        m_Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        blastOff.SetActive(true);
        sr.enabled = false; 
        Destroy(flame);

        if (col.gameObject.tag == "B_Obstacle")
            Destroy(col.gameObject);
        
    }
}
