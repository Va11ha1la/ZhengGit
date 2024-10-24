using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;

public class CalendarController : MonoBehaviour
{
    public GameObject handSprite;
    public Sprite[] circleSprite; 
    public Image blackoutPanel; 
    public GameObject hintMessage; 
    public RectTransform canvasRectTransform; // Canvas��RectTransform

    private Vector3 initialHandPosition;
    private Vector3 targetHandPosition; // �ֱ۵�Ŀ��λ�ã�������Ӵ���

    public GameObject[] DateSlots;

    private DayCheck dayCheck;
    private string jsonFilePath;

    public List<Sprite> wallImages;
    public GameObject wallImage;

    public GameObject transitionAnimator; // ת����Animator ����
    int date;

    //��������������
    [Header("����������")]
    public Vector3[] punchV;
    public  float[] duration;
    public int[] vibrato;
    public  float[] elasticity;
    public float[] pinlu;//ÿ�������붶һ��
    public float[] MoveSpeed ;//���ƶ�ʱ�䣨�ٶȷ��ȣ�
    





    void Start()
    {

        transitionAnimator.gameObject.SetActive(false);

        jsonFilePath = Path.Combine(Application.persistentDataPath, "DayData.json");
        string jsonData = File.ReadAllText(jsonFilePath);
        dayCheck = JsonUtility.FromJson<DayCheck>(jsonData);
        date = dayCheck.DayCount;
        int p = dayCheck.DayCount * 3 + dayCheck.ClickCheck;
        wallImage.GetComponent<SpriteRenderer>().sprite = wallImages[p];

        if (date > 0)
        {
            for (int i = 1; i < dayCheck.DayCount+1; i++)
            {
                DateSlots[i].GetComponent<Image>().sprite = circleSprite[i-1];
            }
        }
        handSprite.SetActive(false);
        //blackoutPanel.transform.SetSiblingIndex(0);
        //blackoutPanel.color = new Color(255, 255, 255, 0);
        hintMessage.gameObject.SetActive(false);
        initialHandPosition = handSprite.transform.position; 
    }

    public void OnDateButtonClicked(Button clickedButton) 
    {
        RectTransform clickedButtonRectTransform = clickedButton.GetComponent<RectTransform>();

        // ʹ�� RectTransformUtility ����ť�� UI ����ת��Ϊ�������꣬���֪ʶ��֮��ϰһ�£�
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvasRectTransform,
            clickedButtonRectTransform.position,
            Camera.main,
            out targetHandPosition
        );

        targetHandPosition.z = 0; 

        if (CanClickDate(clickedButton))
        {
            handSprite.SetActive(true);
            AnimateHandMovement(clickedButton);
        }
        else
        {
            ShowHintMessage();
        }
    }

    //private void AnimateHandMovement(Button clickedButton)
    //{

    //    handSprite.SetActive(true); 


    //    handSprite.transform.DOMove(targetHandPosition, MoveSpeed[date])

    //        .OnComplete(() =>
    //        {
    //            DrawCircleOnCalendar(clickedButton); 
    //            AnimateHandRetract(); // ����
    //        });


    //    InvokeRepeating(nameof(ShakeHand), 0, pinlu[dayCheck.DayCount]); // ÿ��0.5�붶��һ�Σ��ɵ�
    //}

    //private void ShakeHand()
    //{

    //    handSprite.transform.DOPunchPosition(
    //        punch: punchV[date], // �����ķ����ǿ�ȣ����涼���Ե�
    //        duration: duration[date],                   // �����ĳ���ʱ��
    //        vibrato: vibrato[date],                       // ������Ƶ��
    //        elasticity: elasticity[date]                // ����Ч��
    //    );
    //}
    private void AnimateHandMovement(Button clickedButton)
    {
        handSprite.SetActive(true);

        // ʹ�� DOTween �� OnUpdate �����ƶ������д�������
        handSprite.transform.DOMove(targetHandPosition, MoveSpeed[date])
            .SetEase(Ease.Linear) // �����ƶ�
            .OnUpdate(() =>
            {
                // �����ֵ�λ�ã�ͬʱ�ƶ���
                ShakeHand();
            })
            .OnComplete(() =>
            {
                DrawCircleOnCalendar(clickedButton);
                AnimateHandRetract(); // �����ض���
            });
    }

    private void ShakeHand()
    {
        // �ֲ�����Ч����ʹ�� DOShakePosition ʵ�ֳ�������
        handSprite.transform.DOShakePosition(
            duration: duration[date],    // �����ĳ���ʱ��
            strength: punchV[date],      // ������ǿ��
            vibrato: vibrato[date],      // ������Ƶ��
            randomness: 90,              // �����������
            snapping: false,             // �Ƿ�����������λ��
            fadeOut: true                // ��������ʱ�Ƿ��𽥼���
        );
    }




    private void AnimateHandRetract()
    {
      

        // ʹ�� DOTween �����ֱ۵���ʼλ��
        handSprite.transform.DOMove(initialHandPosition, 2.0f).OnComplete(() =>
        {
            handSprite.SetActive(false);
           // blackoutPanel.gameObject.SetActive(true);
           //blackoutPanel.transform.SetAsLastSibling();
            BlackoutTransition(); // �ڳ�����
        });
    }

    private void DrawCircleOnCalendar(Button clickedButton)
    {
        int date = dayCheck.DayCount;
        clickedButton.GetComponent<Image>().sprite = circleSprite[date]; 
    }

    private void BlackoutTransition()
    {
        transitionAnimator.SetActive(true);
        for(int i = 0; i < DateSlots.Length; i++)
        {
            DateSlots[i].gameObject.SetActive(false);
        }
        
        transitionAnimator.GetComponent<Animator>().SetTrigger("StartTrans");

        StartCoroutine(LoadSceneAfterAnimation());
    }
    //�ȴ�����������ɺ�����³���
    private IEnumerator LoadSceneAfterAnimation()
    {
        
        yield return new WaitForSeconds(2.9f);

       
        SceneManager.LoadScene("StartScene");
    }
    private bool CanClickDate(Button clickedButton)
    {
       

        if (clickedButton.name != DateSlots[date+1].GetComponent<Button>(). name)
        {
            return false;
        }
        return true;
    }

    private void ShowHintMessage()
    {
        hintMessage.gameObject.SetActive(true);
        // 2����Զ�������ʾ��Ϣ
        DOVirtual.DelayedCall(2f, () => hintMessage.gameObject.SetActive(false));
    }
}
