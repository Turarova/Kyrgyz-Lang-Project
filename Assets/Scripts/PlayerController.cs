using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Настройки движения")]
    [Tooltip("Скорость передвижения персонажа")]
    public float moveSpeed = 5f;

    [Header("Состояние")]
    [Tooltip("Можно ли управлять персонажем прямо сейчас")]
    public bool canMove = false; // На старте управление заблокировано

    private Vector3 targetPosition;
    private bool isMoving = false;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPosition = transform.position;
    }

    void Update()
    {
        // Если персонаж еще просыпается — игнорируем любые клики
        if (!canMove) return;

        // 1. Отслеживание клика мыши / тапа
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            mouseWorldPos.z = transform.position.z;
            
            targetPosition = mouseWorldPos;
            isMoving = true;

            if (spriteRenderer != null)
            {
                if (targetPosition.x < transform.position.x)
                    spriteRenderer.flipX = true;
                else if (targetPosition.x > transform.position.x)
                    spriteRenderer.flipX = false;
            }
        }

        // 2. Движение к точке
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
            {
                isMoving = false;
            }
        }

        // 3. Анимация ходьбы
        if (animator != null)
        {
            animator.SetBool("IsWalking", isMoving);
        }
    }

    // Этот метод вызовет анимация в самом конце пробуждения
    public void OnWakeUpComplete()
    {
        canMove = true;
        targetPosition = transform.position; // Сбрасываем старые клики, если игрок нажимал во время заставки
    }
}