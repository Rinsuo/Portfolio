using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private AudioSource audioSource;
    public AudioClip MoveSound;
    public AudioClip CantMoveSound;
    public AudioClip WinSound;

    public float volumeMultiply = 1f;

    void Awake()
    {
        Instance = this;
        audioSource = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>();
    }

    public void Sound(int v)
    {
        if (v == 1)
        { audioSource.PlayOneShot(MoveSound, (float)0.5 * volumeMultiply); }
        else if (v == 2)
        { audioSource.PlayOneShot(CantMoveSound, (float)0.2 * volumeMultiply); }
        else if (v == 3)
        { audioSource.PlayOneShot(WinSound, (float)0.5 * volumeMultiply); }
    }
}
