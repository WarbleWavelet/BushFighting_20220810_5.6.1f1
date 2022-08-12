using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : BaseManager {

    public AudioMgr(GameFacade facade) : base(facade) { }
    private const string Sound_Prefix =     DefinePath.Sound_Prefix;
    public const string Sound_Alert =       DefinePath.Sound_Alert;
    public const string Sound_ArrowShoot =  DefinePath.Sound_ArrowShoot;
    public const string Sound_Bg_Fast =     DefinePath.Sound_Bg_Fast;
    public const string Sound_Bg_Moderate = DefinePath.Sound_Bg_Moderate;
    public const string Sound_Miss =        DefinePath.Sound_Miss;
    public const string Sound_ShootPerson = DefinePath.Sound_ShootPerson;
    public const string Sound_Timer =       DefinePath.Sound_Timer;
    public const string Sound_ButtonClick = DefinePath.Sound_ButtonClick;

    private AudioSource bgAudioSource; //±³¾°
    private AudioSource uiAudioSource; //UI

    public override void OnInit()
    {
        GameObject audioSourceGO = new GameObject("AudioSource(GameObject)");
        bgAudioSource = audioSourceGO.AddComponent<AudioSource>();
        uiAudioSource = audioSourceGO.AddComponent<AudioSource>();

        PlaySound(bgAudioSource, LoadSound(Sound_Bg_Moderate),0.5f, true);
    }

    public void PlayBGMAudio(string soundName)
    {
        PlaySound(bgAudioSource, LoadSound(soundName), 0.5f, true);
    }
    public void PlayUIAudio(string soundName=Sound_ButtonClick)
    {
        PlaySound(uiAudioSource, LoadSound(soundName), 1f);
    }

    private void PlaySound( AudioSource audioSource,AudioClip clip,float volume, bool loop=false)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
    }
    private AudioClip LoadSound(string soundsName)
    {
        return Resources.Load<AudioClip>(Sound_Prefix + soundsName);
    }
}
