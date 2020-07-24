using PixelAdventureAPI.PlayerLogics;
using UnityEngine;

namespace PixelAdventureAPI.Traps
{
    public class Trampoline : MonoBehaviour
    {
        private static readonly int k_JUMP_HASH = Animator.StringToHash("jump");
        private static readonly string k_PLAYER_TAG = "Player";

        [SerializeField] private float m_JumpForce = 10f;

        private static Rigidbody2D m_PlayerRigidbody2D = null;
        private Animator m_Animator = null;

        private void Start() 
        {
            m_PlayerRigidbody2D = PlayerMovement.GetInstance().GetRigidbody2D();
            m_Animator = GetComponent<Animator>(); 
        }

        private void OnTriggerEnter2D(Collider2D collider) 
        {
            if (collider.CompareTag(k_PLAYER_TAG))
            {
                m_Animator.SetTrigger(k_JUMP_HASH);
                m_PlayerRigidbody2D.velocity = new Vector2
                (
                    x : m_PlayerRigidbody2D.velocity.x,
                    y : m_JumpForce
                );
            }
        }


    }

}

