using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapInspector : MonoBehaviour
{
    public SpaceCraft spaceCraft;
    public ControlPanel controlPanel;

    public Slider mapSlider;
    [SerializeField] Button playButton; // the button that manipulates slider
    [SerializeField] Button startButton; // the button that starts the record
    [SerializeField] Button rocketButton; // the button that launches a rocket

    // min value may be required
    private Vector3 cameraPos; // adjusts the position of the main camera with respect to slider
    private float tempPos; // temporary position for camera
    [HideInInspector] public float cameraVelocity; // camera's velocity to adjust background speed

    [HideInInspector] public bool moveEnd; // go towards maxValue when play button is pressed
    private float slideDistance; // distance between camera and pointed value by slider
    private bool onLine; // to determine if playbutton is touched or not
    private SpriteRenderer sr;

    private Image img;

    void Start()
    {
        //isClicked = false;
        moveEnd = false;
        mapSlider.maxValue = spaceCraft.maxValue;

        rocketButton.gameObject.SetActive(false); // at inspector, rocket button should not be visible

        sr = spaceCraft.player.GetComponent<SpriteRenderer>();
        //sr.enabled = false; // do not show the spacecraft for the map inspection

        //GetComponentInChildren<Button>().restartSign.image.enabled = false; // restart pops up after slider is at the end
    }

    // !!!! it brokes the record if record button is pressed while the handle is moving !!!!
    void Update()
    {
        //print(mapSlider.value);
        //spaceCraft.mainCamera.transform.Translate(0f, mapSlider.value, 0f);

        if (moveEnd && mapSlider.value != spaceCraft.maxValue) // go forward if value is not at max and handle isn't moving
            for (int i = 0; i < mapSlider.maxValue; i++)
            {
                tempPos = cameraPos.y;
                mapSlider.value += Time.deltaTime * 0.2f; // it can be slower or faster
                // as far as I understand cameraPosition should get same value what slider takes
                if (GetComponent<Camera>().transform.position.y < spaceCraft.maxValue) // move until camera reaches maxValue
                {       
                    cameraPos = GetComponent<Camera>().transform.position + new Vector3(0f, 0.2f * Time.deltaTime, 0f);
                    GetComponent<Camera>().transform.position = cameraPos; // assign camera's new position
                }
                //print("b " + cameraPos.y.ToString());
                cameraVelocity = (cameraPos.y - tempPos) / Time.deltaTime;
                //print("c " + GetComponent<Camera>().transform.position.y.ToString());
                //print("b " + cameraPos.y.ToString());
                //spaceCraft.mainCamera.transform.Translate(0f, spaceCraft.mainCamera.transform.localScale, 0f);
                //spaceCraft.mainCamera.transform.Translate(Vector2.up * mapSlider.value * Time.deltaTime);
            }
        else // stop if the handle is moving
        {
            mapSlider.value = mapSlider.value;
            onLine = false; // false if handle stops or it reaches the max value
        }
        
        slideDistance = mapSlider.value - cameraPos.y;
        //print(slideDistance);
        //if (mapSlider.value < cameraPos.y + )
    }

    public void SliderCatcher() // it adjusts main camera with respect to the value of slider
    { // i ignored the background movement for now
        // printing inside the function affects speed of background??
        slideDistance = mapSlider.value - cameraPos.y; // calculate it to determine how much unit to go

        if (slideDistance < -0.01f)
        {
            moveEnd = true;
            for (int i = 0; i < mapSlider.value; i++)
                if (cameraPos.y > mapSlider.value)
                {
                    cameraPos = GetComponent<Camera>().transform.position - new Vector3(0f, 1f * Time.deltaTime, 0f);
                    GetComponent<Camera>().transform.position = cameraPos; // assign camera's new position
                }
            //isClicked = false;
        }
        else if (slideDistance > 0.01f)
        {
            moveEnd = true;
            for (int i = 0; i < mapSlider.value; i++)
                if (cameraPos.y < mapSlider.value)
                {
                    cameraPos = GetComponent<Camera>().transform.position + new Vector3(0f, 1f * Time.deltaTime, 0f);
                    GetComponent<Camera>().transform.position = cameraPos; // assign camera's new position
                }
            //isClicked = false;
        }

        if (Mathf.Abs(slideDistance) < 0.01f && !onLine) // 
            moveEnd = false;

        //if (!spaceCraft.isFinished) // if the game starts, disable the quick-start button
        //    startButton.gameObject.SetActive(false);
        //else if (mapSlider.value == spaceCraft.maxValue)
        //{
        //    //GetComponentInChildren<Button>().playSign.image.enabled = false;
        //    //GetComponentInChildren<Button>().restartSign.image.enabled = true;

        //}
    }

    public void OnPointerUp()
    {
        moveEnd = false;
        //isClicked = true;
        //print("zort");
    }

    public void Replay() // function to show the map and start the game
    { // !!this function can broke the code due to changes!! ----------------
        onLine = true;
        
        if (mapSlider.value == spaceCraft.maxValue)
        {
            mapSlider.value = 0; // reset the slider value if the button is pressed at max
            GetComponent<Camera>().transform.position = new Vector3(0f, 0f, -5f); // return initial position of camera
            moveEnd = false; // stop the handle when play is pressed at the end
            
            // make restart unvisible and play image to visible to start from beginning
            //GetComponentInChildren<Button>().playSign.image.enabled = true;
            //GetComponentInChildren<Button>().restartSign.image.enabled = false;
        }
        else if (!moveEnd)
            moveEnd = true;       
        else if (moveEnd)
            moveEnd = false;
    }

    public void StartPlay() // newly record play
    {
        moveEnd = false; // camera continues to move and does not reset its position when it is not assigned as false

        // deactivate inspector buttons and activate rocket to start the record
        playButton.gameObject.SetActive(false);
        mapSlider.gameObject.SetActive(false);
        //rocketButton.gameObject.SetActive(true);
        
        // make the playback button visible but deactivated
        controlPanel.playBackButton.gameObject.SetActive(true);
        controlPanel.playBackButton.enabled = false;

        spaceCraft.isFinished = false;

        // lock the camera at the beginning to start
        //sr.enabled = true; // show the spacecraft to start
        GetComponent<Camera>().transform.position = new Vector3(0f, 0f, -5f); // reset the position of camera

        // adjust the variable as true to bring the spacecraft inside screen and omit the obstacles
        spaceCraft.preStart = true;

        for (int i = 0; i < spaceCraft.obstacles.Length; i++)
        {
            spaceCraft.obstacles[i].SetActive(false);
        }

        spaceCraft.isRecording = true; // record the actions when start button is triggered
        spaceCraft.clock = 3; 
    }
}
