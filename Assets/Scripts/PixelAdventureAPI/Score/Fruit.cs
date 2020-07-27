using PixelAdventureAPI.PlayerLogics;
using UnityEngine;

namespace PixelAdventureAPI
{
    /// <summary>
    /// OnTriggerEnter2D increase player's score and destroy itself.
    /// </summary>
    public class Fruit : MonoBehaviour
    {
        [SerializeField] private int m_Value = 10;
        private static readonly string k_PLAYER_TAG = "Player";
        private static readonly int k_COLLECTED_HASH = Animator.StringToHash("isCollected");

        private static PlayerScore m_PlayerScore = null;
        private void Start() => m_PlayerScore = PlayerScore.GetInstance(); 

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(k_PLAYER_TAG))
            {
                m_PlayerScore.Value += m_Value;
                GetComponent<Animator>().SetTrigger(k_COLLECTED_HASH);
                Destroy(this.gameObject, 0.5f);
            }
        }

    }

}
