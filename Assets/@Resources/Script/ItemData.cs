// ItemData.cs ���� �ڵ�
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName; // ������ �̸�
    public string description; // ������ ����
    public Sprite icon; // ������ ������
    // public int maxStack; // �ִ� �� ������ ��ĥ �� �ִ��� (���߿� �߰�)
}