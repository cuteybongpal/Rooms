// ItemData.cs 예시 코드
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName; // 아이템 이름
    public string description; // 아이템 설명
    public Sprite icon; // 아이템 아이콘
    // public int maxStack; // 최대 몇 개까지 겹칠 수 있는지 (나중에 추가)
}