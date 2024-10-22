using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FoodController : MonoBehaviour
{
    public Image spoonCursor;
    public Sprite[] spoonCondition;//勺子状态
    public Transform foodZone;
    public Transform EatZone;
    public float sensitivity = 0.5f;//鼠标敏感度
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
    

    private int shakeCount = 0; // 计数音游点击成功和失败
    private bool isShaking = false;
    private string shakeDirection; // 抖动方向（"left" 或 "right"）
    private float shakeReactionTime = 0.5f; // 玩家按键反应时间
    private bool hasPressedCorrectKey = false;
    private Coroutine shakeCoroutine; // 控制抖动的协程

    public Camera mainCamera;


    private void Start()
    {
        
        maxEat = 4;
        
        fullnessSlide.maxValue = 12;
        Cursor.visible = false;
        spoonCursor.transform.position = new Vector3(Screen.width - 100, 100, 0);
        spoonCursor.sprite = spoonCondition[0];
        previousMousePosition = Input.mousePosition; // 初始化鼠标位置
        isHoldFoodFlag = false;
    }

    private void Update()
    {
        if (currentEat < maxEat)
        {
            HandleCursorMove();
            CheckFoodZone();
            CheckEatZone();
        }
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

      
        float clampedX = Mathf.Clamp(spoonPosition.x, -canvas.pixelRect.width *0.25f, canvas.pixelRect.width / 2);
        float clampedY = Mathf.Clamp(spoonPosition.y, -canvas.pixelRect.height / 2, canvas.pixelRect.height *0.25f);
        spoonCursor.rectTransform.localPosition = new Vector3(clampedX, clampedY, spoonCursor.rectTransform.localPosition.z);

     
        if (isInFoodZone && !isShaking)
        {
            shakeDirection = Random.Range(0, 2) == 0 ? "left" : "right";
            StartShaking();
        }
    }
    private void CheckFoodZone()
    {
        // 检查光标是否在食物区域
        if (RectTransformUtility.RectangleContainsScreenPoint(foodZone.GetComponent<RectTransform>(), spoonCursor.transform.position, canvas.worldCamera) && !isHoldFoodFlag)
        {
            if (!isInFoodZone)
            {
                isInFoodZone = true;
                isInEatZone = false;
                spoonValue = 3;
                spoonCursor.sprite = spoonCondition[spoonValue]; // 更改为盛饭菜的勺子
                isHoldFoodFlag = true;
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
                
                spoonCursor.sprite = spoonCondition[spoonValue]; // 更改为盛饭菜的勺子
                isHoldFoodFlag = false;
                SuccessfulGrab();
                spoonValue = 0;
            }
        }

    }
    
    // 开始抖动环节
    private void StartShaking()
    {
        if (!isShaking)
        {
            isShaking = true;
            sensitivity = 1f;
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
                sensitivity = 0.5f;
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
            spoonCursor.transform.DOPunchPosition(new Vector3(shakeDirection == "left" ? -50f : 50f, 0, 0), 0.4f, 4, 0).OnComplete(() =>
            {
                // 大幅抖动
                spoonCursor.transform.DOPunchPosition(new Vector3(shakeDirection == "left" ? -100f : 100f, 0, 0), 0.5f, 1, 0).OnComplete(() =>
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

        while (Time.time < startTime + shakeReactionTime)
        {
            // 检测按键，且需要判断是否在冷却时间内
            if ((shakeDirection == "left" && Input.GetKeyDown(KeyCode.LeftArrow) && Time.time >= lastPressTime + cooldownTime) ||
                (shakeDirection == "right" && Input.GetKeyDown(KeyCode.RightArrow) && Time.time >= lastPressTime + cooldownTime))
            {
                hasPressedCorrectKey = true;
                lastPressTime = Time.time; // 更新最后按下的时间
               
                yield break; // 成功后退出协程
            }

            yield return null; // 等待下一帧
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
            shakeCount++;
            if (shakeCount >= 3)
            {
                ResetMealState();
            }
        }

    }
    private void SuccessfulGrab()
    {
        fullnessValue += spoonValue;
        fullnessSlide.value = fullnessValue;
        ResetMealState();
        ShakeCamera();
        currentEat ++;
        Debug.Log("Success");
        
    }
    private void ResetMealState()
    {
        isInFoodZone = false;
        spoonValue = 0;
        spoonCursor.sprite = spoonCondition[spoonValue]; // 重置为空勺子
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
                mainCamera.transform.DOPunchPosition(new Vector3(0, -0.75f, 0), 0.4f, 4, 0);
            });
        });
    }
}

