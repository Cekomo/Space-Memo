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

    private float slideClock; // timer for sliding gradually

    void Start()
    {
        mapSlider.maxValue = spaceCraft.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        print(mapSlider.value);
        //spaceCraft.mainCamera.transform.Translate(0f, mapSlider.value, 0f);


    }

    public void SlidingPlay() // function to start/continue sliding
    {

    }

    public void StartPlay() // function to start the game
    {

    }
}
