using PixelAdventureAPI.PlayerLogics;
using PixelAdventureAPI.Others;
using UnityEngine;

namespace PixelAdventureAPI.Traps
{
    /// <summary>
    /// Trap.
    /// Two parts: 
    ///     1) solid, 
    ///     2) damage.
    /// Solid part acts like ground.
    /// Damage part damages and knockbacks the player.
    /// Two states: 
    ///     1) on, 
    ///     2) off.
    /// Knockback 180 degrees.
    /// </summary>
    public class Fire : MonoBehaviour
    {
        private static readonly string k_PLAYER_TAG = "Player";
        private static readonly int k_ON_HASH       = Animator.StringToHash("on");
        private static readonly int k_OFF_HASH      = Animator.StringToHash("off");
        private static readonly int k_PREPARE_HASH  = Animator.StringToHash("prepare");

        [SerializeField] private KnockbackInfo m_KnockbackInfo = null;

        private static PlayerMovement m_PlayerMovement = null;
        private CircleCollider2D m_CircleCollider2D = null;
        private Animator m_Animator = null;
        private Damager m_Damager = null;
        
        private static readonly float k_SWITCH_TIME = 3.3f;
        private static readonly float k_PREPARE_TO_HIT_TIME = 2.1f;
        private float m_Timer = 0f;
        private bool m_DamageWhileOff = false;
        private bool m_On = false;

        private void Start()
        {
            m_PlayerMovement = PlayerMovement.GetInstance();
            m_CircleCollider2D = GetComponent<CircleCollider2D>();
            m_Animator = GetComponent<Animator>();
            m_Damager = GetComponent<Damager>();
        }

        /**
         * Update on/off timer.
         * Switches between states (on/off).
         * Update animation.
         */
        private void Update() 
        {
            if (m_Timer > k_SWITCH_TIME)
            {
                m_Timer = 0f;
                m_On = !m_On;
                if (!m_On)
                    m_DamageWhileOff = false; 
            }
            else
            {
                if (m_Timer > k_PREPARE_TO_HIT_TIME) m_Animator.SetTrigger(k_PREPARE_HASH);
                m_Timer += Time.deltaTime;
            }

            if (m_On) m_Animator.SetTrigger(k_ON_HASH);
            else m_Animator.SetTrigger(k_OFF_HASH);
        }

        // Apply damage and knockback.
        private void Logics()
        {
            m_Damager.Apply();

            m_PlayerMovement.ApplyKnockback180
            (
                knockbackInfo  : m_KnockbackInfo,
                colliderCenter : m_CircleCollider2D.bounds.center
            );
        }

        #region << apply logics >>
        // Apply logics when player enters damage area.
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (m_On && collider.CompareTag(k_PLAYER_TAG))
                Logics();
        }

        // Apply logics while player stays in damage area.
        private void OnTriggerStay2D(Collider2D collider) 
        {
            if (!m_DamageWhileOff && m_On && collider.CompareTag(k_PLAYER_TAG))
            {
                Logics();
                m_DamageWhileOff = true;
            }
        }
        #endregion

        

    }
}

