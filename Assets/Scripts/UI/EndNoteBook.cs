using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndNoteBook : MonoBehaviour
{
    private void Start()
    {
        for (int i = 1; i <= 18; i++)
        {
            if (File.Exists(Application.persistentDataPath + $"/WritingBackup/writing{i}.png"))
            {
                byte[] b= File.ReadAllBytes(Application.persistentDataPath + $"/WritingBackup/writing{i}.png");
                Texture2D tex=new Texture2D(1,1);
                tex.LoadImage(b);
                transform.GetChild(i-1).GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                transform.GetChild(i-1).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"9月{i}日";
            }
            else
            {
                transform.GetChild(i-1).GetComponent<Image>().color=Color.clear;
                transform.GetChild(i-1).GetChild(0).GetComponent<TextMeshProUGUI>().text = " ";
            }
        }
    }
}
