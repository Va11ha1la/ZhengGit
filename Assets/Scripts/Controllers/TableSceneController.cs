using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableSceneController : MonoBehaviour
{
    public Image backgroundImage;
    public Button notebookButton;
    DayCheck dayCheck;

    private void Awake()
    {
        string jsonData = File.ReadAllText(Path.Combine(Application.persistentDataPath, "DayData.json"));
        dayCheck = JsonUtility.FromJson<DayCheck>(jsonData);
    }
    
    void Start()
    {
        char c='a';
        switch (dayCheck.ClickCheck)
        {
            case 1:
                c = 'a';
                break;  
            case 2 :
                c = 'b';
                break;  
            case 3 :
                c = 'c';
                break; 
        }
        Texture2D texture = Resources.Load<Texture2D>($"Image/Table/{dayCheck.DayCount+1}{c}_table");
        backgroundImage.sprite =Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        
        Texture2D notebook=Resources.Load<Texture2D>($"Image/Table/{c}_notebook");
        notebookButton.GetComponent<Image>().sprite = Sprite.Create(notebook, new Rect(0, 0, notebook.width, notebook.height), new Vector2(0.5f, 0.5f));
    }
}
