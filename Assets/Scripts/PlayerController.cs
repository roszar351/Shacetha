using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Responsible for handling input and logic behind player movement.
 * 
 */
public class PlayerController : MonoBehaviour
{
    public float leftCooldown = 1f;
    public float rightCooldown = 1f;

    public PlayerAnimations playerAnimations;
    public Transform leftAttackPoint;
    public Transform rightAttackPoint;
    public LayerMask enemyLayers;

    //private float horizontalSpeed = 0f;
    //private float verticalSpeed = 0f;
    private bool stopInput = false;
    private float stopMovementTimer = 0f;
    private float currentLeftCooldown = 0f;
    private float currentRightCooldown = 0f;

    private Rigidbody2D rb;
    private Vector2 movementVector;
    private CharacterStats myStats;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myStats = GetComponent<CharacterStats>();
        movementVector = new Vector2(0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (stopInput)
            return;

        movementVector.x = 0;
        movementVector.y = 0;
        stopMovementTimer -= Time.deltaTime;

        if (Input.GetButton("Up"))
        {
            movementVector.y = 1f;
        }
        if (Input.GetButton("Down"))
        {
            movementVector.y = -1f;
        }
        if (Input.GetButton("Right"))
        {
            movementVector.x = 1f;
        }
        if (Input.GetButton("Left"))
        {
            movementVector.x = -1f;
        }
        
        HandleAttack(); 
    }

    private void FixedUpdate()
    {
        if (stopInput)
            return;

        Move();
    }

    public void StopMovement(float forHowLong)
    {
        // We only need to set the timer if the new animation time is longer than what the timer is currently at
        if(forHowLong > stopMovementTimer)
            stopMovementTimer = forHowLong;
    }

    public void ResumeMovement()
    {
        stopMovementTimer = 0f;
    }

    // For pausing the game or other similar situation where player shouldnt be able to move/attack.
    public void StopInput()
    {
        stopInput = true;
    }
    public void ResumeInput()
    {
        stopInput = false;
    }

    private void Move()
    {
        if (stopMovementTimer > 0)
            return;

        // Normalizing as movementVector is initially used as just direction and then speed and fixedDeltaTime is the actual speed
        // Helped prevent quicker movement in diagonals(hopefully :)
        rb.MovePosition(rb.position + movementVector.normalized * myStats.movementSpeed * Time.fixedDeltaTime);

        // TODO: animation based on what is in the movementVector
    }

    // Handle the attack input
    private void HandleAttack()
    {
        currentLeftCooldown -= Time.deltaTime;
        currentRightCooldown -= Time.deltaTime;

        // TODO: add animation + sound for attacks
        if (Input.GetMouseButton(0) && currentLeftCooldown <= 0)
        {
            movementVector.x = 0;
            movementVector.y = 0;

            playerAnimations.PlayLeftAttack();
            currentLeftCooldown = leftCooldown;

            Invoke("CreateDamageArea", .4f);
        }
        if (Input.GetMouseButton(1) && currentRightCooldown <= 0)
        {
            movementVector.x = 0;
            movementVector.y = 0;

            playerAnimations.PlayRightAttack();
            currentRightCooldown = rightCooldown;

            Invoke("CreateDamageArea", .4f);
        }
    }

    private void CreateDamageArea()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(leftAttackPoint.position, myStats.attackRange, enemyLayers);
        foreach (Collider2D enemy in enemiesHit)
        {
            enemy.GetComponent<CharacterStats>().TakeDamage(myStats.damage);
        }
    }
}
