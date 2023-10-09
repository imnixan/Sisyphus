using System;

using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip memeSound;
    private AudioSource sound;

    private void Start()
    {
        sound = gameObject.AddComponent<AudioSource>();
        sound.volume = PlayerPrefs.GetInt("Sound", 1);
        sound.PlayOneShot(memeSound);
    }
}
