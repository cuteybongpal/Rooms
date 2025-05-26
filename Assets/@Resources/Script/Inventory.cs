// Inventory.cs
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    public List<ItemData> items = new List<ItemData>();
    public int space = 20;

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
    }

    public bool AddItem(ItemData item)
    {
        // ���� ����� �α� �߰� ����
        Debug.Log("--- 3. Inventory: AddItem() �Լ� ����. ������: " + item.itemName + " ---");
        if (items.Count >= space)
        {
            return false;
        }

        items.Add(item);

        // ���� ����� �α� �߰� ����
        Debug.Log("--- 4. Inventory: ������ ����Ʈ�� �߰� �Ϸ�. ���� ������ ��: " + items.Count + "��. ---");

        if (onInventoryChangedCallback != null)
        {
            // ���� ����� �α� �߰� ����
            Debug.Log("--- 5. Inventory: onInventoryChangedCallback �̺�Ʈ ȣ��! UI ������Ʈ ��ȣ ����. ---");
            onInventoryChangedCallback.Invoke();
        }
        return true;
    }

    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
        if (onInventoryChangedCallback != null)
        {
            onInventoryChangedCallback.Invoke();
        }
    }

    public bool HasItem(ItemData item)
    {
        return items.Contains(item);
    }
}