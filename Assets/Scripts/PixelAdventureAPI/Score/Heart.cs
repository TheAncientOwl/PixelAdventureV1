using PixelAdventureAPI.PlayerLogics;
using UnityEngine;

namespace PixelAdventureAPI
{
    /// <summary>
    /// OnTriggerEnter2D increase player's health and destroy itself.
    /// </summary>
    public class Heart : MonoBehaviour
    {
        [SerializeField] private float m_HealthAmount = 10f;
        private static readonly string k_PLAYER_TAG = "Player";

        private static PlayerHealth m_PlayerHealth = null;
        private void Start() => m_PlayerHealth = PlayerHealth.GetInstance();

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(k_PLAYER_TAG))
            {
                m_PlayerHealth.Value += m_HealthAmount;
                Destroy(this.gameObject);
            }
        }

    }
}


