using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PostProcessMgr : MonoBehaviour
{
	private string jsonFilePath;
	private DayCheck dayCheck;
	private void Start()
	{
		jsonFilePath = Path.Combine(Application.persistentDataPath, "DayData.json");
		LoadDayCheckData();
		UpdatePostProcess(dayCheck.DayCount+1);
	}

	private void UpdatePostProcess(int day)
	{
		GameObject temp;
		char temp2;
		switch (day)
		{
			case 2:
				Camera.main.GetComponent<PostProcessVolume>().profile=Resources.Load<PostProcessProfile>("PostProcess_Profiles/2high");
				break;
			case 3:
				Camera.main.GetComponent<PostProcessVolume>().profile=Resources.Load<PostProcessProfile>("PostProcess_Profiles/3high");
				break;
			case 4:
				Camera.main.gameObject.AddComponent<ChromaticAberrationEffect>().material=Resources.Load<Material>("Shader/ChromaticAberration");
				Camera.main.gameObject.GetComponent<ChromaticAberrationEffect>().material.SetFloat("_Intensity",1f);
				temp = GameObject.Find("day4");
				temp.GetComponent<CanvasGroup>().alpha = 0.3f;
				temp2 = 'a';
				switch (dayCheck.ClickCheck)
				{
					case 1:
						temp2 = 'a';
						break;
					case 2:
						temp2 = 'b';
						break;
					case 3:
						temp2 = 'c';
						break;
				}
				temp.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Image/PostprogressTexture/{temp2}");
				break;
			case 5:
				Camera.main.GetComponent<PostProcessVolume>().profile=Resources.Load<PostProcessProfile>("PostProcess_Profiles/5high");
				break;
			case 9:
				Camera.main.GetComponent<PostProcessVolume>().profile=Resources.Load<PostProcessProfile>("PostProcess_Profiles/2low");
				break;
			case 10:
				Camera.main.GetComponent<PostProcessVolume>().profile=Resources.Load<PostProcessProfile>("PostProcess_Profiles/3low");
				break;
			case 11:
				Camera.main.gameObject.AddComponent<ChromaticAberrationEffect>().material=Resources.Load<Material>("Shader/ChromaticAberration");
				Camera.main.gameObject.GetComponent<ChromaticAberrationEffect>().material.SetFloat("_Intensity",0.5f);
				temp = GameObject.Find("day4");
				temp.GetComponent<CanvasGroup>().alpha = 0.1f;
				temp2 = 'a';
				switch (dayCheck.ClickCheck)
				{
					case 1:
						temp2 = 'a';
						break;
					case 2:
						temp2 = 'b';
						break;
					case 3:
						temp2 = 'c';
						break;
				}
				temp.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Image/PostprogressTexture/{temp2}");
				break;
			case 12:
				Camera.main.GetComponent<PostProcessVolume>().profile=Resources.Load<PostProcessProfile>("PostProcess_Profiles/5low");
				break;
			default:
				Camera.main.GetComponent<PostProcessVolume>().profile = null;
				break;
		}
	}
	private void LoadDayCheckData()
	{
		string jsonData = File.ReadAllText(jsonFilePath);
		dayCheck = JsonUtility.FromJson<DayCheck>(jsonData);
	}
}

	
