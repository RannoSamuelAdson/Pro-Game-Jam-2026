using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private TMPro.TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    [SerializeField] GameObject resolutionOption;
    [SerializeField] GameObject fullscreenOption;

    private void Start()
    {
        resolutionDropdown = resolutionOption.transform.GetChild(1).GetChild(1).GetComponent<TMPro.TMP_Dropdown>();
        if (BuildConsts.isWebGL || BuildConsts.isMobile)
        {
            resolutionOption.SetActive(false);
        }
        else
        {
            // handle resolution
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();
            filteredResolutions = new();
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (!filteredResolutions.Any(x => x.width == resolutions[i].width && x.height == resolutions[i].height))  //check if resolution already exists in list
                {
                    filteredResolutions.Add(resolutions[i]);  //add resolution to list if it doesn't exist yet
                }
            }
            List<string> options = new();
            int currentResolutionIndex = 0;
            for (int i = 0; i < filteredResolutions.Count; i++)
            {
                string option = filteredResolutions[i].width + "x" + filteredResolutions[i].height;
                options.Add(option);
                if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
            resolutionDropdown.onValueChanged.AddListener(delegate { SetResolution(resolutionDropdown.value); });
        }
        fullscreenToggle.isOn = Screen.fullScreen;
        // add the listeners so i dont need to manually drag them in the editor
        fullscreenToggle.onValueChanged.AddListener(delegate { SetFullscreen(fullscreenToggle.isOn); });
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
