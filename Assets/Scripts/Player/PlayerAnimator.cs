using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int RunId = Animator.StringToHash("Run");
        private static readonly int JumpId = Animator.StringToHash("Jump");
        
        [SerializeField] private Animator _animator;

        public void RunTrue()
        {
            _animator.SetBool(RunId, true);
        }

        public void RunFalse()
        {
            _animator.SetBool(RunId, false);
        }
        
        public void JumpTrue()
        {
            _animator.SetBool(JumpId, true);
        }

        public void JumpFalse()
        {
            _animator.SetBool(JumpId, false);
        }
    }
}

// これは上手く行かなかったが残しておく
// Unity Editor で Animator をアタッチしようとしてもできない

// 現状 PlayerJumpController や PlayerMovement に直書きなのでいつか直したい