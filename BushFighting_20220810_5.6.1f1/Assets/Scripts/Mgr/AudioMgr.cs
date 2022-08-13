using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : BaseManager {

    public AudioMgr(GameFacade facade) : base(facade) { }
   private const string Sound_Prefix =      DefinePath.Sound_Prefix;
    public const string Sound_Alert =       DefinePath.Sound_Alert;
    public const string Sound_ArrowShoot =  DefinePath.Sound_ArrowShoot;
    public const string Sound_Bg_Fast =     DefinePath.Sound_Bg_Fast;
    public const string Sound_Bg_Moderate = DefinePath.Sound_Bg_Moderate;
    public const string Sound_Miss =        DefinePath.Sound_Miss;
    public const string Sound_ShootPerson = DefinePath.Sound_ShootPerson;
    public const string Sound_Timer =       DefinePath.Sound_Timer;
    public const string Sound_ButtonClick = DefinePath.Sound_ButtonClick;

    private AudioSource bgAudioSource; //±≥æ∞
    private AudioSource uiAudioSource; //UI

    public override void OnInit()
    {
        GameObject audioSourceGO = new GameObject("AudioSource(GameObject)");
        bgAudioSource = audioSourceGO.AddComponent<AudioSource>();
        uiAudioSource = audioSourceGO.AddComponent<AudioSource>();

        PlayMusic(bgAudioSource, LoadAudio(Sound_Bg_Moderate),0.5f, true);
    }


    private AudioClip LoadAudio(string soundsName)
    {
        return Resources.Load<AudioClip>(Sound_Prefix + soundsName);
    }


    /// <summary>
    /// BGM
    /// </summary>
    /// <param name="soundName"></param>

    public void PlayBGMusic(string soundName)
    {
        PlayMusic(bgAudioSource, LoadAudio(soundName), 0.5f, true);
    }

    /// <summary>
    /// UI“Ù–ß
    /// </summary>
    /// <param name="soundName"></param>
    public void PlayUIAudio(string soundName=Sound_ButtonClick)
    {
        PlayMusic(uiAudioSource, LoadAudio(soundName), 1f);
    }

    private void PlayMusic( AudioSource audioSource,AudioClip clip,float volume, bool loop=false)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
    }

}
