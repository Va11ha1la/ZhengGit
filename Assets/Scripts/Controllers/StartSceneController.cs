using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{
    public static StartSceneController Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public Transform cameraTransform;
    public Button startBtn;
    public CheckGameSituation checkGameSituation;

    public Button[] Btns;
 

    private Vector3 cameraTargetPosition = new Vector3(0, -7, -10); // 相机的目标位置展示墙的下半部分
    private float animationSpeed = 3f;
    

    void Start()
    {
        if(checkGameSituation.isStarted == true)
        {
            cameraTransform.position = cameraTargetPosition;

            CameraController.Instance.initialPosition = cameraTransform.position;
            CameraController.Instance.StartGameFlag = true;
            startBtn.gameObject.SetActive(false);
            for (int i = 0; i < Btns.Length; i++)
            {
                Btns[i].gameObject.SetActive(true);
            }
        }
        if(checkGameSituation.isStarted == false) {startBtn.onClick.AddListener(StartGame);
        for(int i = 0; i < Btns.Length; i++)
            {
                Btns[i].gameObject.SetActive(false);
            }
        }
        
    }

    void StartGame()
    {
        DataManager.Instance.InitGameData();
        StartCoroutine(MoveCameraToShowLevelSelect());
    }

    IEnumerator MoveCameraToShowLevelSelect()
    {
        startBtn.gameObject.SetActive(false);
        //while (Vector3.Distance(cameraTransform.position, cameraTargetPosition) > 0.01f)
        //{

        //    cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTargetPosition, animationSpeed * Time.deltaTime);
        //    yield return null;
        //}
        while (Vector3.Distance(cameraTargetPosition,cameraTransform.position)> 0.1f)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTargetPosition, animationSpeed * Time.deltaTime);
            yield return null;
        }


        cameraTransform.position = cameraTargetPosition;

        CameraController.Instance.initialPosition = cameraTransform.position;
        CameraController.Instance.StartGameFlag = true;
        checkGameSituation.isStarted = true;
        for (int i = 0; i < Btns.Length; i++)
        {
            Btns[i].gameObject.SetActive(true);
         
        }
    }
}

