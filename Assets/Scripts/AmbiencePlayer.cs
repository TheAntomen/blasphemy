using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class which plays background sound, does not destroy on load.
/// </summary>
public class AmbiencePlayer : MonoBehaviour
{
    // Private variables
    private AudioSource source;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        source = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (source.isPlaying) return;
        source.Play();
    }
    public void StopMusic()

    {
        source.Stop();
    }
}
