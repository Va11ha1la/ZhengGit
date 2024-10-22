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
    public RectTransform canvasRectTransform; // Canvas的RectTransform

    private Vector3 initialHandPosition;
    private Vector3 targetHandPosition; // 手臂的目标位置（点击格子处）

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

        // 使用 RectTransformUtility 将按钮的 UI 坐标转换为世界坐标，这个知识点之后复习一下（
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
                AnimateHandRetract(); // 手缩
            });

      
        InvokeRepeating(nameof(ShakeHand), 0, 0.5f); // 每隔0.5秒抖动一次，可调
    }

    private void ShakeHand()
    {
       
        handSprite.transform.DOPunchPosition(
            punch: new Vector3(0.1f, 0.1f, 0), // 抖动的方向和强度，下面都可以调
            duration: 0.1f,                   // 抖动的持续时间
            vibrato: 1,                       // 抖动的频率
            elasticity: 0.5f                   // 弹性效果
        );
    }


   

    private void AnimateHandRetract()
    {
        //// 停止定时抖动
        //CancelInvoke(nameof(ShakeHand));
        // 使用 DOTween 缩回手臂到初始位置
        handSprite.transform.DOMove(initialHandPosition, 2.0f).OnComplete(() =>
        {
            handSprite.SetActive(false);
            blackoutPanel.transform.SetSiblingIndex(1);
            BlackoutTransition(); // 黑场过渡
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
        return true; // 示例中暂时允许所有点击
    }

    private void ShowHintMessage()
    {
        hintMessage.gameObject.SetActive(true);
        // 2秒后自动隐藏提示信息
        DOVirtual.DelayedCall(2f, () => hintMessage.gameObject.SetActive(false));
    }
}
