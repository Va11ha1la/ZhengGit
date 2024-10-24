using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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
		switch (day)
		{
			case 2:
				Camera.main.GetComponent<PostProcessVolume>().profile=Resources.Load<PostProcessProfile>("PostProcess_Profiles/2high");
				break;
			case 4:
				Camera.main.gameObject.AddComponent<ChromaticAberrationEffect>().material=Resources.Load<Material>("Shader/ChromaticAberration");
				Camera.main.gameObject.GetComponent<ChromaticAberrationEffect>().material.SetFloat("_Intensity",1f);
				break;
			case 5:
				Camera.main.GetComponent<PostProcessVolume>().profile=Resources.Load<PostProcessProfile>("PostProcess_Profiles/5high");
				break;
			case 9:
				Camera.main.GetComponent<PostProcessVolume>().profile=Resources.Load<PostProcessProfile>("PostProcess_Profiles/2low");
				break;
			case 11:
				Camera.main.gameObject.AddComponent<ChromaticAberrationEffect>().material=Resources.Load<Material>("Shader/ChromaticAberration");
				Camera.main.gameObject.GetComponent<ChromaticAberrationEffect>().material.SetFloat("_Intensity",0.5f);
				break;
			case 12:
				Camera.main.GetComponent<PostProcessVolume>().profile=Resources.Load<PostProcessProfile>("PostProcess_Profiles/5low");
				break;
		}
	}
	private void LoadDayCheckData()
	{
		string jsonData = File.ReadAllText(jsonFilePath);
		dayCheck = JsonUtility.FromJson<DayCheck>(jsonData);
	}
}

	
