using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * Responsible for handling input and logic behind player movement.
 * 
 */
public class PlayerController : MonoBehaviour
{
    // Uninitialized public variables
    public PlayerAnimations playerAnimations;
    public so_NPCStats myStats;
    public HandsController myHands;

    // Initialized private variables
    private bool stopInput = false;
    private float stopMovementTimer = 0f;

    // Uninitialized private variables
    private Rigidbody2D rb;
    private Vector2 movementVector;
    private int currentHp;
    private int totalArmor;


    private float invincibleTimer = 1f;
    private float currentInvincibleTimer = 0f;

    // Events
    public event System.Action<int, int> OnHealthChanged;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movementVector = new Vector2(0f, 0f);
        currentHp = myStats.maxHp;
        totalArmor = myStats.baseArmor;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopInput)
            return;

        currentInvincibleTimer -= Time.deltaTime;

        Look();
        HandleMove();
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

    public void UpdateArmor(int modifierValue)
    {
        totalArmor = myStats.baseArmor + modifierValue;
    }

    public void TakeDamage(int damage)
    {
        if (currentInvincibleTimer > 0)
            return;

        currentInvincibleTimer = invincibleTimer;
        float tempValue = 100f + totalArmor;
        if(tempValue <= 0)
        {
            tempValue = 1;
        }

        AudioManager.instance.PlayOneShotSound("DamagedPlayer");

        damage = (int)(damage * (100f / tempValue));
        currentHp -= damage;
        TextPopup.Create(transform.position, damage);

        if (OnHealthChanged != null)
        {
            OnHealthChanged(myStats.maxHp, currentHp);
        }

        if (currentHp <= 0)
            Die();
    }

    public void Heal(int damage)
    {
        currentHp += damage;
        if (currentHp > myStats.maxHp)
            currentHp = myStats.maxHp;

        TextPopup.Create(transform.position, damage, Color.green);

        if (OnHealthChanged != null)
        {
            OnHealthChanged(myStats.maxHp, currentHp);
        }

        if (currentHp <= 0)
            Die();
    }

    private void Look()
    {
        Vector2 lookDirection = (GetMousePosition() - transform.position).normalized;
        playerAnimations.UpdateIdleAnimation(lookDirection);
    }

    private void HandleMove()
    {
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
    }

    private void Move()
    {
        if (stopMovementTimer > 0)
        {
            AudioManager.instance.StopSound("MovementPlayer");
            playerAnimations.PlayMovementAnimation(new Vector2(0, 0));
            return;
        }

        if (movementVector.x == 0 && movementVector.y == 0)
        {
            AudioManager.instance.StopSound("MovementPlayer");
        }
        else
        {
            AudioManager.instance.PlaySound("MovementPlayer");
        }

        playerAnimations.PlayMovementAnimation(movementVector);

        // Normalizing as movementVector is initially used as just direction and then speed and fixedDeltaTime is the actual speed
        // Helped prevent quicker movement in diagonals(hopefully :)
        rb.MovePosition(rb.position + movementVector.normalized * myStats.movementSpeed * Time.fixedDeltaTime);
    }

    // Handle the attack input
    private void HandleAttack()
    {
        // dont attack if clicking in UI
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // TODO: add animation + sound for attacks

        if (Input.GetMouseButton(0))
        {
            movementVector.x = 0;
            movementVector.y = 0;

            myHands.UseLeftHand();
            playerAnimations.StopPlayerMovement(.5f);
        }
        if (Input.GetMouseButton(1))
        {
            movementVector.x = 0;
            movementVector.y = 0;

            myHands.UseRightHand();
            playerAnimations.StopPlayerMovement(.5f);
        }
    }

    private void Die()
    {
        AudioManager.instance.PlayOneShotSound("DeathPlayer");
        //Destroy(gameObject);
        PlayerManager.instance.KillPlayer();
    }

    private Vector3 GetMousePosition()
    {
        Vector3 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        temp.z = 0;
        return temp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            TakeDamage(collision.GetComponent<CurrentItemStats>().GetModifierValue());
        }
    }
}
