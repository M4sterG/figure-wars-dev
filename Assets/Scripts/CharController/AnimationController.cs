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
            AllAnims.ForEach(anim => anim.SetBool("isGrounded", value));
            isGrounded = value;
        }

        public bool jumping = false;

        public AnimationController(Animator headAnim, Animator topAnim
            , Animator legsAnim, Animator shoesAnim, Animator handsAnim)
        {
            this.headAnim = headAnim;
            this.topAnim = topAnim;
            this.legsAnim = legsAnim;
            this.handsAnim = handsAnim;
            this.shoesAnim = shoesAnim;
        }

        public void goIdle(float mouseX, float mouseY)
        {
            AllAnims.ForEach(anim =>
            {
                anim.SetFloat("Horizontal", mouseX);
                anim.SetFloat("Vertical", mouseY);
            });
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
                anim.SetInteger("EquippedWeapon", GameManager.weaponPositions[weapon]));
        }

        public void setDir(int dir)
        {
            AllAnims.ForEach(anim => anim.SetInteger("MoveDir", dir));
        }

        public void fullRunForward()
        {
            if (isGrounded)
            {
                foreach (var anim in AllAnims)
                {
                    anim.Play("full_run_forward");
                }
            }
        }
    }
}