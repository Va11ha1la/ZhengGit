using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseIcon : MonoBehaviour
{
    public void Pause(GameObject obj)
    {
        obj.SetActive(true);
        Time.timeScale = 0;
        gameObject.SetActive(false);
    }
}
