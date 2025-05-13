// 상호작용 가능한 객체들이 구현해야 하는 인터페이스입니다.
public interface IInteractable
{
    // 이 인터페이스를 구현하는 모든 클래스는 Interact() 라는 이름의 함수를 가져야 합니다.
    // 이 함수는 객체와 상호작용했을 때 호출될 로직을 담습니다.
    void Interact();
}