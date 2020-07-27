using PixelAdventureAPI.PlayerLogics;
using PixelAdventureAPI.Others;
using UnityEngine;

namespace PixelAdventureAPI.Enemies.API
{
    /// <summary>
    /// OnTriggerEnter2D check if the object should die or apply damage and knockback to player.
    /// </summary>
    public class SimpleCollision : MonoBehaviour
    {
        private static readonly string k_PLAYER_TAG = "Player";

        [SerializeField] private ColliderType m_ColliderType = ColliderType.BoxCollider2D;
        [SerializeField] private Knockback m_Knockback = null;

        private static PlayerMovement m_PlayerMovement = null;
        private static Rigidbody2D m_PlayerRigidbody2D = null;

        private Collider2D m_Collider2D = null;
        private Damager m_Damager = null;

        public delegate void OnDeathEvent();
        private OnDeathEvent m_OnDeath = null;
        public void AddOnDeath(OnDeathEvent e) => m_OnDeath += e;

        private void Start()
        {
            m_PlayerMovement = PlayerMovement.GetInstance();
            m_PlayerRigidbody2D = m_PlayerMovement.GetRigidbody2D();
            switch (m_ColliderType)
            {
                case ColliderType.BoxCollider2D:
                    m_Collider2D = GetComponent<BoxCollider2D>();
                    break;
                case ColliderType.CircleCollider2D:
                    m_Collider2D = GetComponent<BoxCollider2D>();
                    break;
                default:
                    Debug.LogError("COLLIDER TYPE NOT SUPPORTED!");
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

                    m_PlayerMovement.ApplyKnockback180
                    (
                        knockback: m_Knockback,
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

    }

}

