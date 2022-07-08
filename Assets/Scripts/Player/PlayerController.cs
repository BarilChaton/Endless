using System;
using System.Collections;
using UnityEngine;
using Endless.Attacker;
using Endless.CooldownCore;
using Endless.GunSwap;

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
        [SerializeField] private bool canUseFlashlight = true;
        [SerializeField] private bool canCrouch = true;
        [SerializeField] public bool canUseHeadbob = true;
        [SerializeField] private bool canInteract = true;

        [Header("Controls")]
        [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;
        [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        [SerializeField] private KeyCode flashLightKey = KeyCode.F;

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

        [Header("Interaction")]
        [SerializeField] private Vector3 interactionRayPoint = default;
        [SerializeField] private float interactionDistance = default;
        [SerializeField] LayerMask interactionLayer = default;
        private Interactable currentInteractable;

        [Header("Guns!!")]
        [SerializeField] GameObject GrenadeProjectile;
        [SerializeField] public GunCore currWeap;

        [Header("Flashlight")]
        [SerializeField] GameObject flashLight;
        [SerializeField] public GameObject deathScreen;

        private Camera playerCamera;
        private CharacterController characterController;

        private Vector3 moveDirection;
        private Vector2 currentInput;

        private float rotationX = 0;
        public bool PlayerDead = false; /// TODO: FIX PLAYERDEAD IN ITS OWN WAY TO LESS PRESSURE UPDATE

        // Hiding the cooldown amount as I only want scripts to edit the number
        [HideInInspector] private float GrenadeCd;
        [HideInInspector] public float GrenadeCdAmt = 3f;

        void Awake()
        {
            instance = this;
            playerCamera = Camera.main;
            characterController = GetComponent<CharacterController>();
            defaultYPos = playerCamera.transform.localPosition.y;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            currWeap = GetComponent<WeaponSwapper>().defaultGun.GetComponent<GunCore>();
        }

        private void OnEnable()
        {
            Cursor.visible = false;
        }

        void Update()
        {
            currWeap = GetComponent<WeaponSwapper>().currentGun.GetComponent<GunCore>();
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

                if (canUseFlashlight)
                    HandleFlashlight();

                if (canInteract)
                {
                    HandleInteractionCheck();
                    HandleInteractionInput();
                }

                ApplyFinalMovements();
            }

            // Checking for machine gun. If no, single clicks. Else, hold mouse to shoot.
            HandleShoot();

            // Grenade stuffs
            if (Input.GetKeyDown(KeyCode.G) && GrenadeCd < Time.time)
            {
                ThrowGrenade();
            }

            // Weapon  stuff
            HandleWeaponSwap();

            // PAUSE
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HandlePause();
            }
        }

        private void HandlePause()
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            GetComponent<Pause>().enabled = true;
            GetComponent<PlayerController>().enabled = false;
        }

        private void HandleInteractionCheck()
        {
            if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
            {
                if (hit.collider.gameObject.layer == 11 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
                {
                    hit.collider.TryGetComponent(out currentInteractable);

                    if (currentInteractable)
                    {
                        currentInteractable.OnFocus();
                    }
                }
                else if (currentInteractable)
                {
                    currentInteractable.OnLooseFocus();
                    currentInteractable = null;
                }
            }
        }

        private void HandleInteractionInput()
        {
            if (Input.GetKeyDown(interactKey) && currentInteractable != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
            {
                currentInteractable.OnInteract();
            }
        }

        private void HandleShoot()
        {
            currWeap = GetComponent<WeaponSwapper>().currentGun.GetComponent<GunCore>();
            if (Input.GetMouseButton(0) && currWeap.CurrentCD < Time.time)
            {
                currWeap.CurrentCD = Cooldown.CdCalc(currWeap.ShotCooldown);
                currWeap.ShootGun(playerCamera);
            }
        }

        private void HandleWeaponSwap()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                GetComponent<WeaponSwapper>().GunSwap(1);
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
            if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
            {
                timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : runBobSpeed);
                playerCamera.transform.localPosition = new Vector3(
                    playerCamera.transform.localPosition.x,
                    defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : runBobAmount),
                    playerCamera.transform.localPosition.z);

                // Movement animation for gun
                try { if (!currWeap.gunAnim.GetBool("RunTrigger")) currWeap.gunAnim.SetBool("RunTrigger", true); }
                catch { print("No animation exists for running"); }
            }
            else
            {
                try { currWeap.gunAnim.SetBool("RunTrigger", false); }
                catch { print("No animation exists for running"); }
                playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, defaultYPos, playerCamera.transform.localPosition.z);
            }
        }

        private void HandleFlashlight()
        {
            if (!flashLight.activeInHierarchy)
            {
                if (Input.GetKeyUp(flashLightKey))
                {
                    flashLight.SetActive(true);
                }
            }
            else
            {
                if (Input.GetKeyUp(flashLightKey))
                {
                    flashLight.SetActive(false);
                }
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