using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPanel : MonoBehaviour
{

    public void RestartGame() // to restart the game when pressed button
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
