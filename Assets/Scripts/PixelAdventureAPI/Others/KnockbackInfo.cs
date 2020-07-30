using UnityEngine;

namespace PixelAdventureAPI.Others
{
    [System.Serializable]
    public class KnockbackInfo
    {
        public float duration = 0.08f;
        public Vector2 force = new Vector2(1.53f, 0.35f);
    }
}