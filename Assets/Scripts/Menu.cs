using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class for handling the menu and the buttons pressed
/// </summary>
public class Menu : MonoBehaviour
{

    Animation anim; // May implement this in a later stage

    public void StartNormal()
    {
        GameInfo.difficulty = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void StartHard()
    {
        GameInfo.difficulty = 2;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
