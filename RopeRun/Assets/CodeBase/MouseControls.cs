using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControls : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject _target;
    
    public static event System.Action<Vector3> OnMouseClick;
    public static event System.Action OnMouseRelease;



    void Start()
    {
        _camera = Camera.main;

    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Ray rayOrigin = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(rayOrigin, out hit, Mathf.Infinity, layerMask))
            {

                _target.transform.position = hit.point;
                OnMouseClick?.Invoke(hit.point);
                //hit.point = new Vector3(hit.point.x, hit.point.y, 0);
                //Debug.Log(hit.collider.name);
            }
        }
        if(Input.GetButtonUp("Fire1"))
        {
            OnMouseRelease?.Invoke();
        }
    }
}
