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
    public Button playBackButton; // to play recorded content with obstacles activated

    private int sliderMax; // reference for maxValue 

    void Start()
    {
        sliderMax = spaceCraft.maxValue;
        //print(playGameButton.gameObject.transform.GetChild(0));
        //print(playGameButton.gameObject.transform.GetChild(1));

        rocketButton.gameObject.SetActive(false); // rocketButton deactivated at inspector mode
        playBackButton.gameObject.SetActive(false); // playback button deactivated till record playing
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
            //rocketButton.gameObject.SetActive(true);
        }
        else
        {
            try // try-catch is implemented because code throws error every time when ship explodes
            {
                if (spaceCraft.player.transform.position.y > spaceCraft.maxValue)
                    playBackButton.enabled = true;
            }
            catch { }
            
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

    public void PlayBackButton()
    {
        playBackButton.gameObject.SetActive(false);
        rocketButton.gameObject.SetActive(true); // activate rocket button to launch rocket simulatenously with record

        for (int i = 0; i < spaceCraft.obstacles.Length; i++)
        {
            spaceCraft.obstacles[i].SetActive(true);
        }

        spaceCraft.isFinished = false; // activate auto play 
        spaceCraft.isRecording = false; // to deactivate recording movement part

        spaceCraft.player.transform.position = new Vector3(0f, -6.5f, -4f); // bring the ship to its initial position
        spaceCraft.p_RigidBody.velocity = new Vector2(0f, 0f);

        GetComponent<Camera>().transform.position = new Vector3(0, 0, -5);

        spaceCraft.preStart = true;
        spaceCraft.clock = 3; // it initially represent pre-movement of spaceship's stopping period
        spaceCraft.currentVelocity = 0; // reset it to detect speed for the record session
    }
}
