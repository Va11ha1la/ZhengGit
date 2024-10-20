using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class FrameController : MonoBehaviour
{
    public DayCheck dayCheck; // ��¼����������������Ǹ���
    public Image frameImage; // �ؿ����UI
    public int FrameType;//1��ʾ������2��ʾ�Է���3��ʾ�ʼ�
    private string jsonFilePath; 

    private Color greyedOutColor = new Color(0.5f, 0.5f, 0.5f, 1f);


    public void OnFrameClicked()
    {
        jsonFilePath = Path.Combine(Application.persistentDataPath, "DayData.json");
        LoadDayCheckData(); // ��������
        if (dayCheck.ClickCheck < 3) // ÿ�������3��
        {
            dayCheck.ClickCheck++;
            LoadSceneBasedOnClickOrder();
            SaveDayCheckData(); // ��������

           
            frameImage.color = greyedOutColor; // �������
            GetComponent<Button>().interactable = false; // ��ֹ�ٴε��
            
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
        dayCheck.ClickCheck = 0; // ���õ������
        dayCheck.DayCount++; // ��������
        SaveDayCheckData();
  
    }

    //��ȡDayData����
    private void LoadDayCheckData()
    {
       
            string jsonData = File.ReadAllText(jsonFilePath);
            dayCheck = JsonUtility.FromJson<DayCheck>(jsonData);

    }
    //����
    private void SaveDayCheckData()
    {
        string jsonData = JsonUtility.ToJson(dayCheck);
        File.WriteAllText(jsonFilePath, jsonData);
    }
}
