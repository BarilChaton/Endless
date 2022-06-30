using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Endless.Attacker;
using Endless.CooldownCore;

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
        [SerializeField] public bool canUseHeadbob = true;

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
        [SerializeField] string defaultWeapon = "Shotgun";
        private GunCore currWeap;

        private Camera playerCamera;
        private CharacterController characterController;

        private Vector3 moveDirection;
        private Vector2 currentInput;

        private float rotationX = 0;
        public bool PlayerDead = false; /// TODO: FIX PLAYERDEAD IN ITS OWN WAY TO LESS PRESSURE UPDATE

        // Hiding the cooldown amount as I only want scripts to edit the number
        private float GrenadeCd;
        [HideInInspector] public float GrenadeCdAmt = 3f;

        void Awake()
        {
            instance = this;
            playerCamera = GetComponentInChildren<Camera>();
            characterController = GetComponent<CharacterController>();
            defaultYPos = playerCamera.transform.localPosition.y;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            currWeap = GameObject.Find(defaultWeapon).GetComponent<GunCore>();
        }

        void Update()
        {
            // General movement stuff goes here
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

            // Checking for machine gun. If no, single clicks. Else, hold mouse to shoot.
            HandleShoot();

            // Grenade stuffs
            if (Input.GetKeyDown(KeyCode.G) && GrenadeCd < Time.time)
            {
                ThrowGrenade();
            }

        }

        private void HandleShoot()
        {
            if (Input.GetMouseButton(0) && currWeap.CurrentCD < Time.time)
            {
                currWeap.CurrentCD = Cooldown.CdCalc(currWeap.ShotCooldown);
                ShootGunMain(currWeap);
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
            if (ShouldCrouch || Input.GetKeyUp(crouchKey)) StartCoroutine(CrouchStand());
            //if () ;
        }

        private void HandleHeadBob()
        {
            if (!characterController.isGrounded)
                return;
            GunCore gunCore = currWeap.GetComponent<GunCore>();
            if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
            {
                timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : runBobSpeed);
                playerCamera.transform.localPosition = new Vector3(
                    playerCamera.transform.localPosition.x,
                    defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : runBobAmount),
                    playerCamera.transform.localPosition.z);

                // Movement animation for gun
                if (!gunCore.gunAnim.GetBool("RunTrigger")) gunCore.gunAnim.SetBool("RunTrigger", true);
            }
            else
            {
                gunCore.gunAnim.SetBool("RunTrigger", false);
            }
        }

        private void ApplyFinalMovements()
        {
            if (!characterController.isGrounded)
                moveDirection.y -= gravity * Time.deltaTime;

            characterController.Move(moveDirection * Time.deltaTime);
        }

        private IEnumerator CrouchStand()
        {
            // If anything is above the players head, the player won't be able to stand and clip through any ceiling.
            if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
                yield break;
            // If not then run as usual.

            // Hold key stuff m8
            bool crouchAnimRelease = false;
            if (duringCrouchAnimation)
            {
                crouchAnimRelease = true;
                isCrouching = !isCrouching;
            }

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

            if (!crouchAnimRelease) isCrouching = !isCrouching;

            duringCrouchAnimation = false;
            crouchAnimRelease = false;
        }

        private void ThrowGrenade()
        {
            // Activate Cooldown
            GrenadeCd = Cooldown.CdCalc(GrenadeCdAmt);

            // Spawn Grenade and do its script thing
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0);
            GameObject thrownGrendade = Instantiate(GrenadeProjectile, spawnPosition, Camera.main.transform.rotation);

            // Using grenade's script to do the rest
            GameObject.Find(thrownGrendade.name).GetComponent<Grenade>().ThrowGrenade(thrownGrendade);
        }

        private void ShootGunMain(GunCore currWeap)
        {
            currWeap.ShootGun(playerCamera);
        }
    }
}