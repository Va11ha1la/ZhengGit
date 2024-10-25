using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Kaishi_Anim : MonoBehaviour
{
    public GameObject[] button;
    public GameObject ui;
    public void ButtonOn()
    {
        ui.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            int a = i;
            button[i].SetActive(true);
            DOTween.To(()=>button[a].GetComponent<CanvasGroup>().alpha,x=>button[a].GetComponent<CanvasGroup>().alpha=x,1,0.7f).SetEase(Ease.OutBounce).onComplete+=()=>button[a].GetComponent<Button>().interactable=true;
        }
        button[3].SetActive(true);
        DOTween.To(() => button[3].GetComponent<CanvasGroup>().alpha,
            x => button[3].GetComponent<CanvasGroup>().alpha = x, 1, 0.7f).SetEase(Ease.OutBounce);
    }
}
