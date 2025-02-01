using UnityEngine;

namespace CodeBase.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        private Transform _following;
        
        [Header("Camera Follow")][Space]
        [Range(-180, 180)][SerializeField] private float _rotationAngleX = 0f;
        [Range(-180, 180)][SerializeField] private float _rotationAngleY = -90f;
        [SerializeField] private Vector3 _distance = new Vector3(15f, 0f, 0f);
        [Range(0, 10)][SerializeField] private float _smoothness = 4f;
        [SerializeField] private float _lookAheadDistance = 5f; // Расстояние взгляда камеры вперед

        private Vector3 _previousPosition;

        private void Start()
        {
            if (_following != null)
            {
                _previousPosition = _following.position;
            }
        }

        private void Update() => Move();

        public void Follow(Transform following)
        {
            _following = following.transform;
            _previousPosition = following.position;
        }

        private void Move()
        {
            if (_following == null) return;

            // Вычисляем направление движения игрока
            Vector3 direction = _following.position - _previousPosition;
            direction = direction.normalized;

            // Определяем позицию камеры с учетом направления движения
            Vector3 lookAhead = direction * _lookAheadDistance;
            Vector3 targetPosition = _following.position + _distance + lookAhead;

            Vector3 nextPosition = Vector3.Lerp(
                transform.position,
                targetPosition,
                Time.deltaTime * _smoothness);

            Quaternion rotation = Quaternion.Euler(_rotationAngleX, _rotationAngleY, 0);

            transform.rotation = rotation;
            transform.position = nextPosition;

            // Обновляем предыдущую позицию игрока
            _previousPosition = _following.position;
        }
        
    }
}