using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePlayerInput
{
    KEYBOARD = 0,
    GAMEPAD = 1,
}
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public ePlayerInput playerInput;
    public Rigidbody body;
    public float moveSpeed;
    public float turnSpeedY;
    public float turnSpeedX;
    public float jumpSpeed;

    public float maxLookUpAngle;
    public float maxLookDownAngle;

    public float interactionDistance;

    public PlayerInventory inventory;

    public Transform upDownTransform;

    void Awake()
    {
        Instance = this;
    }
    void OnDestroy()
    {
        Instance = null;
    }

    void Update()
    {
        if (Room.Instance.Mode == eMode.NORMAL)
        {
            UpdateLook();
            CheckPointingRaycast();
            CheckInteractions();
        }
    }

    void FixedUpdate()
    {
        if (Room.Instance.Mode == eMode.NORMAL)
        {
            UpdateMovement();
            UpdateJump();
        }
    }

    public void StartDefusal()
    {
        //do something?
    }
    public void ReturnToNormal()
    {

    }

    void UpdateMovement()
    {
        var move = GetMoveVector();
        if (move.sqrMagnitude <= 0)
            return;

        var point = transform.forward;
        point.y = 0;
        point.Normalize();

        var right = transform.right;
        right.y = 0;
        right.Normalize();

        var moveForce = Time.fixedDeltaTime * moveSpeed * ((move.y * point) + (move.x * right));
        body.AddForce(moveForce, ForceMode.VelocityChange);
    }

    void UpdateLook()
    {
        var lookDelta = GetLookVector();

        //turn in y axis:
        var yawAngle = lookDelta.x * Time.deltaTime * turnSpeedX;
        var yaw = Quaternion.AngleAxis(yawAngle, Vector3.up);
        transform.rotation = yaw * transform.rotation;

        var pitchAngle = lookDelta.y * Time.deltaTime * turnSpeedY;
        //what's our pointing angle relative to Y axis?
        var currentAngle = Vector3.Angle(upDownTransform.forward, Vector3.up);

        if (pitchAngle < 0 && currentAngle < (90 - maxLookUpAngle))
            pitchAngle = 0f;

        if (pitchAngle > 0 && currentAngle > (90 + maxLookDownAngle))
            pitchAngle = 0f;

        var pitch = Quaternion.AngleAxis(pitchAngle, upDownTransform.right);
        upDownTransform.rotation = pitch * upDownTransform.rotation;
    }

    void UpdateJump()
    {
        var jumpButton = GetJumpInput();
        var onGround = false;

        //ground check

        var layerMask = LayerMask.GetMask("Blockers");
        layerMask |= LayerMask.GetMask("Floor");

        var ray = new Ray(transform.position, Vector3.down);
        var hit = Physics.Raycast(ray, out var hitInfo, interactionDistance, layerMask);

        if (hit)
        {
            //prox?
            var hitLocation = hitInfo.point;
            var dist = (hitLocation - transform.position).magnitude;
            var currentHeight = transform.localScale.y + 0.05f; //just get a little closer
            if (dist <= currentHeight)
                onGround = true;
        }

        if(jumpButton && onGround)
        {
            body.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
        }
    }

    void CheckPointingRaycast()
    {
        var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        var ray = Camera.main.ScreenPointToRay(screenCenter);

        var layerMask = LayerMask.GetMask("Interactives");
        var blockerLayerMask = LayerMask.GetMask("Blockers");
        var hit = Physics.Raycast(ray, out var hitInfo, interactionDistance, layerMask | blockerLayerMask);

        if (hit)
        {
            Room.Instance.CurrentPlayerPointingTarget(hitInfo.collider);
        }
        else
        {
            Room.Instance.CurrentPlayerPointingAtNothing();
        }
    }

    void CheckInteractions()
    {
        (bool action1, bool action2) = GetActionInput();

        if (action1)
        {
            Room.Instance.InteractWithTarget();
        }
        else if (action2)
        {
            Room.Instance.SecondaryInteractionWithTarget();
        }
    }

    public void GainCollectableItem(CollectableItem zItem)
    {
        inventory.TryAddItem(zItem);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        var ray = Camera.main.ScreenPointToRay(screenCenter);
        Gizmos.DrawRay(ray.origin, ray.direction * interactionDistance);
    }


    (bool action1, bool action2) GetActionInput()
    {

        //quite simply, do we have input?
        bool action1 = false;
        bool action2 = false;

        switch (playerInput)
        {
            default:
            case ePlayerInput.KEYBOARD:
                action1 = Input.GetKeyDown(KeyCode.E);
                action2 = Input.GetKeyDown(KeyCode.F);
                break;

            case ePlayerInput.GAMEPAD:
                action1 = PlayerInputManager.GetButtonDown(0, ePadButton.FACE_DOWN);
                action2 = PlayerInputManager.GetButtonDown(0, ePadButton.FACE_LEFT);
                break;
        }
        return (action1, action2);
    }

    bool GetJumpInput()
    {
        bool action = false;

        switch (playerInput)
        {
            default:
            case ePlayerInput.KEYBOARD:
                action = Input.GetKeyDown(KeyCode.Space);
                break;

            case ePlayerInput.GAMEPAD:
                action = PlayerInputManager.GetButtonDown(0, ePadButton.FACE_UP);
                break;
        }
        return action;
    }
    Vector2 GetMoveVector()
    {
        var move = Vector2.zero;

        switch (playerInput)
        {
            default:
            case ePlayerInput.KEYBOARD:
                if (Input.GetKey(KeyCode.W))
                    move.y += 1;
                if (Input.GetKey(KeyCode.S))
                    move.y -= 1;
                if (Input.GetKey(KeyCode.A))
                    move.x -= 1;
                if (Input.GetKey(KeyCode.D))
                    move.x += 1;
                return move;

            case ePlayerInput.GAMEPAD:
                move.y += PlayerInputManager.GetAxis(0, ePadAxis.L_STICK_VERTICAL);
                move.x += PlayerInputManager.GetAxis(0, ePadAxis.L_STICK_HORIZONTAL);
                return move;
        }
    }

    Vector2 GetLookVector()
    {
        var look = Vector2.zero;

        switch (playerInput)
        {
            default:
            case ePlayerInput.KEYBOARD:
                //mouse movement?
                // move.x = Input.GetAxis("Mouse X");
                // move.y = Input.GetAxis("Mouse Y");

                look.x -= Input.GetKey(KeyCode.LeftArrow) ? 1 : 0;
                look.x += Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
                look.y += Input.GetKey(KeyCode.UpArrow) ? 1 : 0; //weird inversions
                look.y -= Input.GetKey(KeyCode.DownArrow) ? 1 : 0;

                look.x = Mathf.Clamp(look.x, -1f, 1f);
                look.y = Mathf.Clamp(look.y, -1f, 1f);

                return look;

            case ePlayerInput.GAMEPAD:
                look.y += PlayerInputManager.GetAxis(0, ePadAxis.R_STICK_VERTICAL);
                look.x += PlayerInputManager.GetAxis(0, ePadAxis.R_STICK_HORIZONTAL);
                return look;
        }

    }
}

