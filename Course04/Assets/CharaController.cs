using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharaController : MonoBehaviour
{
    private CharacterController cc;
    private Animator anim;

    [SerializeField]
    private float walkSpeed = 3.0f;

    [SerializeField]
    private float sprintSpeed = 10.0f;

    private Vector3 moveDir = Vector3.zero;
    private Vector3 sprintDir = Vector3.zero;

    [SerializeField]
    private float rotationSpeed = 720f;


    private void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = (moveDir + sprintDir) * Time.fixedDeltaTime;

        sprintDir = Vector3.Lerp(sprintDir, Vector3.zero, 5f * Time.fixedDeltaTime);

        if (cc.isGrounded)
        {
            direction -= Vector3.up;
        }
        else
        {
            direction += Physics.gravity * Time.fixedDeltaTime;
        }

        if (moveDir.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        cc.Move(direction);
        anim.SetFloat("Speed", cc.velocity.magnitude);
    }

    public void Move(Vector2 direction)
    {
        moveDir = new Vector3(direction.x, 0, direction.y).normalized;
        // moveDir = transform.TransformDirection(moveDir);
        moveDir *= walkSpeed;
    }

    public void Sprint()
    {
        sprintDir = new Vector3(0, 0, sprintSpeed);
        sprintDir = transform.TransformDirection(sprintDir);
    }
}
