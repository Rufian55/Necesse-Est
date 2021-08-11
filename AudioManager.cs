/*************************************************************************
 * AudioManager.cs - manages audio functionality.
 *************************************************************************/
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour {
    
    #pragma warning disable 0649
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource mainTrack;
    [SerializeField] private Slider volume;
    [SerializeField] private TMP_Text indicatedVolume;
    #pragma warning restore 0649
    private readonly string attenuation = "volume";

    private void Awake() {
        AddListeners();
    }

    void Start() {
        mainTrack.loop = true;
        mainTrack.Play();
        AdjustVolume(0.5f);
    }

    void OnDisable() {
        RemoveListeners();    
    }

    private void AdjustVolume(float value) {
        mixer.SetFloat(attenuation, Mathf.Log10(value) * 20f);
        indicatedVolume.text = (value * 100).ToString("N0");
    }

    private void AddListeners() {
        volume.onValueChanged.AddListener(delegate { AdjustVolume(volume.value); });
    }

    private void RemoveListeners() {
        volume.onValueChanged.RemoveListener(delegate { AdjustVolume(volume.value); });
    }

}
