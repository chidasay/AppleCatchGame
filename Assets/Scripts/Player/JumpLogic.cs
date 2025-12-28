using System;

namespace Player
{
    // ジャンプが実行されたタイミング
    public enum JumpTiming
    {
        Immediate,    // 入力直後のジャンプ
        CoyoteTime,   // コヨーテタイム中のジャンプ
        Buffered,     // 先行入力によるジャンプ
        Extra,        // 多段ジャンプ
    }

    // ジャンプが発生した際のコンテキスト情報
    public readonly struct JumpContext
    {
        public readonly JumpTiming Timing;    // ジャンプ発生タイミング
        public readonly int JumpCount;        // 多段ジャンプの現在回数

        public JumpContext(JumpTiming timing, int jumpCount)
        {
            Timing = timing;
            JumpCount = jumpCount;
        }
    }

    // ジャンプの挙動を制御するロジッククラス
    // コヨーテタイム、先行入力、多段ジャンプの機能
    public class JumpLogic
    {
        private const float DisabledTimer = float.PositiveInfinity;

        public JumpLogic(PlayerJumpSetting setting)
        {
            _setting = setting;
        }
        
        private int _extraJumpCount;
        private float _coyoteTimeTimer = DisabledTimer;
        private float _bufferTimeTimer = DisabledTimer;
        private readonly PlayerJumpSetting _setting;
        
        // ジャンプが開始された時に発火するイベント
        public event Action<JumpContext> OnJumpStarted;
        // ジャンプによる上昇が停止した時に発火するイベント
        public event Action OnJumpStopped;
        
        // ジャンプ可能な足場に接触しているか
        public bool HasFooting { get; private set; }
        // ジャンプ上昇中かどうか
        public bool IsJumpingUp { get; private set; }
        // 前回のジャンプからの経過時間
        public float JumpElapsedTime { get; private set; } = float.PositiveInfinity;
        // コヨーテタイム中かどうか
        public bool IsCoyoteTime => _coyoteTimeTimer < _setting.CoyoteTime;
        // 先行入力が有効かどうか
        public bool IsBuffered => _bufferTimeTimer < _setting.BufferTime;
        
        // ジャンプを開始するか、先行入力を登録する
        // ジャンプ可能な状態であれば即座にジャンプして、不可能な状態であれば先行入力として記録
        public void TryJumpStart()
        {
            if (CanJump())
            {
                ExecuteJump(true);
                return;
            }
            StartBufferTimer();
        }
        
        // 上昇中のジャンプを停止させる
        public void StopJumpingUp()
        {
            if (!IsJumpingUp)
            {
                return;
            }
            IsJumpingUp = false;
            OnJumpStopped?.Invoke();
        }
        
        // ジャンプ可能かどうかを判定し、可能ならtrueで不可能ならfalseで返す
        public bool CanJump()
        {
            // 短時間の連続ジャンプを抑制
            if (JumpElapsedTime < _setting.JumpThrottle)
            {
                return false;
            }

            if (HasFooting || IsCoyoteTime)
            {
                return true;
            }
            return _extraJumpCount < _setting.JumpLimitCount;
        }

        private void ExecuteJump(bool isImmediate)
        {
            JumpTiming timing;
            if (HasFooting)
            {
                timing = isImmediate 
                    ? JumpTiming.Immediate
                    : JumpTiming.Buffered;
            }
            else if (IsCoyoteTime)
            {
                timing = JumpTiming.CoyoteTime;
                ConsumeCoyoteTime();
            }
            else
            {
                timing = JumpTiming.Extra;
                _extraJumpCount++;
            }
            IsJumpingUp = true;
            JumpElapsedTime = 0f;
            OnJumpStarted?.Invoke(new JumpContext(
                timing,
                _extraJumpCount
            ));
        }
        
        // フレーム毎の更新処理
        public void Tick(float deltaTime)
        {
            JumpElapsedTime += deltaTime;

            if (!HasFooting)
            {
                _coyoteTimeTimer += deltaTime;
            }

            if (IsBuffered)
            {
                _bufferTimeTimer += deltaTime;
            }

            if (IsJumpingUp && (JumpElapsedTime > _setting.JumpLimitTime))
            {
                StopJumpingUp();
            }

            if (IsBuffered && CanJump())
            {
                ConsumeBufferTime();
                ExecuteJump(false);
            }
        }
        
        // 壁や地面との接触を設定する
        public void SetContact()
        {
            if (HasFooting)
            {
                return;
            }
            HasFooting = true;
            ResetExtraJumpCount();
            StopJumpingUp();
            ConsumeCoyoteTime();
        }
        
        // 壁や地面との接触が失われた際に呼び出す
        public void ClearContact()
        {
            if (!HasFooting)
            {
                return;
            }
            HasFooting = false;

            if (IsJumpingUp)
            {
                ConsumeCoyoteTime();
            }
            else
            {
                StartCoyoteTimer();
            }
        }
        
        // 多段ジャンプ回数をリセットする
        public void ResetExtraJumpCount()
        {
            _extraJumpCount = 0;
        }

        private void ConsumeCoyoteTime()
        {
            _coyoteTimeTimer = DisabledTimer;
        }
        
        private void StartCoyoteTimer()
        {
            _coyoteTimeTimer = 0f;
        }
        
        private void ConsumeBufferTime()
        {
            _bufferTimeTimer = DisabledTimer;
        }

        private void StartBufferTimer()
        {
            _bufferTimeTimer = 0f;
        }
    }
}
