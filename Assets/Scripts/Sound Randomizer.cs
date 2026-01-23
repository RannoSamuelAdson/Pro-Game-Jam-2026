using UnityEngine;

public class SoundRandomizer : MonoBehaviour
{
    public AudioClip[] sounds;   // List of possible click sounds
    public AudioSource audioSource;   // AudioSource to play the sounds
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
    public void PlayRandomSound()
    {
        Debug.Log("Playing sound");
        if (sounds.Length == 0) return;

        int index = Random.Range(0, sounds.Length);
        audioSource.PlayOneShot(sounds[index]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
