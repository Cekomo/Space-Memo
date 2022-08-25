using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public SpaceCraft spaceCraft;
    
    public GameObject backGround; // to represent background
    private Rigidbody2D bg_Rigidbody; // to adjust the position of the background
    //private float bgSpeed = 0.3f; // to adjust the background speed considering space-craft

    void Start()
    {
        bg_Rigidbody = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (!spaceCraft.isFinished)
        {
            // I would like to set the speed of background slightly less than spacecraft since..
            //..stars are far away and the movement should seem realistic
            //if (Input.touchCount > 0)
            transform.Translate(Vector2.up * (spaceCraft.currentVelocity * 0.7f) * Time.deltaTime);

            // (0.3f * 5) to make it slightly less than spacecraft moving speed
            // should have a view like the spacecraft travels among stars 
            //if (spaceCraft.isRight && (spaceCraft.player.transform.position.x < 1.75f || spaceCraft.player.transform.position.x < -1.75f)) 
            //    // to force the background wrt spacecraft towards right and enable counter movement when boundary is exceeded
            //    bg_Rigidbody.AddForce(transform.right * bgSpeed * 2f * 0.8f); // bgSpeed * 2 = moveSpeed / 3 
            //if (spaceCraft.isLeft && (spaceCraft.player.transform.position.x > -1.75f || spaceCraft.player.transform.position.x > 1.75f))
            //    // to force the background wrt spacecraft towards left and enable counter movement when boundary is exceeded
            //    bg_Rigidbody.AddForce(transform.right * -bgSpeed * 2f * 0.8f);


            // to prevent the stops for counter movement on boundaries 
            //if (spaceCraft.player.transform.position.x > 1.75f && spaceCraft.isLeft)
            //    bg_Rigidbody.AddForce(transform.right * -bgSpeed * 2f * 0.8f);
            //else if (spaceCraft.player.transform.position.x < -1.75f && spaceCraft.isRight)
            //    bg_Rigidbody.AddForce(transform.right * bgSpeed * 2f * 0.8f);
            //else if (Input.touchCount == 0 || Mathf.Abs(spaceCraft.player.transform.position.x) > 1.75f)
            //{ // to zero the speed when touch finger up or the x-axis boundary is exceeded
            //    bg_Rigidbody.velocity = new Vector2(0, spaceCraft.currentVelocity * 0.7f * Time.deltaTime);
            //}

            // to prevent the stops for counter movement on boundaries and stop otherwise
            //if (Input.touchCount == 0 || (spaceCraft.player.transform.position.x > 1.75f && !spaceCraft.isLeft))
            //    bg_Rigidbody.velocity = new Vector2(spaceCraft.speedFading, spaceCraft.currentVelocity * 0.7f * Time.deltaTime);
            
            //else if (Input.touchCount == 0 || (spaceCraft.player.transform.position.x < -1.75f && !spaceCraft.isRight))                          
            //    bg_Rigidbody.velocity = new Vector2(spaceCraft.speedFading, spaceCraft.currentVelocity * 0.7f * Time.deltaTime);
        }

        if (spaceCraft.isFinished) // to prevent background slipping after collision
            bg_Rigidbody.velocity = new Vector2(0, 0);
    }
}
