using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIButtonSoundPlayer : MonoBehaviour
{
    public SoundRandomizer clickSoundRandomizer;
    public List<Button> buttons = new List<Button>();

    private void Update()
    {
        foreach (Button btn in buttons)
        {
            if (btn == null) continue;

            // If button is active in hierarchy and doesn't already have the listener, add it
            if (btn.gameObject.activeInHierarchy && !listenerAdded.Contains(btn))
            {
                btn.onClick.AddListener(() => clickSoundRandomizer.PlayRandomSound());
                listenerAdded.Add(btn);
            }
        }
    }

    // Keep track of which buttons we’ve already added the listener to
    private HashSet<Button> listenerAdded = new HashSet<Button>();
}
