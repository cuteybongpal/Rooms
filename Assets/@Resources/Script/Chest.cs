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

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && closedSprite != null)
        {
            spriteRenderer.sprite = closedSprite;
        }
    }

    public void Interact()
    {
        // ���� ����� �α� �߰� ����
        Debug.Log("--- 1. Chest: Interact() �Լ� ȣ���. ��ȣ�ۿ� ����. ---");

        if (!isOpen && itemInChest != null)
        {
            // ���� ����� �α� �߰� ����
            Debug.Log("--- 2. Chest: ������(" + itemInChest.itemName + ")�� �κ��丮�� �߰� �õ�. ---");
            bool success = Inventory.instance.AddItem(itemInChest);

            if (success)
            {
                isOpen = true;
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = openSprite;
                }
                UIManager.instance.ShowNotification("You got a " + itemInChest.itemName + "!");
            }
            else
            {
                // ���� ����� �α� �߰� ����
                Debug.Log("--- 2-1. Chest: �κ��丮 �߰� ����! (���� ���� �Ǵ� �ٸ� ����) ---");
                UIManager.instance.ShowNotification("Inventory is full!");
            }
        }
    }
}