using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class AudioManager : MonoBehaviour
{

    public static AudioManager singlton;
    public AudioSource step1, step2, dash, playerPassDoor, die,
        pushedBackByDoorEarly,
        touchWallSFX,
        passThrowDoorSFX,
        pushedBackByDoorSFX,
        reachGoolSFX,
        pushBoxSFX,
        playerTouchPlayerSFX;
    public AudioSource inGameMusic, mainMenuMusic, gameOverMusic;
    public AudioSource[] eatSFXs;
    protected List<AudioSource> _loopingSounds = new List<AudioSource>();
    private void Awake()
    {

        if (singlton != null && singlton != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            singlton = this;
        }
    }

    AudioSource getSFX(SFXEnum sfx)
    {

        AudioSource audio = null;

        switch (sfx)
        {
            case SFXEnum.step1SFX:
                audio = step1;
                break;
            case SFXEnum.step2SFX:
                audio = step2;
                break;
            case SFXEnum.playerPassThrowDoorSFX:
                audio = passThrowDoorSFX;
                break;
            case SFXEnum.playerPushedBackByDoorSFX:
                audio = pushedBackByDoorSFX;
                break;
            case SFXEnum.playerReachGoolSFX:
                audio = reachGoolSFX;
                break;
            case SFXEnum.playerTouchPlayerSFX:
                audio = playerTouchPlayerSFX;
                break;
            case SFXEnum.playerTouchWallSFX:
                audio = touchWallSFX;
                break;
            case SFXEnum.pushBoxSFX:
                audio = pushBoxSFX;
                break;
            case SFXEnum.playerPassThrowDoorEarlySFX:
                audio = pushedBackByDoorEarly;
                break;
            case SFXEnum.dashSFX:
                audio = dash;
                break;
            case SFXEnum.playerPassDoor:
                audio = playerPassDoor;
                break;
            case SFXEnum.characterDieSFX:
                audio = die;
                break;


        }

        return audio;
    }



    public AudioSource playSFX(SFXEnum sfx, bool isLooping = false)
    {
        AudioSource sfxToPlay = getSFX(sfx);
        sfxToPlay.loop = isLooping;
        sfxToPlay.Play();

        if(isLooping) _loopingSounds.Add(sfxToPlay);

        return sfxToPlay;
    }

    public void stopLoop(AudioSource source)
    {
        if (source != null)
        {
            source.Stop();
            _loopingSounds.Remove(source);
           
        }
    }


    public IEnumerator playSFXAfter(SFXEnum sfx, float time)
    {
        yield return new WaitForSeconds(time);
        playSFX(sfx, false);
    }



}
