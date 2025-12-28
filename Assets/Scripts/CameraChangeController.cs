using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChangeController : MonoBehaviour
{
    private GameObject mainCamera;
    private GameObject subCamera;

    static private int cameraNow = 0; // 0でメイン、1でサブ


    // Start is called before the first frame update
    void Start()
    {
        // メインカメラとサブカメラを取得
        mainCamera = GameObject.Find("MainCamera");
        subCamera = GameObject.Find("SubCamera");

        // サブカメラを非アクティブ
        subCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 右クリックされたらもう一方のカメラをアクティブにする
        if (Input.GetMouseButtonDown(1))
        {
            if(cameraNow == 0)
            {
                mainCamera.SetActive(false);
                subCamera.SetActive(true);
                cameraNow = 1;
            } else if(cameraNow == 1)
            {
                mainCamera.SetActive(true);
                subCamera.SetActive(false);
                cameraNow = 0;
            }
        }
    }
}
