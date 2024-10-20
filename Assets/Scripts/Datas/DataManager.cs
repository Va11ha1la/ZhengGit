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
        
        string jsonStr = JsonUtility.ToJson(dayCheck);
        File.WriteAllText(Application.persistentDataPath + "/DayData.json", jsonStr);
        print(Application.persistentDataPath);
    
    }

}
[SerializeField]
public class DayCheck
{
    public int ClickCheck ;//这里是检查今天点击了几次-->方便搞早中晚画面效果

    public int DayCount ;//这里是查看第几天


}
