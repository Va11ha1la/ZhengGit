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
    public RectTransform canvasRectTransform; // Canvas的RectTransform

    private Vector3 initialHandPosition;
    private Vector3 targetHandPosition; // 手臂的目标位置（点击格子处）

    public GameObject[] DateSlots;

    private DayCheck dayCheck;
    private string jsonFilePath;

    public List<Sprite> wallImages;
    public GameObject wallImage;

    public GameObject transitionAnimator; // 转场用Animator 物体
    int date;

    //调整抖动参数用
    [Header("抖动调参用")]
    public Vector3[] punchV;
    public  float[] duration;
    public int[] vibrato;
    public  float[] elasticity;
    public float[] pinlu;//每隔多少秒抖一次
    public float[] MoveSpeed ;//手移动时间（速度反比）
    





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

    //private void AnimateHandMovement(Button clickedButton)
    //{

    //    handSprite.SetActive(true); 


    //    handSprite.transform.DOMove(targetHandPosition, MoveSpeed[date])

    //        .OnComplete(() =>
    //        {
    //            DrawCircleOnCalendar(clickedButton); 
    //            AnimateHandRetract(); // 手缩
    //        });


    //    InvokeRepeating(nameof(ShakeHand), 0, pinlu[dayCheck.DayCount]); // 每隔0.5秒抖动一次，可调
    //}

    //private void ShakeHand()
    //{

    //    handSprite.transform.DOPunchPosition(
    //        punch: punchV[date], // 抖动的方向和强度，下面都可以调
    //        duration: duration[date],                   // 抖动的持续时间
    //        vibrato: vibrato[date],                       // 抖动的频率
    //        elasticity: elasticity[date]                // 弹性效果
    //    );
    //}
    private void AnimateHandMovement(Button clickedButton)
    {
        handSprite.SetActive(true);

        // 使用 DOTween 的 OnUpdate 来在移动过程中触发抖动
        handSprite.transform.DOMove(targetHandPosition, MoveSpeed[date])
            .SetEase(Ease.Linear) // 线性移动
            .OnUpdate(() =>
            {
                // 抖动手的位置，同时移动手
                ShakeHand();
            })
            .OnComplete(() =>
            {
                DrawCircleOnCalendar(clickedButton);
                AnimateHandRetract(); // 手缩回动画
            });
    }

    private void ShakeHand()
    {
        // 手部抖动效果，使用 DOShakePosition 实现持续抖动
        handSprite.transform.DOShakePosition(
            duration: duration[date],    // 抖动的持续时间
            strength: punchV[date],      // 抖动的强度
            vibrato: vibrato[date],      // 抖动的频率
            randomness: 90,              // 抖动的随机性
            snapping: false,             // 是否吸附到整数位置
            fadeOut: true                // 抖动结束时是否逐渐减弱
        );
    }




    private void AnimateHandRetract()
    {
      

        // 使用 DOTween 缩回手臂到初始位置
        handSprite.transform.DOMove(initialHandPosition, 2.0f).OnComplete(() =>
        {
            handSprite.SetActive(false);
           // blackoutPanel.gameObject.SetActive(true);
           //blackoutPanel.transform.SetAsLastSibling();
            BlackoutTransition(); // 黑场过渡
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
    //等待动画播放完成后加载新场景
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
        // 2秒后自动隐藏提示信息
        DOVirtual.DelayedCall(2f, () => hintMessage.gameObject.SetActive(false));
    }
}
