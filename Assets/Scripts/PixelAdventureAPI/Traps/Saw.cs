using PixelAdventureAPI.PlayerLogics;
using PixelAdventureAPI.Others;
using UnityEngine;

namespace PixelAdventureAPI.Traps
{
    public class Saw : MonoBehaviour
    {
        private static readonly string k_PLAYER_TAG = "Player";
        private static readonly int k_ON_HASH = Animator.StringToHash("on");

        [Header("Knockback")]
        [SerializeField] private float m_KnockbackDuration = 0.2f;
        [SerializeField] private Vector2 m_KnockbackVelocity = Vector2.zero;

        private static PlayerMovement m_PlayerMovement = null;

        private CircleCollider2D m_CircleCollider2D = null;
        private Damager m_Damager = null;

        private void Start()
        {
            m_PlayerMovement = PlayerMovement.GetInstance();
            m_CircleCollider2D = GetComponent<CircleCollider2D>();
            m_Damager = GetComponent<Damager>();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(k_PLAYER_TAG))
            {
                m_Damager.Apply();

                StartCoroutine(m_PlayerMovement.KnockbackRoutine(
                    duration: m_KnockbackDuration,
                    velocity: m_KnockbackVelocity,
                    direction: new Vector2
                    (
                        x: m_PlayerMovement.GetCenterX() <= m_CircleCollider2D.bounds.center.x ? -1f : 1f,
                        y: m_PlayerMovement.GetVelocityY() == 0f ? 0f :
                        (m_PlayerMovement.GetCenterY() < m_CircleCollider2D.bounds.center.y ? -1f : 1f)
                    )
                ));
            }
        }

        
    }

}

