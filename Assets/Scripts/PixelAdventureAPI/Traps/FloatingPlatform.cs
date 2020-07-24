using UnityEngine;

namespace PixelAdventureAPI.Traps
{
    public class FloatingPlatform : MonoBehaviour
    {
        private Transform m_Transform = null;

        [Header("Movement")]
        [SerializeField] private Vector2 m_Direction = Vector2.zero;
        [SerializeField] private float k_MOVE_TIME = 5f;

        private float m_MoveTimer = 0f;

        private void Start() 
        {  
            m_Transform = GetComponent<Transform>();
        }

        private void Update()
        {
            if (m_MoveTimer > k_MOVE_TIME)
            {
                m_MoveTimer = 0f;
                m_Direction *= -1;
            }
            else
                m_MoveTimer += Time.deltaTime;

            m_Transform.Translate(m_Direction.x * Time.deltaTime, m_Direction.y * Time.deltaTime, 0f);
        }
       
    }

}

