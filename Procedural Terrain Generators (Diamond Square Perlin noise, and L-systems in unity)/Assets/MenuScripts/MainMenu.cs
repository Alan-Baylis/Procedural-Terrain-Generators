using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void PerlinNoiseLatest()
    {
        SceneManager.LoadScene("Perlin");
    }

    public void DiamondSquare()
    {
        SceneManager.LoadScene("DiamondSquare");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }

    public void OptionsMenu()
    {

    }
}
