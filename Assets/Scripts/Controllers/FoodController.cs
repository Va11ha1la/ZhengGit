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
    public Sprite[] spoonCondition; //����״̬
    public Sprite[] Day6spoonCondition; //����״̬
    public SpriteRenderer Bowl;
    public Sprite[] FoodCondition;//����״̬
    public Sprite[] Day6FoodCondition;
    public Transform foodZone;
    public Transform foodZoneDay6;
    public Transform EatZone;

    [Header("�����ж�")]
    public CheckHungry checkHungry;
    public TMP_Text TipText;


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

    public GameObject TransAnim;

    private int shakeCount = 0; // �������ε���ɹ���ʧ��
    private bool isShaking = false;
    private string shakeDirection; // ��������"left" �� "right"��
    
    private bool hasPressedCorrectKey = false;
    private Coroutine shakeCoroutine; // ���ƶ�����Э��
    //public Image blackoutPanel;


    public Camera mainCamera;

    public GameObject transitionAnimator; // ת����Animator ����
    public GameObject[] TransObjectToBeClosed;//�����ڱΣ���Ҫ�ص����������
    public bool canTransition;

  
    public GameObject EatTablemage;

    private DayCheck dayCheck;
    private string jsonFilePath;
    int date;

    [Header("�����ñ���")]
    public float[] randomLeft;
    public float[] randomRight;
    //private float shakeReactionTime = 0.5f; // ��Ұ�����Ӧʱ��
    //public float sensitivity = 0.5f;//������ж�
    public float sensitivity;
    public float[] sensitivity1;
    public float[] shakeReactionTime ;
    public float[] duration;
    public int[] vibrato;
    public float[] elasticity;

  

    [Header("������λ��")]
    public GameObject[] Drops;
    public Vector3[] Drops1;//�洢�����������λ��
    public Vector3[] Drops2;//�洢�����������λ��
    public Vector3[] Drops3;//�洢�����������λ��
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
        previousMousePosition = Input.mousePosition; // ��ʼ�����λ��
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
                        TipText.text = "...��...��...";
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
                                TipText.text = "...�о�������";
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
                    // ����ֵ >= 2 ʱ���� HungryDay
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
        
        // ��ȡ��ǰ���λ�ã���Ļ���꣩
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 delta = currentMousePosition - previousMousePosition;
        previousMousePosition = currentMousePosition;
        
        Vector3 adjustedDelta = delta * sensitivity;

        // ����Ļ����ת��ΪCanvas��RectTransform����
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
                    spoonCursor.sprite = spoonCondition[spoonValue]; // ����Ϊʢ���˵�����
                    if (date == 5)
                    {
                        spoonCursor.sprite = Day6spoonCondition[spoonValue];
                    }
                    isHoldFoodFlag = true;

                    CheckFood();
                }
            }

        }
        // ������Ƿ���ʳ������
        else
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(foodZone.GetComponent<RectTransform>(), spoonCursor.transform.position, canvas.worldCamera) && !isHoldFoodFlag)
            {
                if (!isInFoodZone)
                {
                    isInFoodZone = true;
                    isInEatZone = false;
                    spoonValue = 2;
                    spoonCursor.sprite = spoonCondition[spoonValue]; // ����Ϊʢ���˵�����
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
        // ������Ƿ��ڳԷ�����
        if (RectTransformUtility.RectangleContainsScreenPoint(EatZone.GetComponent<RectTransform>(), spoonCursor.transform.position, canvas.worldCamera) && isHoldFoodFlag)
        {
            if (!isInEatZone)
            {
                isInEatZone = true;
                isInFoodZone = false;
                isHoldFoodFlag = false;
                SuccessfulGrab();
                spoonValue = 0;
                spoonCursor.sprite = spoonCondition[spoonValue]; // ����Ϊ��ʢ���˵�����
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

    // ��ʼ��������
    private void StartShaking()
    {
        if (!isShaking)
        {
            isShaking = true;
            sensitivity = sensitivity1[date];
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
                sensitivity = sensitivity1[date]+0.5f;
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
            spoonCursor.transform.DOPunchPosition(new Vector3(shakeDirection == "left" ? -50f : 50f, 0, 0), duration[date], vibrato[date], elasticity[date]).OnComplete(() =>
            {
                // �������
                spoonCursor.transform.DOPunchPosition(new Vector3(shakeDirection == "left" ? -100f : 100f, 0, 0), duration[date]+0.1f, 1, elasticity[date]).OnComplete(() =>
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

        while (Time.time < startTime + shakeReactionTime[date])
        {
            // ��ⰴ��������Ҫ�ж��Ƿ�����ȴʱ����
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


            // ��������״̬����һЩ���ˣ�
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
        spoonCursor.sprite = spoonCondition[spoonValue]; // ����Ϊ������
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

