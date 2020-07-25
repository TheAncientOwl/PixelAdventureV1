using UnityEngine;

namespace PixelAdventureAPI.Others
{
    public class Camera2D : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform m_Player = null;

        [Header("Free Zone")]
        [SerializeField] private Vector2 m_Offset = Vector2.zero;

        [Header("Movement")]
        [SerializeField] private float m_Speed = 3f;
        [SerializeField] private Rigidbody2D m_PlayerRigidbody2D = null;

        [Header("Bounds")]
        [SerializeField] private float m_Left = 0f;
        [SerializeField] private float m_Right = 0f;
        [SerializeField] private float m_Top = 0f;
        [SerializeField] private float m_Bottom = 0f;

        private const float k_CAMERA_DEPTH = -5f;
        private Vector2 m_Threshold = Vector2.zero;

        private void Start() => m_Threshold = CalculateThreshold();

        private void FixedUpdate() 
        {
            Vector3 newPosition = transform.position;

            float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * m_Player.position.x);
            float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * m_Player.position.y);

            if (Mathf.Abs(xDifference) >= m_Threshold.x)
                newPosition.x = m_Player.position.x;

            if (Mathf.Abs(yDifference) >= m_Threshold.y)
                newPosition.y = m_Player.position.y;

            float moveSpeed = m_PlayerRigidbody2D.velocity.magnitude > m_Speed ? m_PlayerRigidbody2D.velocity.magnitude : m_Speed;

            transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
            
            transform.position = new Vector3
            (
                x: Mathf.Clamp(transform.position.x, m_Left, m_Right),
                y: Mathf.Clamp(transform.position.y, m_Top, m_Bottom),
                z: k_CAMERA_DEPTH
            ); 
        }

        private Vector2 CalculateThreshold()
        {
            Camera m_Camera = Camera.main;
            Rect aspect = m_Camera.pixelRect;
            Vector2 t = new Vector2
            (
                x : m_Camera.orthographicSize * aspect.width / aspect.height,
                y : m_Camera.orthographicSize
            ); 
            t.x -= m_Offset.x;
            t.y -= m_Offset.y;

            return t;
        }

        private void OnDrawGizmos() 
        {
            Gizmos.color = Color.blue;
            Vector2 border = CalculateThreshold();
            Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2f, border.y * 2f, 1f));    
        }

    }
}


