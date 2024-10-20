using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    public bool StartGameFlag;

   
    
    private void Awake()
    {
        Instance = this;
    }

    public float moveSpeed = 0.5f;
    public float maxOffset = 2f;

    public Vector3 initialPosition;
    public Vector3[] BtnPositions;

    private void Start()
    {
        
     
    }

    private void Update()
    {
        if (StartGameFlag)
        {
            Vector3 mousePosition = Input.mousePosition;

            float xOffset = Mathf.Clamp((mousePosition.x / Screen.width - 0.5f) * moveSpeed, -maxOffset, maxOffset);
            float yOffset = Mathf.Clamp((mousePosition.y / Screen.height - 0.5f) * moveSpeed, -maxOffset, maxOffset);
            transform.position = new Vector3(initialPosition.x + xOffset, initialPosition.y + yOffset, initialPosition.z);

            

        }
    }



}
