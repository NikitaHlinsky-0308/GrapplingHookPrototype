using System;
using CodeBase.Interactional;
using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerDetection : MonoBehaviour
    {
        
        public static event Action<Collision> OnCollide;
        public static event Action<Collider> OnTrigger;
        public static event Action<Collider> OnClosestObjectDetected;
        public static event Action OnNoObjectsDetected;
        
        [SerializeField] private float sphereRadius;
        [SerializeField] private LayerMask layerToSphere;
        
        [SerializeField] private LayerMask ignoreLayer;

        private void Update()
        {
            SphereCollisionDetection(transform.position, sphereRadius, layerToSphere);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer != ignoreLayer)
            {
                OnCollide?.Invoke(other);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != ignoreLayer)
            {
                OnTrigger?.Invoke(other);
            }        
        }

        public void SphereCollisionDetection(Vector3 center, float radius, LayerMask layerMask)
        {
            Collider[] hitColliders = Physics.OverlapSphere(center, radius, layerMask);

            if (hitColliders.Length == 0)
            {
                OnNoObjectsDetected?.Invoke();
                return;
            }

            Collider closestCollider = null;
            float closestDistance = float.MaxValue;

            foreach (var hitCollider in hitColliders)
            {
                // Проверяем наличие компонента AnchorPoint и его состояние
                AnchorPoint anchorPoint = hitCollider.GetComponent<AnchorPoint>();
                if (anchorPoint != null && anchorPoint.isUsed)
                {
                    continue; // Пропускаем, если точка уже используется
                }

                float distance = Vector3.Distance(center, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCollider = hitCollider;
                }
            }

            if (closestCollider != null)
            {
                //Debug.Log("Closest object detected: " + closestCollider.name);
                OnClosestObjectDetected?.Invoke(closestCollider);
            }
            else
            {
                // Если подходящих объектов нет
                OnNoObjectsDetected?.Invoke();
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, sphereRadius);
        }
    }
}