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

           
            frameImage.color = greyedOutColor; // 将相框变灰
            GetComponent<Button>().interactable = false; // 禁止再次点击
            
        }
    }

    private void LoadSceneBasedOnClickOrder()
    {
        switch (FrameType)
        {
            case 1:
                UnityEngine.SceneManagement.SceneManager.LoadScene("CalendarScene");
                break;
            case 2:
                UnityEngine.SceneManagement.SceneManager.LoadScene("EatFoodScene");
                break;
            case 3:
                UnityEngine.SceneManagement.SceneManager.LoadScene("TableScene");
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
        File.WriteAllText(jsonFilePath, jsonData);
    }
}
