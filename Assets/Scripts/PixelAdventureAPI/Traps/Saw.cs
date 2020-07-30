using PixelAdventureAPI.PlayerLogics;
using PixelAdventureAPI.Others;
using UnityEngine;

namespace PixelAdventureAPI.Traps
{
    /// <summary>
    /// Trap.
    /// Knockback 360 degrees.
    /// </summary>
    public class Saw : MonoBehaviour
    {
        private static readonly string k_PLAYER_TAG = "Player";
        private static readonly int k_ON_HASH = Animator.StringToHash("on");

        [SerializeField] private KnockbackInfo m_Knockback = null;

        private static PlayerMovement m_PlayerMovement = null;

        private CircleCollider2D m_CircleCollider2D = null;
        private Damager m_Damager = null;

        private void Start()
        {
            m_PlayerMovement = PlayerMovement.GetInstance();
            m_CircleCollider2D = GetComponent<CircleCollider2D>();
            m_Damager = GetComponent<Damager>();
        }

        // Apply damage and knockback.
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(k_PLAYER_TAG))
            {
                m_Damager.Apply();

                m_PlayerMovement.ApplyKnockback360
                (
                    knockbackInfo  : m_Knockback,
                    colliderCenter : m_CircleCollider2D.bounds.center
                );

            }
        }
        
    }
}

