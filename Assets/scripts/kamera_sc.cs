using UnityEngine;

public class kamera_sc : MonoBehaviour
{
    public Transform target;        // Takip edilecek karakterin transformu
    public Vector3 offset;          // Başlangıç offseti (kameranın karaktere göre konumu)

    public float smoothSpeed = 0.125f;  // Takip hızı
    public float mouseSensitivity = 100f;  // Fare hassasiyeti

    private float yaw = 0f;   // Yatay dönüş açısı
    private float pitch = 5f; // Sabit dikey açı (değişmeyecek)

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // Fareyi kilitle ve gizle (istersen kaldırabilirsin)
    }

    void LateUpdate()
    {
        // Sadece yatay fare hareketlerini oku
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;

        // Dikey açı sabit kalıyor, pitch değişmiyor

        // Yeni offseti hesapla: offset uzaklığını koruyarak target etrafında döndür
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position + rotation * offset;
    
        // Kamerayı yumuşakça hedef pozisyona yaklaştır
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Kameranın hedefe bakmasını sağla
        transform.LookAt(target.position + Vector3.up * 1.5f);  // Karakterin biraz üstüne bak
    }
}
