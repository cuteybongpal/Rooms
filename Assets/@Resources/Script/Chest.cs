// Chest.cs
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [Header("상자 내용물")]
    public ItemData itemInChest;

    [Header("상자 상태")]
    public Sprite openSprite;
    public Sprite closedSprite;

    private SpriteRenderer spriteRenderer;
    private bool isOpen = false;
    // private UIManager uiManager; // 이 변수는 이제 필요 없습니다.

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // uiManager = FindObjectOfType<UIManager>(); // 이 줄도 필요 없습니다.

        if (spriteRenderer != null && closedSprite != null)
        {
            spriteRenderer.sprite = closedSprite;
        }
    }

    public void Interact()
    {
        if (!isOpen && itemInChest != null)
        {
            bool success = Inventory.instance.AddItem(itemInChest);

            if (success)
            {
                isOpen = true;
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = openSprite;
                }
                // 'UIManager.instance'로 직접 접근
                UIManager.instance.ShowNotification("You got a " + itemInChest.itemName + "!");
            }
            else
            {
                // 'UIManager.instance'로 직접 접근
                UIManager.instance.ShowNotification("Inventory is full!");
            }
        }

    }
}