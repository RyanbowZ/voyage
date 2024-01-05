using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButtonBase : MonoBehaviour
{
   private void OnMouseDown() {
       Debug.Log("yes");
       DownEvent();
   }

   virtual public void DownEvent(){}
}
