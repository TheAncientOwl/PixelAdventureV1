using PixelAdventureAPI.PlayerLogics;
using PixelAdventureAPI.Others;
using UnityEngine;

namespace PixelAdventureAPI.Enemies.RinoLogics
{
    public class RinoCollision : MonoBehaviour
    {
        private static readonly string k_PLAYER_TAG = "Player";
        private static readonly int k_HIT_HASH = Animator.StringToHash("hit");

        [Header("Knockback")]
        [SerializeField] private float m_KnockbackDuration = 0.21f;
        [SerializeField] private Vector2 m_KnockbackVelocity = Vector2.zero;

        [Header("On Death")]
        [SerializeField] private float m_PlayerJumpVelocity = 7f;

        [Header("Player Score")]
        [SerializeField] private int m_Points = 10;

        private static PlayerMovement m_PlayerMovement = null;
        private static Rigidbody2D m_PlayerRigidbody2D = null;

        private CircleCollider2D m_CircleCollider2D = null;
        private Transform m_Transform = null;
        private Damager m_Damager = null;

        private void Start()
        {
            m_PlayerMovement = PlayerMovement.GetInstance();
            m_PlayerRigidbody2D = m_PlayerMovement.GetRigidbody2D();
            m_CircleCollider2D = GetComponent<CircleCollider2D>();
            m_Transform = GetComponent<Transform>();
            m_Damager = GetComponent<Damager>();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(k_PLAYER_TAG))
            {
                if (m_PlayerRigidbody2D.velocity.y < -1f)
                {
                    Debug.Log(m_PlayerRigidbody2D.velocity.y);
                    PlayerScore.GetInstance().Value += m_Points;
                    m_Points = 0;
                    GetComponent<Animator>().SetTrigger(k_HIT_HASH);
                    GetComponent<RinoMovement>().StopAllCoroutines();
                    m_PlayerRigidbody2D.velocity = new Vector2(m_PlayerRigidbody2D.velocity.x, m_PlayerJumpVelocity);
                    Destroy(this.gameObject, 0.33f);
                }
                else
                {
                    m_Damager.Apply();
                    StartCoroutine(m_PlayerMovement.KnockbackRoutine(
                        duration: m_KnockbackDuration,
                        velocity: m_KnockbackVelocity,
                        direction: new Vector2
                        (
                            x: m_PlayerMovement.GetCenterX() < m_CircleCollider2D.bounds.center.x ? -1f : 1f,
                            y: 0f
                        )
                    ));
                }

            }
        }
    }
}

