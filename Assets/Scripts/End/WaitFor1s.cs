using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitFor1s : MonoBehaviour
{
    private float time;
    private int count = 1;
    void Update()
    {
        time += Time.deltaTime*count;
        if (time >= 1)
        {
            GetComponent<Button>().interactable = true;
            GetComponent<Button>().onClick.AddListener(Close);
            count = 0;
        }
    }

    private void Close()
    {
        GetComponent<Button>().interactable = false;
        gameObject.SetActive(false);
        if (EndButton.aa[0])
        {
            EndButton.aa[1] = true;
        }
        else
        {
            EndButton.aa[0] = true;
        }
        GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
