﻿using PixelAdventureAPI.PlayerLogics;
using PixelAdventureAPI.Others;
using UnityEngine;

namespace PixelAdventureAPI.Traps
{
    public class Saw : MonoBehaviour
    {
        private static readonly string k_PLAYER_TAG = "Player";
        private static readonly int k_ON_HASH = Animator.StringToHash("on");

        [SerializeField] private Knockback m_Knockback = null;

        private static PlayerMovement m_PlayerMovement = null;

        private CircleCollider2D m_CircleCollider2D = null;
        private Damager m_Damager = null;

        private void Start()
        {
            m_PlayerMovement = PlayerMovement.GetInstance();
            m_CircleCollider2D = GetComponent<CircleCollider2D>();
            m_Damager = GetComponent<Damager>();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(k_PLAYER_TAG))
            {
                m_Damager.Apply();

                m_PlayerMovement.ApplyKnockback360
                (
                    knockback      : m_Knockback,
                    colliderCenter : m_CircleCollider2D.bounds.center
                );

            }
        }

        
    }

}

