using UnityEngine;

public class SideScrolling : MonoBehaviour
{
    private Transform player;
    private Camera mainCamera;
    private float cameraY;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        mainCamera = Camera.main;

        // Lưu vị trí y của camera
        cameraY = mainCamera.transform.position.y;
    }

    // LateUpdate để đảm bảo rằng camera di chuyển sau khi người chơi đã di chuyển
    void LateUpdate()
    {
        // Lấy vị trí hiện tại của người chơi
        Vector3 currentPlayerPosition = player.position;

        // Giữ nguyên vị trí y của camera
        currentPlayerPosition.y = Mathf.Clamp(currentPlayerPosition.y, mainCamera.ViewportToWorldPoint(Vector3.zero).y + 0.5f, mainCamera.ViewportToWorldPoint(Vector3.one).y - 0.5f);

        // Cập nhật vị trí mới của camera theo vị trí của người chơi
        mainCamera.transform.position = new Vector3(currentPlayerPosition.x, cameraY, mainCamera.transform.position.z);

        // Cập nhật vị trí mới của người chơi
        player.position = currentPlayerPosition;
    }
}
