using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimEvent : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public void Interactable()
    {
        button1.interactable = true;
        button2.interactable = true;
    }
}
