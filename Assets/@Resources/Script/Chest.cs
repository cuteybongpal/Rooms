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
        // ▼▼▼ 디버그 로그 추가 ▼▼▼
        Debug.Log("--- 1. Chest: Interact() 함수 호출됨. 상호작용 시작. ---");

        if (!isOpen && itemInChest != null)
        {
            // ▼▼▼ 디버그 로그 추가 ▼▼▼
            Debug.Log("--- 2. Chest: 아이템(" + itemInChest.itemName + ")을 인벤토리에 추가 시도. ---");
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
                // ▼▼▼ 디버그 로그 추가 ▼▼▼
                Debug.Log("--- 2-1. Chest: 인벤토리 추가 실패! (공간 부족 또는 다른 문제) ---");
                UIManager.instance.ShowNotification("Inventory is full!");
            }
        }
    }
}