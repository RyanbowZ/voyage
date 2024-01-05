using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField] GameObject Pgame;
    [SerializeField] GameObject PendGame;
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")){
            Pgame.SetActive(false);
            PendGame.SetActive(true);
        }

    }
}
