using UnityEngine;
using UnityEngine.Audio;
using System;
public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    public static AudioManager instance;

    bool isMute = false;
    public void Mute()
    {
        isMute = true;
    }

    public void UnMute()
    {
        isMute = false;
    }

    private void Awake()
    {

        if (instance == null)
            instance = this;

        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        if (saveload.isSoundEffectOn)
            UnMute();
        else
            Mute();
    }

    public void Play(string name)
    {
        if (isMute == false)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                return;
            }
            s.source.Play();
        }
    }
}
