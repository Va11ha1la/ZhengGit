using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FoodController : MonoBehaviour
{
    public Image spoonCursor;
    public Sprite[] spoonCondition;//����״̬
    public Transform foodZone;
    public Transform EatZone;
    public float sensitivity = 0.5f;//������ж�
    private Vector3 previousMousePosition;
    public Canvas canvas;
    public bool isHoldFoodFlag;
    //����ֵ
    public int fullnessValue;
    public int spoonValue;//���������еı�����
    public int maxEat;
    public int currentEat;
    private bool isInFoodZone = false;
    private bool isInEatZone = false;
    public Slider fullnessSlide;
    

    private int shakeCount = 0; // �������ε���ɹ���ʧ��
    private bool isShaking = false;
    private string shakeDirection; // ��������"left" �� "right"��
    private float shakeReactionTime = 0.5f; // ��Ұ�����Ӧʱ��
    private bool hasPressedCorrectKey = false;
    private Coroutine shakeCoroutine; // ���ƶ�����Э��

    public Camera mainCamera;


    private void Start()
    {
        
        maxEat = 4;
        
        fullnessSlide.maxValue = 12;
        Cursor.visible = false;
        spoonCursor.transform.position = new Vector3(Screen.width - 100, 100, 0);
        spoonCursor.sprite = spoonCondition[0];
        previousMousePosition = Input.mousePosition; // ��ʼ�����λ��
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
        
        // ��ȡ��ǰ���λ�ã���Ļ���꣩
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 delta = currentMousePosition - previousMousePosition;
        previousMousePosition = currentMousePosition; 

        Vector3 adjustedDelta = delta * sensitivity;

        // ����Ļ����ת��ΪCanvas��RectTransform����
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
        // ������Ƿ���ʳ������
        if (RectTransformUtility.RectangleContainsScreenPoint(foodZone.GetComponent<RectTransform>(), spoonCursor.transform.position, canvas.worldCamera) && !isHoldFoodFlag)
        {
            if (!isInFoodZone)
            {
                isInFoodZone = true;
                isInEatZone = false;
                spoonValue = 3;
                spoonCursor.sprite = spoonCondition[spoonValue]; // ����Ϊʢ���˵�����
                isHoldFoodFlag = true;
            }
        }
      
    }
    private void CheckEatZone()
    {
        // ������Ƿ��ڳԷ�����
        if (RectTransformUtility.RectangleContainsScreenPoint(EatZone.GetComponent<RectTransform>(), spoonCursor.transform.position, canvas.worldCamera) && isHoldFoodFlag)
        {
            if (!isInEatZone)
            {
                isInEatZone = true;
                isInFoodZone = false;
                
                spoonCursor.sprite = spoonCondition[spoonValue]; // ����Ϊʢ���˵�����
                isHoldFoodFlag = false;
                SuccessfulGrab();
                spoonValue = 0;
            }
        }

    }
    
    // ��ʼ��������
    private void StartShaking()
    {
        if (!isShaking)
        {
            isShaking = true;
            sensitivity = 1f;
            shakeCoroutine = StartCoroutine(RandomShakeRoutine());
        }
    }

    // ֹͣ��������
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
            // ����ȴ�һ��ʱ��
            float randomTime = Random.Range(2f, 5f);
            yield return new WaitForSeconds(randomTime);

            if (!isInFoodZone) yield break; // �������Ѿ��뿪ʳ������ֹͣ����

            // ���������������
            shakeDirection = Random.Range(0, 2) == 0 ? "left" : "right";

            // ��΢��������ǰ��ʾ��
            spoonCursor.transform.DOPunchPosition(new Vector3(shakeDirection == "left" ? -50f : 50f, 0, 0), 0.4f, 4, 0).OnComplete(() =>
            {
                // �������
                spoonCursor.transform.DOPunchPosition(new Vector3(shakeDirection == "left" ? -100f : 100f, 0, 0), 0.5f, 1, 0).OnComplete(() =>
                {
                    // �ȴ��������
                    StartCoroutine(WaitForPlayerInput());
                });
            });
        }
    }
    // �ȴ���������Э��
    private IEnumerator WaitForPlayerInput()
    {
        float startTime = Time.time;
        hasPressedCorrectKey = false;
        float cooldownTime = 0.5f; 
        float lastPressTime = -cooldownTime; 

        while (Time.time < startTime + shakeReactionTime)
        {
            // ��ⰴ��������Ҫ�ж��Ƿ�����ȴʱ����
            if ((shakeDirection == "left" && Input.GetKeyDown(KeyCode.LeftArrow) && Time.time >= lastPressTime + cooldownTime) ||
                (shakeDirection == "right" && Input.GetKeyDown(KeyCode.RightArrow) && Time.time >= lastPressTime + cooldownTime))
            {
                hasPressedCorrectKey = true;
                lastPressTime = Time.time; // ��������µ�ʱ��
               
                yield break; // �ɹ����˳�Э��
            }

            yield return null; // �ȴ���һ֡
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


            // ��������״̬����һЩ���ˣ�
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
        spoonCursor.sprite = spoonCondition[spoonValue]; // ����Ϊ������
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

