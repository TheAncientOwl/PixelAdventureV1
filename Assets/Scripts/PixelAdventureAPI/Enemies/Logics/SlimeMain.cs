using PixelAdventureAPI.PlayerLogics;
using PixelAdventureAPI.Enemies.API;
using UnityEngine;

namespace PixelAdventureAPI.Enemies.Logics
{
    /// <summary>
    /// Manages SimpleCollision
    /// </summary>
    public class SlimeMain : MonoBehaviour
    {
        [Header("On Death")]
        [SerializeField] private float m_PlayerJumpVelocity = 9f;
        [SerializeField] private int m_Points = 5;

        public void Start()
        {
            SimpleCollision collision = GetComponent<SimpleCollision>();
            collision.AddOnDeath(OnDeathEvent);
        }

        private void OnDeathEvent()
        {
            PlayerScore.GetInstance().Value += m_Points;
            m_Points = 0;
            GetComponent<Animator>().SetTrigger("hit");
            GetComponent<BasicMovement>().StopAllCoroutines();
            Rigidbody2D playerRigidbody2d = PlayerMovement.GetInstance().GetRigidbody2D();
            playerRigidbody2d.velocity = new Vector2(playerRigidbody2d.velocity.x, m_PlayerJumpVelocity);
            Destroy(this.gameObject, 0.49f);
        }

    }
}