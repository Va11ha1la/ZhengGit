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
        Texture2D tex=new Texture2D(1,1);
        for (int i = 0; i < 18; i++)
        {
            if (File.Exists(Application.persistentDataPath + $"/WritingBackup/writing{i + 1}.png"))
            {
                byte[] b= File.ReadAllBytes(Application.persistentDataPath + $"/WritingBackup/writing{i + 1}.png");
                tex.LoadImage(b);
                Debug.Log(tex);
                transform.GetChild(i).GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"9月{i + 1}日";
            }
            else
            {
                transform.GetChild(i).GetComponent<Image>().color=Color.clear;
                transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = " ";
            }
        }
    }
}
