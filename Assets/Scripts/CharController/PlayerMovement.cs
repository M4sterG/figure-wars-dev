using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using DefaultNamespace.CharController;
using Scripts.Classes.Main;
using Scripts.Weapons;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;

public class PlayerMovement : MonoBehaviour
{
    private AnimationController animController;
    public Transform player;
    public Animator headAnim;
    public Animator topAnim;
    public Animator legsAnim;
    public Animator shoesAnim;
    public Animator handsAnim;

    private float MouseX
    {
        get => Input.GetAxis("Mouse X") * 60f * Time.deltaTime;
    }


    private Transform camera;

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

    private JumpDirection jumpDir = JumpDirection.None;
    private WeaponType eq = WeaponType.Melee;


    private enum JumpDirection
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


    private static Dictionary<JumpDirection, Tuple<float, float>> airborneDirMultiplier
        = new Dictionary<JumpDirection, Tuple<float, float>>
        {
            // (Dir, <xDir, zDir>)
            {JumpDirection.Front, new Tuple<float, float>(0f, 1f)},
            {JumpDirection.Back, new Tuple<float, float>(0f, -1f)},
            {JumpDirection.Left, new Tuple<float, float>(-1f, 0f)},
            {JumpDirection.Right, new Tuple<float, float>(1f, 0f)},
            {JumpDirection.None, new Tuple<float, float>(0f, 0f)}
        };

    private static Dictionary<JumpDirection, KeyCode> basicDirKeys = new Dictionary<JumpDirection, KeyCode>()
    {
        {JumpDirection.Front, KeyCode.W},
        {JumpDirection.Back, KeyCode.S},
        {JumpDirection.Left, KeyCode.A},
        {JumpDirection.Right, KeyCode.D},
    };

    void Awake()
    {
        audios = GetComponents<AudioSource>();
        animController = new AnimationController(headAnim, topAnim, legsAnim, shoesAnim, handsAnim);
    }

    private int jumpCount = 0;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
        animController.setGrounded(isGrounded);
        checkMove();
        camera = Camera.main.transform;
        if (Input.GetKey(KeyCode.B))
        {
            swapType = 1;
        }

        if (Input.GetKey(KeyCode.K))
        {
            swapType = 0;
        }

        if (Input.GetKey(KeyCode.N))
        {
            swapType = 2;
        }

        checkMove();
        checkSwap();
        checkShoot();
        swapCounter += Time.deltaTime;

        if (isGrounded && velocity.y < 0)
        {
            jumpCount = 0;
            velocity.y = -4f;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }


        Vector3 move = getMoveByState();
        controller.Move(moveSpeed * Time.deltaTime * Vector3.ClampMagnitude(move, 1.0f));

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void checkMove()
    {
        bool moving = false;
        if (Input.GetKey(KeyCode.A))
        {
            moving = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moving = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moving = true;
        }

        if (Input.GetKey(KeyCode.W))
        {
            moving = true;
            animController.fullRunForward();
        }

        if (!moving)
        {
            animController.goIdle();
        }
    }

    private void checkShoot()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            switch (eq)
            {
                case WeaponType.Melee:
                    animController.shootMelee();
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
            swapCounter = 0f;
            swapped = true;
            rocketLauncher.SetActive(false);
            return;
        }

        if (Input.GetKey(KeyCode.E) && eq != WeaponType.Shotgun)
        {
            eq = WeaponType.Shotgun;
            swapCounter = 0f;
            swapped = true;
            rocketLauncher.SetActive(false);
            return;
        }

        if (Input.GetKey(KeyCode.Alpha2) && eq != WeaponType.Rifle)
        {
            eq = WeaponType.Rifle;
            swapCounter = 0f;
            swapped = true;
            rocketLauncher.SetActive(false);
            return;
        }

        if (Input.GetKey(KeyCode.R) && eq != WeaponType.Bazooka)
        {
            eq = WeaponType.Bazooka;
            swapCounter = 0f;
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
            Vector3 transRight = transform.right;
            Vector3 transForward = transform.forward;
            move = transRight * getX() + transForward * getZ();
            setJumpXZ(transRight, transForward);
            return move;
        }
        else
        {
            Tuple<float, float> dirs = getDirs();
            xDir = dirs.Item1;
            zDir = dirs.Item2;
            return jumpX * xDir + jumpZ * zDir;
        }
    }

    private Tuple<float, float> getDirs()
    {
        if (jumpDir == JumpDirection.None)
        {
            return new Tuple<float, float>(0f, 0f);
        }

        Tuple<float, float> dirs = airborneDirMultiplier[jumpDir];
        if (Input.GetKey(basicDirKeys[jumpDir]))
        {
            float xDir = dirs.Item1 * 1.25f;
            float zDir = dirs.Item2 * 1.25f;
            return new Tuple<float, float>(xDir, zDir);
        }

        return dirs;
    }

    private JumpDirection getJumpDir()
    {
        if (Input.GetKey(KeyCode.A))
        {
            return JumpDirection.Left;
        }

        if (Input.GetKey(KeyCode.S))
        {
            return JumpDirection.Back;
        }

        if (Input.GetKey(KeyCode.D))
        {
            return JumpDirection.Right;
        }

        if (Input.GetKey(KeyCode.W))
        {
            return JumpDirection.Front;
        }

        return JumpDirection.None;
    }

    private void Jump()
    {
        if (isGrounded)
        {
            jumpDir = getJumpDir();
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