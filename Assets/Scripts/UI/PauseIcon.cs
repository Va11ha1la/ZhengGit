using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseIcon : MonoBehaviour
{
    public void Pause(GameObject obj)
    {
        obj.SetActive(true);
        gameObject.SetActive(false);
    }
}
