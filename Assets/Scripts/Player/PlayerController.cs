using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.Attacker;


namespace Endless.PlayerCore
{

    public class PlayerController : MonoBehaviour
    {
        // For the Billboard Script..
        public static PlayerController instance;

        public bool CanMove { get; private set; } = true;
        private bool IsSprinting => canSprint && Input.GetKey(sprintKey);
        private bool ShouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;
        private bool ShouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchAnimation && characterController.isGrounded;

        [Header("Functional Options")]
        [SerializeField] private bool canSprint = true;
        [SerializeField] private bool canJump = true;
        [SerializeField] private bool canCrouch = true;
        [SerializeField] private bool canUseHeadbob = true;

        [Header("Controls")]
        [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;
        [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

        [Header("Movement Parameters")] // Using header to organize stuff in the Unity inspector.
        [SerializeField] private float runSpeed = 3.0f;
        [SerializeField] private float sprintSpeed = 6.0f;
        [SerializeField] private float crouchSpeed = 3.0f;

        [Header("Look Parameters")] // Using header to organize stuff in the Unity inspector.
        [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
        [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
        [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
        [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

        [Header("Jumping Parameters")]
        [SerializeField] private float jumpForce = 8.0f;
        [SerializeField] private float gravity = 30.0f;

        [Header("Headbob parameters")]
        [SerializeField] private float runBobSpeed = 14f;
        [SerializeField] private float runBobAmount = 0.5f;
        [SerializeField] private float sprintBobSpeed = 18f;
        [SerializeField] private float sprintBobAmount = 1f;
        [SerializeField] private float crouchBobSpeed = 8f;
        [SerializeField] private float crouchBobAmount = 0.25f;
        private float defaultYPos = 0;
        private float timer;

        [Header("Crouch Parameters")]
        [SerializeField] private float crouchHeight = 0.5f;
        [SerializeField] private float standingHeight = 2.0f;
        [SerializeField] private float timeToCrouch = 0.25f;
        [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
        [SerializeField] private Vector3 standingCenter = new Vector3(0, 0, 0);
        private bool isCrouching;
        private bool duringCrouchAnimation;

        [Header("Guns!!")]
        [SerializeField] GameObject GrenadeProjectile;
        public Animator gunAnim;
        public int currentAmmo = 20;

        private Camera playerCamera;
        private CharacterController characterController;

        private Vector3 moveDirection;
        private Vector2 currentInput;

        private float rotationX = 0;
        public bool PlayerDead = false;

        void Awake()
        {
            instance = this;
            playerCamera = GetComponentInChildren<Camera>();
            characterController = GetComponent<CharacterController>();
            defaultYPos = playerCamera.transform.localPosition.y;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            if (!PlayerDead)
            {
                if (CanMove)
                {
                    HandleMovementInput();
                    HandleMouseLook();

                    if (canJump)
                        HandleJump();

                    if (canCrouch)
                        HandleCrouch();

                    if (canUseHeadbob)
                        HandleHeadBob();

                    ApplyFinalMovements();
                }

                // Shooting..
                if (Input.GetMouseButtonDown(0))
                {
                    if (currentAmmo > 0)
                    {
                        Ray ray = playerCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            Debug.Log("I'm shooting at" + hit.transform.name);
                        }
                        else
                        {
                            Debug.Log("I'm looking at nothing");
                        }
                        currentAmmo--;
                        gunAnim.SetTrigger("TriggerShooting");
                    }
                }

                // Grenade stuffs
                if (Input.GetKeyDown(KeyCode.G))
                {
                    GameObject.Find(GrenadeProjectile.name).GetComponent<Grenade>().ThrowGrenade(GrenadeProjectile, this.gameObject);
                }
            }
        }

        private void HandleMovementInput()
        {
            currentInput = new Vector2((isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : runSpeed) * Input.GetAxis("Vertical"), (isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : runSpeed) * Input.GetAxis("Horizontal"));

            float moveDirectionY = moveDirection.y;
            moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
            moveDirection.y = moveDirectionY;
        }

        private void HandleMouseLook()
        {
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
            rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
        }

        private void HandleJump()
        {
            if (ShouldJump)
                moveDirection.y = jumpForce;
        }

        private void HandleCrouch()
        {
            if (ShouldCrouch)
                StartCoroutine(CrouchStand());
        }

        private void HandleHeadBob()
        {
            if (!characterController.isGrounded)
                return;

            if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
            {
                timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : runBobSpeed);
                playerCamera.transform.localPosition = new Vector3(
                    playerCamera.transform.localPosition.x,
                    defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : runBobAmount),
                    playerCamera.transform.localPosition.z);
            }
        }

        private void ApplyFinalMovements()
        {
            if (!characterController.isGrounded)
                moveDirection.y -= gravity * Time.deltaTime;

            characterController.Move(moveDirection * Time.deltaTime);
        }

        //private void Shooting()
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        if (currentAmmo > 0)
        //        {
        //            Ray ray = playerCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        //            RaycastHit hit;
        //            if (Physics.Raycast(ray, out hit))
        //            {
        //                Debug.Log("I'm shooting at" + hit.transform.name);
        //            }
        //            else
        //            {
        //                Debug.Log("I'm looking at nothing");
        //            }
        //            currentAmmo--;
        //            gunAnim.SetTrigger("TriggerShooting");
        //        }
        //    }
        //}

        private IEnumerator CrouchStand()
        {
            // If anything is above the players head, the player won't be able to stand and clip through any ceiling.
            if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
                yield break;

            // If not then run as usual.

            duringCrouchAnimation = true;

            float timeElapsed = 0;
            float targetHeight = isCrouching ? standingHeight : crouchHeight;
            float currentHeight = characterController.height;
            Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
            Vector3 currentCenter = characterController.center;

            while (timeElapsed < timeToCrouch)
            {
                characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
                characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // Make sure not to get some strange values in between frames.
            characterController.height = targetHeight;
            characterController.center = targetCenter;

            isCrouching = !isCrouching;

            duringCrouchAnimation = false;
        }
    }
}