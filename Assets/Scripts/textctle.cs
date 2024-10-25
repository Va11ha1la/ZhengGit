using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class textctle : MonoBehaviour
{
    public string[] text;
    private void Start()
    {
        string jsonFilePath = Path.Combine(Application.persistentDataPath, "DayData.json");
        string jsonData = File.ReadAllText(jsonFilePath);
        DayCheck dayCheck = JsonUtility.FromJson<DayCheck>(jsonData);
        if (dayCheck.DayCount <= 6)
        {
            GetComponent<Text>().text = text[dayCheck.DayCount-1];
            DOTween.To(() => GetComponent<CanvasGroup>().alpha,
                x => GetComponent<CanvasGroup>().alpha = x, 1, 0.7f).SetEase(Ease.OutBounce);
        }
    }
}
