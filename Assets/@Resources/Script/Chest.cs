// Chest.cs
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [Header("���� ���빰")]
    public ItemData itemInChest;

    [Header("���� ����")]
    public Sprite openSprite;
    public Sprite closedSprite;

    private SpriteRenderer spriteRenderer;
    private bool isOpen = false;
    // private UIManager uiManager; // �� ������ ���� �ʿ� �����ϴ�.

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // uiManager = FindObjectOfType<UIManager>(); // �� �ٵ� �ʿ� �����ϴ�.

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
                // 'UIManager.instance'�� ���� ����
                UIManager.instance.ShowNotification("You got a " + itemInChest.itemName + "!");
            }
            else
            {
                // 'UIManager.instance'�� ���� ����
                UIManager.instance.ShowNotification("Inventory is full!");
            }
        }

    }
}