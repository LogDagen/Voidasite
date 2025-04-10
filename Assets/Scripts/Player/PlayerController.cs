/*****************************************************************************
// File Name : PlayerController.cs
// Author : Logan Dagenais
// Creation Date : February 28, 2025
//
// Brief Description : This code controls the player, including its movements,
Jump, shooting, charging shot, punch, enemy interactions, reflecting and if 
player is on slope.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private float standingSpeed;
    [SerializeField] private PlayerInput playerInput;
    private float chargeSpeed;
    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    private bool grounded;
    [SerializeField] private Transform orientation;
    [SerializeField] private float groundDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float raycastDistance;
    private bool readyToJump;
    private float horizontalInput;
    private float verticalInput;
    Vector3 moveDirection;
    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;
    [Header("Crouching")]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float CrouchYScale;
    private float StartYScale;
    private bool isCrouching;
    private bool cantUncrouch;
    [SerializeField] private LayerMask Uncrouch;
    [Header("Shooting")]
    [SerializeField] private Transform FirePoint;
    [SerializeField] private float Shootrange;
    [SerializeField] private float Shootcooldown;
    [SerializeField] private float gunShotRadius;
    [SerializeField] private int ShootDamage;
    [SerializeField] private bool readyToShoot;
    [SerializeField] private LayerMask enemyLayerMask;
    private Animator RightHandAnim;
    [Header("ChargedShooting")]
    [SerializeField] private int ChargeDamage;
    [SerializeField] private float ChargeCooldown;
    [SerializeField] private bool readyToCharge;
    [SerializeField] private bool isCharging;
    [Header("Punch")]
    [SerializeField] private Transform PunchPos;
    private Animator LeftHandAnim;
    [SerializeField] private float attackRangeX;
    [SerializeField] private float attackRangeY;
    [SerializeField] private float attackRangeZ;
    [SerializeField] private float PunchCooldown;
    private bool readyToPunch;
    [SerializeField] private int PunchDamage;
    [SerializeField] private LayerMask ProjectileLayerMask;
    [SerializeField] private GameObject FreezeFrame;
    [Header("Health")]
    public int Health;
    public int MaxHealth;
    private HealthBar healthbar;
    [Header("Special Attack")]
    public int Energy;
    public int MaxEnergy;
    private EnergyBar energybar;
    [SerializeField] private GameObject SpecialProjectile;
    [SerializeField] private Transform SpecialSpawn;
    [Header("Dash")]
    [SerializeField] private float DashSpeed;
    public float DashStamina;
    public float MaxDashStamina;
    private float setDashSpeed;
    private StaminaBar staminaBar;
    [SerializeField] private float DashCooldown;
    [SerializeField] private float DashRegenTime;
    private bool StaminaRegening;
    private bool readyToDash;
    [Header("KeyKard")]
    public bool HasKeyCard;
    [SerializeField] private LayerMask KeyCardReader;
    [Header("DialogueBox")]
    [SerializeField] private Dialogue DialogueBox;
    public bool IsReading;
    [Header("Death")]
    [SerializeField] private GameObject deathScreen;
    private Rigidbody rb;
    private GameManager gameManager;

    //Start is called on the first frame
    /// <summary>
    /// gets components and sets bools and floats
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerInput.currentActionMap.Enable();
        readyToJump = true;
        readyToShoot = true;
        readyToCharge = true;
        readyToPunch = true;
        readyToDash = true;
        IsReading = false;
        chargeSpeed = 1;
        standingSpeed = moveSpeed;
        StartYScale = transform.localScale.y;
        RightHandAnim = GameObject.FindGameObjectWithTag("RightHand").GetComponent<Animator>();
        LeftHandAnim = GameObject.FindGameObjectWithTag("LeftHand").GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        healthbar = FindObjectOfType<HealthBar>();
        energybar = FindObjectOfType<EnergyBar>();
        staminaBar = FindObjectOfType<StaminaBar>();
        gameManager.StartLevel();
    }
    //Update gets called every frame
    /// <summary>
    /// checks if grounded and drags player
    /// </summary>
    private void Update()
    {
        //checks if player is on ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        cantUncrouch = Physics.Raycast(transform.position, Vector3.up, playerHeight * 0.5f + 0.2f, Uncrouch);
        MyInput();
        SpeedControl();
        // handles drag if player is on the ground
        if (grounded && readyToDash == true)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    /// <summary>
    /// lets the player move, cant be called when move is pressed or slope does not work
    /// </summary>
    private void FixedUpdate()
    {
        MovePlayer();
    }
    /// <summary>
    /// gets inputs for both vertical and horizontal inputs
    /// </summary>
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
    /// <summary>
    /// Cooldown to shoot again
    /// </summary>
    /// <returns>cooldown</returns>
    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(Shootcooldown);
        readyToShoot = true;
        RightHandAnim.SetTrigger("Idle");
        yield break;
    }/// <summary>
    /// Cooldown to charge shoot
    /// </summary>
    /// <returns>cooldown</returns>
    IEnumerator ChargeShootCooldown()
    {
        yield return new WaitForSeconds(ChargeCooldown);
        readyToCharge = true;
        yield break;
    }
    /// <summary>
    /// cooldown to punch
    /// </summary>
    /// <returns>cooldown</returns>
    IEnumerator PunchAtkCooldown()
    {
        yield return new WaitForSeconds(PunchCooldown);
        readyToPunch = true;
        yield break;
    }
    IEnumerator dashCooldown()
    {
        setDashSpeed = DashSpeed;
        yield return new WaitForSeconds(DashCooldown);
        readyToDash = true;
        setDashSpeed = 0;
        yield break;
    }
    IEnumerator DashstaminaRegen()
    {
        while (true)
        {
            StaminaRegening = true;
            DashStamina += 1;
            staminaBar.HealStamina(1);
            if (DashStamina >= MaxDashStamina)
            {
                DashStamina = MaxDashStamina;
                StaminaRegening = false;
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(DashRegenTime);
            }
        }
    }
    IEnumerator FrameFreezeVisual()
    {
        FreezeFrame.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        FreezeFrame.SetActive(false);
    }
    /// <summary>
    /// Damages player and checks if they die
    /// </summary>
    /// <param name="damage">damage taken</param>
    public void Damage(int damage)
    {
        Health -= damage;
        healthbar.DamageHealth(damage);
        if (Health <= 0)
        {
            death();
        }
    }
    /// <summary>
    /// Heals health
    /// </summary>
    /// <param name="heal">regen</param>
    public void Heal(int heal)
    {
        Health += heal;
        healthbar.HealHealth(heal);
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
            healthbar.SetHealth(MaxHealth);
        }
    }
    /// <summary>
    /// gains energy
    /// </summary>
    /// <param name="energy">energy gained</param>
    public void GainEnergy(int energy)
    {
        Energy += energy;
        energybar.GainEnergy(energy);
        if (Energy > MaxEnergy)
        {
            Energy = MaxEnergy;
            energybar.SetEnergy(Energy);
        }
    }
    /// <summary>
    /// depletes energy
    /// </summary>
    /// <param name="energy">energy depleted</param>
    public void LoseEnergy(int energy)
    {
        Energy -= energy;
        energybar.lowerEnergy(energy);
        if (Energy < 0)
        {
            Energy = 0;
            energybar.SetEnergy(0);
        }
    }
    /// <summary>
    /// Gets called from Unity not code, makes the object jump vertically
    /// </summary>
    void OnJump()
    {
        //checks if it can jump
        if (readyToJump == true && grounded == true)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    /// <summary>
    /// Checks if the player is crouching then either crouches or Uncrouches the player
    /// </summary>
    void OnCrouch()
    {
        if (isCrouching == false)
        {
            transform.localScale = new Vector3(transform.localScale.x, CrouchYScale, transform.localScale.y);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            moveSpeed = crouchSpeed;
            isCrouching = true;
        }
        else if (cantUncrouch == false)
        {
            transform.localScale = new Vector3(transform.localScale.x, StartYScale, transform.localScale.z);
            moveSpeed = standingSpeed;
            isCrouching = false;
        }
    }
    /// <summary>
    /// Gets called from Unity not code, Lets player shoot
    /// </summary>
    void OnShoot()
    {
        if (readyToShoot == true && isCharging == false)
        {
            readyToShoot = false;
            RightHandAnim.SetTrigger("Shoot");
            //simulate gun shot radius
            Collider[] EnemyColliders;
            EnemyColliders = Physics.OverlapSphere(transform.position, gunShotRadius, enemyLayerMask);
            //Alert enemies in range
            foreach (var enemyCollider in EnemyColliders)
            {
                enemyCollider.GetComponent<EnemyAwareness>().Aggrivate();
            }
            RaycastHit hit;

            if (Physics.Raycast(FirePoint.position, FirePoint.transform.TransformDirection(Vector3.forward), out hit, Shootrange))
            {
                Debug.DrawRay(FirePoint.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);

                EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();

                if (enemy != null)
                {
                    enemy.Damage(ShootDamage);
                }
                if (hit.transform.gameObject.tag == "VictoryCapsule")
                {
                    gameManager.CompleteLevel();
                    gameManager.UnlockNewLevel();
                }
            }
            StartCoroutine(ShootCooldown());
        }
    }
    /// <summary>
    /// Gets called from Unity not code, Lets player charge up their ranged shot
    /// </summary>
    void OnChargeShoot()
    {
        if (readyToCharge == true && readyToShoot == true )
        {
            isCharging = true;
            RightHandAnim.SetTrigger("Charge");
            chargeSpeed = 0.25f;
        }
    }
    /// <summary>
    /// Gets called from Unity not code, initiates charge shot
    /// </summary>
    void OnReleaseCharge()
    {
        if (isCharging == true)
        {
            chargeSpeed = 1;
            isCharging = false;
            readyToShoot = false;
            readyToCharge = false;
            RightHandAnim.SetTrigger("Shoot");
            //simulate gun shot radius
            Collider[] EnemyColliders;
            EnemyColliders = Physics.OverlapSphere(transform.position, gunShotRadius, enemyLayerMask);
            //Alert enemies in range
            foreach (var enemyCollider in EnemyColliders)
            {
                enemyCollider.GetComponent<EnemyAwareness>().Aggrivate();
            }
            RaycastHit hit;
            //damages hit enemies
            if (Physics.Raycast(FirePoint.position, FirePoint.transform.TransformDirection(Vector3.forward), out hit, Shootrange))
            {
                Debug.DrawRay(FirePoint.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.blue);

                EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();

                if (enemy != null)
                {
                    enemy.Damage(ChargeDamage);
                    gameManager.Freeze();
                }
                if (hit.transform.gameObject.tag == "BreakableWall")
                {
                    hit.transform.gameObject.SetActive(false);
                }
                if (hit.transform.gameObject.tag == "VictoryCapsule")
                {
                    gameManager.CompleteLevel();
                    gameManager.UnlockNewLevel();
                }
            }
            StartCoroutine(ShootCooldown());
            StartCoroutine(ChargeShootCooldown());
        }
    }
    void OnReadDialogue()
    {
        if (IsReading)
        {
            DialogueBox.ReadDialogue();
        }
    }
    /// <summary>
    /// Gets called from Unity not code, Lets player punch and reflect projectiles
    /// </summary>
    void OnPunch()
    {
        if (readyToPunch == true)
        {
            readyToPunch = false;
            LeftHandAnim.SetTrigger("Punch");
            //finds objects to effect
            Collider[] enemiesToDamage = Physics.OverlapBox(PunchPos.position, new Vector3(attackRangeX, attackRangeY, attackRangeZ), PunchPos.rotation, enemyLayerMask);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<EnemyHealth>().Damage(PunchDamage);
            }
            Collider[] ProjectileReflect = Physics.OverlapBox(PunchPos.position, new Vector3(attackRangeX, attackRangeY, attackRangeZ), PunchPos.rotation, ProjectileLayerMask);
            for (int i = 0; i < ProjectileReflect.Length; i++)
            {
                ProjectileReflect[i].GetComponent<RangedEnemyBullet>().reflect();
                LeftHandAnim.SetTrigger("Counter");
                StartCoroutine(FrameFreezeVisual());
                gameManager.Freeze();
            }
            Collider[] KeycardReader = Physics.OverlapBox(PunchPos.position, new Vector3(attackRangeX, attackRangeY, attackRangeZ), PunchPos.rotation, KeyCardReader);
            for (int i = 0; i < KeycardReader.Length; i++)
            {
                if (HasKeyCard == true)
                {
                    HasKeyCard = false;
                    KeycardReader[i].GetComponent<CardReader>().CardRead();
                }
            }
            //starts cooldown
            StartCoroutine(PunchAtkCooldown());
        }
    }
    /// <summary>
    /// Gets called from unity not code, lets player use special attack when they have enough energy
    /// </summary>
    void OnSpecialAttack()
    {
        if (Energy == MaxEnergy)
        {
            Energy = 0;
            energybar.SetEnergy(0);
            Instantiate(SpecialProjectile, SpecialSpawn.position, SpecialSpawn.rotation);
        }
    }
    /// <summary>
    /// Gets called from unity not code, lets player dash forward excluding rotation
    /// </summary>
    void OnDash()
    {
        if (readyToDash == true && DashStamina >= 33 && verticalInput != 0)
        {
            DashStamina -= 33;
            staminaBar.LowerStamina(33);
            readyToDash = false;
            if (StaminaRegening == false)
            {
                StartCoroutine(DashstaminaRegen());
            }
            StartCoroutine(dashCooldown());
        }
    }
    void OnPause()
    {
        gameManager.PauseGame();
    }
    /// <summary>
    /// moves player based on input
    /// </summary>
    private void MovePlayer()
    {
        //calculates movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDireciton() * moveSpeed * 20f, ForceMode.Force);
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 100f, ForceMode.Force);
            }
        }
        // speed on ground
        else if (grounded == true)
        {
            rb.AddForce(moveDirection.normalized * (moveSpeed+setDashSpeed) * chargeSpeed * 10f, ForceMode.Force);
        }
        // speed in air
        else if (grounded == false)
        {
            rb.AddForce(moveDirection.normalized * (moveSpeed+setDashSpeed) * 10f * airMultiplier, ForceMode.Force);
        }
        // turn off gravity while on slope
        rb.useGravity = !OnSlope();
    }
    /// <summary>
    /// controls speed to be max speed
    /// </summary>
    private void SpeedControl()
    {
        //limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        //limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            //limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }
    /// <summary>
    /// lets the player jump when it is called
    /// </summary>
    private void Jump()
    {
        exitingSlope = true;
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //adds force
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    /// <summary>
    /// puts readytojump to true
    /// </summary>
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }
    /// <summary>
    /// detects if the player is on a slope, max angle can be changed in Unity
    /// </summary>
    /// <returns></returns>
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    /// <summary>
    /// gets the direction of the slope the player is on
    /// </summary>
    /// <returns></returns>
    private Vector3 GetSlopeMoveDireciton()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    /// <summary>
    /// called when player loses all HP
    /// </summary>
    private void death()
    {
        playerInput.currentActionMap.Disable();
        gameManager.Death();
    }
}