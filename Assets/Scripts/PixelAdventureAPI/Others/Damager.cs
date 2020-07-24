using PixelAdventureAPI.PlayerLogics;
using UnityEngine;

namespace PixelAdventureAPI.Others
{
    public class Damager : MonoBehaviour
    {
        private static readonly int k_DAMAGED_HASH = Animator.StringToHash("isDamaged");
        [SerializeField] private float m_Damage = 10f;
        private static Animator m_PlayerAnimator = null;
        private static PlayerHealth m_PlayerHealth = null;

        private void Start()
        {
            m_PlayerAnimator = PlayerMovement.GetInstance().GetPlayerAnimator();
            m_PlayerHealth = PlayerHealth.GetInstance();
        }

        public void Apply()
        {
            m_PlayerHealth.Value -= m_Damage;
            m_PlayerAnimator.SetTrigger(k_DAMAGED_HASH);
        }

    }
}


