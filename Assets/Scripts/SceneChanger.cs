using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class for changing between scenes
/// </summary>
public class SceneChanger : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        Knight player = other.gameObject.GetComponent<Knight>();

        if (player != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}
