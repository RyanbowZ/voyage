using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimAudio : MonoBehaviour
{
    public AudioSource M_other1;
    public AudioSource M_other2;
    public void PlayAppearSthMusic(){
        AudioManager.PlayAppearSthMusic();
    }
    public void PlayOtherAudio1(){
        M_other1.Play();
    }
    public void PlayOtherAudio2(){
        M_other2.Play();
    }
}
