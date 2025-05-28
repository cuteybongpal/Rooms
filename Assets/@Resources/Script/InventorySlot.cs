// InventorySlot.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro를 사용하므로 추가

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI itemCountText;

    public void ClearSlot()
    {
        icon.enabled = false; // 아이콘 안보이게
        itemCountText.enabled = false; // 카운트 안보이게
    }

    public void DrawSlot(ItemData item)
    {
        icon.enabled = true;
        itemCountText.enabled = true;

        icon.sprite = item.icon;
        itemCountText.text = "1"; // 지금은 개수 1로 고정, 나중에 스택 기능 추가 시 수정
    }
}