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
        // ���� ����� �α� �߰� ����
        Debug.Log("--- A. InventoryUI: OnEnable() ȣ��� (�κ��丮 â ����). ---");
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
        // ���� ����� �α� �߰� ����
        Debug.Log("--- 6. InventoryUI: UpdateUI() �Լ� ȣ���! ȭ�� ���ΰ�ħ ����. ---");
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                // ���� ����� �α� �߰� ����
                Debug.Log("--- 7. InventoryUI: ���� " + i + "�� ������ " + inventory.items[i].itemName + " �׸��� ��... ---");
                slots[i].DrawSlot(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
} 