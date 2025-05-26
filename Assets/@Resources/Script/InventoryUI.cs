// InventoryUI.cs
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform slotContainer; // 3-1���� ���� SlotContainer
    public GameObject slotPrefab;   // 3-2���� ���� Slot ������

    private Inventory inventory;
    private InventorySlot[] slots;

    void Start()
    {
        inventory = Inventory.instance;
        // �κ��丮 ���� �̺�Ʈ�� �߻��ϸ� UpdateUI �Լ��� ȣ���ϵ��� ���
        inventory.onInventoryChangedCallback += UpdateUI;

        // �κ��丮 ������ŭ �̸� ���Ե��� ����
        slots = new InventorySlot[inventory.space];
        for (int i = 0; i < inventory.space; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotContainer);
            slots[i] = slotObj.GetComponent<InventorySlot>();
        }

        UpdateUI(); // �ʱ� UI ������Ʈ
    }

    void UpdateUI()
    {
        // �κ��丮�� �ִ� �����۵��� ���Կ� �׸�
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