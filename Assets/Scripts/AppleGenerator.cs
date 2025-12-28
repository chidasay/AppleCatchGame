using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleGenerator : MonoBehaviour
{
    public GameObject applePrefab;
    public float span = 5.0f;
    public int appleStartNum = 3;
    private float delta = 0;

    static private int positionNum = 12;
    static private int appleEverSum = 0;

    // ランダムな位置の配列
    private Vector3[] arrayPosition = new Vector3[positionNum];
    // ランダムな位置にappleが既にあるかを保持する配列
    private  bool[] arrayApplePut = new bool[positionNum];

    // Start is called before the first frame update
    void Start()
    {
        // ランダムな位置の場所の初期化
        arrayPosition[0] = new Vector3(0f, 1f, 3f);
        arrayPosition[1] = new Vector3(43f, 2f, -6f);
        arrayPosition[2] = new Vector3(30f, 1f, 8f);
        arrayPosition[3] = new Vector3(30f, 3f, -35f);
        arrayPosition[4] = new Vector3(-5f, 9.5f, -22.7f);
        arrayPosition[5] = new Vector3(27.35f, 4.5f, -9.5f);
        arrayPosition[6] = new Vector3(-1.5f, 1f, 22f);
        arrayPosition[7] = new Vector3(14.35f, 6f, -42f);
        arrayPosition[8] = new Vector3(17.35f, 9f, -16f);
        arrayPosition[9] = new Vector3(-14.7f, 13f, 13.75f);
        arrayPosition[10] = new Vector3(41.5f, 7f, -39f);
        arrayPosition[11] = new Vector3(-7f, 1f, -40f);

        for(int i = 0; i < appleStartNum; i++) AppleAppear();

    }

    // Update is called once per frame
    void Update()
    {
        this.delta += Time.deltaTime;
        if(this.delta >= this.span)
        {
            this.delta = 0;
            AppleAppear();
        }

        // もし林檎の数が3つ未満なら、時間に関係なく3つにする
        // フリーズして怖いので一旦コメントアウト
        // int nowApple = 0;
        // for(int i = 0; i < positionNum; i++) if (arrayApplePut[i]) nowApple++;
        // while (nowApple < 3) AppleAppear();
    }

    // 破壊されたappleインスタンスに場所のboolをfalseにする
    public void SetArrayApplePutFalse(int n)
    {
        arrayApplePut[n] = false;
    }

    // SumReset
    public void AppleEverSumReset()
    {
        appleEverSum = 0;
    }

    // appleを出現させる
    private void AppleAppear()
    {
        GameObject apple = Instantiate(applePrefab);
        int dice;
        // まだ置かれていない場所からランダムに選ぶ
        // ただし、一番初めは必ずキャラクタの目の前に設置する
        if (appleEverSum == 0)
        {
            dice = 0;
        } else
        {
            dice = Random.Range(0, positionNum);
            while (arrayApplePut[dice]) dice = Random.Range(0, positionNum);
        }
        apple.transform.position = arrayPosition[dice];
        apple.GetComponent<AppleController>().setPosition(dice);
        arrayApplePut[dice] = true;
        appleEverSum++;
    }
}