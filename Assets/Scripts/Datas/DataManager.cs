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
    /// <summary>
    /// 存纹理图到硬盘
    /// </summary>
    /// <param name="path"></param>
    /// <param name="textureToSave"></param>
    public static void SaveTexture(Texture2D textureToSave)
    {
        // 确保纹理已应用
        textureToSave.Apply();
        string path = Application.persistentDataPath + "/WritingBackup/writing1.png";
        // 将Texture2D转换为PNG格式的字节数组
        byte[] textureData = textureToSave.EncodeToPNG();

        // 获取目录路径
        string directory = Path.GetDirectoryName(path);

        // 确保目录存在
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        // 保存文件
        File.WriteAllBytes(path, textureData);
        Debug.Log($"Texture saved to: {path}");
    }
}
[SerializeField]
public class DayCheck
{
    public int ClickCheck ;//�����Ǽ��������˼���-->���������������Ч��

    public int DayCount ;//�����ǲ鿴�ڼ���


}

