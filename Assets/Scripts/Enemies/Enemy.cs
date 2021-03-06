using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    public new string name = "Enemy";
    public so_NPCStats myStats;
    public HandsController myHands;
    public EnemyAnimations myAnimations;
    public so_NPCStats playerStats;
    public int whichMaterial = 0; // temporary solution for deciding which material to create
    
    protected int currentHp;
    protected int totalArmor;
    protected Transform target;
    protected Rigidbody2D rb;
    protected bool attacking = false;
    protected float currentInvincibleTimer = 0f;
    protected float speed;

    [SerializeField]
    protected string deathSoundName = "DeathMonster";
    [SerializeField]
    protected string damageSoundName = "DamagedMonster";
    [SerializeField]
    protected float invincibleTime = 1f;

    protected float dissolveAmount;
    protected bool isDying;
    //protected Material myMaterial;
    protected MaterialPropertyBlock propBlock;

    [SerializeField]
    protected Renderer myRenderer;
    [SerializeField]
    private so_GameEvent onDeathEvent;

    private static readonly int DissolveValue = Shader.PropertyToID("_DissolveValue");

    protected virtual void Start()
    {
        switch (whichMaterial)
        {
            case 0:
                myRenderer.material = Instantiate(GameAssets.I.diffuseMaterial1);
                break;
            case 1:
                myRenderer.material = Instantiate(GameAssets.I.diffuseMaterial2);
                break;
        }
        //myMaterial = myRenderer.material;
        isDying = false;
        dissolveAmount = 1f;
        propBlock = new MaterialPropertyBlock();
        //myMaterial.SetFloat(DissolveValue, dissolveAmount);

        currentHp = myStats.maxHp;
        SetTarget(PlayerManager.instance.player.transform);
        rb = GetComponent<Rigidbody2D>();
        totalArmor = myStats.baseArmor;
        speed = myStats.movementSpeed;
    }

    private void Update()
    {
        if (isDying)
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount - Time.deltaTime);
            myRenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat(DissolveValue, dissolveAmount);
            myRenderer.SetPropertyBlock(propBlock);
            //myMaterial.SetFloat(DissolveValue, dissolveAmount);
        }

        currentInvincibleTimer -= Time.deltaTime;
        if(currentInvincibleTimer <= 0)
            myAnimations.UpdateInvincibilityBool(false);
    }

    private void LateUpdate()
    {
        if(target != null)
            myAnimations.UpdateIdleAnimation(target.position - transform.position);
    }

    public void UpdateArmor(int modifierValue)
    {
        totalArmor = myStats.baseArmor + modifierValue;
    }

    public virtual void Move()
    {
        Debug.LogError("Implement Move Method!");
    }

    public virtual void MoveAway()
    {
        Debug.LogError("Implement MoveAway Method!");
    }

    public virtual void Attack()
    {
        Debug.LogError("Implement Attack Method!");
    }

    public virtual bool UseAbility()
    {
        Debug.LogError("Implement UseAbility Method!");
        return false;
    }

    public virtual void Idle()
    {
        myAnimations.PlayMovementAnimation(new Vector2(0, 0));
    }

    public virtual void SetTarget(Transform target)
    {
        this.target = target;

        if(myHands != null && myHands.enabled)
            myHands.SetFollowTarget(target);
    }

    public virtual void TakeDamage(int damageAmount)
    {
        if (currentInvincibleTimer > 0)
            return;
        
        myAnimations.UpdateInvincibilityBool(true);
        
        currentInvincibleTimer = invincibleTime;
        //Debug.Log("Damage taken!");
        damageAmount = (int)(damageAmount * (100f / (100f + totalArmor)));
        currentHp -= damageAmount;

        AudioManager.instance.PlayOneShotSound(damageSoundName);

        TextPopup.Create(transform.position, damageAmount);

        if (currentHp <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            Invoke(nameof(Die), 0.5f);
        }
    }

    // Can be overwritten to return the desired bool value
    public virtual bool CheckBool(int whichBool = 0)
    {
        return false;
    }

    protected virtual void Die()
    {
        if (isDying)
            return;
        
        isDying = true;
        AudioManager.instance.PlayOneShotSound(deathSoundName);
        if(myHands != null)
            myHands.gameObject.SetActive(false);
        onDeathEvent.Raise();
        
        Destroy(gameObject, 1f);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            int tempDmg = playerStats.baseDamage > 0 ? playerStats.baseDamage : 0;
            TakeDamage(collision.GetComponent<CurrentItemStats>().GetModifierValue() + tempDmg);
        }
    }
}
