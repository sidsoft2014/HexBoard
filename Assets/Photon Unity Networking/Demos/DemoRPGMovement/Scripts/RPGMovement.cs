using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RPGMovement : MonoBehaviour
{
    public float ForwardSpeed;
    public float BackwardSpeed;
    public float StrafeSpeed;
    public float RotateSpeed;

    private CharacterController m_CharacterController;
    private Vector3 m_LastPosition;
    private Animator m_Animator;
    private PhotonView m_PhotonView;
    private PhotonTransformView m_TransformView;

    private float m_AnimatorSpeed;
    private Vector3 m_CurrentMovement;
    private float m_CurrentTurnSpeed;

    private void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();
        m_PhotonView = GetComponent<PhotonView>();
        m_TransformView = GetComponent<PhotonTransformView>();
    }

    private void Update()
    {
        if (m_PhotonView.isMine == true)
        {
            ResetSpeedValues();

            UpdateRotateMovement();

            UpdateForwardMovement();
            UpdateBackwardMovement();
            UpdateStrafeMovement();

            MoveCharacterController();
            ApplyGravityToCharacterController();

            ApplySynchronizedValues();
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        Vector3 movementVector = transform.position - m_LastPosition;

        float speed = Vector3.Dot(movementVector.normalized, transform.forward);
        float direction = Vector3.Dot(movementVector.normalized, transform.right);

        if (Mathf.Abs(speed) < 0.2f)
        {
            speed = 0f;
        }

        if (speed > 0.6f)
        {
            speed = 1f;
            direction = 0f;
        }

        if (speed >= 0f)
        {
            if (Mathf.Abs(direction) > 0.7f)
            {
                speed = 1f;
            }
        }

        m_AnimatorSpeed = Mathf.MoveTowards(m_AnimatorSpeed, speed, Time.deltaTime * 5f);

        m_Animator.SetFloat("Speed", m_AnimatorSpeed);
        m_Animator.SetFloat("Direction", direction);

        m_LastPosition = transform.position;
    }

    private void ResetSpeedValues()
    {
        m_CurrentMovement = Vector3.zero;
        m_CurrentTurnSpeed = 0;
    }

    private void ApplySynchronizedValues()
    {
        m_TransformView.SetSynchronizedValues(m_CurrentMovement, m_CurrentTurnSpeed);
    }

    private void ApplyGravityToCharacterController()
    {
        m_CharacterController.Move(transform.up * Time.deltaTime * -9.81f);
    }

    private void MoveCharacterController()
    {
        m_CharacterController.Move(m_CurrentMovement * Time.deltaTime);
    }

    private void UpdateForwardMovement()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetAxisRaw("Vertical") > 0.1f)
        {
            m_CurrentMovement = transform.forward * ForwardSpeed;
        }
    }

    private void UpdateBackwardMovement()
    {
        if (Input.GetKey(KeyCode.S) || Input.GetAxisRaw("Vertical") < -0.1f)
        {
            m_CurrentMovement = -transform.forward * BackwardSpeed;
        }
    }

    private void UpdateStrafeMovement()
    {
        if (Input.GetKey(KeyCode.Q) == true)
        {
            m_CurrentMovement = -transform.right * StrafeSpeed;
        }

        if (Input.GetKey(KeyCode.E) == true)
        {
            m_CurrentMovement = transform.right * StrafeSpeed;
        }
    }

    private void UpdateRotateMovement()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetAxisRaw("Horizontal") < -0.1f)
        {
            m_CurrentTurnSpeed = -RotateSpeed;
            transform.Rotate(0.0f, -RotateSpeed * Time.deltaTime, 0.0f);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetAxisRaw("Horizontal") > 0.1f)
        {
            m_CurrentTurnSpeed = RotateSpeed;
            transform.Rotate(0.0f, RotateSpeed * Time.deltaTime, 0.0f);
        }
    }
}