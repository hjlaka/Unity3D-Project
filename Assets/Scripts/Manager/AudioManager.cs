using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SingleTon<AudioManager>
{
    [System.Serializable]
    public struct SoundClip
    {
        public string name;
        public AudioClip clip;
    }

    [SerializeField]
    private SoundClip[] bgm;
    [SerializeField]
    private SoundClip[] sfx;

    private Dictionary<string, AudioClip> bgmDictionary;
    private Dictionary<string, AudioClip> sfxDictionary;

    [SerializeField]
    private AudioSource bgmPlayer;
    [SerializeField]
    private AudioSource[] sfxPlayer;


    private void Awake()
    {
        bgmDictionary = new Dictionary<string, AudioClip>();
        sfxDictionary = new Dictionary<string, AudioClip>();

        for(int i = 0; i < bgm.Length; i++)
        {
            bgmDictionary.Add(bgm[i].name, bgm[i].clip);
        }

        for (int i = 0; i < sfx.Length; i++)
        {
            sfxDictionary.Add(sfx[i].name, sfx[i].clip);
        }
    }

    private void CreatePool()
    {
        // 인스펙터에서 직접 할당
    }

    public void PlayeBGM(string bgmName)
    {
        AudioClip bgmClip = bgmDictionary.GetValueOrDefault(bgmName);

        if (null == bgmClip)
        {
            Debug.LogError(string.Format("요청한 BGM 없음 : ", bgmName));
            return;
        }

        bgmPlayer.clip = bgmClip;
        bgmPlayer.Play();
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX(string sfxName)
    {
        AudioClip sfxClip = sfxDictionary.GetValueOrDefault(sfxName);

        if (null == sfxClip)
        {
            Debug.LogError(string.Format("요청한 SFX 없음 : ", sfxName));
            return;
        }

        AudioSource idlePlayer = GetSFXPlayer();
        
        if (idlePlayer != null) 
        {
            idlePlayer.clip = sfxClip;
            idlePlayer.Play();
        }
    }

    private AudioSource GetSFXPlayer()
    {
        // 재생 중이지 않은 오디오소스 반환

        for(int i = 0; i < sfxPlayer.Length; i++)
        {
            if (sfxPlayer[i].isPlaying)
                continue;

            return sfxPlayer[i];
        }

        Debug.LogAssertion("모든 오디오가 플레이 중");
        return null;
    }
}
