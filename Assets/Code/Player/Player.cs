using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public Rigidbody body;
    public float moveSpeed;
    public float turnSpeedY;
    public float turnSpeedX;
    public float jumpSpeed;

    public float maxLookUpAngle;
    public float maxLookDownAngle;

    public float mouseTurnSpeedX;
    public float mouseTurnSpeedY;
    public TextMesh mouseSpeedVisual;

    public float interactionDistance;

    public PlayerInventory inventory;
    public PlayerInstructions instructions;

    public Transform upDownTransform;

    public Animator explosion;

    void Awake()
    {
        Application.targetFrameRate = 60;
        Instance = this;
        ReturnToSpawn();
        mouseSpeedVisual.gameObject.EnsureActive(false);
    }

    void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
        Instance = null;
    }

    public void Reset()
    {
        explosion.SetTrigger("Reset");
        ReturnToSpawn();
        inventory.Reset();
        instructions.Reset();
    }

    public void Explode()
    {
        explosion.SetTrigger("Explode");
    }

    void ReturnToSpawn()
    {
        transform.position = Room.Instance.playerSpawn.position;
        transform.rotation = Room.Instance.playerSpawn.rotation;
    }

    void Update()
    {
        if (Room.Instance.Mode == eMode.NORMAL && PlaySession.Paused == false)
        {
            UpdateLookNonMouse();
            UpdateLookMouse();
            CheckPointingRaycast();
            CheckInteractions();

            Cursor.lockState = CursorLockMode.Locked;
        }

        if (PlaySession.Paused)
        {
            Cursor.lockState = CursorLockMode.None;

        }

        UpdateMouseSpeedInput();

    }

    void FixedUpdate()
    {
        if (Room.Instance.Mode == eMode.NORMAL && PlaySession.Paused == false)
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


    void UpdateLookNonMouse()
    {
        var look = GetLookVectorNonMouse();
        look.x *= turnSpeedX;
        look.y *= turnSpeedY;
        UpdateLook(look);
    }
    void UpdateLookMouse()
    {
        var look = GetLookVectorMouse();
        look.x *= mouseTurnSpeedX;
        look.y *= mouseTurnSpeedY;
        UpdateLook(look);
    }

    void UpdateLook(Vector2 zRot)
    {
        //turn in y axis:
        var yawAngle = zRot.x * Time.deltaTime;
        var yaw = Quaternion.AngleAxis(yawAngle, Vector3.up);
        transform.rotation = yaw * transform.rotation;

        var pitchAngle = zRot.y * Time.deltaTime;
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

        if (jumpButton && onGround)
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
        instructions.ItemCollected(zItem);
    }

    public bool CanCollect(eCollectableItem zItem)
    {
        return inventory.CanCollect(zItem);
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

        action1 = Input.GetKeyDown(KeyCode.E);

        action1 |= Input.GetMouseButtonDown(0);

        action1 |= PlayerInputManager.GetButtonDown(0, ePadButton.FACE_DOWN);
        return (action1, action2);
    }

    bool GetJumpInput()
    {
        bool action = false;

        action |= Input.GetKeyDown(KeyCode.Space);
        action |= PlayerInputManager.GetButtonDown(0, ePadButton.FACE_UP);
        return action;
    }
    Vector2 GetMoveVector()
    {
        var move = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            move.y += 1;
        if (Input.GetKey(KeyCode.S))
            move.y -= 1;
        if (Input.GetKey(KeyCode.A))
            move.x -= 1;
        if (Input.GetKey(KeyCode.D))
            move.x += 1;

        move.y += PlayerInputManager.GetAxis(0, ePadAxis.L_STICK_VERTICAL);

        move.x += PlayerInputManager.GetAxis(0, ePadAxis.L_STICK_HORIZONTAL);
        return move;
    }

    Vector2 GetLookVectorMouse()
    {
        var look = Vector2.zero;

        //mouse movement?
        look.x += Input.GetAxis("Mouse X");
        look.y += Input.GetAxis("Mouse Y");

        return look;
    }

    Vector2 GetLookVectorNonMouse()
    {
        var look = Vector2.zero;

        look.x -= Input.GetKey(KeyCode.LeftArrow) ? 1 : 0;
        look.x += Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
        look.y += Input.GetKey(KeyCode.UpArrow) ? 1 : 0; //weird inversions
        look.y -= Input.GetKey(KeyCode.DownArrow) ? 1 : 0;

        look.x = Mathf.Clamp(look.x, -1f, 1f);
        look.y = Mathf.Clamp(look.y, -1f, 1f);

        look.y += PlayerInputManager.GetAxis(0, ePadAxis.R_STICK_VERTICAL);
        look.x += PlayerInputManager.GetAxis(0, ePadAxis.R_STICK_HORIZONTAL);
        return look;
    }

    private void UpdateMouseSpeedInput()
    {
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            mouseTurnSpeedX += 10;
        }
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            mouseTurnSpeedX -= 10;
        }
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            mouseTurnSpeedY += 10;
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            mouseTurnSpeedY -= 10;
        }

        mouseSpeedVisual.text  = $"M:{mouseTurnSpeedX}|{mouseTurnSpeedY}";
        if(Input.GetKeyDown(KeyCode.P))
        {
            mouseSpeedVisual.gameObject.EnsureActive(mouseSpeedVisual.gameObject.activeSelf == false);
        }
    }

    public void PreExplode()
    {

    }
}

