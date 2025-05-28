// InventoryUI.cs
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform slotContainer;
    public GameObject slotPrefab;

    private Inventory inventory;
    private InventorySlot[] slots;

    void OnEnable()
    {
        // ▼▼▼ 디버그 로그 추가 ▼▼▼
        Debug.Log("--- A. InventoryUI: OnEnable() 호출됨 (인벤토리 창 켜짐). ---");
        if (inventory != null && slots != null)
        {
            UpdateUI();
        }
    }

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onInventoryChangedCallback += UpdateUI;
        slots = new InventorySlot[inventory.space];
        for (int i = 0; i < inventory.space; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotContainer);
            slots[i] = slotObj.GetComponent<InventorySlot>();
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        // ▼▼▼ 디버그 로그 추가 ▼▼▼
        Debug.Log("--- 6. InventoryUI: UpdateUI() 함수 호출됨! 화면 새로고침 시작. ---");
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                // ▼▼▼ 디버그 로그 추가 ▼▼▼
                Debug.Log("--- 7. InventoryUI: 슬롯 " + i + "에 아이템 " + inventory.items[i].itemName + " 그리는 중... ---");
                slots[i].DrawSlot(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
} 