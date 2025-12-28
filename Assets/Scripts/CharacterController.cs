using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    public float speed = 8.0f;        // 走る速さ
    public float jumpForce1 = 8.5f;   // ジャンプ力1
    public float jumpForce2 = 3.5f;   // ジャンプ力2
    const int numMaxJump = 2;         // ジャンプできる最大回数
    private int numJump = 0;          // ジャンプ回数の箱

    Rigidbody rb;
    Collider col;
    Animator animator;
    GameObject director;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        this.director = GameObject.Find("GameDirector");
    }

    // Update is called once per frame
    void Update()
    {
        // 走る
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(horizontalInput * speed, rb.velocity.y, verticalInput * speed);

        // ジャンプ（二段ジャンプ可能）
        if(Input.GetButtonDown("Jump") && numJump < numMaxJump)
        {
            if (numJump == 0) // 一段階目ジャンプ
            {
                animator.SetBool("Jump", true);
                rb.AddForce(Vector3.up * jumpForce1, ForceMode.Impulse);
            } else if(numJump == 1) // 二段階目ジャンプ
            {
                animator.SetBool("Jump2", true);
                rb.AddForce(Vector3.up * jumpForce2, ForceMode.Impulse);
            }
            numJump++;
        }

        // ジャンプと建物におけるバグ修正用
        // AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        // bool isJumping = state.IsName("Jump");
        // if (!state.IsName("Jump") && !state.IsName("Jump2") && rb.velocity.y == 0f)
        // {
        //     numJump = 0;
        // }

        // 走る方向に体の向きを変える
        if (Mathf.Abs(rb.velocity.x) > 0.1f || Mathf.Abs(rb.velocity.z) > 0.1f)
        {
            // 向かせたい方向
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0, verticalInput));
            // 回す
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // アニメーション制御
        if(verticalInput != 0 || horizontalInput != 0)
        {
            animator.SetBool("Run", true);
        }
        if(verticalInput == 0 && horizontalInput == 0)
        {
            animator.SetBool("Run", false);
        }
    }

    // 現在は地面や屋根の衝突
    private void OnCollisionEnter(Collision collision)
    {
        // 下にある物体との判定
        // Vector3 origin = transform.position + Vector3.up * 0.01f;
        // bool isGrounded = Physics.Raycast(origin, Vector3.down, 0.8f);

        // if (isGrounded)
        // {
        //     numJump = 0;
        //     // アニメーションをジャンプでなくする
        //     animator.SetBool("Jump", false);
        //     animator.SetBool("Jump2", false);
        // }

        // 壁にぶつかった際に変な挙動をするので一旦下記のようにする
        if(collision.gameObject.tag != "Apple")
        {
            numJump = 0;
            // アニメーションをジャンプでなくする
            animator.SetBool("Jump", false);
            animator.SetBool("Jump2", false);
        }
    }

    // トリガー
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Apple")
        {
            // Debug.Log("Apple");
            Destroy(other.gameObject);
            FindObjectOfType<AppleGenerator>().SetArrayApplePutFalse(other.GetComponent<AppleController>().getPosition());
            this.director.GetComponent<GameDirector>().GetApple();
        }
    }
}
