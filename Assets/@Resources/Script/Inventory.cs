// Inventory.cs 수정

using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    // 인벤토리가 변경될 때 호출될 이벤트를 정의합니다.
    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    public List<ItemData> items = new List<ItemData>();
    public int space = 20; // 인벤토리 공간

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
    }

    public bool AddItem(ItemData item)
    {
        if (items.Count >= space)
        {
            Debug.Log("인벤토리가 꽉 찼습니다.");
            return false;
        }

        items.Add(item);
        Debug.Log(item.itemName + "을(를) 획득했다!");

        // 아이템이 변경되었음을 모두에게 알림!
        if (onInventoryChangedCallback != null)
        {
            onInventoryChangedCallback.Invoke();
        }
        return true;
    }

    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
        // 아이템이 변경되었음을 모두에게 알림!
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
