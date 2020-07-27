using PixelAdventureAPI.PlayerLogics;
using PixelAdventureAPI.Others;
using UnityEngine;

namespace PixelAdventureAPI.Traps
{
    /// <summary>
    /// Trap.
    /// Knockback 180 degrees.
    /// </summary>
    public class Spikes : MonoBehaviour
    {
        private static readonly string k_PLAYER_TAG = "Player";

        [SerializeField] private Knockback m_Knockback = null;

        private static PlayerMovement m_PlayerMovement = null;

        private BoxCollider2D m_BoxCollider2D = null;
        private Damager m_Damager = null;

        private void Start()
        {
            m_PlayerMovement = PlayerMovement.GetInstance();
            m_BoxCollider2D = GetComponent<BoxCollider2D>();
            m_Damager = GetComponent<Damager>();
        }

        // Apply damage and knockback.
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(k_PLAYER_TAG))
            {
                m_Damager.Apply();

                m_PlayerMovement.ApplyKnockback180
                (
                    knockback      : m_Knockback,
                    colliderCenter : m_BoxCollider2D.bounds.center
                );
            }
        }

    }
}

