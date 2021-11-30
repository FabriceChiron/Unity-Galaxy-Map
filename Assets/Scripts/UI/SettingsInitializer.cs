using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsInitializer : MonoBehaviour
{
    void Start()
    {
        // On récupère tous les éléments UI qui nous interessent dans la scène puis on appelle leur méthode LoadPrefs pour charger les valeurs précédemment sauvegardées dans PlayerPrefs

        SliderSetting[] _sliders = FindObjectsOfType<SliderSetting>(true);
        foreach (SliderSetting sliderSetting in _sliders)
        {
            Debug.Log(sliderSetting);
            sliderSetting.LoadPrefs();
        }

        ToggleSetting[] _toggles = FindObjectsOfType<ToggleSetting>(true);
        foreach (ToggleSetting toggleSetting in _toggles)
        {
            toggleSetting.LoadPrefs();
        }
    }
}
