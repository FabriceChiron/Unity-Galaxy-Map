using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsInitializer : MonoBehaviour
{
    void Start()
    {
        // On r�cup�re tous les �l�ments UI qui nous interessent dans la sc�ne puis on appelle leur m�thode LoadPrefs pour charger les valeurs pr�c�demment sauvegard�es dans PlayerPrefs

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
