using System.Collections;
using PixelAdventureAPI.Others;
using UnityEngine;

namespace PixelAdventureAPI.Enemies.AngryPigLogics
{
    public class AngryPigMovement : MonoBehaviour
    {

        private Animator m_Animator = null;
        private static readonly int k_AGGRO_HASH   = Animator.StringToHash("aggro");
        private static readonly int k_WALKING_HASH = Animator.StringToHash("isWalking");

        [Header("Movement")]
        [SerializeField] private float m_WalkSpeed = 0f;
        [SerializeField] private float m_RunSpeed = 0f;
        [SerializeField] private float m_AggroRange = 0f;
        [SerializeField] private float m_KeepAggroTime = 0.05f;
        [SerializeField] private bool m_MoveLeft = true;
        [SerializeField] private LayerMask m_PlayerLayerMask = 0;
        [SerializeField] private float m_IdleTime = 1f;
        [SerializeField] private Transform m_LeftTarget = null;
        [SerializeField] private Transform m_RightTarget = null;
        private float m_LeftX = 0f;
        private float m_RightX = 0f;

        private Transform m_Transform = null;
        private bool m_MoveLock = false;
        private bool m_AggroPlayer = false;
        private float m_AggroTimer = 0f;

        private void Start() 
        {
            m_Animator = GetComponent<Animator>();
            m_Transform = GetComponent<Transform>();
            m_LeftX = m_LeftTarget.transform.position.x;
            m_RightX = m_RightTarget.transform.position.x;
            m_Animator.SetBool(k_WALKING_HASH, true);
        }

        private void Update() {
            
        }
        
        private void FixedUpdate() 
        {
            m_AggroPlayer = Utils.Raycast
            (
                origin: m_Transform.position,
                direction: m_MoveLeft ? Vector2.left : Vector2.right,
                distance: Mathf.Min(m_MoveLeft ? m_Transform.position.x - m_LeftX : m_RightX - m_Transform.position.x, m_AggroRange),
                layerMask: m_PlayerLayerMask
            ).collider != null;
            
            m_Animator.SetBool(k_AGGRO_HASH, m_AggroPlayer);

            // Debug.DrawLine
            // (
            //     start : m_Transform.position,
            //     end   : new Vector3
            //     (
            //         x : m_Transform.position.x + 
            //         (
            //             m_MoveLeft ? 
            //                 -Mathf.Min(m_Transform.position.x - m_LeftX, m_AggroRange) :
            //                 Mathf.Min(m_RightX - m_Transform.position.x, m_AggroRange)
            //         ), 
            //         y : m_Transform.position.y, 
            //         z :m_Transform.position.z
            //     ),
            //     color : m_AggroPlayer ? Color.red : Color.green
            // );

            if (!m_MoveLock)
            {
                if (m_MoveLeft)
                {
                    StartCoroutine(MoveTowards(m_LeftX));
                }   
                else
                {
                    StartCoroutine(MoveTowards(m_RightX));
                }
            }
        }

        
        private IEnumerator MoveTowards(float x)
        {
            m_MoveLock = true;

            if (m_Transform.position.x > x)
            {
                while (m_Transform.position.x > x)
                {
                    m_Transform.Translate
                    (
                        x : -(m_AggroPlayer ? m_RunSpeed : m_WalkSpeed) * Time.deltaTime,
                        y : 0f,
                        z : 0f
                    );
                    yield return null;
                }
            }
            else
            {
                while (m_Transform.position.x < x)
                {
                    m_Transform.Translate
                    (
                        x: (m_AggroPlayer ? m_RunSpeed : m_WalkSpeed) * Time.deltaTime,
                        y: 0f,
                        z: 0f
                    );
                    yield return null;
                }
            }

            m_Animator.SetBool(k_WALKING_HASH, false);
            yield return new WaitForSeconds(m_IdleTime);
            m_Animator.SetBool(k_WALKING_HASH, true);

            m_MoveLock = false;
            m_MoveLeft = !m_MoveLeft;

            Vector3 scale = m_Transform.localScale;
            scale.x *= -1;
            m_Transform.localScale = scale;

            yield return 0;
        }
        
    }

}

