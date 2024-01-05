using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//播放摩天轮转动时的音效
public class MTLAudio : MonoBehaviour
{
    public AudioSource Mmtl;

    private void OnMouseDown() {
        Mmtl.Play();
    }

    private void OnMouseUp() {
        Mmtl.Pause();
    }
}
