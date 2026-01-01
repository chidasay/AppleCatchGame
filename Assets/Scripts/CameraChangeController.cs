using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraNow
{
    MainCamera,
    SubCamera
}

public class CameraChangeController : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject subCamera;

    private CameraNow now = CameraNow.MainCamera;
    
    void Start()
    {
        subCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if(now == CameraNow.MainCamera)
            {
                mainCamera.SetActive(false);
                subCamera.SetActive(true);
                now = CameraNow.SubCamera;
            } else {
                mainCamera.SetActive(true);
                subCamera.SetActive(false);
                now = CameraNow.MainCamera;
            }
        }
    }
}
