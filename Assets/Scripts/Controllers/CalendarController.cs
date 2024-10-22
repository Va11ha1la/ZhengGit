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
    public Sprite circleSprite; 
    public Image blackoutPanel; 
    public GameObject hintMessage; 
    public RectTransform canvasRectTransform; // Canvas��RectTransform

    private Vector3 initialHandPosition;
    private Vector3 targetHandPosition; // �ֱ۵�Ŀ��λ�ã�������Ӵ���

    public GameObject[] DateSlots;

    private DayCheck dayCheck;
    private string jsonFilePath;
    int date;


    void Start()
    {
        jsonFilePath = Path.Combine(Application.persistentDataPath, "DayData.json");
        string jsonData = File.ReadAllText(jsonFilePath);
        dayCheck = JsonUtility.FromJson<DayCheck>(jsonData);
        date = dayCheck.DayCount;
        if (date > 0)
        {
            for (int i = 1; i < dayCheck.DayCount+1; i++)
            {
                DateSlots[i].GetComponent<Image>().sprite = circleSprite;
            }
        }

        blackoutPanel.transform.SetSiblingIndex(0);
        handSprite.SetActive(false);
        blackoutPanel.color = new Color(255, 255, 255, 0);
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

    private void AnimateHandMovement(Button clickedButton)
    {

        handSprite.SetActive(true); 

   
        handSprite.transform.DOMove(targetHandPosition, 2.0f)
           
            .OnComplete(() =>
            {
                DrawCircleOnCalendar(clickedButton); 
                AnimateHandRetract(); // ����
            });

      
        InvokeRepeating(nameof(ShakeHand), 0, 0.5f); // ÿ��0.5�붶��һ�Σ��ɵ�
    }

    private void ShakeHand()
    {
       
        handSprite.transform.DOPunchPosition(
            punch: new Vector3(0.1f, 0.1f, 0), // �����ķ����ǿ�ȣ����涼���Ե�
            duration: 0.1f,                   // �����ĳ���ʱ��
            vibrato: 1,                       // ������Ƶ��
            elasticity: 0.5f                   // ����Ч��
        );
    }


   

    private void AnimateHandRetract()
    {
        //// ֹͣ��ʱ����
        //CancelInvoke(nameof(ShakeHand));
        // ʹ�� DOTween �����ֱ۵���ʼλ��
        handSprite.transform.DOMove(initialHandPosition, 2.0f).OnComplete(() =>
        {
            handSprite.SetActive(false);
            blackoutPanel.transform.SetSiblingIndex(1);
            BlackoutTransition(); // �ڳ�����
        });
    }

    private void DrawCircleOnCalendar(Button clickedButton)
    {
        clickedButton.GetComponent<Image>().sprite = circleSprite; 
    }

    private void BlackoutTransition()
    {
        blackoutPanel.DOFade(1.0f, 1.0f).OnComplete(() =>
        {
            SceneManager.LoadScene("StartScene");
        });
    }

    private bool CanClickDate(Button clickedButton)
    {
       

        if (clickedButton.name != DateSlots[date+1].GetComponent<Button>(). name)
        {
            return false;
        }
        return true; // ʾ������ʱ�������е��
    }

    private void ShowHintMessage()
    {
        hintMessage.gameObject.SetActive(true);
        // 2����Զ�������ʾ��Ϣ
        DOVirtual.DelayedCall(2f, () => hintMessage.gameObject.SetActive(false));
    }
}
