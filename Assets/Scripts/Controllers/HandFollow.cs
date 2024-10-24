using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HandFollow : MonoBehaviour
{
    private string jsonData;
    private DayCheck dayCheck;
    private RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        jsonData = File.ReadAllText(Path.Combine(Application.persistentDataPath, "DayData.json"));
        dayCheck = JsonUtility.FromJson<DayCheck>(jsonData);
        char c = dayCheck.ClickCheck == 1 ? 'a' : dayCheck.ClickCheck == 2 ? 'b' : 'c';
        Texture2D texture = Resources.Load<Texture2D>($"Image/Table/{c}_hand");
        GetComponent<Image>().sprite =Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        rectTransform.pivot = new Vector2(0.165f, 0.8f);
    }

    private void Update()
    {
        rectTransform.position=new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
    }

    public void OnOpenTheNotebook()
    {
        jsonData = File.ReadAllText(Path.Combine(Application.persistentDataPath, "DayData.json"));
        dayCheck = JsonUtility.FromJson<DayCheck>(jsonData);
        char c = dayCheck.ClickCheck == 1 ? 'a' : dayCheck.ClickCheck == 2 ? 'b' : 'c';
        Texture2D texture = Resources.Load<Texture2D>($"Image/Table/{c}_handwithpen");
        GetComponent<Image>().sprite =Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        rectTransform.pivot = new Vector2(0.037f, 0.6f);
    }
}
