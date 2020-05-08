using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using DefaultNamespace.CharController;
using Scripts.Classes.Main;
using Scripts.Weapons;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;
using UnityEngine.Rendering.UI;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private AnimationController animController;
    public Transform player;
    public Animator headAnim;
    public Animator topAnim;
    public Animator legsAnim;
    public Animator shoesAnim;
    public Animator handsAnim;

    private float lookX = 0f;
    private float lookY = 0f;

    private float MouseX
    {
        get => Input.GetAxis("Mouse X") * 60f * Time.deltaTime;
    }

    private float MouseY
    {
        get => Input.GetAxis("Mouse Y") * 60f * Time.deltaTime;
    }


    private Transform camera;
    private Vector3 transRight;
    private Vector3 transForward;

    public GameObject rocketPrefab;
    public Transform rocketSpawnPoint;
    private float rocketSpeed = 50f;
    public GameObject rocketLauncher;

    private AudioSource[] audios;

    // Start is called before the first frame update
    public CharacterController controller;

    public float moveSpeed = 6f;

    public float gravity;

    public const float groundDist = 0.4f;
    private const float jumpHeight = 3f;

    private Vector3 jumpX, jumpZ;


    public LayerMask groundMask;

    private Vector3 velocity;
    public Transform groundCheck;

    private bool isGrounded = false;
    private const int MAX_JUMPS = 2;

    private Direction jumpDirection = Direction.None;
    private Direction dir = Direction.None;
    private WeaponType eq = WeaponType.Melee;

    internal enum Direction
    {
        Left,
        Right,
        Front,
        Back,
        FrontLeft,
        FrontRight,
        BackLeft,
        BackRight,
        None
    }

    // 1 is KW, 2 is Bombard, 3 is Driver
    private Dictionary<int, float> swapSpeeds = new Dictionary<int, float>
    {
        {0, 0.25f}, //0.22f or 0.25f - KW-79
        {1, 0.37f}, //0.35f or 0.37f - Bombard
        {2, 0.48f} //0.45f or 0.48f - Driver
    };

    private float sgFireRate = 0.8f;
    private float rifleFireRate = 9.5f; // 1000 rifle fire rate (10 bullets per second)
    private float rifleFire = 0f;

    public int swapType;
    private float swapCounter = 0f;
    private bool swapped = false;
    
    private static Dictionary<Direction, int> animDir = new Dictionary<Direction, int>
    {
        {Direction.Left, 0},
        {Direction.Front, 1},
        {Direction.Right, 2},
        {Direction.Back, 3},
        {Direction.BackLeft, 4},
        {Direction.BackRight, 5}
    };
    


    private static Dictionary<Direction, Tuple<float, float>> airborneDirMultiplier
        = new Dictionary<Direction, Tuple<float, float>>
        {
            // (Dir, <xDir, zDir>)
            {Direction.Front, new Tuple<float, float>(0f, 1f)},
            {Direction.Back, new Tuple<float, float>(0f, -1f)},
            {Direction.Left, new Tuple<float, float>(-1f, 0f)},
            {Direction.Right, new Tuple<float, float>(1f, 0f)},
            {Direction.None, new Tuple<float, float>(0f, 0f)},
            {Direction.BackLeft, new Tuple<float, float>(0f, -1f)},
            {Direction.BackRight, new Tuple<float, float>(0f, -1f)},
            {Direction.FrontRight, new Tuple<float, float>(0f, 1f)},
            {Direction.FrontLeft, new Tuple<float, float>(0f, 1f)}
        };

    private static Dictionary<Direction, KeyCode> basicDirKeys = new Dictionary<Direction, KeyCode>()
    {
        {Direction.Front, KeyCode.W},
        {Direction.Back, KeyCode.S},
        {Direction.Left, KeyCode.A},
        {Direction.Right, KeyCode.D},
        {Direction.BackLeft, KeyCode.A},
        {Direction.BackRight, KeyCode.D},
        {Direction.FrontLeft, KeyCode.A},
        {Direction.FrontRight, KeyCode.D}
    };

    void Awake()
    {
        camera = Camera.main.transform;
        audios = GetComponents<AudioSource>();
        animController = new AnimationController(headAnim, topAnim, legsAnim, shoesAnim, handsAnim);
    }

    private int jumpCount = 0;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
        transRight = transform.right;
        transForward = transform.forward;
        animController.setGrounded(isGrounded);
        checkMove();
        camera = Camera.main.transform;
        if (isGrounded)
        {
            if (velocity.y < 0)
            {
                jumpCount = 0;
                velocity.y = -4f;
            }
        }

        checkSwap();
        checkShoot();
        swapCounter += Time.deltaTime;
        swapCounter += Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }


        Vector3 move = getMoveByState();
        controller.Move(moveSpeed * Time.deltaTime * clampedMove(move));

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private Vector3 clampedMove(Vector3 move)
    {
        float max = isMovingBackwards() ? 0.5f : 1f;
        return Vector3.ClampMagnitude(move, max);
    }
    

    private void setMoveDir(Direction dir)
    {
        int direction = -1;
        bool moving = animDir.TryGetValue(dir, out direction);
        animController.setDir(direction);
    }

    private void resetLook()
    {
        lookX = 0f;
        lookY = 0f;
    }

    private bool isMovingBackwards()
    {
        Direction comparison = dir;
        if (!isGrounded)
        {
            comparison = jumpDirection;
        }
        return comparison == Direction.Back || comparison == Direction.BackLeft || comparison == Direction.BackRight;
        
    }

    private void checkMove()
    {
        bool moving = false;
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            moving = true;
            dir = Direction.BackLeft;
            resetLook();
            setMoveDir(dir);
            return;
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            moving = true;
            dir = Direction.BackRight;
            resetLook();
            setMoveDir(dir);
            return;
        }

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S))
        {
            moving = true;
            dir = Direction.Left;
            resetLook();
            setMoveDir(dir);
            return;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moving = true;
            dir = Direction.Right;
            resetLook();
            setMoveDir(dir);
            return;
        }

        if (Input.GetKey(KeyCode.S))
        {
            dir = Direction.Back;
            moving = true;
            resetLook();
            setMoveDir(dir);
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir = Direction.Front;
            moving = true;
            resetLook();
            setMoveDir(dir);
            return;
        }

        dir = Direction.None;
        dir = Direction.None;
        lookX += MouseX;
        lookY += MouseY;
        animController.goIdle(lookX / 90F, lookY / 90f);
        animController.setDir(-1);
    
}

    private void checkShoot()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            switch (eq)
            {
                case WeaponType.Melee:
                    break;
                case WeaponType.Shotgun:
                    // swapType to be set in unity debugger
                    // just so it's easier to switch between swap types
                    if (swapCounter >= swapSpeeds[swapType] && swapped)
                    {
                        audios[1].Play(0);
                        swapped = false;
                        swapCounter = 0f;
                    }
                    else if (swapCounter >= sgFireRate)
                    {
                        audios[1].Play(0);
                        swapCounter = 0f;
                    }

                    break;
                case WeaponType.Rifle:
                    if (swapped)
                    {
                        // rifle swaps faster than KW 
                        if (swapCounter >= swapSpeeds[0] * 0.8f)
                        {
                            rifleFire = Time.time + 1f / rifleFireRate;
                            swapped = false;
                            audios[2].Play(0);
                            swapCounter = 0f;
                        }
                    }
                    else if (Time.time >= rifleFire)
                    {
                        rifleFire = Time.time + 1f / rifleFireRate;
                        audios[2].Play(0);
                    }

                    break;
                case WeaponType.Bazooka:
                    shootBazooka();
                    break;
                default:
                    break;
            }
        }
    }


    private void shootBazooka()
    {
        if (swapCounter >= swapSpeeds[swapType] && swapped)
        {
            audios[3].Play(0);
            swapped = false;
            swapCounter = 0f;
            GameObject rocket = Instantiate(rocketPrefab, rocketSpawnPoint
                .TransformPoint(0, 0, -0.1f), rocketSpawnPoint.rotation);
            // rocket gets shot in the direction you're looking at
            rocket.GetComponent<Rigidbody>().AddForce(camera.forward * rocketSpeed, ForceMode.Impulse);
            // rocket launcher looks in the direction the camera is looking at
            Destroy(rocket, 10);
        }
    }

    private void checkSwap()
    {
        if (Input.GetKey(KeyCode.Alpha1) && eq != WeaponType.Melee)
        {
            audios[0].Play(0);
            eq = WeaponType.Melee;
            animController.setWeapon(eq);
            swapCounter = 0f;
            swapped = true;
            rocketLauncher.SetActive(false);
            return;
        }

        if (Input.GetKey(KeyCode.E) && eq != WeaponType.Shotgun)
        {
            eq = WeaponType.Shotgun;
            animController.setWeapon(eq);
            swapCounter = 0f;
            swapped = true;
            rocketLauncher.SetActive(false);
            return;
        }

        if (Input.GetKey(KeyCode.Alpha2) && eq != WeaponType.Rifle)
        {
            eq = WeaponType.Rifle;
            animController.setWeapon(eq);
            swapCounter = 0f;
            swapped = true;
            rocketLauncher.SetActive(false);
            return;
        }

        if (Input.GetKey(KeyCode.R) && eq != WeaponType.Bazooka)
        {
            eq = WeaponType.Bazooka;
            animController.setWeapon(eq);
            swapCounter = 0f;
            swapped = true;
            rocketLauncher.SetActive(true);
            return;
        }
    }

    public float getX()
    {
        return Input.GetAxis("Horizontal");
    }

    public float getZ()
    {
        return Input.GetAxis("Vertical");
    }

    private void setJumpXZ(Vector3 x, Vector3 z)
    {
        jumpX = x;
        jumpZ = z;
    }

    private Vector3 getMoveByState()
    {
        float xDir;
        float zDir;
        Vector3 move;
        if (isGrounded)
        {
            
            // moves towards the direction it's supposed to go in
            move =  Mathf.Abs(getX()) * DirectionMovement.getXMultiplier(dir) * transRight
                    + DirectionMovement.getZMultiplier(dir)* Mathf.Abs(getZ()) * transForward;
            if (isMovingBackwards())
            {
                move *= 0.5f;
            }
            return move;
        }
        else
        {
            if (jumpDirection == Direction.None)
            {
                Debug.Log("???");
                return Vector3.zero;
            }
            Tuple<float, float> dirs = getJumpDirs();
            xDir = dirs.Item1;
            zDir = dirs.Item2;
            return jumpX * xDir + jumpZ * zDir;
        }
    }

    private Tuple<float, float> getJumpDirs()
    {
        if (jumpDirection == Direction.None)
        {
            return new Tuple<float, float>(0f, 0f);
        }

        Tuple<float, float> dirs = airborneDirMultiplier[jumpDirection];
        
        if (Input.GetKey(basicDirKeys[jumpDirection]))
        {
            float xDir = dirs.Item1 * 1.25f;
            float zDir = dirs.Item2 * 1.25f;
            return new Tuple<float, float>(xDir, zDir);
        }
        return dirs;
    }

    private Direction getJumpDir()
    {
        if (Input.GetKey(KeyCode.A))
        {
            return Direction.Left;
        }

        if (Input.GetKey(KeyCode.S))
        {
            return Direction.Back;
        }

        if (Input.GetKey(KeyCode.D))
        {
            return Direction.Right;
        }

        if (Input.GetKey(KeyCode.W))
        {
            return Direction.Front;
        }

        return Direction.None;
    }

    private void Jump()
    {
        if (isGrounded)
        {
            setJumpXZ(transRight, transForward);
            jumpDirection = dir;
            
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount = 1;
        }
        else
        {
            if (jumpCount < MAX_JUMPS && eq == WeaponType.Melee)
            {
                jumpCount = 2;
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
    }
}