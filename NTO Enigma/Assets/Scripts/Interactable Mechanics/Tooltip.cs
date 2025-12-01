using System;
using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Collider))]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private string tooltipKey;
        [SerializeField] private float radius = 3;

        private Transform _characterTransform;

        private void Awake()
        {
            _characterTransform = FindFirstObjectByType<Character>().transform;
        }

        private void OnMouseEnter()
        {
            if (Vector3.Distance(_characterTransform.position, transform.position) > radius)
                return;
            MainGameplayUI.SetTooltip(tooltipKey, this);    
        }

        private void OnMouseExit()
        {
            MainGameplayUI.ResetTooltip();
        }
    }
}