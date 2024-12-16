using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 grapplePoint;
    public float _jointSpring = 10f;
    public float _grapplePull = 3f;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    public Rigidbody playerRb;
    private float maxDistance = 100f;
    private SpringJoint joint;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Debug.Log("PlayerSpeed: " + playerRb.velocity);
    }

    // void LateUpdate()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         StartGrapple();
    //     }
    //     else if (Input.GetMouseButtonUp(0))
    //     {
    //         StopGrapple();
    //     }
    // }


    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    
    
    private IEnumerator GrapplePull()
    {
        while (joint != null && joint.maxDistance > 0f)
        {
            joint.maxDistance -= Time.deltaTime * _grapplePull; // Adjust the speed of the pull
            yield return null;
        }
    }
    
    void StartGrapple()
    {
        //RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            // The distance grapple will try to keep from grapple point.
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = 0f; // Set to 0 to pull the player all the way to the grapple point

            // Adjust these values to fit your game.
            joint.spring = _jointSpring; // Increase spring value to pull the player faster
            joint.damper = 7f;
            joint.massScale = 4.5f;

            // Add a force to pull the player towards the grapple point
            //playerRb.AddForce((grapplePoint - player.position).normalized * 2f, ForceMode.Impulse);

            // Start the coroutine to gradually decrease the rope length
            StartCoroutine(GrapplePull());
        }
    }

    void StartGrapple(Vector3 mousePoint)
    {
        //Ray ray = transform.position; 
        RaycastHit hit;

        if (Physics.Raycast(transform.position, mousePoint - transform.position, out hit, Mathf.Infinity, whatIsGrappleable))
        {

            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            // The distance grapple will try to keep from grapple point.
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = 0f; // Set to 0 to pull the player all the way to the grapple point

            // Adjust these values to fit your game.
            joint.spring = _jointSpring; // Increase spring value to pull the player faster
            joint.damper = 7f;
            joint.massScale = 4.5f;

            // Add a force to pull the player towards the grapple point
            //playerRb.AddForce((grapplePoint - player.position).normalized * 2f, ForceMode.Impulse);

            // Start the coroutine to gradually decrease the rope length
            StartCoroutine(GrapplePull());
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple()
    {
        Destroy(joint);
    }


    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }

    void OnEnable()
    {
        MouseControls.OnMouseClick += StartGrapple;
        MouseControls.OnMouseRelease += StopGrapple;
    }

    void OnDisable()
    {
        MouseControls.OnMouseClick -= StartGrapple;
        MouseControls.OnMouseRelease -= StopGrapple;
    }
}