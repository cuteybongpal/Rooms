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
        // ▼▼▼ 디버그 로그 추가 ▼▼▼
        Debug.Log("--- 3. Inventory: AddItem() 함수 진입. 아이템: " + item.itemName + " ---");
        if (items.Count >= space)
        {
            return false;
        }

        items.Add(item);

        // ▼▼▼ 디버그 로그 추가 ▼▼▼
        Debug.Log("--- 4. Inventory: 아이템 리스트에 추가 완료. 현재 아이템 수: " + items.Count + "개. ---");

        if (onInventoryChangedCallback != null)
        {
            // ▼▼▼ 디버그 로그 추가 ▼▼▼
            Debug.Log("--- 5. Inventory: onInventoryChangedCallback 이벤트 호출! UI 업데이트 신호 보냄. ---");
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