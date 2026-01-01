using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement :  MonoBehaviour
    {
        private static readonly int Run = Animator.StringToHash("Run");
        [SerializeField] private float _speed = 8.0f;
        
        private Rigidbody _rigidbody;
        private Collider _collider;
        private Animator _animator;

        [SerializeField] private GameObject _director;
        
        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            _rigidbody.velocity = new Vector3(horizontalInput * _speed , _rigidbody.velocity.y, verticalInput * _speed);

            // 滑らかな移動
            if (Mathf.Abs(_rigidbody.velocity.x) > 0.1f || Mathf.Abs(_rigidbody.velocity.z) > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(horizontalInput, 0, verticalInput));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
            
            // 改良以前はここでジャンプに関する処理をしていた
            // PlayerJumpControllerに切り出し
            
            // 仮のアニメーション（走る）
            if(verticalInput != 0 || horizontalInput != 0)
            {
                _animator.SetBool(Run, true);
            }
            if(verticalInput == 0 && horizontalInput == 0)
            {
                _animator.SetBool(Run, false);
            }
        }

        private void OnTriggerEnter(Collider appleCollider)
        {
            if (appleCollider.gameObject.CompareTag("Apple"))
            {
                Destroy(appleCollider.gameObject);
                FindObjectOfType<AppleGenerator>().SetArrayApplePutFalse(appleCollider.GetComponent<AppleController>().getPosition());
                _director.GetComponent<GameDirector>().GetApple();
            }
        }
    }
}
