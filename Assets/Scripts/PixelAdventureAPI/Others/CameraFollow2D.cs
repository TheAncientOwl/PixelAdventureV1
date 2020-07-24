using UnityEngine;

namespace PixelAdventureAPI.Others
{
    public class CameraFollow2D : MonoBehaviour
    {
        [SerializeField] private Transform m_Player = null;
        [SerializeField] private float m_TimeOffset = 0f;
        [SerializeField] private Vector2 m_Offset = Vector2.zero;
        [SerializeField] private Vector2 m_MinPos = Vector3.zero;
        [SerializeField] private Vector2 m_MaxPos = Vector3.zero;
        private const float k_CAMERA_DEPTH = -5f;

        private void Update()
        {
            transform.position = Vector2.Lerp
            (
                a: transform.position,
                b: new Vector2(m_Player.position.x + m_Offset.x, m_Player.position.y + m_Offset.y),
                t: m_TimeOffset * Time.deltaTime
            );

            transform.position = new Vector3
            (
                x: Mathf.Clamp(transform.position.x, m_MinPos.x, m_MaxPos.x),
                y: Mathf.Clamp(transform.position.y, m_MinPos.y, m_MaxPos.y),
                z: k_CAMERA_DEPTH
            );
        }

    }
}

