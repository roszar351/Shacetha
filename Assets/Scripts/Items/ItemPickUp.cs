using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private so_Item item;
    
    private SpriteRenderer _mySprite;
    
    void Start()
    {
        _mySprite = GetComponentInChildren<SpriteRenderer>();
        if (_mySprite == null || item == null)
        {
            Debug.LogError("Missing item or Sprite renderer in: " + this.name);
            Destroy(gameObject);
            return;
        }
        _mySprite.sprite = item.weaponSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            PlayerManager.instance.playerInventory.Add(item);
            Destroy(gameObject);
        }
    }
}
