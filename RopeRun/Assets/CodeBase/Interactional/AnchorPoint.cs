using System;
using System.Collections.Generic;
using CodeBase.Interactional.Interface;
using CodeBase.Player;
using UnityEngine;

namespace CodeBase.Interactional
{
    public class AnchorPoint : MonoBehaviour, IInteractable
    {
        
        [SerializeField] private LayerMask ignoreLayer;
        [SerializeField] private float accelerationAmount = 5;
        [SerializeField] private ForceMode forceMode = ForceMode.Impulse;
        [SerializeField] private Material[] materials;
        
        [HideInInspector] public bool isUsed = false;
        private Renderer anchorRenderer;
        
        private void Start()
        {
            anchorRenderer = gameObject.GetComponentInChildren<Renderer>();
            anchorRenderer.material = materials[0];
        }

        public void OnHit(PlayerController player)
        {
            isUsed = true;
            player.isFixedLength = true;
            player.playerRb.drag = 0;
            anchorRenderer.material = materials[1];
            
            ApplyAcceleration(player);
        }

        private void ApplyAcceleration(PlayerController player)
        {
            Vector3 currentDir = player.playerRb.velocity.normalized;
            player.playerRb.AddForce(currentDir * accelerationAmount, forceMode);
        }
    }
}
