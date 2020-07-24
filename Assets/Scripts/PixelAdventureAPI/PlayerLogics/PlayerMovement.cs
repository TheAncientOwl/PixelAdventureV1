using System.Collections;
using UnityEngine;

namespace PixelAdventureAPI.PlayerLogics
{
    public class PlayerMovement : MonoBehaviour
    {
        #region << instance >>
        private static PlayerMovement m_Instance = null;
        public static PlayerMovement GetInstance() => m_Instance;
        public Animator GetPlayerAnimator() => m_Animator;
        public Rigidbody2D GetRigidbody2D() => m_Rigidbody2D;
        #endregion

        private Rigidbody2D m_Rigidbody2D = null;
        #region << animator var >>

        private Animator m_Animator = null;
        private static readonly int k_Y_VELOCITY_HASH  = Animator.StringToHash("yVelocity");
        private static readonly int k_RUN_HASH         = Animator.StringToHash("isRunning");
        private static readonly int k_GROUNDED_HASH    = Animator.StringToHash("isGrounded");
        private static readonly int k_DOUBLE_JUMP_HASH = Animator.StringToHash("isDoubleJumping");

        #endregion //===========================================================================\\
        #region << horizontal movement var >>

        private static readonly string k_HORIZONTAL = "Horizontal";
        [Header("Horizontal Movement")]
        [Range(0, 1)]
        [SerializeField] private float m_MovementSmoothing = .05f;
        [SerializeField] private float m_RunSpeed = 350f;
        private Vector2 m_AuxVelocity = Vector2.zero;
        private bool m_FacingRight = true;
        private float m_Direction = 0;

        #endregion //===========================================================================\\       
        #region << jump var >>

        [Header("Jump")]
        [SerializeField] private Vector2 m_JumpVelocity = Vector2.up;
        [SerializeField] private Vector2 m_BoxCastSize = Vector2.zero;
        [SerializeField] private LayerMask m_GroundLayerMask = 0;
        private const float k_EXTRA_HEIGHT_CHECK = 0.018f;
        private CircleCollider2D m_CircleCollider2D = null;

        private bool m_Grounded = false;
        private const float k_GROUNDED_BUFFER = 0.09f;
        private float m_GroundedBuffer = 0f;

        private bool m_CanJumpInAir = false;
        private const float k_JUMP_BUFFER = 0.2f;
        private float m_JumpBuffer = 0f;

        #endregion //===========================================================================\\

        private bool m_IsKnockbacked = false;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_CircleCollider2D = GetComponent<CircleCollider2D>();
            m_Instance = this;
        }

        private void Update()
        {
            if (m_IsKnockbacked)
            {
                m_CanJumpInAir = false;
                return;
            }  
                
            m_Direction = Input.GetAxisRaw(k_HORIZONTAL);

            m_GroundedBuffer = m_Grounded ? k_GROUNDED_BUFFER : m_GroundedBuffer - Time.deltaTime;

            m_JumpBuffer = Input.GetKeyDown(KeyCode.Space) ? k_JUMP_BUFFER : m_JumpBuffer - Time.deltaTime;

            if (m_JumpBuffer > 0)
            {
                m_JumpBuffer = 0;
                if (m_GroundedBuffer > 0) /// Jump
                {
                    m_GroundedBuffer = 0;
                    m_CanJumpInAir = true;
                    m_Grounded = false;
                    m_Rigidbody2D.velocity = m_JumpVelocity;
                }
                else if (m_CanJumpInAir) /// Double Jump
                {
                    m_CanJumpInAir = false;
                    m_Rigidbody2D.velocity = m_JumpVelocity;
                    m_Animator.SetTrigger(k_DOUBLE_JUMP_HASH);
                }
            }
            
            m_Animator.SetBool(k_GROUNDED_HASH, m_Grounded);
            if (m_Grounded)
                m_Animator.SetBool(k_RUN_HASH, m_Direction != 0);
            else
                m_Animator.SetFloat(k_Y_VELOCITY_HASH, m_Rigidbody2D.velocity.y);
        }

        private void FixedUpdate()
        {
            m_Grounded = Physics2D.BoxCast
            (
                origin    : m_CircleCollider2D.bounds.center + Vector3.down * 0.5f,
                size      : m_BoxCastSize,
                angle     : 0f,
                direction : Vector2.down,
                distance  : k_EXTRA_HEIGHT_CHECK,
                layerMask : m_GroundLayerMask
            ).collider != null;

            float move = m_RunSpeed * m_Direction * Time.fixedDeltaTime;
            m_Rigidbody2D.velocity = Vector2.SmoothDamp
            (
                current: m_Rigidbody2D.velocity,
                target: new Vector2(move, m_Rigidbody2D.velocity.y),
                currentVelocity: ref m_AuxVelocity,
                smoothTime: m_MovementSmoothing
            );

            if ((move > 0 && !m_FacingRight) || (move < 0 && m_FacingRight))
            {
                m_FacingRight = !m_FacingRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }

        public float GetCenterX() => m_CircleCollider2D.bounds.center.x;

        public float GetCenterY() => m_CircleCollider2D.bounds.center.y;

        public float GetVelocityY() => m_Rigidbody2D.velocity.y;

        public IEnumerator KnockbackRoutine(float duration, Vector2 velocity, Vector2 direction)
        {
            m_IsKnockbacked = true;
            float timer = 0f;
            velocity.x *= direction.x;
            velocity.y *= direction.y;
            velocity *= Time.fixedDeltaTime;

            m_Rigidbody2D.velocity = Vector2.zero;
            while (timer <= duration)
            {
                timer += Time.deltaTime;
                m_Rigidbody2D.velocity = velocity;
                yield return null;
            }

            m_IsKnockbacked = false;
            yield return 0;
        }

        

    }
}