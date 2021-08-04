using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class AudioManagerNew : Singleton<AudioManagerNew>
{
    [SerializeField] AudioSource FXPlayer;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();       
    }
    public void PlayFX(AudioData audio)
    {
        /*FXPlayer.clip = clip;
        FXPlayer.volume = volume;*/
        FXPlayer.PlayOneShot(audio. clip, audio.volume);
    }
    public void PlayRandomFX(AudioData audio)
    {
        FXPlayer.pitch = Random.Range(0.9f, 1.1f);
        PlayFX(audio);
    }
    public void PlayRandomFX(AudioData[] audios)
    {
        PlayRandomFX(audios[Random.Range (0,audios.Length)]);
    }
    public void PlayFoodSteps(AudioData[] audios)
    {
        PlayRandomFX( audios);
    }
}
[System.Serializable]public class AudioData
{
    public AudioClip clip;
    public float volume;

}

