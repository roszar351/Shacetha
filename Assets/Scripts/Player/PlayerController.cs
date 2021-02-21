using System;
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
    private bool _stopInput = false;
    private float _stopMovementTimer = 0f;

    // Uninitialized private variables
    private Rigidbody2D _rb;
    private Vector2 _movementVector;
    private int _currentHp;
    private int _totalArmor;

    private float _dissolveAmount;
    private bool _isDying;

    //private Material myMaterial;
    private MaterialPropertyBlock _propBlock;

    [SerializeField] private Renderer myRenderer;

    private float _invincibleTimer = 1f;
    private float _currentInvincibleTimer = 0f;
    private int _constantDamage = 0;

    private static readonly int DissolveValue = Shader.PropertyToID("_DissolveValue");

    // Events
    public event System.Action<int, int> OnHealthChanged;

    private void Start()
    {
        //myRenderer.material = Instantiate(GameAssets.i.diffuseMaterial);
        //myMaterial = myRenderer.material;
        _isDying = false;
        _dissolveAmount = 1f;
        _propBlock = new MaterialPropertyBlock();
        //myMaterial.SetFloat(DissolveValue, dissolveAmount);

        _rb = GetComponent<Rigidbody2D>();
        _movementVector = new Vector2(0f, 0f);
        _currentHp = myStats.maxHp;
        _totalArmor = myStats.baseArmor;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDying)
        {
            _dissolveAmount = Mathf.Clamp01(_dissolveAmount - Time.deltaTime);
            myRenderer.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat(DissolveValue, _dissolveAmount);
            myRenderer.SetPropertyBlock(_propBlock);
            //myMaterial.SetFloat(DissolveValue, dissolveAmount);
        }

        if (_stopInput)
            return;

        if (_constantDamage > 0)
            TakeDamage(_constantDamage);

        _currentInvincibleTimer -= Time.deltaTime;

        Look();
        HandleMove();
        HandleAttack();
    }

    private void FixedUpdate()
    {
        if (_stopInput)
            return;

        Move();
    }

    public void StopMovement(float forHowLong)
    {
        // We only need to set the timer if the new animation time is longer than what the timer is currently at
        if (forHowLong > _stopMovementTimer)
            _stopMovementTimer = forHowLong;
    }

    public void ResumeMovement()
    {
        _stopMovementTimer = 0f;
    }

    // For pausing the game or other similar situation where player shouldnt be able to move/attack.
    public void StopInput()
    {
        _stopInput = true;
    }

    public void ResumeInput()
    {
        _stopInput = false;
    }

    public void UpdateArmor(int modifierValue)
    {
        _totalArmor = myStats.baseArmor + modifierValue;
    }

    public void TakeDamage(int damage)
    {
        if (_currentInvincibleTimer > 0)
            return;

        _currentInvincibleTimer = _invincibleTimer;
        float tempValue = 100f + _totalArmor;
        if (tempValue <= 0)
        {
            tempValue = 1;
        }

        AudioManager.instance.PlayOneShotSound("DamagedPlayer");

        damage = (int) (damage * (100f / tempValue));
        _currentHp -= damage;
        TextPopup.Create(transform.position, damage);

        if (OnHealthChanged != null)
        {
            OnHealthChanged(myStats.maxHp, _currentHp);
        }

        if (_currentHp <= 0)
            Die();
    }

    public void Heal(int damage)
    {
        _currentHp += damage;
        if (_currentHp > myStats.maxHp)
            _currentHp = myStats.maxHp;

        TextPopup.Create(transform.position, damage, Color.green);

        if (OnHealthChanged != null)
        {
            OnHealthChanged(myStats.maxHp, _currentHp);
        }

        if (_currentHp <= 0)
            Die();
    }

    public void TakeConstantDamage(int damage)
    {
        _constantDamage = damage;
    }

    public void StopConstantDamage()
    {
        _constantDamage = 0;
    }

    private void Look()
    {
        Vector2 lookDirection = (GetMousePosition() - transform.position).normalized;
        playerAnimations.UpdateIdleAnimation(lookDirection);
    }

    private void HandleMove()
    {
        _movementVector.x = 0;
        _movementVector.y = 0;
        _stopMovementTimer -= Time.deltaTime;

        if (Input.GetButton("Up"))
        {
            _movementVector.y = 1f;
        }

        if (Input.GetButton("Down"))
        {
            _movementVector.y = -1f;
        }

        if (Input.GetButton("Right"))
        {
            _movementVector.x = 1f;
        }

        if (Input.GetButton("Left"))
        {
            _movementVector.x = -1f;
        }
    }

    private void Move()
    {
        if (_stopMovementTimer > 0)
        {
            AudioManager.instance.StopSound("MovementPlayer");
            playerAnimations.PlayMovementAnimation(new Vector2(0, 0));
            return;
        }

        if (_movementVector.x == 0 && _movementVector.y == 0)
        {
            AudioManager.instance.StopSound("MovementPlayer");
        }
        else
        {
            AudioManager.instance.PlaySound("MovementPlayer");
        }

        playerAnimations.PlayMovementAnimation(_movementVector);

        // Normalizing as movementVector is initially used as just direction and then speed and fixedDeltaTime is the actual speed
        // Helped prevent quicker movement in diagonals(hopefully :)
        _rb.MovePosition(_rb.position + _movementVector.normalized * (myStats.movementSpeed * Time.fixedDeltaTime));
    }

    // Handle the attack input
    private void HandleAttack()
    {
        // dont attack if clicking in UI
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // TODO: add animation + sound for attacks

        if (Input.GetMouseButton(0))
        {
            _movementVector.x = 0;
            _movementVector.y = 0;

            myHands.UseLeftHand();
            playerAnimations.StopPlayerMovement(.5f);
        }

        if (Input.GetMouseButton(1))
        {
            _movementVector.x = 0;
            _movementVector.y = 0;

            myHands.UseRightHand();
            playerAnimations.StopPlayerMovement(.5f);
        }
    }

    private void Die()
    {
        _isDying = true;
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
            CurrentItemStats currentItem = collision.GetComponent<CurrentItemStats>();
            TakeDamage(currentItem.GetModifierValue() + currentItem.myCharStats.baseDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 16)
        {
            TakeDamage(other.GetComponent<StaticTrap>().GetDamage());
        }
    }
}