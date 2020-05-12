using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Scripts.Classes.Main;
using Scripts.Weapons;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DefaultNamespace.CharController
{
    public class AnimationController
    {
        public Animator headAnim;
        public Animator topAnim;
        public Animator legsAnim;
        public Animator shoesAnim;
        public Animator handsAnim;
        private bool isGrounded;

        private List<Animator> AllAnims
        {
            get { return new List<Animator>{headAnim, topAnim, legsAnim, shoesAnim, handsAnim}; }
        }

        public void setGrounded(bool value)
        {
            AllAnims.ForEach(anim => anim.SetBool(IsGrounded, value));
            isGrounded = value;
        }

        public void setSecondJumping(bool second)
        {
            AllAnims.ForEach(anim => anim.SetBool(SecondJump, second));
        }
        
        private static readonly int IsGrounded = Animator.StringToHash($"isGrounded");
        private static readonly int SecondJump = Animator.StringToHash("SecondJump");
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int EquippedWeapon = Animator.StringToHash("EquippedWeapon");
        private static readonly int MoveDir = Animator.StringToHash("MoveDir");
        private static readonly int SwapCounter = Animator.StringToHash("SwapCounter");

        public AnimationController(Animator headAnim, Animator topAnim
            , Animator legsAnim, Animator shoesAnim, Animator handsAnim)
        {
            this.headAnim = headAnim;
            this.topAnim = topAnim;
            this.legsAnim = legsAnim;
            this.handsAnim = handsAnim;
            this.shoesAnim = shoesAnim;
        }

        public void setLookAngles(float mouseX, float mouseY)
        {
            AllAnims.ForEach(anim =>
            {
                anim.SetFloat(Horizontal, mouseX);
                anim.SetFloat(Vertical, mouseY);
            });
        }

        public void setSwapCounter(float time)
        {
            AllAnims.ForEach(anim => anim.SetFloat(SwapCounter, time));
        }

        public void shootMelee()
        {
            // topAnim.Play("attack_sword");
            // handsAnim.Play("attack_sword");
            // headAnim.Play("attack_sword");
        }

        public void setWeapon(WeaponType weapon)
        {
            AllAnims.ForEach(anim => 
                anim.SetInteger(EquippedWeapon, GameManager.weaponPositions[weapon]));
        }

        public void setDir(int dir)
        {
            AllAnims.ForEach(anim => anim.SetInteger(MoveDir, dir));
        }
        
    }
}