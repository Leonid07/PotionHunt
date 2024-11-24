using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovetment : MonoBehaviour
{
    public Animator _animator;
    public float speed;
    public float rotationSpeed = 720f;  // Скорость вращения вокруг оси Y
    public VariableJoystick variableJoystickMove;
    public VariableJoystick variableJoystickRotate;

    Vector3 directionMove;
    public CharacterController controller;

    public float gravity = -9.81f;  // Сила гравитации
    public float groundDistance = 0.2f;  // Расстояние до земли
    public LayerMask groundMask;  // Маска слоя земли

    Vector3 velocity;
    bool isGrounded;

    [Header("Particle")]
    public GameObject particleSouls;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        CanvasLoading.InstanceCanvasLoading.canvasPanel.SetActive(false);
    }

    void FixedUpdate()
    {
        // Проверяем, находится ли персонаж на земле
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // Обнуляем вертикальную скорость при приземлении
        }

        // Преобразуем направление в локальные координаты для перемещения
        directionMove = transform.right * variableJoystickMove.Horizontal + transform.forward * variableJoystickMove.Vertical;

        // Если есть движение, то запускаем анимацию бега
        if (directionMove != Vector3.zero)
        {
            _animator.SetBool("isRun", true);
        }
        else
        {
            _animator.SetBool("isRun", false);
        }

        // Передвигаем персонажа через CharacterController
        controller.Move(directionMove * speed * Time.fixedDeltaTime);

        // Вращение персонажа в зависимости от джойстика вращения
        float rotateHorizontal = variableJoystickRotate.Horizontal;

        // Поворачиваем персонажа на основе горизонтального ввода с джойстика вращения
        transform.Rotate(Vector3.up * rotateHorizontal * rotationSpeed * Time.fixedDeltaTime);

        // Применяем гравитацию
        velocity.y += gravity * Time.fixedDeltaTime;
        controller.Move(velocity * Time.fixedDeltaTime);
    }
    public GameObject panelLose;
    public void Dead()
    {
        _animator.Play("Die");
        particleSouls.SetActive(true);
        gameObject.tag = "Untagged";

        panelLose.SetActive(true);

        GetComponent<PlayerMovetment>().enabled = false;
    }
}
