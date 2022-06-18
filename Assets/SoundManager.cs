using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
    public AudioMixerGroup[] audioMixerGroups;

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

    public void ChangeMusic(AudioClip audioClip)
    {
        Play(SoundType.Music, audioClip);
    }

    public void PlayOneShot(SoundType chanel, AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        audioSources[(int)chanel].PlayOneShot(clip, volume);
    }

    public void PlayOneShot(SoundType chanel, AudioClip clip, Transform parent, float volume = 1f)
    {
        if (clip == null) return;

        AudioSource audio = PoolManager.Pop("Sound").GetComponent<AudioSource>();
        audio.transform.SetParent(parent);
        audio.transform.localPosition = Vector3.zero;
        audio.outputAudioMixerGroup = audioMixerGroups[(int)chanel];
        audio.PlayOneShot(clip, volume);

        PoolManager.Push(audio.gameObject, clip.length);
    }
}
