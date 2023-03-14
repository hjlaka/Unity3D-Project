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
    [SerializeField]
    private Transform sfxsTransform;

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

        CreatePool();
    }

    private void CreatePool()
    {
        sfxPlayer = sfxsTransform.GetComponentsInChildren<AudioSource>();
    }

    public void PlayeBGM(string bgmName)
    {
        AudioClip bgmClip = bgmDictionary.GetValueOrDefault(bgmName);

        if (null == bgmClip)
        {
            Debug.LogError(string.Format("��û�� BGM ���� : ", bgmName));
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
            Debug.LogError(string.Format("��û�� SFX ���� : ", sfxName));
            return;
        }

        AudioSource idlePlayer = GetSFXPlayer();
        
        if (idlePlayer != null) 
        {
            idlePlayer.clip = sfxClip;
            idlePlayer.Play();
            Debug.Log(string.Format("SFX �÷���: {0}", idlePlayer));
        }
    }

    private AudioSource GetSFXPlayer()
    {
        // ��� ������ ���� ������ҽ� ��ȯ

        for(int i = 0; i < sfxPlayer.Length; i++)
        {
            if (sfxPlayer[i].isPlaying)
                continue;

            Debug.Log(string.Format("�� SFX �÷��̾� �߰�: {0}��° �÷��̾�", i));
            return sfxPlayer[i];
        }

        Debug.LogAssertion("��� ������� �÷��� ��");
        return null;
    }
}
