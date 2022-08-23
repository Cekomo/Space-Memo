using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public SpaceCraft spaceCraft;
    
    public GameObject backGround; // to represent background
    private float bgSpeed = 0.3f; // to adjust the background speed considering space-craft

    void Update()
    {
        // I would like to set the speed of background slightly less than spacecraft since..
        //.. stars are far away and the movement should seem realistic
        transform.Translate(Vector2.up * (spaceCraft.currentVelocity * 0.7f) * Time.deltaTime);

        // (0.3f * 5) to make it slightly less than spacecraft moving speed
        // should have a view like the spacecraft travels among stars 
        if (spaceCraft.isRight) // to move the background wrt spacecraft towards right
            transform.Translate(Vector2.right * bgSpeed * 5 * Time.deltaTime);
        if (spaceCraft.isLeft) // to move the background wrt spacecraft towards left
            transform.Translate(Vector2.right * -bgSpeed * 5 * Time.deltaTime);
    }
}
