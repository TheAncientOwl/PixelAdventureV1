using PixelAdventureAPI.Others;
using System.Collections;
using UnityEngine;

namespace PixelAdventureAPI.Enemies.API
{
    /// <summary>
    /// Move transform between two points.
    /// </summary>
    public class BasicMovement : MonoBehaviour
    {
        [SerializeField] private bool m_MoveLeft = true;

        [SerializeField] private float m_Speed = 3.4f;

        [Header("Target")]
        [SerializeField] private Transform m_LeftTarget = null;
        [SerializeField] private Transform m_RightTarget = null;

        private TargetX m_TargetX = new TargetX(0f, 0f);
        private Transform m_Transform = null;
        private bool m_MoveLock = false;

        private void Start()
        {
            m_Transform = GetComponent<Transform>();
            m_TargetX = new TargetX
            (
                left: m_LeftTarget.transform.position.x,
                right: m_RightTarget.transform.position.x
            );
        }

        // Move transform.
        private void FixedUpdate()
        {
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
                        x: -m_Speed * Time.deltaTime,
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
                        x: m_Speed * Time.deltaTime,
                        y: 0f,
                        z: 0f
                    );
                    yield return null;
                }
            }

            m_MoveLock = false;
            m_MoveLeft = !m_MoveLeft;

            Vector3 scale = m_Transform.localScale;
            scale.x *= -1;
            m_Transform.localScale = scale;

            yield return 0;
        }
    }
}