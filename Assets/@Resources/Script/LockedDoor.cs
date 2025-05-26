// LockedDoor.cs
using UnityEngine;

public class LockedDoor : MonoBehaviour, IInteractable
{
    [Tooltip("�� ���� ���� �� �ʿ��� ���� ������ ������")]
    public ItemData requiredKey;

    private bool isLocked = true;

    public void Interact()
    {
        if (isLocked)
        {
            if (Inventory.instance.HasItem(requiredKey))
            {
                isLocked = false;
                UIManager.instance.ShowNotification("Click! The door opened."); // �˸����� ����
                Inventory.instance.RemoveItem(requiredKey);
                gameObject.SetActive(false);
            }
            else
            {
                UIManager.instance.ShowNotification("The door is locked."); // �˸����� ����
            }
        }
        else
        {
            UIManager.instance.ShowNotification("The door is already open."); // �˸����� ����
        }
    }
}