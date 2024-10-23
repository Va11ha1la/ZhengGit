using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager :MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    public CheckGameSituation startCheckSO;
    private void Awake()
    {
        Instance = this;
    }
    public void InitGameData()
    {
      
        DayCheck dayCheck = new DayCheck();
        dayCheck.ClickCheck = 0;
        dayCheck.DayCount = 0;
        dayCheck.BtnIsClick = new bool[] { false, false, false };

        string jsonStr = JsonUtility.ToJson(dayCheck);
        File.WriteAllText(Application.persistentDataPath + "/DayData.json", jsonStr);
        print(Application.persistentDataPath);
    
    }



}
[SerializeField]
public class DayCheck
{
    public int ClickCheck ;

    public int DayCount ;

    public bool[] BtnIsClick;

}

