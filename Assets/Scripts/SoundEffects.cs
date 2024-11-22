using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public static SoundEffects instance;

    private AudioSource audioSource;

    public AudioClip stompSound;
    public AudioClip damageSound;
    public AudioClip punchSound;
    public AudioClip jumpSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // keeps persistent
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
