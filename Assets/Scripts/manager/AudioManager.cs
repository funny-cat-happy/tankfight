using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource[] _audioSource;  //0播放普通音效 1播放提示音效 2播放背景音乐
    public enum AudioKind
    {
        normalaudio,
        tip,
    }
    private AudioClip[] _backgroundAudioClips;
    private AudioClip GunFireClip;
    private void Awake()
    {
        Instance = this;
        _audioSource = transform.GetComponents<AudioSource>();
        _backgroundAudioClips = new AudioClip[5];
        _backgroundAudioClips[0] = Resources.Load<AudioClip>("Audio/alert0");
        _backgroundAudioClips[1] = Resources.Load<AudioClip>("Audio/alert1");
        _backgroundAudioClips[2] = Resources.Load<AudioClip>("Audio/alert23");
        _backgroundAudioClips[3] = Resources.Load<AudioClip>("Audio/alert45");
        _backgroundAudioClips[4] = Resources.Load<AudioClip>("Audio/alert6");
        GunFireClip = Resources.Load<AudioClip>("Audio/GunFire");
    }
    
    public void PlaySound(string SoundName)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/"+SoundName);
        // AudioClip clip = this.GetType().GetField(SoundName + "Clip").GetValue(this) as AudioClip;
        _audioSource[0].clip = clip;
        _audioSource[0].PlayOneShot(clip);
    }
    public void PlaySound(string SoundName,AudioKind kind)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/"+SoundName);
        _audioSource[(int)kind].clip = clip;
        switch (kind)
        {
            case AudioKind.normalaudio:
                _audioSource[(int)kind].volume = 0.2f;
                _audioSource[(int)kind].PlayOneShot(clip);
                break;
            case AudioKind.tip:
                _audioSource[(int)kind].volume = 0.7f;
                _audioSource[(int)kind].PlayOneShot(clip);
                break;
        }

    }
    public void PlaySound(string SoundName,float SoundValue,int id)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/"+SoundName);
        _audioSource[id].clip = clip;
        _audioSource[id].volume = SoundValue;
        _audioSource[id].PlayOneShot(clip);
    }

    public void PlayBackGroundSound(int stage)
    {
        switch (stage)
        {
            case 0:
                _audioSource[2].clip = _backgroundAudioClips[0];
                break;
            case 1:
                _audioSource[2].clip = _backgroundAudioClips[1];
                break;
            case 2:
                _audioSource[2].clip = _backgroundAudioClips[2];
                break;
            case 4:
                _audioSource[2].clip = _backgroundAudioClips[3];
                break;
            case 6:
                _audioSource[2].clip = _backgroundAudioClips[4];
                break;
        }
        _audioSource[2].volume = 1.0f;
        _audioSource[2].loop = true;
        _audioSource[2].Play();
    }
}
