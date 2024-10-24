using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;


public class FoodController : MonoBehaviour
{
    
    public Image spoonCursor;
    public Image spoonCursorDay6;
    public Sprite Day6Cursor;
    public Sprite[] spoonCondition; //勺子状态
    public Sprite[] Day6spoonCondition; //勺子状态
    public SpriteRenderer Bowl;
    public Sprite[] FoodCondition;//盘子状态
    public Sprite[] Day6FoodCondition;
    public Transform foodZone;
    public Transform foodZoneDay6;
    public Transform EatZone;

    [Header("饥饿判断")]
    public CheckHungry checkHungry;
    public TMP_Text TipText;


    private Vector3 previousMousePosition;
    public Canvas canvas;
    public bool isHoldFoodFlag;
    //饱腹值
    public int fullnessValue;
    public int spoonValue;//勺子上能有的饱腹度
    public int maxEat;
    public int currentEat;
    private bool isInFoodZone = false;
    private bool isInEatZone = false;
    public Slider fullnessSlide;

    public GameObject TransAnim;

    private int shakeCount = 0; // 计数音游点击成功和失败
    private bool isShaking = false;
    private string shakeDirection; // 抖动方向（"left" 或 "right"）
    
    private bool hasPressedCorrectKey = false;
    private Coroutine shakeCoroutine; // 控制抖动的协程
    //public Image blackoutPanel;


    public Camera mainCamera;

    public GameObject transitionAnimator; // 转场用Animator 物体
    public GameObject[] TransObjectToBeClosed;//避免遮蔽，把要关的物体放这里
    public bool canTransition;

  
    public GameObject EatTablemage;

    private DayCheck dayCheck;
    private string jsonFilePath;
    int date;

    [Header("抖动该变量")]
    public float[] randomLeft;
    public float[] randomRight;
    //private float shakeReactionTime = 0.5f; // 玩家按键反应时间
    //public float sensitivity = 0.5f;//鼠标敏感度
    public float sensitivity;
    public float[] sensitivity1;
    public float[] shakeReactionTime ;
    public float[] duration;
    public int[] vibrato;
    public float[] elasticity;

  

    [Header("丢弃物位置")]
    public GameObject[] Drops;
    public Vector3[] Drops1;//存储三个丢弃物的位置
    public Vector3[] Drops2;//存储三个丢弃物的位置
    public Vector3[] Drops3;//存储三个丢弃物的位置
    private void Start()
    {

        TipText.gameObject.SetActive(false);
        jsonFilePath = Path.Combine(Application.persistentDataPath, "DayData.json");
        string jsonData = File.ReadAllText(jsonFilePath);
        dayCheck = JsonUtility.FromJson<DayCheck>(jsonData);
        date = dayCheck.DayCount;
        char c = dayCheck.ClickCheck == 0 ? 'a' : dayCheck.ClickCheck == 1 ? 'b' : 'c';
        string name = "EatTable";
        Sprite BGImage = Resources.Load<Sprite>($"Image/Backgrounds/{name}{dayCheck.DayCount + 1}{c}");

        EatTablemage.GetComponent<SpriteRenderer>().sprite = BGImage;
        canTransition = false;
        transitionAnimator.gameObject.SetActive(false);
        maxEat = 3;
        //blackoutPanel.transform.SetSiblingIndex(0);
        //blackoutPanel.color = new Color(255, 255, 255, 0);
        fullnessSlide.maxValue = 6;
        Cursor.visible = false;
        spoonCursor.transform.position = new Vector3(Screen.width - 100, 100, 0);
        spoonCursor.sprite = spoonCondition[0];
        if(date == 5)
        {
            spoonCursor.sprite = Day6spoonCondition[0];
        }
        previousMousePosition = Input.mousePosition; // 初始化鼠标位置
        isHoldFoodFlag = false;
        sensitivity = sensitivity1[date] + 0.5f;
        for (int i = 0; i < Drops.Length; i++)
        {
            switch (i)
            {
                case 0:
                    Debug.Log(Drops1[date]);
                    Debug.Log(Drops[i].gameObject.transform.position);
                    Drops[i].gameObject.transform.localPosition = Drops1[date];
                    break;
                case 1:
                    Drops[i].gameObject.transform.localPosition = Drops2[date];
                    break;
                case 2:
                    Drops[i].gameObject.transform.localPosition = Drops3[date];
                    break;
            }
        }

        if(date == 5)
        {
            spoonCursorDay6.gameObject.SetActive(true);
            spoonCursor.color = new Color(255, 255, 255, 0);
            spoonCursor = spoonCursorDay6;
            spoonCursor.sprite = Day6Cursor;
            Bowl.sprite = Day6FoodCondition[2];
            
            
            
        }
    }

    private void Update()
    {
        if (currentEat < maxEat)
        {
            HandleCursorMove();
            CheckFoodZone();
            CheckEatZone();
        }
        else
        {
            //blackoutPanel.transform.SetAsLastSibling();
            //blackoutPanel.DOFade(1.0f, 2.0f).OnComplete(() =>
            //{
            HandleCursorMove();
         
            CheckEatZone();
            if (canTransition)
            {
                if (fullnessValue < 2)
                {
                    if (checkHungry.HungryDay == 1)
                    {
                        TipText.gameObject.SetActive(true);
                        TipText.text = "...饿...饿...";
                        TipText.DOFade(1f, 1f) 
                            .OnComplete(() =>
                            {
                            
                                DOVirtual.DelayedCall(3f, () =>
                                {
                                    TipText.DOFade(0f, 1f)
                                        .OnComplete(() =>
                                        {
                                           
                                            BlackoutTransition();
                                        });
                                });
                            });

                       
                        checkHungry.HungryDay++;
                    }
                    else
                    {
                        TipText.gameObject.SetActive(true);
                        TipText.DOFade(1f, 1f).OnComplete(() =>
                        {
                         
                            DOVirtual.DelayedCall(3f, () =>
                            {
                                TipText.text = "...感觉饥肠辘辘";
                                TipText.DOFade(0f, 1f)
                                    .OnComplete(() =>
                                    {
                                     
                                        BlackoutTransition();
                                    });
                            });
                        });

                        
                        checkHungry.HungryDay++;
                    }
                }
                else
                {
                    // 满腹值 >= 2 时重置 HungryDay
                    checkHungry.HungryDay = 0;
                }
            }

            //});
        }
    }
    private void BlackoutTransition()
    {
        transitionAnimator.SetActive(true);
        for (int i = 0; i < TransObjectToBeClosed.Length; i++)
        {
            TransObjectToBeClosed[i].gameObject.SetActive(false);
        }

        transitionAnimator.GetComponent<Animator>().SetTrigger("StartTrans");

        StartCoroutine(LoadSceneAfterAnimation());
    }
    private IEnumerator LoadSceneAfterAnimation()
    {

        yield return new WaitForSeconds(2.9f);


        SceneManager.LoadScene("StartScene");
    }
    public void HandleCursorMove()
    {
        
        // 获取当前鼠标位置（屏幕坐标）
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 delta = currentMousePosition - previousMousePosition;
        previousMousePosition = currentMousePosition;
        
        Vector3 adjustedDelta = delta * sensitivity;

        // 将屏幕坐标转换为Canvas的RectTransform坐标
        Vector3 spoonPosition = spoonCursor.rectTransform.localPosition + adjustedDelta;

        if (date != 5)
        {
            float clampedX = Mathf.Clamp(spoonPosition.x, -canvas.pixelRect.width * 0.25f, canvas.pixelRect.width / 2);
            float clampedY = Mathf.Clamp(spoonPosition.y, -canvas.pixelRect.height / 2f, canvas.pixelRect.height * 0.25f);

            spoonCursor.rectTransform.localPosition = new Vector3(clampedX, clampedY, spoonCursor.rectTransform.localPosition.z);
        }
        else
        {
            float clampedX = Mathf.Clamp(spoonPosition.x, -canvas.pixelRect.width * 0.25f, canvas.pixelRect.width / 2);
            float clampedY = Mathf.Clamp(spoonPosition.y, -canvas.pixelRect.height / 1f, canvas.pixelRect.height  *0.25f);

            spoonCursor.rectTransform.localPosition = new Vector3(clampedX, clampedY, spoonCursor.rectTransform.localPosition.z);
        }
        
     
        if (isInFoodZone && !isShaking)
        {
            shakeDirection = Random.Range(0, 2) == 0 ? "left" : "right";
            StartShaking();
        }
    }
    private void CheckFoodZone()
    {
        if (date == 5)
        {
            RectTransform foodZoneRect = spoonCursor.GetComponent<RectTransform>();
            foodZoneRect.pivot = new Vector2(0, 0.5f);
            if (RectTransformUtility.RectangleContainsScreenPoint(foodZone.GetComponent<RectTransform>(), foodZoneRect.transform.position+new Vector3(100,-50,0), canvas.worldCamera) && !isHoldFoodFlag)
            {
                if (!isInFoodZone)
                {
                    isInFoodZone = true;
                    isInEatZone = false;
                    spoonValue = 2;
                    spoonCursor.sprite = spoonCondition[spoonValue]; // 更改为盛饭菜的勺子
                    if (date == 5)
                    {
                        spoonCursor.sprite = Day6spoonCondition[spoonValue];
                    }
                    isHoldFoodFlag = true;

                    CheckFood();
                }
            }

        }
        // 检查光标是否在食物区域
        else
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(foodZone.GetComponent<RectTransform>(), spoonCursor.transform.position, canvas.worldCamera) && !isHoldFoodFlag)
            {
                if (!isInFoodZone)
                {
                    isInFoodZone = true;
                    isInEatZone = false;
                    spoonValue = 2;
                    spoonCursor.sprite = spoonCondition[spoonValue]; // 更改为盛饭菜的勺子
                    if (date == 5)
                    {
                        spoonCursor.sprite = Day6spoonCondition[spoonValue];
                    }
                    isHoldFoodFlag = true;

                    CheckFood();
                }
            }
        }
      
    }
    private void CheckEatZone()
    {
        // 检查光标是否在吃饭区域
        if (RectTransformUtility.RectangleContainsScreenPoint(EatZone.GetComponent<RectTransform>(), spoonCursor.transform.position, canvas.worldCamera) && isHoldFoodFlag)
        {
            if (!isInEatZone)
            {
                isInEatZone = true;
                isInFoodZone = false;
                isHoldFoodFlag = false;
                SuccessfulGrab();
                spoonValue = 0;
                spoonCursor.sprite = spoonCondition[spoonValue]; // 更改为不盛饭菜的勺子
                if (date == 5)
                {
                    spoonCursor.sprite = Day6spoonCondition[spoonValue];
                }
            }
        }

    }
    private void CheckFood()
    {
        Bowl.sprite = FoodCondition[currentEat];
        if (date == 5)
        {
            Bowl.sprite = Day6FoodCondition[spoonValue];
        }
        currentEat++;
    }

    // 开始抖动环节
    private void StartShaking()
    {
        if (!isShaking)
        {
            isShaking = true;
            sensitivity = sensitivity1[date];
            shakeCoroutine = StartCoroutine(RandomShakeRoutine());
        }
    }

    // 停止抖动环节
    private void StopShaking()
    {

        if (isShaking)
        {
            isShaking = false;
            if (shakeCoroutine != null)
            {
                StopCoroutine(shakeCoroutine);
                sensitivity = sensitivity1[date]+0.5f;
            }
        }
    }



    private IEnumerator RandomShakeRoutine()
    {
        while (isInFoodZone)
        {
            // 随机等待一段时间
            float randomTime = Random.Range(2f, 5f);
            yield return new WaitForSeconds(randomTime);

            if (!isInFoodZone) yield break; // 如果玩家已经离开食物区域，停止抖动

            // 随机决定抖动方向
            shakeDirection = Random.Range(0, 2) == 0 ? "left" : "right";

            // 轻微抖动（提前提示）
            spoonCursor.transform.DOPunchPosition(new Vector3(shakeDirection == "left" ? -50f : 50f, 0, 0), duration[date], vibrato[date], elasticity[date]).OnComplete(() =>
            {
                // 大幅抖动
                spoonCursor.transform.DOPunchPosition(new Vector3(shakeDirection == "left" ? -100f : 100f, 0, 0), duration[date]+0.1f, 1, elasticity[date]).OnComplete(() =>
                {
                    // 等待玩家输入
                    StartCoroutine(WaitForPlayerInput());
                });
            });
        }
    }
    // 等待玩家输入的协程
    private IEnumerator WaitForPlayerInput()
    {
        float startTime = Time.time;
        hasPressedCorrectKey = false;
        float cooldownTime = 0.5f; 
        float lastPressTime = -cooldownTime; 

        while (Time.time < startTime + shakeReactionTime[date])
        {
            // 检测按键，且需要判断是否在冷却时间内
            if ((shakeDirection == "left" && Input.GetKeyDown(KeyCode.LeftArrow) && Time.time >= lastPressTime + cooldownTime) ||
                (shakeDirection == "right" && Input.GetKeyDown(KeyCode.RightArrow) && Time.time >= lastPressTime + cooldownTime))
            {
                hasPressedCorrectKey = true;
                lastPressTime = Time.time; 
               
                yield break; 
            }

            yield return null; 
        }

        if (!hasPressedCorrectKey)
        {
            FailedGrab();
        }
    }

    private void FailedGrab()
    {
        if (spoonValue > 0)
        {
            spoonValue -= 1;


            // 更换勺子状态（少一些饭菜）
            spoonCursor.sprite = spoonCondition[spoonValue];
            if (date == 5)
            {
                spoonCursor.sprite = Day6spoonCondition[spoonValue];
            }
            shakeCount++;
            if (shakeCount >= 2)
            {
                ResetMealState();
               
                if (currentEat >= maxEat)
                {
                    canTransition = true;
                }
            }
        }

    }
    private void SuccessfulGrab()
    {
        fullnessValue += spoonValue;
        fullnessSlide.value = fullnessValue;
        ResetMealState();
        ShakeCamera();
   
        Debug.Log("Success");
        
    }
    private void ResetMealState()
    {
        isInFoodZone = false;
        spoonValue = 0;
        spoonCursor.sprite = spoonCondition[spoonValue]; // 重置为空勺子
        if (date == 5)
        {
            spoonCursor.sprite = Day6spoonCondition[spoonValue];
        }
        shakeCount = 0;
        hasPressedCorrectKey = false;
        isHoldFoodFlag = false;
        StopShaking();
    }

    public void ShakeCamera()
    {

        mainCamera.transform.DOPunchPosition(new Vector3(0, -2.5f, 0), 0.4f, 4, 0).OnComplete(() =>
        {
            mainCamera.transform.DOPunchPosition(new Vector3(0, -1.25f, 0), 0.4f, 4, 0).OnComplete(() =>
            {
                mainCamera.transform.DOPunchPosition(new Vector3(0, -0.75f, 0), 0.4f, 4, 0).OnComplete(() =>
                {

                    if (currentEat >= maxEat)
                    {
                        canTransition = true;
                    }
                });
            });
        });
    }
}

