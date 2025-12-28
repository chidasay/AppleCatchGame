using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearDirector : MonoBehaviour
{
    GameObject pointText;

    // Start is called before the first frame update
    void Start()
    {
        pointText = GameObject.Find("Point");
    }

    // Update is called once per frame
    void Update()
    {
        // “¾“_•\Ž¦
        this.pointText.GetComponent<TextMeshProUGUI>().text = PStatic.point.ToString() + " point";

        // ‰æ–Ê‘JˆÚ
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}