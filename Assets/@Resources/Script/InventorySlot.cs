// InventorySlot.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro�� ����ϹǷ� �߰�

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI itemCountText;

    public void ClearSlot()
    {
        icon.enabled = false; // ������ �Ⱥ��̰�
        itemCountText.enabled = false; // ī��Ʈ �Ⱥ��̰�
    }

    public void DrawSlot(ItemData item)
    {
        icon.enabled = true;
        itemCountText.enabled = true;

        icon.sprite = item.icon;
        itemCountText.text = "1"; // ������ ���� 1�� ����, ���߿� ���� ��� �߰� �� ����
    }
}