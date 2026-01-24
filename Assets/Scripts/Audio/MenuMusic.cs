using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    void Awake()
    {
        AudioManager.Instance.InitializeMusic(FMODEvents.Instance.MenuMusic);
    }

    private void Start()
    {
        AudioManager.Instance.StartMusic();
    }
}
