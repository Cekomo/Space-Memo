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

    void OnCollisionEnter2D(Collision2D col)
    {
        blastOff = rocketClon.transform.GetChild(1).gameObject;
        flame = rocketClon.transform.GetChild(0).gameObject;
        sr = gameObject.GetComponent<SpriteRenderer>();

        // if the rocket hits a rock, destroy the rocket 
        //if (col.gameObject.tag == "Obstacle") // adjust it just as rocket
        blastOff.SetActive(true);
        sr.enabled = false; 
        Destroy(flame);
        
    }
}
