using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TipsController : MonoBehaviour
{
    public int index;
    [Serializable]
    class aa
    {
        public bool hasRead;
    }
    void Start()
    {
        aa b=new aa();
        b.hasRead = false;
        string path = Application.persistentDataPath + $"/tips{index}.json";
        if (!File.Exists(path))
        {
            aa a = new aa();
            a.hasRead = false;
            string jsonStr = JsonUtility.ToJson(a);
            File.WriteAllText(Application.persistentDataPath + $"/tips{index}.json", jsonStr);
            Time.timeScale = 0;
        }
        else
        {
            string jsonStr = File.ReadAllText(Application.persistentDataPath + $"/tips{index}.json");
            b=JsonUtility.FromJson<aa>(jsonStr);
        }
        if (b.hasRead)
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
        SoundManager.instance?.PlaySound(Globals.Button1,0.8f);
        aa a = new aa();
        a.hasRead = true;
        string jsonStr = JsonUtility.ToJson(a);
        File.WriteAllText(Application.persistentDataPath + $"/tips{index}.json", jsonStr);
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
