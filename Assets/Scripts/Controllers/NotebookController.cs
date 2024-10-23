using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotebookController : MonoBehaviour
{
    public GameObject notebook;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnNotebookClick); 
    }

    private void OnNotebookClick()
    {
        notebook.SetActive(true);
        GetComponent<Button>().interactable = false;
    }
}
