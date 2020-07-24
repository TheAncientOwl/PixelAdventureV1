using PixelAdventureAPI.PlayerLogics;
using PixelAdventureAPI.Others;
using System.Collections;
using UnityEngine;

namespace PixelAdventureAPI.Traps
{
    public class SpikeHead : MonoBehaviour
    {
        private static readonly string k_PLAYER_TAG = "Player";
        private static readonly int k_IDLE_HASH       = Animator.StringToHash("idle");
        private static readonly int k_BLINK_HASH      = Animator.StringToHash("blink");
        private static readonly int k_BOTTOM_HIT_HASH = Animator.StringToHash("bottomHit");

        private static PlayerMovement m_PlayerMovement = null;
        private CircleCollider2D m_CircleCollider2D = null;
        private Transform m_Transform = null;
        private Animator m_Animator = null;
        private Damager m_Damager = null;

        [Header("Movement")]
        [SerializeField] private float m_UpForce = 0f;
        [SerializeField] private float m_DownForce = 0f;
        [SerializeField] private Transform m_Bottom = null;
        [SerializeField] private LayerMask m_PlayerLayerMask = 0;
        private Vector2 m_BoxCastSize = Vector2.zero;
        private Vector2 m_BoxcastPos = Vector2.zero;
        private float m_TopPosY = 0f;
        private bool m_MoveLock = false;

        [Header("Knockback")]
        [SerializeField] private float m_KnockbackDuration = 0.2f;
        [SerializeField] private Vector2 m_KnobackVelocity = Vector2.zero;

        private void Start()
        {
            m_PlayerMovement = PlayerMovement.GetInstance();
            m_CircleCollider2D = GetComponent<CircleCollider2D>();
            m_Damager = GetComponent<Damager>();
            m_Transform = GetComponent<Transform>();
            m_Animator = GetComponent<Animator>();
            m_TopPosY = m_Transform.position.y;

            m_BoxCastSize.x = GetComponent<BoxCollider2D>().size.x * 2;
            m_BoxCastSize.y = Mathf.Abs(m_Bottom.position.y - m_Transform.position.y);

            m_BoxcastPos.y = m_Transform.position.y - m_BoxCastSize.y * 0.5f;
            m_BoxcastPos.x = m_Transform.position.x;
            
            m_DownForce = -m_DownForce;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(k_PLAYER_TAG))
            {
                m_Damager.Apply();
                StartCoroutine(m_PlayerMovement.KnockbackRoutine(
                    duration: m_KnockbackDuration,
                    velocity: m_KnobackVelocity,
                    direction: new Vector2
                    (
                        x: m_PlayerMovement.GetCenterX() <= m_CircleCollider2D.bounds.center.x ? -1f : 1f,
                        y: m_PlayerMovement.GetVelocityY() == 0f ? 0f :
                        (m_PlayerMovement.GetCenterY() < m_CircleCollider2D.bounds.center.y ? -1f : 1f)
                    )
                ));
            }
        }

        private void FixedUpdate()
        {
            if (!m_MoveLock)
            {
                if 
                (
                    Utils.BoxCast
                    (   
                        origin: m_BoxcastPos,
                        size: m_BoxCastSize,
                        angle: 0f,
                        direction: Vector2.down,
                        distance: 0f,
                        layerMask: m_PlayerLayerMask
                    ).collider != null
                )
                {
                    m_MoveLock = true;
                    StartCoroutine(MoveDown());
                }
            }
        }

        private IEnumerator MoveDown()
        {
            m_Animator.SetTrigger(k_BLINK_HASH);
            while (m_Transform.position.y > m_Bottom.position.y)
            {
                m_Transform.Translate(new Vector3(
                    x: 0f,
                    y: m_DownForce * Time.deltaTime,
                    z: 0f
                ));

                yield return null;
            }

            m_Animator.SetTrigger(k_BOTTOM_HIT_HASH);
            StartCoroutine(MoveUp());
            yield return 0;
        }

        private IEnumerator MoveUp()
        {
            m_Animator.SetTrigger(k_IDLE_HASH);
            while (m_Transform.position.y < m_TopPosY)
            {
                m_Transform.Translate(new Vector3(
                    x: 0f,
                    y: m_UpForce * Time.deltaTime,
                    z: 0f
                ));

                yield return null;
            }

            m_MoveLock = false;
            yield return 0;
        }

        

    }
}

