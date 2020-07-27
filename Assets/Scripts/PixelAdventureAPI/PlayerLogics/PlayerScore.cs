using UnityEngine;
using UnityEngine.UI;

namespace PixelAdventureAPI.PlayerLogics
{
    public class PlayerScore : MonoBehaviour
    {
        #region << instance >>
        private static PlayerScore m_Instance = null;
        public static PlayerScore GetInstance() => m_Instance;
        private void Awake() => m_Instance = this;
        #endregion

        [SerializeField] private Text m_ScoreText = null;
        private int m_Score = 0;
        /// <summary>
        /// On update also update UI score text.
        /// </summary>
        public int Value
        {
            get { return m_Score; }
            set
            {
                m_Score = value;
                m_ScoreText.text = m_Score.ToString();
            }
        }
    }
}


