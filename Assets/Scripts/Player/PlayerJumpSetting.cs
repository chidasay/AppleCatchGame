using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerJumpSetting", menuName = "OcclusionCountermeasures/Player")]
    public class PlayerJumpSetting : ScriptableObject
    {
        [SerializeField] private int _jumpLimitCount = 1;
        [SerializeField] private float _jumpLimitTime = 1f;
        [SerializeField] private float _jumpThrottle = 0.1f; // ジャンプ入力を受け付けるインターバル
        [SerializeField] private float _coyoteTime = 0.15f;
        [SerializeField] private float _bufferTime = 0.15f;

        public int JumpLimitCount => _jumpLimitCount;
        public float JumpLimitTime => _jumpLimitTime;
        public float JumpThrottle => _jumpThrottle;
        public float CoyoteTime => _coyoteTime;
        public float BufferTime => _bufferTime;
    }
}
