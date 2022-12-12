using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string startLevel;

    public GameObject Credits;
    public GameObject kurpitsahillo;
    public GameObject playButton;

    public bool state = true;
    public void NewGame()
    {
        SceneManager.LoadScene(startLevel);
    }

    public void Ohjeet()
    {
        Credits.SetActive(state);
        state = !state;

    }
}
