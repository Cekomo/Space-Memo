using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public SpaceCraft spaceCraft;

    private Vector2 startPos; // pressDown point

    private int screenX = Screen.width; // to determine swipe action on x-axis
    private int screenY = Screen.height; // to determine swipe action on y-axis 

    private bool isRight; // to move towards rigth only during single finger touch 
    private bool isLeft; // to move towards left only during single finger touch
    private bool isUp;  // to move towards rigth only during single finger touch

    [HideInInspector] public int[] movementCatcher; // assign the actions of spacecraft as numbers
    [HideInInspector] public float[] holdDownTime; // holding down time of each movement
    private float holdDownClock; // to count holding down time for each element of holdDownTime
    private int k; // to set the index of movementCatcher

    [HideInInspector] public float[] idleTime; // the time between two actions where the system is being idle as touchInput
    private float idleClock; // to count idle time for each element of holdDownTime

    void Start()
    {
        movementCatcher = new int[100]; // maximum move amount that a scene will handle
        holdDownTime = new float[100];
        idleTime = new float[100];

        for (int i = 0; i < movementCatcher.Length; i++)
            movementCatcher[i] = 0;

        for (int i = 0; i < idleTime.Length; i++)
            idleTime[i] = 0;
        
        k = 0;
    }

    // isRecordings were all isFinished
    void Update()
    {
        //print(spaceCraft.isFinished);
        
        if (Input.touchCount > 0) // to handle index out of bound error
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                startPos = Input.touches[0].position;

        if (Input.touchCount > 0)
        { // 1: go right, 2: go left, 3: go up
            if (Input.touches[0].position.x - startPos.x >= screenX / 5 && spaceCraft.isRightLeft && isRight && spaceCraft.isRecording)
            {
                movementCatcher[k] = 1;

                isLeft = false; isUp = false;
            }
            else if (Input.touches[0].position.x - startPos.x <= -screenX / 5 && spaceCraft.isRightLeft && isLeft && spaceCraft.isRecording)
            { 
                movementCatcher[k] = 2;

                isRight = false; isUp = false;
            }
            else if (Input.touches[0].position.y - startPos.y >= screenY / 6 && isUp && spaceCraft.isRecording)
            { // trigger if swipe is along one sixth the screen
                movementCatcher[k] = 3;

                spaceCraft.isRightLeft = true; // start the array with upward movement
                isLeft = false; isRight = false;
            }
        }

        if ((!isLeft || !isRight || !isUp) && spaceCraft.isRecording)
        { // to count holding down time for go up/left/right
            holdDownClock += Time.deltaTime;
        }

        if (spaceCraft.isRightLeft && isLeft && isRight && isUp && spaceCraft.isRecording)
        { // to count the time when there is no screen touching
            idleClock += Time.deltaTime;
        }

        // !!first element is passed as unassigned!!
        if (Input.touchCount == 0 && (!isUp || !isRight || !isLeft) && spaceCraft.isRecording) // to make them all true for the next move
        {
            isUp = true; isLeft = true; isRight = true;
            holdDownTime[k] = holdDownClock;
            holdDownClock = 0f;
            k++;
        }

        if (Input.touchCount > 0 && spaceCraft.isRightLeft && spaceCraft.isRecording)
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //print(k);
                idleTime[k-1] = idleClock; // add idle time intervals to the array
                idleClock = 0f; // reset the timer
            }

        // below statements show action and idle time between actions as array

        //if (!spaceCraft.isFinished)
        //    for (int i = 0; i < k; i++)
        //        print(i.ToString() + ": Move number: " + movementCatcher[i].ToString() + " Hold down time: " + holdDownTime[i].ToString());

        //if (!spaceCraft.isFinished)
        //    for (int i = 0; i < k - 1; i++)
        //        print(i.ToString() + ": Idle time: " + idleTime[i]);
    }
}
