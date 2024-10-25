using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Kaishi_Anim : MonoBehaviour
{
    public GameObject button;

    public void ButtonOn()
    {
        DOTween.To(()=>button.GetComponent<CanvasGroup>().alpha,x=>button.GetComponent<CanvasGroup>().alpha=x,1,0.7f).SetEase(Ease.OutBounce).onComplete+=()=>button.GetComponent<Button>().interactable=true;
    }

}
