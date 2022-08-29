using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        moveEnd = false;
        mapSlider.maxValue = spaceCraft.maxValue;
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
    }

    public void StartPlay() // function to show the map and start the game
    {
        if (mapSlider.value == spaceCraft.maxValue)
        { // if play button is activated when the slider at the upper end
            // ..then start the game by disabling slider panel
            playButton.gameObject.SetActive(false);
            mapSlider.gameObject.SetActive(false);
            spaceCraft.isFinished = false;
        }
        else if (!moveEnd)
            moveEnd = true;       
        else if (moveEnd)
            moveEnd = false;
    }
}
