using UnityEngine;

namespace PixelAdventureAPI.Others
{
    /// <summary>
    /// Camera follow player.
    /// Freezone.
    /// Moves by Vector3.Lerp.
    /// Bounds.
    /// </summary>
    public class Camera2D : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform m_Player = null;

        [Header("Free Zone")]
        [SerializeField] private Vector2 m_Offset = Vector2.zero;

        [Header("Bounds")]
        [SerializeField] private float m_Left = 0f;
        [SerializeField] private float m_Right = 0f;
        [SerializeField] private float m_Top = 0f;
        [SerializeField] private float m_Bottom = 0f;

        private const float k_CAMERA_DEPTH = -5f;
        private Vector2 m_Threshold = Vector2.zero;

        private void Start() => m_Threshold = CalculateThreshold();

        /// Move camera
        private void LateUpdate() 
        {
            Vector3 newPosition = transform.position;

            if (Mathf.Abs(transform.position.x - m_Player.position.x) >= m_Threshold.x)
                newPosition.x = m_Player.position.x;

            if (Mathf.Abs(transform.position.y - m_Player.position.y) >= m_Threshold.y)
                newPosition.y = m_Player.position.y;

            if (newPosition != transform.position)
            {
                transform.position = Vector3.Lerp
                (
                    a : transform.position,
                    b : newPosition,
                    t : Time.deltaTime
                );

                transform.position = new Vector3
                (
                    x: Mathf.Clamp(transform.position.x, m_Left, m_Right),
                    y: Mathf.Clamp(transform.position.y, m_Top, m_Bottom),
                    z: k_CAMERA_DEPTH
                );
            }
            
        }

        private Vector2 CalculateThreshold()
        {
            Camera camera = Camera.main;
            Rect aspect = camera.pixelRect;
            Vector2 t = new Vector2
            (
                x: camera.orthographicSize * aspect.width / aspect.height,
                y: camera.orthographicSize
            );
            t -= m_Offset;

            return t;
        }

        // Draw the freezone.
        private void OnDrawGizmos() 
        {
            Gizmos.color = Color.blue;
            Vector2 border = CalculateThreshold();
            Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2f, border.y * 2f, 1f));    
        }

    }
}


