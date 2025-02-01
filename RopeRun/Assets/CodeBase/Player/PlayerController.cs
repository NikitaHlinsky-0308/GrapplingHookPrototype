using System;
using System.Collections;
using CodeBase.Camera;
using CodeBase.Interactional.Interface;
using CodeBase.Managers;
using CodeBase.Managers.UI;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player")] public Rigidbody playerRb;
        public Transform gunTip, camera, player;

        [Header("Grappling")] [SerializeField] private Vector3 grapplePoint;
        [SerializeField] private float _jointSpring = 10f;
        [SerializeField] private float _grapplePull = 3f;
        [SerializeField] private float maxDistance = 100f;
        [SerializeField] private LayerMask whatIsGrappleable;
        [SerializeField] private LayerMask speedLayer;
        [SerializeField] private LayerMask stopLayer;
        [SerializeField] private LayerMask finishLayer;
        [SerializeField] private UnityEngine.Camera mainCamera;
        [SerializeField] private bool freeGrappleMode;

        public bool isFixedLength;
        private float defaultRBdrag;
        private SpringJoint joint;
        private bool isObjectInSphere;
        private Vector3 closestObjectInSphere;
        private bool isGameBegin;
        private bool isStopZone;


        private void Start()
        {
            playerRb = GetComponent<Rigidbody>();
            defaultRBdrag = playerRb.drag;

            mainCamera.GetComponent<CameraFollow>().Follow(this.gameObject.transform);
        }


        private IEnumerator GrapplePull()
        {
            while (joint != null && joint.maxDistance > 0f)
            {
                if (!isFixedLength)
                {
                    joint.maxDistance -= Time.deltaTime * _grapplePull; // Уменьшать длину, только если не фиксированная
                }

                yield return null;
            }
        }

        private void StartGrapple(Vector3 mousePoint)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, mousePoint - transform.position, out hit, Mathf.Infinity,
                    whatIsGrappleable))
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

                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.OnHit(this); // Передача текущего скрипта Player
                }

                // Add a force to pull the player towards the grapple point
                //playerRb.AddForce((grapplePoint - player.position).normalized * 2f, ForceMode.Impulse);

                // Start the coroutine to gradually decrease the rope length
                StartCoroutine(GrapplePull());
            }
        }

        private void AdjustableGrapple(Vector3 targetPoint)
        {
            if (targetPoint != null && player)
            {
                grapplePoint = targetPoint;
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

                Collider[] hitColliders = Physics.OverlapSphere(targetPoint, 0.1f, whatIsGrappleable);
                if (hitColliders.Length > 0)
                {
                    IInteractable interactable = hitColliders[0].GetComponent<IInteractable>();
                    if (interactable != null)
                    {
                        interactable.OnHit(this);
                    }
                }
                else
                {
                    Debug.LogWarning("No interactable object found at grapple point.");
                }

                // Add a force to pull the player towards the grapple point
                //playerRb.AddForce((grapplePoint - player.position).normalized * 2f, ForceMode.Impulse);

                // Start the coroutine to gradually decrease the rope length
                StartCoroutine(GrapplePull());
            }
        }

        private void UseAutoGrapple(Vector3 obj)
        {
            if (isGameBegin)
            {
                if (isObjectInSphere && isObjectInSphere != false)
                {
                    AdjustableGrapple(closestObjectInSphere);
                }
            }
            else
            {
                isGameBegin = true;
                playerRb.useGravity = true;
            }
        }


        void StopGrapple()
        {
            Destroy(joint);
            playerRb.drag = defaultRBdrag;
            isFixedLength = false;
        }


        public bool IsGrappling() =>
            joint != null;

        public Vector3 GetGrapplePoint() =>
            grapplePoint;

        private void HandleCollide(Collision obj)
        {
            if (obj.gameObject.layer == finishLayer)
            {
                // change to UI finish panel 
                //LevelManager.Instance.LoadNextLevel();
                UIManager.Instance.ShowFinishPanel();
                Debug.Log("collide finish");
            }
        }
        
        private void HandleTrigger(Collider obj)
        {
            if (obj.gameObject.layer == stopLayer)
            {
                isStopZone = true;
                //Debug.Log("isStopZone enabled");
            }
        }


        private void HandleSphericalCollide(Collider obj)
        {
            isObjectInSphere = true;
            closestObjectInSphere = obj.transform.position;
        }

        private void HandleNoSphericalCollide()
        {
            isObjectInSphere = false;
        }

        private void HandleClick(Vector3 obj)
        {
            if (!freeGrappleMode)
            {
                UseAutoGrapple(obj);
            }
            else
            {
                StartGrapple(obj);
            }
        }
        
        void OnEnable()
        {
            if (!freeGrappleMode)
            {
                InputHandler.OnMouseRelease += StopGrapple;
                PlayerDetection.OnClosestObjectDetected += HandleSphericalCollide;
                PlayerDetection.OnNoObjectsDetected += HandleNoSphericalCollide;
            }
            else
            {
                InputHandler.OnMouseRelease += StopGrapple;
            }

            InputHandler.OnMouseClick += HandleClick;
            PlayerDetection.OnTrigger += HandleTrigger;
            PlayerDetection.OnCollide += HandleCollide;
        }



        void OnDisable()
        {
            InputHandler.OnMouseClick -= StartGrapple;
            InputHandler.OnMouseRelease -= StopGrapple;
            PlayerDetection.OnTrigger -= HandleTrigger;
            PlayerDetection.OnCollide -= HandleCollide;
            PlayerDetection.OnClosestObjectDetected -= HandleSphericalCollide;
            PlayerDetection.OnNoObjectsDetected -= HandleNoSphericalCollide;
        }
    }
}