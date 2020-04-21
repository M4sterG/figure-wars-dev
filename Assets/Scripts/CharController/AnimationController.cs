using System.Collections;
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

        public bool jumping = false;

        public AnimationController(Animator headAnim, Animator topAnim
            , Animator legsAnim, Animator shoesAnim, Animator handsAnim)
        {
            this.headAnim = headAnim;
            this.topAnim = topAnim;
            this.legsAnim = shoesAnim;
            this.handsAnim = handsAnim;
        }

        public void shootMelee()
        {
            topAnim.Play("attack_sword");
            handsAnim.Play("attack_sword");
            headAnim.Play("attack_sword");
        }
    }
}