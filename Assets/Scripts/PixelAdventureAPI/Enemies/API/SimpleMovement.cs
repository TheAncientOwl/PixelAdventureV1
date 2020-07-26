using PixelAdventureAPI.Others;
using System.Collections;
using UnityEngine;

namespace PixelAdventureAPI.Enemies.API
{
    public class SimpleMovement : MonoBehaviour
    {
        [SerializeField] private float m_WalkSpeed = 3.4f;
        [SerializeField] private float m_RunSpeed = 8.5f;
        [SerializeField] private float m_AggroRange = 5f;
        [SerializeField] private float m_KeepAggroTime = 0.3f;
        [SerializeField] private bool m_MoveLeft = true;
        [SerializeField] private LayerMask m_PlayerLayerMask = 0;
        [SerializeField] private float m_IdleTime = 1f;
        [SerializeField] private Transform m_LeftTarget = null;
        [SerializeField] private Transform m_RightTarget = null;

        private TargetX m_TargetX = new TargetX(0f, 0f);
        private Transform m_Transform = null;
        private bool m_MoveLock = false;
        private bool m_AggroPlayer = false;
        private float m_AggroTimer = 0f;

        #region  << events >>
        public delegate void AggroEvent(float aggroTimer);
        public delegate void MovementEvent();
        
        private MovementEvent m_OnStart = null;
        public void AddOnStart(MovementEvent e) => m_OnStart += e;

        private MovementEvent m_OnIdle = null;
        public void AddOnIdle(MovementEvent e) => m_OnIdle += e;

        private MovementEvent m_AfterIdle = null;
        public void AddAfterIdle(MovementEvent e) => m_AfterIdle += e;

        private AggroEvent m_OnAggro = null;
        public void AddOnAggro(AggroEvent e) => m_OnAggro += e;
        #endregion

        private void Start() 
        {
            m_Transform = GetComponent<Transform>();
            m_TargetX = new TargetX
            (
                left  : m_LeftTarget.transform.position.x,
                right : m_RightTarget.transform.position.x
            );
            m_OnStart?.Invoke();
            m_OnStart = null;
        }

        private void Update() => m_AggroTimer = m_AggroPlayer ? m_KeepAggroTime : m_AggroTimer - Time.deltaTime;

        private void FixedUpdate()
        {
            m_AggroPlayer = Utils.Raycast
            (
                origin: m_Transform.position,
                direction: m_MoveLeft ? Vector2.left : Vector2.right,
                distance: Mathf.Min(m_MoveLeft ? m_Transform.position.x - m_TargetX.left : m_TargetX.right - m_Transform.position.x, m_AggroRange),
                layerMask: m_PlayerLayerMask
            ).collider != null;

            m_OnAggro?.Invoke(m_AggroTimer);

            if (!m_MoveLock)
            {
                if (m_MoveLeft)
                {
                    StartCoroutine(MoveTowards(m_TargetX.left));
                }
                else
                {
                    StartCoroutine(MoveTowards(m_TargetX.right));
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
                        x: -(m_AggroTimer > 0f ? m_RunSpeed : m_WalkSpeed) * Time.deltaTime,
                        y: 0f,
                        z: 0f
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
                        x: (m_AggroTimer > 0f ? m_RunSpeed : m_WalkSpeed) * Time.deltaTime,
                        y: 0f,
                        z: 0f
                    );
                    yield return null;
                }
            }

            m_OnIdle?.Invoke();
            yield return new WaitForSeconds(m_IdleTime);
            m_AfterIdle?.Invoke();
            
            m_MoveLock = false;
            m_MoveLeft = !m_MoveLeft;

            Vector3 scale = m_Transform.localScale;
            scale.x *= -1;
            m_Transform.localScale = scale;

            yield return 0;
        }

        private struct TargetX
        {
            public float left, right;
            public TargetX(float left, float right)
            {
                this.left = left;
                this.right = right;
            }
        };
    }
}


