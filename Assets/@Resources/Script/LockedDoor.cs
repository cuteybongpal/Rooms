// LockedDoor.cs
using UnityEngine;

public class LockedDoor : MonoBehaviour, IInteractable
{
    [Tooltip("이 문을 여는 데 필요한 열쇠 아이템 데이터")]
    public ItemData requiredKey;

    private bool isLocked = true;

    public void Interact()
    {
        if (isLocked)
        {
            if (Inventory.instance.HasItem(requiredKey))
            {
                isLocked = false;
                UIManager.instance.ShowNotification("Click! The door opened."); // 알림으로 변경
                Inventory.instance.RemoveItem(requiredKey);
                gameObject.SetActive(false);
            }
            else
            {
                UIManager.instance.ShowNotification("The door is locked."); // 알림으로 변경
            }
        }
        else
        {
            UIManager.instance.ShowNotification("The door is already open."); // 알림으로 변경
        }
    }
}