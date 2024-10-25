using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitFor1s : MonoBehaviour
{
    private float time;
    private int count = 1;
    public int index;
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
        EndButton.aa[index] = true;
        GetComponent<Button>().onClick.RemoveAllListeners();
    }
}
