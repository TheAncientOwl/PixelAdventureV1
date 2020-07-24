using PixelAdventureAPI.PlayerLogics;
using PixelAdventureAPI.Others;
using UnityEngine;

namespace PixelAdventureAPI.Traps
{
    public class Spikes : MonoBehaviour
    {
        private static readonly string k_PLAYER_TAG = "Player";

        [Header("Knockback")]
        [SerializeField] private float m_KnockbackDuration = 0.2f;
        [SerializeField] private Vector2 m_KnockbackVelocity = Vector2.zero;

        private static PlayerMovement m_PlayerMovement = null;

        private BoxCollider2D m_BoxCollider2D = null;
        private Damager m_Damager = null;

        private void Start()
        {
            m_PlayerMovement = PlayerMovement.GetInstance();
            m_BoxCollider2D = GetComponent<BoxCollider2D>();
            m_Damager = GetComponent<Damager>();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(k_PLAYER_TAG))
            {
                m_Damager.Apply();

                StartCoroutine(m_PlayerMovement.KnockbackRoutine(
                    duration : m_KnockbackDuration,
                    velocity : m_KnockbackVelocity,
                    direction: new Vector2
                    (
                        x: m_PlayerMovement.GetCenterX() < m_BoxCollider2D.bounds.center.x ? -1f : 1f,
                        y: m_PlayerMovement.GetVelocityY() == 0f ? 0f : 1f
                    )
                ));
            }
        }

        

    }
}

