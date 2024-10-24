using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.EventSystems;
using UnityEngine.UI; // 用于事件系统

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image hoverFrame;
    private void Start()
    {
       hoverFrame.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverFrame.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverFrame.gameObject.SetActive(false);
    }
}