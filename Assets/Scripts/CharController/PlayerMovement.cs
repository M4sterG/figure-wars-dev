using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;

public class PlayerMovement : MonoBehaviour
{
    private AudioSource[] audios;
    // Start is called before the first frame update
    public CharacterController controller;

    public float moveSpeed = 6f;

    public float gravity;

    public const float groundDist = 0.4f;
    private const float jumpHeight = 3f;

    public LayerMask groundMask;

    private Vector3 velocity;
    public Transform groundCheck;

    private bool isGrounded = false;
    private const int MAX_JUMPS = 2;

    private JumpDirection jumpDir = JumpDirection.None;
    private Equipped eq = Equipped.Melee;
    
    private enum Equipped
    {
        Melee, Shotgun
    }

    private enum JumpDirection
    {
        Left, Right, Front, Back, FrontLeft, FrontRight, BackLeft, BackRight, None
    }

    // 1 is KW, 2 is Bombard, 3 is Driver
    private Dictionary<int, float> swapSpeeds = new Dictionary<int, float>
    {
        {0, 0.25f}, //0.22f or 0.25f - KW-79
        {1, 0.37f}, //0.35f or 0.37f - Bombard
        {2, 0.48f} //0.45f or 0.48f - Driver
    };

    private float firingRate = 0.8f;

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
            {JumpDirection.None, new Tuple<float, float>(0f,0f)}
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
    }

    private int jumpCount = 0;
    // Update is called once per frame
    void Update()
    {
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
        checkSwap();
        checkShoot();
        swapCounter += Time.deltaTime;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
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
        controller.Move(move * moveSpeed * Time.deltaTime);
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void checkShoot()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            switch (eq)
            {
                case Equipped.Melee:
                    break;
                case Equipped.Shotgun:
                    // swapType to be set in unity debugger
                    // just so it's easier to switch between swap types
                    if (swapCounter >= swapSpeeds[swapType] && swapped)
                    {
                        audios[1].Play(0);
                        swapped = false;
                        swapCounter = 0f;
                    }
                    else if (swapCounter >= firingRate)
                    {
                        audios[1].Play(0);
                        swapCounter = 0f;
                    }
                    
                    break;
                default:
                    break;
            }
        }
    }

    private void checkSwap()
    {
        if (Input.GetKey(KeyCode.Alpha1) && eq != Equipped.Melee)
        {
            audios[0].Play(0);
            eq = Equipped.Melee;
            swapCounter = 0f;
            swapped = true;
            return;
        }

        if (Input.GetKey(KeyCode.E) && eq != Equipped.Shotgun)
        {
            eq = Equipped.Shotgun;
            swapCounter = 0f;
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

    private Vector3 getMoveByState()
    {
        float xDir;
        float zDir;
        Vector3 move;
        if (isGrounded)
        {
            move = transform.right * getX() + transform.forward * getZ();
            return move;
        }
        else
        {
            Tuple<float, float> dirs = getDirs();
            xDir = dirs.Item1;
            zDir = dirs.Item2;
        }
        return transform.right * xDir + transform.forward * zDir;
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
            if (jumpCount < MAX_JUMPS && eq == Equipped.Melee)
            {
                jumpCount = 2;
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
    }
}
