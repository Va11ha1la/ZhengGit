using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsController : MonoBehaviour
{
    public Tips tips;
    void Start()
    {
        if (tips.hasShowed)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void CloseTips()
    {
        tips.hasShowed = true;
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
