using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();

                if (instance == null)
                {
                    GameObject container = new GameObject("SoundManager");
                    instance = container.AddComponent<SoundManager>();
                }
            }
            return instance;
        }
    }

    private AudioSource[] audioSources;

    void Awake()
    {
        if (instance == null)
            instance = this;

        audioSources = GetComponents<AudioSource>();
    }

    public void Play(SoundType chanel, AudioClip clip)
    {
        audioSources[(int)chanel].Stop();
        audioSources[(int)chanel].clip = clip;
        audioSources[(int)chanel].Play();
    }

    public void PlayOneShot(SoundType chanel, AudioClip clip)
    {
        audioSources[(int)chanel].PlayOneShot(clip);
    }
}
