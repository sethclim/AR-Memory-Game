using MixedReality.Toolkit.UX;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Slider

public class AudioVolumeControl : MonoBehaviour
{
    [SerializeField] public AudioSource bgm1;
    [SerializeField] public AudioSource bgm2;
    [SerializeField] public AudioSource bgm3;
    

    void Start()
    {
        bgm1.Play();
        bgm2.Pause();
        bgm3.Pause();
        // Set the slider's value to the current audio volume
    }

    void Update()
    {
        
    }
    public void ChangeVolume(SliderEventData data) {
        bgm1.volume = data.NewValue;
        bgm2.volume = data.NewValue;
        bgm3.volume = data.NewValue;
    }
    public void ChangeBGM(AudioSource selectedSource) {
        //Pause all
        bgm1.Pause();
        bgm2.Pause();
        bgm3.Pause();

        //Play user's choice
        selectedSource.Play();
    }
}
