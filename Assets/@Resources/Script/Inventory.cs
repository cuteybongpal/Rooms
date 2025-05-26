// Inventory.cs ����

using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    // �κ��丮�� ����� �� ȣ��� �̺�Ʈ�� �����մϴ�.
    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    public List<ItemData> items = new List<ItemData>();
    public int space = 20; // �κ��丮 ����

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
    }

    public bool AddItem(ItemData item)
    {
        if (items.Count >= space)
        {
            Debug.Log("�κ��丮�� �� á���ϴ�.");
            return false;
        }

        items.Add(item);
        Debug.Log(item.itemName + "��(��) ȹ���ߴ�!");

        // �������� ����Ǿ����� ��ο��� �˸�!
        if (onInventoryChangedCallback != null)
        {
            onInventoryChangedCallback.Invoke();
        }
        return true;
    }

    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
        // �������� ����Ǿ����� ��ο��� �˸�!
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
