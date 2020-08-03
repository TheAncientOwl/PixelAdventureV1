using PixelAdventureAPI.PlayerLogics;
using PixelAdventureAPI.Others;
using UnityEngine;
using System;

namespace PixelAdventureAPI.Enemies.API
{
    /// <summary>
    /// OnTriggerEnter2D check if the object should die or apply damage and knockback to player.
    /// Knockback 180 degrees.
    /// </summary>
    public class SimpleCollision : MonoBehaviour
    {
        private static readonly string k_PLAYER_TAG = "Player";

        [SerializeField] private KnockbackType m_KnockbackType = KnockbackType.Degrees180;
        [SerializeField] private ColliderType m_ColliderType = ColliderType.BoxCollider2D;
        [SerializeField] private KnockbackInfo m_KnockbackInfo = null;

        private static Rigidbody2D m_PlayerRigidbody2D = null;

        private Collider2D m_Collider2D = null;
        private Damager m_Damager = null;

        private Action m_OnDeath = null;
        public void AddOnDeath(Action e) => m_OnDeath += e;

        private delegate void ApplyKnockback(KnockbackInfo knockback, Vector2 colliderCenter);
        private ApplyKnockback m_ApplyKnockback = null;

        private void Start()
        {
            PlayerMovement playerMovement = PlayerMovement.GetInstance();
            m_PlayerRigidbody2D = playerMovement.GetRigidbody2D();

            switch (m_ColliderType)
            {
                case ColliderType.BoxCollider2D:
                    m_Collider2D = GetComponent<BoxCollider2D>();
                    break;
                case ColliderType.CircleCollider2D:
                    m_Collider2D = GetComponent<CircleCollider2D>();
                    break;
                default:
                    Debug.LogError("COLLIDER TYPE NOT SUPPORTED!");
                    break;
            }

            switch (m_KnockbackType)
            {
                case KnockbackType.Degrees180:
                    m_ApplyKnockback = playerMovement.ApplyKnockback180;
                    break;
                case KnockbackType.Degrees360:
                    m_ApplyKnockback = playerMovement.ApplyKnockback360;
                    break;
                default:
                    Debug.LogError("Knockback Type Not Supported!");
                    break;
            }

            m_Damager = GetComponent<Damager>();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(k_PLAYER_TAG))
            {
                if (m_PlayerRigidbody2D.velocity.y < -1f) 
                    m_OnDeath?.Invoke();
                else
                {
                    m_Damager.Apply();

                    m_ApplyKnockback(
                        knockback: m_KnockbackInfo,
                        colliderCenter: m_Collider2D.bounds.center
                    );

                }
            }
        }

        private enum ColliderType
        {
            BoxCollider2D,
            CircleCollider2D
        }

        private enum KnockbackType
        {
            Degrees180,
            Degrees360
        }

    }

}

