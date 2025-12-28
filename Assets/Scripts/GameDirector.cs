using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    GameObject timerText;
    GameObject pointText;

    float time = 100.0f;
    int point = 0;

    // Start is called before the first frame update
    void Start()
    {
        timerText = GameObject.Find("Time");
        pointText = GameObject.Find("Point");
    }

    // Update is called once per frame
    void Update()
    {
        // éûä‘ä«óù
        this.time -= Time.deltaTime;
        this.timerText.GetComponent<TextMeshProUGUI>().text = this.time.ToString("F1");

        // ìæì_ä«óù
        this.pointText.GetComponent<TextMeshProUGUI>().text = this.point.ToString() + " point";

        // ÉNÉäÉAâÊñ 
        if(this.time < 0)
        {
            PStatic.point = this.point;
            FindObjectOfType<AppleGenerator>().AppleEverSumReset();
            SceneManager.LoadScene("ClearScene");
        }
    }

    public void GetApple()
    {
        this.point += 100;
    }
}