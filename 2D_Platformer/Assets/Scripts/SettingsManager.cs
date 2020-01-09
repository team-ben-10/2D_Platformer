using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] GameObject keyBindingPrefab;
    [SerializeField] Transform keyBindingContent;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    void Start()
    {
        foreach (var item in InputManager.instance.GetPreset("Player").axes)
        {
            var obj = Instantiate(keyBindingPrefab, keyBindingContent);
            var keybinding = obj.GetComponent<KeybindingPrefab>();
            keybinding.key = item.posKey;
            keybinding.keyName = item.name + "_Pos";
            keybinding.Setup();

            obj = Instantiate(keyBindingPrefab, keyBindingContent);
            keybinding = obj.GetComponent<KeybindingPrefab>();
            keybinding.key = item.negKey;
            keybinding.keyName = item.name + "_Neg";
            keybinding.Setup();
        }

        foreach (var item in InputManager.instance.GetPreset("Player").keyPairs)
        {
            var obj = Instantiate(keyBindingPrefab, keyBindingContent);
            var keybinding = obj.GetComponent<KeybindingPrefab>();
            keybinding.key = item.key;
            keybinding.keyName = item.name;
            keybinding.Setup();
        }

        musicSlider.value = AudioManager.Instance.GetMusicVolume();
        sfxSlider.value = AudioManager.Instance.GetSfxVolume();
    }

    public void ChangeVolumeMusic(float amount)
    {
        AudioManager.Instance.SetMusicVolume(amount);
    }

    public void ChangeVolumeSFX(float amount)
    {
        AudioManager.Instance.SetSfxVolume(amount);
    }

    public void GoBackToOldScene()
    {
        LoadManager.Instance.LoadScene("Start_Scene");
    }
}
