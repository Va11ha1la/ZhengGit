using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndButton : MonoBehaviour
{
    public static bool[] aa;
    public GameObject button;
    public CheckGameSituation checkGameSituation;
    private void Start()
    {
        aa = new bool[] { false, false };
    }

    public void OpenObj(GameObject obj)
    {
        obj.SetActive(true);
    }
        private bool y = false;
    private void Update()
    {
        if (aa[0] && aa[1]&&!y)
        {
            y=true;
            checkGameSituation.isStarted = false;
            button.SetActive(true);
            DOTween.To(() => button.GetComponent<CanvasGroup>().alpha,
                x => button.GetComponent<CanvasGroup>().alpha = x, 1, 0.7f).SetEase(Ease.OutBounce);
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    DayCheck dayCheck = new DayCheck();
                    dayCheck.ClickCheck = 0;
                    dayCheck.DayCount = 0;
                    dayCheck.BtnIsClick = new bool[] { false, false, false };

                    string jsonStr = JsonUtility.ToJson(dayCheck);
                    File.WriteAllText(Application.persistentDataPath + "/DayData.json", jsonStr);
                    
                    
                    SceneManager.LoadScene("StartScene");
                });
        }
    }
}
