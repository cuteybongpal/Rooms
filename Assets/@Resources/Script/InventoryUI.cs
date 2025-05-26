// InventoryUI.cs
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform slotContainer; // 3-1에서 만든 SlotContainer
    public GameObject slotPrefab;   // 3-2에서 만든 Slot 프리팹

    private Inventory inventory;
    private InventorySlot[] slots;

    void Start()
    {
        inventory = Inventory.instance;
        // 인벤토리 변경 이벤트가 발생하면 UpdateUI 함수를 호출하도록 등록
        inventory.onInventoryChangedCallback += UpdateUI;

        // 인벤토리 공간만큼 미리 슬롯들을 생성
        slots = new InventorySlot[inventory.space];
        for (int i = 0; i < inventory.space; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotContainer);
            slots[i] = slotObj.GetComponent<InventorySlot>();
        }

        UpdateUI(); // 초기 UI 업데이트
    }

    void UpdateUI()
    {
        // 인벤토리에 있는 아이템들을 슬롯에 그림
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].DrawSlot(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}