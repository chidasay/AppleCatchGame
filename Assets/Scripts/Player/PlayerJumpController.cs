using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PlayerJumpController : MonoBehaviour
    {
        [Header("ジャンプ設定")]
        [SerializeField] private float _jumpPower = 5f;
        [SerializeField] private PlayerJumpSetting _setting;

        private Rigidbody _rigidbody;
        private JumpLogic _jumpLogic;
        private bool _isGrounded;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            if (_setting == null)
            {
                Debug.LogError("PlayerJumpSettingがアサインされていません");
                return;
            }

            // JumpLogicを初期化
            _jumpLogic = new JumpLogic(_setting);

            // イベントハンドラを登録
            _jumpLogic.OnJumpStarted += OnJumpStarted;
            _jumpLogic.OnJumpStopped += OnJumpStopped;
        }

        private void Update()
        {
            // 設定判定
            CheckGround();
            // JumpLogicを更新
            _jumpLogic.Tick(Time.deltaTime);
            // ジャンプ入力を処理
            HandleJumpInput();
        }

        // 接地判定（後で変えたい）
        private void CheckGround()
        {
            // 下方向にRayCastして接地判定
            bool wasGrounded = _isGrounded;
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);

            // 接地状態が変化したらJumpLogicに通知
            if (_isGrounded && !wasGrounded)
            {
                _jumpLogic.SetContact();
            }
            else if (!_isGrounded && wasGrounded)
            {
                _jumpLogic.ClearContact();
            }
        }

        // ジャンプ入力の処理
        private void HandleJumpInput()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                // TryJumpStartは以下の場合にジャンプを実施する
                // - 接地している場合（即座にジャンプ）
                // - コヨーテタイム中
                // - 空中の場合は先行入力として記録されて、着地時に自動実行
                _jumpLogic.TryJumpStart();
            }

            // 上昇中にスペースキーが離されていたらジャンプ上昇を停止
            if (Keyboard.current.spaceKey.isPressed == false && _jumpLogic.IsJumpingUp)
            {
                _jumpLogic.StopJumpingUp();
            }
        }

        // ジャンプ開始時のイベントハンドラ
        private void OnJumpStarted(JumpContext context)
        {
            Debug.Log("ジャンプ開始");

            // Rigidbodyで上向きの速度を与える
            Vector3 velocity = _rigidbody.velocity;
            velocity.y = _jumpPower;
            _rigidbody.velocity = velocity;
            _rigidbody.velocity = velocity;
        }

        // ジャンプ停止時のイベントハンドラ
        private void OnJumpStopped()
        {
            Debug.Log("ジャンプ上昇停止");

            // 上昇速度を減衰（可変ジャンプ！？）
            Vector3 velocity = _rigidbody.velocity;
            if (velocity.y > 0)
            {
                velocity.y *= 0.5f;
                _rigidbody.velocity = velocity;
            }
        }

        private void OnDrawGizmos()
        {
            // 接地判定の可視化
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawRay(transform.position, Vector3.down * 1.1f);
        }
    }
}
