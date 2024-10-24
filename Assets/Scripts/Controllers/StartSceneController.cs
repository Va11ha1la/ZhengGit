using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{
    public static StartSceneController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public Transform cameraTransform;
    public Button startBtn;
    public CheckGameSituation checkGameSituation;
    public Image blackoutPanel;
    public Button[] Btns;
    string jsonFilePath;
    public DayCheck dayCheck=new DayCheck();

    public List<Sprite> wallImages;
    public GameObject wallImage;


    public Vector3 cameraTargetPosition = new Vector3(0, -7, -10); // 相机的目标位置展示墙的下半部分
    private float animationSpeed = 3f;

    private bool canCheck;

    private void Update()
    {
        
    }
    void Start()
    {
       
        blackoutPanel.gameObject.SetActive(true);
        blackoutPanel.transform.SetSiblingIndex(0);
        blackoutPanel.color = new Color(255, 255, 255, 0);
       
        Cursor.visible = true;
       
        if (checkGameSituation.isStarted)
        {
            jsonFilePath = Path.Combine(Application.persistentDataPath, "DayData.json");
            LoadDayCheckData();

            char c = dayCheck.ClickCheck == 0 ? 'a' : dayCheck.ClickCheck == 1 ? 'b' : 'c';
          
            //更新图片
<<<<<<< Updated upstream
            Texture2D texture = Resources.Load<Texture2D>($"Image/Backgrounds/{dayCheck.DayCount+1}{c}");
            wallImage.GetComponent<SpriteRenderer>().sprite =Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            Texture2D texture1 = Resources.Load<Texture2D>($"Image/Btns/{dayCheck.DayCount+1}{c}_ri");
            Btns[0].GetComponent<Image>().sprite=Sprite.Create(texture1, new Rect(0, 0, texture1.width, texture1.height), new Vector2(0.5f, 0.5f));
            Texture2D texture2 = Resources.Load<Texture2D>($"Image/Btns/{dayCheck.DayCount+1}{c}_eat");
            Btns[1].GetComponent<Image>().sprite=Sprite.Create(texture2, new Rect(0, 0, texture2.width, texture2.height), new Vector2(0.5f, 0.5f));
            Texture2D texture3 = Resources.Load<Texture2D>($"Image/Btns/{dayCheck.DayCount+1}{c}_white");
            Btns[2].GetComponent<Image>().sprite=Sprite.Create(texture3, new Rect(0, 0, texture3.width, texture3.height), new Vector2(0.5f, 0.5f));
            
            
=======
            Sprite BGImage = Resources.Load<Sprite>($"Image/Backgrounds/{dayCheck.DayCount+1}{c}");
            wallImage.GetComponent<SpriteRenderer>().sprite = BGImage;
            Sprite texture1 = Resources.Load<Sprite>($"Image/Btns/{dayCheck.DayCount+1}{c}_ri");
            Btns[0].GetComponent<Image>().sprite=texture1;
            Sprite texture2 = Resources.Load<Sprite>($"Image/Btns/{dayCheck.DayCount + 1}{c}_eat");
            Btns[1].GetComponent<Image>().sprite=texture2;
            Sprite texture3 = Resources.Load<Sprite>($"Image/Btns/{dayCheck.DayCount + 1}{c}_white");
            Btns[2].GetComponent<Image>().sprite=texture3;


>>>>>>> Stashed changes
            checkDayEnd();
            canCheck = true;
            cameraTransform.position = cameraTargetPosition;

            CameraController.Instance.initialPosition = cameraTransform.position;
            CameraController.Instance.StartGameFlag = true;
            startBtn.gameObject.SetActive(false);
            for (int i = 0; i < Btns.Length; i++)
            {
                Btns[i].gameObject.SetActive(true);
                if (dayCheck.DayCount >= 7)
                {
                    Btns[1].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    Btns[1].GetComponent<Button>().interactable = false;
                }
            }
            for (int i = 0; i < dayCheck.BtnIsClick.Length; i++)
            {
                if (dayCheck.BtnIsClick[i] == true )
                {
                    Btns[i].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    Btns[i].GetComponent<Button>().interactable = false;
                }
            }
        }
        if(checkGameSituation.isStarted == false) {
            startBtn.onClick.AddListener(StartGame);
            for(int i = 0; i < Btns.Length; i++)
            {
                Btns[i].gameObject.SetActive(false);
            }
        }
    }

    void StartGame()
    {
        DataManager.Instance.InitGameData();
        StartCoroutine(MoveCameraToShowLevelSelect());
    }

    IEnumerator MoveCameraToShowLevelSelect()
    {
        startBtn.gameObject.SetActive(false);
        //while (Vector3.Distance(cameraTransform.position, cameraTargetPosition) > 0.01f)
        //{

        //    cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTargetPosition, animationSpeed * Time.deltaTime);
        //    yield return null;
        //}
        while (Vector3.Distance(cameraTargetPosition,cameraTransform.position)> 0.1f)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTargetPosition, animationSpeed * Time.deltaTime);
            yield return null;
        }


        cameraTransform.position = cameraTargetPosition;

        CameraController.Instance.initialPosition = cameraTransform.position;
        CameraController.Instance.StartGameFlag = true;
        checkGameSituation.isStarted = true;
        for (int i = 0; i < Btns.Length; i++)
        {
            Btns[i].gameObject.SetActive(true);
         
        }
    }
    //读取DayData数据
    private void LoadDayCheckData()
    {
        string jsonData = File.ReadAllText(jsonFilePath);
        dayCheck = JsonUtility.FromJson<DayCheck>(jsonData);

    }
    //保存
    private void SaveDayCheckData()
    {
        string jsonData = JsonUtility.ToJson(dayCheck);
        Debug.Log(jsonFilePath);
        using (StreamWriter sw = new StreamWriter(jsonFilePath))
        {
            sw.Write(jsonData);
        }
    }
    
    public bool checkDayEnd()
    {
        LoadDayCheckData();
        if(dayCheck.ClickCheck == 3)
        {
            dayCheck.DayCount++;

            blackoutPanel.transform.SetAsLastSibling();

            blackoutPanel.DOFade(1.0f, 1.0f).OnComplete(() =>
            {
                dayCheck.ClickCheck = 0;
                for (int i = 0; i < dayCheck.BtnIsClick.Length; i++)
                {
                    dayCheck.BtnIsClick[i] = false;
                }
                SaveDayCheckData();
                SceneManager.LoadScene("StartScene");
               
            });
            return true;

        }
        return false;
    }
    public void NextDayButton()//速通一天，检查用
    {
        dayCheck.ClickCheck = 3;
        SaveDayCheckData();
        checkDayEnd();
    }
}


