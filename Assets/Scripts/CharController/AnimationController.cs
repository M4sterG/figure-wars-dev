﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.CharController
{
    public class AnimationController
    {
        public Animator headAnim;
        public Animator topAnim;
        public Animator legsAnim;
        public Animator shoesAnim;
        public Animator handsAnim;

        private List<Animator> AllAnims
        {
            get { return new List<Animator>{headAnim, topAnim, legsAnim, shoesAnim, handsAnim}; }
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

        public void goIdle()
        {
            AllAnims.ForEach(anim => anim.Play("Idle"));
        }

        public void shootMelee()
        {
            // topAnim.Play("attack_sword");
            // handsAnim.Play("attack_sword");
            // headAnim.Play("attack_sword");
        }

        public void fullRunForward()
        {
            foreach (var anim in AllAnims)
            {
                anim.Play("full_run_forward");
            }
        }
    }
}