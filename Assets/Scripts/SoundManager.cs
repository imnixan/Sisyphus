using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip memeSound;

    [SerializeField]
    private AudioClip[] screamSounds;

    private AudioSource sound;

    private void Start()
    {
        sound = gameObject.AddComponent<AudioSource>();
        sound.volume = PlayerPrefs.GetInt("Sound", 1);
        sound.PlayOneShot(memeSound);
    }

    public void PlayScream()
    {
        sound.PlayOneShot(screamSounds[Random.Range(0, screamSounds.Length)]);
        Vibrate();
    }

    private void Vibrate()
    {
        if (PlayerPrefs.GetInt("Vibro", 1) == 1)
        {
            Handheld.Vibrate();
        }
    }
}
