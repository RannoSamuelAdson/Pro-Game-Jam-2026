using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private AudioManager audioManager;

    private enum VolumeType
    {
        MASTER,
        MUSIC,
        SFX,
        UI
    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;

    private Slider volumeSlider;
    private EventInstance sfxTestAudio;
    private EventInstance uiTestAudio;
    private bool firstTime = true; // workaround for test audio playing on menu open

    private void Start()
    {
        volumeSlider = GetComponent<Slider>();
        audioManager = AudioManager.Instance;
        sfxTestAudio = audioManager.CreateInstance(FMODEvents.Instance.TestSound);
        uiTestAudio = audioManager.CreateInstance(FMODEvents.Instance.ButtonClick);
        switch (volumeType)
        {
            case VolumeType.MUSIC:
                volumeSlider.value = SaveManager.Instance.systemData.MusicVolume / 5f * 100f;
                break;
            case VolumeType.SFX:
                volumeSlider.value = SaveManager.Instance.systemData.SFXVolume / 5f * 100f;
                break;
            case VolumeType.UI:
                volumeSlider.value = SaveManager.Instance.systemData.UIVolume / 5f * 100f;
                break;
            case VolumeType.MASTER:
                volumeSlider.value = SaveManager.Instance.systemData.MasterVolume / 5f * 100f;
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;
        }
    }

    public void OnSliderValueChanged()
    {
        PLAYBACK_STATE state;
        if (firstTime)
        {
            firstTime = false;
            return;
        }
        float clampedValue = Mathf.Clamp(volumeSlider.value * 5f / 100f, 0f, 1f);
        switch (volumeType)
        {
            case VolumeType.MUSIC:
                SaveManager.Instance.systemData.MusicVolume = clampedValue;
                audioManager.MusicVolume = clampedValue;
                break;
            case VolumeType.SFX:
                SaveManager.Instance.systemData.SFXVolume = clampedValue;
                audioManager.SFXVolume = clampedValue;
                sfxTestAudio.getPlaybackState(out state);
                if (state != PLAYBACK_STATE.PLAYING)
                    sfxTestAudio.start();
                break;
            case VolumeType.UI:
                SaveManager.Instance.systemData.UIVolume = clampedValue;
                audioManager.UIVolume = clampedValue;
                uiTestAudio.getPlaybackState(out state);
                if (state != PLAYBACK_STATE.PLAYING)
                    uiTestAudio.start();
                break;
            case VolumeType.MASTER:
                SaveManager.Instance.systemData.MasterVolume = clampedValue;
                audioManager.MasterVolume = clampedValue;
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;
        }
    }
}



