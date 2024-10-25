using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndButton : MonoBehaviour
{
    public static bool[] aa;

    private void Start()
    {
        aa = new bool[] { false, false };
    }

    public void OpenObj(GameObject obj)
    {
        obj.SetActive(true);
    }
        private float timer=0;
    private void Update()
    {
        if (aa[0] && aa[1])
        {
            timer+=Time.deltaTime;
            if (timer >= 3f)
            {
                DayCheck dayCheck = new DayCheck();
                dayCheck.ClickCheck = 0;
                dayCheck.DayCount = 0;
                dayCheck.BtnIsClick = new bool[] { false, false, false };

                string jsonStr = JsonUtility.ToJson(dayCheck);
                File.WriteAllText(Application.persistentDataPath + "/DayData.json", jsonStr);
                SceneManager.LoadScene("StartScene");
            }
        }
    }
}
