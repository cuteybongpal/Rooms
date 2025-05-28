using UnityEngine;
using System.Collections; // �ڷ�ƾ�� ����ϱ� ���� �ʿ��մϴ�.

public class Door : MonoBehaviour, IInteractable
{
    [Header("Door Sprites")]
    public Sprite doorSprite1_Closed;     // ���� ������ ���� �̹��� (door1)
    public Sprite doorSprite2_Intermediate; // ���� �߰� �ܰ��� ���� �̹��� (door2)
    public Sprite doorSprite3_Open;       // ���� ������ ������ ���� �̹��� (door3)

    [Header("Animation Settings")]
    public float animationStepDelay = 0.25f; // �� �ִϸ��̼� ������ ������ �ð� ���� (��)

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D doorCollider;

    private bool isOpen = false;          // ���� ���� ������ ���� �������� ����
    private bool isAnimating = false;     // ���� �ִϸ��̼��� ��� ������ ����

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<BoxCollider2D>();

        if (spriteRenderer == null)
        {
            Debug.LogError(gameObject.name + ": Door ��ũ��Ʈ�� SpriteRenderer ������Ʈ�� �����ϴ�!");
            enabled = false; return;
        }
        if (doorCollider == null)
        {
            Debug.LogError(gameObject.name + ": Door ��ũ��Ʈ�� BoxCollider2D ������Ʈ�� �����ϴ�!");
            enabled = false; return;
        }
        if (doorSprite1_Closed == null || doorSprite2_Intermediate == null || doorSprite3_Open == null)
        {
            Debug.LogError(gameObject.name + ": Door ��ũ��Ʈ�� Inspector â���� �ϳ� �̻��� Door Sprite�� �Ҵ���� �ʾҽ��ϴ�!");
            enabled = false; return;
        }

        // �ʱ� ����: ���� ���� ����
        spriteRenderer.sprite = doorSprite1_Closed;
        doorCollider.isTrigger = false;
        isOpen = false;
    }

    public void Interact()
    {
        // �̹� �ִϸ��̼� ���̶�� �ߺ� ���� ����
        if (isAnimating)
        {
            Debug.Log(gameObject.name + ": ���� ���� �����̴� ���Դϴ�.");
            return;
        }

        if (isOpen) // ���� �����ִٸ� -> �ݴ� �ִϸ��̼� ����
        {
            StartCoroutine(AnimateDoor(false)); // false�� �ݴ� ���� �ǹ�
        }
        else // ���� �����ִٸ� -> ���� �ִϸ��̼� ����
        {
            StartCoroutine(AnimateDoor(true)); // true�� ���� ���� �ǹ�
        }
    }

    // �� �ִϸ��̼��� ó���ϴ� �ڷ�ƾ
    // bool shouldOpen: true�� ���� �ִϸ��̼�, false�� �ݴ� �ִϸ��̼�
    IEnumerator AnimateDoor(bool shouldOpen)
    {
        isAnimating = true; // �ִϸ��̼� ����

        if (shouldOpen) // �� ����
        {
            Debug.Log(gameObject.name + " �� ������ ����...");
            // ���� ������ ���ȿ��� ���� �Ұ��� (isTrigger = false ���� ����)
            doorCollider.isTrigger = false;

            // spriteRenderer.sprite = doorSprite1_Closed; // �̹� ���� ������ ���̹Ƿ� ���� ����
            // yield return new WaitForSeconds(animationStepDelay); // ���� ���¿��� �ٷ� ù ����������

            spriteRenderer.sprite = doorSprite2_Intermediate;
            yield return new WaitForSeconds(animationStepDelay);

            spriteRenderer.sprite = doorSprite3_Open;
            doorCollider.isTrigger = true; // ������ ���� �� ���� ����
            isOpen = true;
            Debug.Log(gameObject.name + " �� Ȱ¦ ���� (isTrigger = true, ��� ����)");
        }
        else // �� �ݱ�
        {
            Debug.Log(gameObject.name + " �� ������ ����...");
            // ���� ������ �����ϸ� �ٷ� ���� �Ұ���
            doorCollider.isTrigger = false;

            // spriteRenderer.sprite = doorSprite3_Open; // �̹� ���� ������ ���̹Ƿ� ���� ����
            // yield return new WaitForSeconds(animationStepDelay); // ���� ���¿��� �ٷ� ù ����������

            spriteRenderer.sprite = doorSprite2_Intermediate;
            yield return new WaitForSeconds(animationStepDelay);

            spriteRenderer.sprite = doorSprite1_Closed;
            isOpen = false;
            Debug.Log(gameObject.name + " �� ������ ���� (isTrigger = false, ��� �Ұ���)");
        }

        isAnimating = false; // �ִϸ��̼� ����
    }
}