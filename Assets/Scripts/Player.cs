using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public int moveSpeed = 10;
    public int maxSpeed = 5;
    public int jumpForce = 50;

    public Weapon[] backArmWeapons = new Weapon[4];
    public Weapon[] frontArmWeapons = new Weapon[4];

    private PlayerControls inputActions;
    private Rigidbody rb;
    private int currSpeed = 0;
    private bool movingL = false;
    private bool movingR = false;

    // Start is called before the first frame update
    void Start()
    {
        inputActions = new PlayerControls();
        rb = gameObject.GetComponent<Rigidbody>();

        inputActions.GunGuy.MoveLeft.performed += ctx => MoveLeft(true);
        inputActions.GunGuy.MoveLeft.canceled += ctx => MoveLeft(false);
        inputActions.GunGuy.MoveLeft.Enable();

        inputActions.GunGuy.MoveRight.performed += ctx => MoveRight(true);
        inputActions.GunGuy.MoveRight.canceled += ctx => MoveRight(false);
        inputActions.GunGuy.MoveRight.Enable();
    }

    private void MoveLeft(bool pressed)
    {
        if (pressed)
        {
            currSpeed += -moveSpeed;
            movingL = true;
        }
        else
        {
            currSpeed += moveSpeed;
            movingL = false;
        }
    }

    private void MoveRight(bool pressed)
    {
        if (pressed)
        {
            currSpeed += moveSpeed;
            movingR = true;
        }
        else
        {
            currSpeed += -moveSpeed;
            movingR = false;
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < maxSpeed)
            rb.AddForce(currSpeed, 0, 0);

        if (movingL && movingR)
            if (IsGrounded())
                rb.AddForce(currSpeed * 2, jumpForce, 0);
    }

    public bool IsGrounded()
    {
        float capsuleHeight = gameObject.GetComponent<CapsuleCollider>().bounds.extents.y;
        return Physics.Raycast(transform.position, -Vector3.up, capsuleHeight + 0.05f);
    }

    public void EnactForce(Vector3 force)
    {
        rb.AddForce(force);
    }
}
