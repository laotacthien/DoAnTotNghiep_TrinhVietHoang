using UnityEngine;
using UnityEngine.UI;

public class UISoundController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private AudioManager audioManager;

    public Slider backGroundSlider, sfxSlider;
    private void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }
    public void ToggleBackGroundAudio()
    {
        audioManager.ToggleBackGroundAudio();
    }
    public void ToggleSfxAudio()
    {
        audioManager.ToggleSfxAudio();
    }
    public void BackGroundVolume()
    {
        audioManager.BackGroundAudioVolume(backGroundSlider.value);
    }
    public void SfxVolume()
    {
        audioManager.EffectAudioVolume(sfxSlider.value);
    }
}
