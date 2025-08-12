using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class KarakterController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float rotationSpeed = 10f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isJumping;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Yer kontrolü
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Düşme hızını sıfırla (küçük negatif değer yerçekimi için)
        }

        // Klavye inputlarını al
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Kamera yönlerini al, y eksenini sıfırla ve normalize et
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // Hareket vektörünü kamera bazlı oluştur
        Vector3 hareket = camForward * vertical + camRight * horizontal;

        // Koşma durumu
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : walkSpeed;

        // Eğer hareket varsa karakteri hareket yönüne döndür
        if (hareket.sqrMagnitude > 0.01f) // küçük hareketleri yoksay
        {
            Quaternion hedefRotation = Quaternion.LookRotation(hareket);
            transform.rotation = Quaternion.Slerp(transform.rotation, hedefRotation, rotationSpeed * Time.deltaTime);
        }

        // Karakteri hareket ettir
        controller.Move(hareket * speed * Time.deltaTime);

        // Zıplama kontrolü
        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;
            animator.SetBool("isJumping", true);
        }

        // Yerçekimi uygula
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Animasyon parametrelerini güncelle
        animator.SetFloat("Speed", hareket.magnitude * (isRunning ? 2f : 1f));
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isGrounded", isGrounded);

        // Zıplama animasyonu tamamlandığında geri dön
        if (isGrounded && velocity.y <= 0)
        {
            velocity.y = -2f;
            if (isJumping)
            {
                isJumping = false;
                animator.SetBool("isJumping", false);
            }
        }
    }
}
