using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleController : MonoBehaviour
{
    private int position;
    public float appearTime = 20.0f; // —ÑŒç‚ª©“®‚ÅÁ‚¦‚é‚Ü‚Å‚ÌŠÔiŒ»‚ê‚Ä‚¢‚éŠÔj

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", appearTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ŠÔ‚ªŒo‚Â‚Æ©“®‚ÅÁ‚¦‚é
    void Destroy()
    {
        Destroy(gameObject);
        FindObjectOfType<AppleGenerator>().SetArrayApplePutFalse(this.position);
    }

    // setPosition
    public void setPosition(int position)
    {
        this.position = position;
    }

    // getPosition
    public int getPosition()
    {
        return this.position;
    }
}