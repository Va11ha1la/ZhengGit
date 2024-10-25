using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class startUI : MonoBehaviour
{
    public CheckGameSituation checkGameSituation;
    void Update()
    {
        if (checkGameSituation.isStarted)
        {
            gameObject.SetActive(false);
        }
    }
}
