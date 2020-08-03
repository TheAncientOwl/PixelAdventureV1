using PixelAdventureAPI.PlayerLogics;
using PixelAdventureAPI.Enemies.API;
using UnityEngine;

namespace PixelAdventureAPI.Enemies.Logics
{
    /// <summary>
    /// Manages SimpleMovement and SimpleCollision
    /// </summary>
    public class AngryPigMain : MonoBehaviour
    {
        private Animator m_Animator = null;
        private static readonly int k_HIT_HASH     = Animator.StringToHash("hit");
        private static readonly int k_AGGRO_HASH   = Animator.StringToHash("aggro");
        private static readonly int k_WALKING_HASH = Animator.StringToHash("isWalking");

        [Header("On Death")]
        [SerializeField] private float m_PlayerJumpVelocity = 7f;
        [SerializeField] private int m_Points = 10;

        public void Start()
        {
            m_Animator = GetComponent<Animator>();

            SimpleMovement movement = GetComponent<SimpleMovement>();
            movement.AddOnStart(() => m_Animator.SetBool(k_WALKING_HASH, true));
            movement.AddOnIdle(() => m_Animator.SetBool(k_WALKING_HASH, false));
            movement.AddAfterIdle(() => m_Animator.SetBool(k_WALKING_HASH, true));
            movement.AddOnAggro((float aggroTimer) => m_Animator.SetBool(k_AGGRO_HASH, aggroTimer > 0f));

            SimpleCollision collision = GetComponent<SimpleCollision>();
            collision.AddOnDeath(delegate () 
            {
                PlayerScore.GetInstance().Value += m_Points;
                m_Points = 0;
                m_Animator.SetTrigger(k_HIT_HASH);
                Rigidbody2D playerRigidbody2d = PlayerMovement.GetInstance().GetRigidbody2D();
                playerRigidbody2d.velocity = new Vector2(playerRigidbody2d.velocity.x, m_PlayerJumpVelocity);
                Destroy(this.gameObject, 0.5f);
            });
        }
        
    }
}

