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
    public GameObject StartPhoto;
    public Transform cameraTransform;
    public Button startBtn;
    public Button NextDayBtn;
    public CheckGameSituation checkGameSituation;
    public Image blackoutPanel;
    public Button[] Btns;
    string jsonFilePath;
    public DayCheck dayCheck=new DayCheck();

    public List<Sprite> wallImages;
    public GameObject wallImage;
    int date;

    public Vector3 cameraTargetPosition = new Vector3(0, -7, -10); // 相机的目标位置展示墙的下半部分
    private float animationSpeed = 3f;

    private bool canCheck;

    private void Update()
    {
        
    }
    void Start()
    {
        if (!checkGameSituation.isStarted)
        {
            SoundManager.instance.PlayBGM(Globals.BGM1Long,0.8f);
        }
        transitionAnimator.SetActive(false);
        //blackoutPanel.gameObject.SetActive(true);
        //blackoutPanel.transform.SetSiblingIndex(0);
        //blackoutPanel.color = new Color(255, 255, 255, 0);
        NextDayBtn.gameObject.SetActive(false);
        Cursor.visible = true;
       
        if (checkGameSituation.isStarted)
        {
            StartPhoto.gameObject.SetActive(false);
           
            jsonFilePath = Path.Combine(Application.persistentDataPath, "DayData.json");
            LoadDayCheckData();
            date = dayCheck.DayCount;

            char c = dayCheck.ClickCheck == 0 ? 'a' : dayCheck.ClickCheck == 1 ? 'b' : 'c';
            int p = dayCheck.DayCount;
            if (date > 0 && date <= 2)
            {
                SoundManager.instance.PlayBGM(Globals.BGM2, 0.8f);
            }
            //更新图片
            else if (date > 3 && date <= 5)
            {
                SoundManager.instance.PlayBGM(Globals.BGM3_Full, 0.75f);
            }
            else if (date > 5 && date <= 6)
            {
                SoundManager.instance.PlayBGM(Globals.BGM4, 0.75f);
                SoundManager.instance.PlayLoopingSound(Globals.Breath1, 0.4f);
            }
            if (dayCheck.DayCount >=7 && dayCheck.DayCount < 12)
            {
                p = 4;
            }else if (dayCheck.DayCount >= 1 && dayCheck.DayCount < 4)
            {
                p = 0;
            }
                Sprite BGImage = Resources.Load<Sprite>($"Image/Backgrounds/{p + 1}{c}");
            wallImage.GetComponent<SpriteRenderer>().sprite = BGImage;
            Sprite texture1 = Resources.Load<Sprite>($"Image/Btns/{p + 1}{c}_ri");
            Btns[0].GetComponent<Image>().sprite=texture1;
            Sprite texture2 = Resources.Load<Sprite>($"Image/Btns/{p + 1}{c}_eat");
            Btns[1].GetComponent<Image>().sprite=texture2;
            Sprite texture3 = Resources.Load<Sprite>($"Image/Btns/{p + 1}{c}_white");
            Btns[2].GetComponent<Image>().sprite=texture3;

           
            if(dayCheck.ClickCheck==3||(dayCheck.ClickCheck==2 && dayCheck.DayCount > 6))
            {
                if(dayCheck.DayCount==17)SceneManager.LoadScene("End");
                NextDayBtn.gameObject.SetActive(true);
            }
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
        checkGameSituation.isStarted = true;
        for (int i = 0; i < Btns.Length; i++)
        {
            Btns[i].gameObject.SetActive(true);

        }
        while (Vector3.Distance(cameraTargetPosition,cameraTransform.position)> 0.1f)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTargetPosition, animationSpeed * Time.deltaTime);
            yield return null;
        }


        cameraTransform.position = cameraTargetPosition;

        CameraController.Instance.initialPosition = cameraTransform.position;
        CameraController.Instance.StartGameFlag = true;
      
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

    public GameObject transitionAnimator;
    public bool checkDayEnd()
    {
        LoadDayCheckData();
        if(dayCheck.ClickCheck == 3)
        {
            dayCheck.DayCount++;

            for (int i = 0; i < dayCheck.BtnIsClick.Length; i++)
            {
                dayCheck.BtnIsClick[i] = false;
            }
            SaveDayCheckData();
            transitionAnimator.SetActive(true);
            transitionAnimator.GetComponent<Animator>().SetTrigger("StartTrans");
            dayCheck.ClickCheck = 0;
            SaveDayCheckData();
            StartCoroutine(LoadSceneAfterAnimation());
            return true;

        }else if(dayCheck.ClickCheck == 2 && dayCheck.DayCount > 6)
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
                transitionAnimator.SetActive(true);
                SceneManager.LoadScene("StartScene");

            });
            return true;
        }
        return false;
    }
    private IEnumerator LoadSceneAfterAnimation()
    {

        yield return new WaitForSeconds(2.9f);


        SceneManager.LoadScene("StartScene");
    }
    public void NextDayButton()//速通一天，检查用
    {
        dayCheck.ClickCheck = 3;

        SaveDayCheckData();
        checkDayEnd();
    }
}


