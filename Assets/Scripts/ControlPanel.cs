using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
    public SpaceCraft spaceCraft;

    public GameObject bottomLeftPanel; // represents bottom left panel 
    public Button startGameButton; // button to start the game bypassing sliding operation
    public Button playGameButton; // button to inspect the map
    [SerializeField] Slider mapSlide; // to adjust the image of play button
    [SerializeField] Button rocketButton; // to activate and deactive rocket button 

    private int sliderMax; // reference for maxValue 

    void Start()
    {
        sliderMax = spaceCraft.maxValue;
        //print(playGameButton.gameObject.transform.GetChild(0));
        //print(playGameButton.gameObject.transform.GetChild(1));

        rocketButton.gameObject.SetActive(false); // rocketButton deactivated at inspector mode
    }
    
    void Update()
    {
        if (mapSlide.value == sliderMax)
        {
            // when slider is at max, deactivate play sign and activate restart sign
            playGameButton.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            playGameButton.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
        else // play sign is activated all the time excep slider value is at max
        {
            playGameButton.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            playGameButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }

        if (!spaceCraft.isFinished)
        {
            bottomLeftPanel.SetActive(false); // deactivate it when the game starts to play
            startGameButton.gameObject.SetActive(false);
            rocketButton.gameObject.SetActive(true);
        }
        
    }
    
    public void RestartGame() // to restart the game when pressed button
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        bottomLeftPanel.SetActive(true);
        startGameButton.gameObject.SetActive(true);

        // revert to initial position when restart is activated
        playGameButton.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        playGameButton.gameObject.transform.GetChild(1).gameObject.SetActive(false);

        rocketButton.gameObject.SetActive(false);
    }
}
