using PixelAdventureAPI.PlayerLogics;
using PixelAdventureAPI.Enemies.API;
using UnityEngine;

namespace PixelAdventureAPI.Enemies.Logics
{
    public class AngryPigController : MonoBehaviour
    {
        private Animator m_Animator = null;
        private static readonly int k_AGGRO_HASH = Animator.StringToHash("aggro");
        private static readonly int k_WALKING_HASH = Animator.StringToHash("isWalking");
        private static readonly int k_HIT_HASH = Animator.StringToHash("hit");

        [Header("On Death")]
        [SerializeField] private float m_PlayerJumpVelocity = 7f;
        [SerializeField] private int m_Points = 10;

        public void Start()
        {
            m_Animator = GetComponent<Animator>();

            SimpleMovement movement = GetComponent<SimpleMovement>();
            movement.AddOnStart(OnStartEvent);
            movement.AddOnIdle(OnIdleEvent);
            movement.AddAfterIdle(AfterIdleEvent);
            movement.AddOnAggro(OnAggroEvent);

            SimpleCollision collision = GetComponent<SimpleCollision>();
            collision.AddOnDeath(OnDeathEvent);
        }

        #region << animation events >>
        private void OnStartEvent() => m_Animator.SetBool(k_WALKING_HASH, true);
        private void OnIdleEvent() => m_Animator.SetBool(k_WALKING_HASH, false);
        private void AfterIdleEvent() => m_Animator.SetBool(k_WALKING_HASH, true);
        private void OnAggroEvent(float aggroTimer) => m_Animator.SetBool(k_AGGRO_HASH, aggroTimer > 0f);
        #endregion

        private void OnDeathEvent()
        {
            PlayerScore.GetInstance().Value += m_Points;
            m_Points = 0;
            m_Animator.SetTrigger(k_HIT_HASH);
            Rigidbody2D playerRigidbody2d = PlayerMovement.GetInstance().GetRigidbody2D();
            playerRigidbody2d.velocity = new Vector2(playerRigidbody2d.velocity.x, m_PlayerJumpVelocity);
            Destroy(this.gameObject, 0.5f);
        }
    }

}

