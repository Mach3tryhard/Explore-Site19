using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class FirstPersonController : MonoBehaviour
{
    //public bool CanMove { get; private set; } = true;
    public bool CanMove = true;
    private bool IsSprinting => canSprint && Input.GetKey(sprintKey);
    private bool ShouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;
    private bool ShouldCrouch => Input.GetKeyDown(crouchKey) && !duringCrouchingAnimation && characterController.isGrounded;

    [Header("Functional Options")]
    public bool canLook = true;
    public bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canUseHeadbob = true;
    [SerializeField] private bool WillSlideOnSlopes = true;
    public bool canZoom = false;
    public bool canInteract = true;
    [SerializeField] private bool useFootsteps = true;
    [SerializeField] private bool useStamina = true;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode zoomkey = KeyCode.Mouse1;
    [SerializeField] private KeyCode interactkey = KeyCode.Mouse0;

    [Header("Movement Parameters")]
    public float walkSpeed = 5.0f;
    public float sprintSpeed = 10.0f;
    public float crouchSpeed = 2.5f;
    public float slopeSpeed = 15f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float UpperLookLimit = 90.0f;
    [SerializeField, Range(1, 180)] private float LowerLookLimit = 90.0f;

    [Header("Health Options")]
    public float maxHealth = 100;
    [SerializeField] private float timeBeforeRegenStarts = 3;
    [SerializeField] private float healthValueIncrement = 0f;
    [SerializeField] private float healthTimeIncrement = 0.1f;
    public float currentHealth;
    private Coroutine regeneratingHealth;
    public static Action<float> OnTakeDamage;
    public static Action<float> OnDamage;
    public static Action<float> OnHeal;

    [Header("Sprinting Options")]
    [SerializeField] private float SprintingZoomFOV=65;
    [SerializeField]private float timeToZoomOut = 0.2f;

    [Header("Stamina Options")]
    [SerializeField] private float maxStamina = 100;
    public float staminaUseMultiplier = 15;
    public float timeBeforeStaminaRegenStarts = 4;
    [SerializeField] private float staminaValueIncrement = 10f;
    [SerializeField] private float staminaTimeIncrement = 10f;
    public float currentStamina;
    private Coroutine regeneratingStamina;
    public static Action<float> OnStaminaChange;

    [Header("Jumping Parameters")]
    [SerializeField]private float jumpForce = 8.0f; 
    [SerializeField] private float gravity = 30.0f;

    [Header("Crouch Parameters")]
    [SerializeField]private float crouchHeight = 1.25f; 
    [SerializeField]private float standingHeight = 3.0f; 
    [SerializeField]private float timeToCrouch = 0.3f; 
    [SerializeField]private Vector3 crouchingCenter = new Vector3(0,1f,0);
    [SerializeField]private Vector3 standingCenter = new Vector3(0,0,0); 
    private bool isCrouching;
    private bool duringCrouchingAnimation;

    [Header("Headbob Parameters")]
    [SerializeField]private float walkBobSpeed = 14f;
    [SerializeField]private float walkBobAmount = 0.05f;
    [SerializeField]private float sprintBobSpeed = 18f;
    [SerializeField]private float sprintBobAmount = 0.11f;
    [SerializeField]private float crouchBobSpeed = 8f;
    [SerializeField]private float crouchBobAmount = 0.025f;
    private float defaultYPos = 0;
    private float timer;

    [Header("Zoom Parameters")]
    [SerializeField]private float timeToZoom = 0.3f;
    [SerializeField]private float zoomFOV = 30f;
    private float defaultFOV = 0;
    private Coroutine zoomRoutine;

    [Header("Footstep Parameters")]
    [SerializeField]private float baseStepSpeed = 0.5f;
    [SerializeField]private float crouchStepMultipler = 1.5f;
    [SerializeField]private float sprintStepMultipler = 0.6f;
    [SerializeField]private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] concreteClips = default;
    [SerializeField] private AudioClip[] metalClips = default;
    [SerializeField] private AudioClip[] dirtClips = default;
    private float footstepTimer = 0;
    private float GetCurrentOffset => isCrouching ? (baseStepSpeed * crouchStepMultipler) : IsSprinting ? (baseStepSpeed * sprintStepMultipler) : baseStepSpeed;

    // SLIDING PARAMETERS
    private Vector3 hitPointNormal;
    private bool IsSliding
    {
        get
        {
            if(characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
            {
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    private Interacteble currentInteracteble;

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;
    private float rotationX = 0;
    
    private void OnEnable()
    {
        OnTakeDamage += ApplyDamage;
    }
    private void OnDisable()
    {
        OnTakeDamage -= ApplyDamage;
    }

    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        defaultYPos = playerCamera.transform.localPosition.y;
        defaultFOV = playerCamera.fieldOfView;
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(CanMove)
        {
            HandleMovementInput();
            if(canLook)
            {
                HandleMouseLook();
            }
            if(canJump)
            {
                HandleJump();
            }
            if(canCrouch)
            {
                HandleCrouch();
            }
            if(canUseHeadbob)
            {
                HandleHeadbob();
            }
            if(canZoom)
            {
                HandleZoom();
            }
            if(useFootsteps)
            {
                Handle_Footsteps();
            }
            if(canInteract)
            {
                HandleInteractionCheck();
                HandleInteractionInput();
            }

            if(useStamina)
            {
                HandleStamina();
                HandleFovZoomOut();
            }
            ApplyFinalMovements();
        }
    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2((isCrouching ? crouchSpeed : IsSprinting ?  sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"),(isCrouching ? crouchSpeed : IsSprinting ?  sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -UpperLookLimit, LowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void HandleJump()
    {
        if(ShouldJump)
        {
            moveDirection.y = jumpForce;
        }
    }

    private void HandleCrouch()
    {
        if(ShouldCrouch)
        {
            StartCoroutine(CrouchStand());
        }
    }

    private void HandleHeadbob()
    {
        if(!characterController.isGrounded) return;

        if(Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z)> 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z
            );
        }
    }

    private void HandleFovZoomOut()
    {
        bool willMove=false;
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S))willMove=true;
        bool isMoving=false;
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S))isMoving=true;

        if( Input.GetKey(KeyCode.LeftShift) && willMove)
        {
            if(zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }

            zoomRoutine = StartCoroutine(SprintZoom(true));
        }
        if( Input.GetKeyDown(KeyCode.LeftShift) && isMoving)
        {
            if(zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }

            zoomRoutine = StartCoroutine(SprintZoom(true));
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) || currentStamina==0)
        {
            if(zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }

            zoomRoutine = StartCoroutine(SprintZoom(false));
        }
    }

    private void HandleStamina()
    {
        if(IsSprinting && currentInput != Vector2.zero)
        {
            if(regeneratingStamina != null)
            {
                StopCoroutine(regeneratingStamina);
                regeneratingStamina = null;
            }
            currentStamina -= staminaUseMultiplier * Time.deltaTime;

            if(currentStamina < 0)
            {
                currentStamina = 0;
            }

            OnStaminaChange?.Invoke(currentStamina);

            if(currentStamina<=0)
            {
                canSprint = false;
            }
        }

        if(!IsSprinting && currentStamina< maxStamina && regeneratingStamina == null)
        {
            regeneratingStamina = StartCoroutine(RegenerateStamina());
        }
    }

    private void HandleZoom()
    {
        if(Input.GetKeyDown(zoomkey))
        {
            if(zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }

            zoomRoutine = StartCoroutine(ToggleZoom(true));
        }
        if(Input.GetKeyUp(zoomkey))
        {
            if(zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
                zoomRoutine = null;
            }

            zoomRoutine = StartCoroutine(ToggleZoom(false));
        }
    }

    private void HandleInteractionCheck()
    {
        if(Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance) && hit.collider.gameObject.layer== 9)
        {
            if(hit.collider.gameObject.layer == 9 && (currentInteracteble == null || hit.collider.gameObject.GetInstanceID() != currentInteracteble.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentInteracteble);
                if(currentInteracteble!=null)
                {
                    currentInteracteble.OnLoseFocus();
                }
                if(currentInteracteble)
                {
                    currentInteracteble.OnFocus();
                }
            }
        }
        else
        if(currentInteracteble)
        {
            currentInteracteble.OnLoseFocus();
            currentInteracteble = null;
        }
    }

    private void HandleInteractionInput()
    {
        if(Input.GetKeyDown(interactkey) && currentInteracteble != null && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            currentInteracteble.OnInteract();
        }
    }

    private void Handle_Footsteps()
    {
        if(!characterController.isGrounded) return;
        if(currentInput == Vector2.zero) return;
        if(useFootsteps == false) return;

        footstepTimer -= Time.deltaTime;

        if(footstepTimer <= 0)
        {
            if(Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                if(hit.collider.tag == "Footsteps/metal")
                {
                    footstepAudioSource.PlayOneShot(metalClips[UnityEngine.Random.Range(0, metalClips.Length - 1)]);
                }
                else
                if(hit.collider.tag == "Footsteps/concrete")
                {
                    footstepAudioSource.PlayOneShot(concreteClips[UnityEngine.Random.Range(0, concreteClips.Length - 1)]);
                }
                else
                if(hit.collider.tag == "Footsteps/dirt")
                {
                    footstepAudioSource.PlayOneShot(dirtClips[UnityEngine.Random.Range(0, dirtClips.Length - 1)]);
                }
                else
                {
                    footstepAudioSource.PlayOneShot(concreteClips[UnityEngine.Random.Range(0, concreteClips.Length - 1)]);

                }
            }
            footstepTimer = GetCurrentOffset;
        }
    }

    private void ApplyDamage(float dmg)
    {
        currentHealth -= dmg;
        OnDamage?.Invoke(currentHealth);

        if(currentHealth<=0)
        {
            KillPlayer();
        }
        else
        if(regeneratingHealth != null)
        {
            StopCoroutine(regeneratingHealth);
        }

        regeneratingHealth = StartCoroutine(RegenerateHealth());
    }

    private void KillPlayer()
    {
        currentHealth = 0;

        if(regeneratingHealth != null)
        {
            StopCoroutine(regeneratingHealth);
        }

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("DEAD");
    }

    private void ApplyFinalMovements()
    {
        if(!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        if(characterController.velocity.y <-1 && characterController.isGrounded)
        {
            moveDirection.y=0;
        }
        if(WillSlideOnSlopes && IsSliding)
        {
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;
        }
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private IEnumerator CrouchStand()
    {
        if(isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
            yield break;

        duringCrouchingAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        while(timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed/timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed/timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchingAnimation = false;
    }

    private IEnumerator SprintZoom(bool isEnter)
    {
        float targetFov = isEnter ? SprintingZoomFOV : defaultFOV;
        float startingFOV = playerCamera.fieldOfView;
        float timeElapsed = 0;

        while(timeElapsed< timeToZoomOut)
        {
            playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFov, timeElapsed/timeToZoomOut);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.fieldOfView = targetFov;
        zoomRoutine = null;
    }

    private IEnumerator ToggleZoom(bool isEnter)
    {
        float targetFov = isEnter ? zoomFOV : defaultFOV;
        float startingFOV = playerCamera.fieldOfView;
        float timeElapsed = 0;

        while(timeElapsed< timeToZoom)
        {
            playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFov, timeElapsed/timeToZoom);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.fieldOfView = targetFov;
        zoomRoutine = null;
    }

    private IEnumerator RegenerateHealth()
    {
        yield return new WaitForSeconds(timeBeforeRegenStarts);
        WaitForSeconds timeToWait = new WaitForSeconds(healthTimeIncrement);

        while(currentHealth< maxHealth)
        {
            currentHealth += healthValueIncrement;

            if(currentHealth>maxHealth)
            {
                currentHealth = maxHealth;
            }

            OnHeal?.Invoke(currentHealth);
            yield return timeToWait;
        }

        regeneratingHealth = null;
    }

    private IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(timeBeforeStaminaRegenStarts);
        WaitForSeconds timeToWait = new WaitForSeconds(staminaTimeIncrement);

        while(currentStamina< maxStamina)
        {
            if(currentStamina>0)
            {
                canSprint = true;
            }
            currentStamina += staminaValueIncrement;

            if(currentStamina>maxStamina)
            {
                currentStamina=maxStamina;
            }

            OnStaminaChange?.Invoke(currentStamina);
            yield return timeToWait;
        }

        regeneratingStamina = null;
    }
}
