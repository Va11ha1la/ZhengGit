using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class FrameController : MonoBehaviour
{
    public DayCheck dayCheck; // 记录点击次数和天数的那个类
    public Image frameImage; // 关卡入口UI
    public int FrameType;//1表示日历，2表示吃饭，3表示笔记
    private string jsonFilePath; 

    private Color greyedOutColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    

    public void OnFrameClicked()
    {
        jsonFilePath = Path.Combine(Application.persistentDataPath, "DayData.json");
        LoadDayCheckData(); // 加载数据
        if (dayCheck.ClickCheck < 3) // 每天最多点击3次
        {
            dayCheck.ClickCheck++;
            LoadSceneBasedOnClickOrder();
            SaveDayCheckData(); // 保存数据


        }
    }

    private void LoadSceneBasedOnClickOrder()
    {
        switch (FrameType)
        {
            case 1:
                UnityEngine.SceneManagement.SceneManager.LoadScene("CalendarScene");
                dayCheck.BtnIsClick[0] = true;
                SaveDayCheckData(); // 保存数据
                break;
            case 2:
                UnityEngine.SceneManagement.SceneManager.LoadScene("EatFoodScene");
                dayCheck.BtnIsClick[1] = true;
                SaveDayCheckData(); // 保存数据
                break;
            case 3:
                UnityEngine.SceneManagement.SceneManager.LoadScene("TableScene");
                dayCheck.BtnIsClick[2] = true;
                SaveDayCheckData(); // 保存数据
                break;
        }
    }

    public void ResetForNewDay()
    {
        dayCheck.ClickCheck = 0; // 重置点击次数
        dayCheck.DayCount++; // 增加天数
        SaveDayCheckData();
  
    }

    //读取DayData数据
    private void LoadDayCheckData()
    {
            string jsonData = File.ReadAllText(jsonFilePath);
            dayCheck = JsonUtility.FromJson<DayCheck>(jsonData);

    }
    //保存
    private void SaveDayCheckData()
    {
        string jsonData = JsonUtility.ToJson(dayCheck);
        
        using(StreamWriter sw=new StreamWriter(jsonFilePath)){
            sw.Write(jsonData);
        }
    }
}
