using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapInspector : MonoBehaviour
{
    public SpaceCraft spaceCraft;
    
    [SerializeField] public Slider mapSlider;
    [SerializeField] Button playButton; // the button that manipulates slider
    // min value may be required
    private Vector3 cameraPos; // adjusts the position of the main camera with respect to slider
    private float tempPos; // temporary position for camera
    [HideInInspector] public float cameraVelocity; // camera's velocity to adjust background speed

    [HideInInspector] public bool moveEnd; // go towards maxValue when play button is pressed
    private float slideDistance; // distance between camera and pointed value by slider
    private bool isClicked; // to determine if sldier is clicked or not
    //private SpriteRenderer sr;

    void Start()
    {
        isClicked = false;
        moveEnd = false;
        mapSlider.maxValue = spaceCraft.maxValue;

        //sr = spaceCraft.player.GetComponent<SpriteRenderer>();
        //sr.enabled = false; // do not show the spacecraft for the map inspection
    }

    // Update is called once per frame
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
            mapSlider.value = mapSlider.value;
        
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
    }

    public void OnPointerUp()
    {
        moveEnd = false;
        //isClicked = true;
        //print("zort");
    }

    public void StartPlay() // function to show the map and start the game
    {
        if (mapSlider.value == spaceCraft.maxValue)
        { // if play button is activated when the slider at the upper end
            // ..then start the game by disabling slider panel
            playButton.gameObject.SetActive(false);
            mapSlider.gameObject.SetActive(false);
            spaceCraft.isFinished = false;

            // lock the camera at the beginning to start
            //sr.enabled = true; // show the spacecraft to start
            GetComponent<Camera>().transform.position = new Vector3(0f, 0f, -5f); // reset the position of camera
        }
        else if (!moveEnd)
            moveEnd = true;       
        else if (moveEnd)
            moveEnd = false;
    }
}
