using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIButtonEvents : MonoBehaviour
{
    public GameObject button;
    private void Start()
    {
        transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => button.SetActive(true));
    }

    public GameObject setting;
    public void SetActive(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void SetInactive(GameObject obj)
    {
        obj.SetActive(false);
    }
    public void Replay()
    {
        
    }

    public void Setting()
    {
        setting.SetActive(true);
    }

    public void Contents()
    {
        
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
