using UnityEngine;

namespace CodeBase
{
    public class InputHandler : MonoBehaviour
    {

        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private LayerMask layerMask;
    
        public static event System.Action<Vector3> OnMouseClick;
        public static event System.Action OnMouseRelease;
    


        void Start()
        {
            _camera = UnityEngine.Camera.main;

        }

        void Update()
        {
            if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
            {
                HandleClick(Input.mousePosition);
            }

            if (Input.GetButtonUp("Fire1") || Input.GetKeyUp(KeyCode.Space))
            {
                OnMouseRelease?.Invoke();
            }
        }

        private void HandleClick(Vector3 screenPosition)
        {
            Ray rayOrigin = _camera.ScreenPointToRay(screenPosition);
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, out hit, Mathf.Infinity, layerMask))
            {
                OnMouseClick?.Invoke(hit.point);
            }
        }
    
    }
}
