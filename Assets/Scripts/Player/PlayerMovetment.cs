using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovetment : MonoBehaviour
{
    public Animator _animator;
    public float speed;
    public float rotationSpeed = 720f;  // �������� �������� ������ ��� Y
    public VariableJoystick variableJoystickMove;
    public VariableJoystick variableJoystickRotate;

    Vector3 directionMove;
    public CharacterController controller;

    public float gravity = -9.81f;  // ���� ����������
    public float groundDistance = 0.2f;  // ���������� �� �����
    public LayerMask groundMask;  // ����� ���� �����

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
        // ���������, ��������� �� �������� �� �����
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // �������� ������������ �������� ��� �����������
        }

        // ����������� ����������� � ��������� ���������� ��� �����������
        directionMove = transform.right * variableJoystickMove.Horizontal + transform.forward * variableJoystickMove.Vertical;

        // ���� ���� ��������, �� ��������� �������� ����
        if (directionMove != Vector3.zero)
        {
            _animator.SetBool("isRun", true);
        }
        else
        {
            _animator.SetBool("isRun", false);
        }

        // ����������� ��������� ����� CharacterController
        controller.Move(directionMove * speed * Time.fixedDeltaTime);

        // �������� ��������� � ����������� �� ��������� ��������
        float rotateHorizontal = variableJoystickRotate.Horizontal;

        // ������������ ��������� �� ������ ��������������� ����� � ��������� ��������
        transform.Rotate(Vector3.up * rotateHorizontal * rotationSpeed * Time.fixedDeltaTime);

        // ��������� ����������
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
