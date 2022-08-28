using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapInspector : MonoBehaviour
{
    public SpaceCraft spaceCraft;
    
    [SerializeField] Slider mapSlider;
    [SerializeField] Button playButton; // the button that manipulates slider
    // min value may be required

    private bool moveEnd; // go towards maxValue when play button is pressed

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

        if (moveEnd) // go forward if value is not at max and handle isn't moving
            for (int i = 0; i < mapSlider.maxValue; i++)
            {
                mapSlider.value += Time.deltaTime / 5;
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
        }
        else if (!moveEnd)
            moveEnd = true;       
        else if (moveEnd)
            moveEnd = false;
    }
}
