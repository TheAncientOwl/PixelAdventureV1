using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace PixelAdventureAPI.PlayerLogics
{
    public class PlayerHealth : MonoBehaviour
    {
        #region << instance >>
        private static PlayerHealth m_Instance = null;
        public static PlayerHealth GetInstance() => m_Instance;
        private void Awake() => m_Instance = this;
        #endregion
        
        [SerializeField] private Slider m_HealthBarSlider = null;
        private const float k_MAX_HEALTH = 100000000000f;
        private float m_Health = k_MAX_HEALTH;

        /// <summary>
        /// On update, also update UI healthbar.
        /// </summary>
        public float Value
        {
            get { return m_Health; }
            set
            {
                m_Health = value;
                m_HealthBarSlider.value = m_Health;
                if (m_Health <= 0)
                {
                    SceneManager.LoadScene("GameDemo");
                }
                m_Health = Mathf.Min(m_Health, k_MAX_HEALTH);
            }
        }
    }
}


