using System;
using System.Collections.Generic;
using System.Text;
using Scripts.Classes.Main;

namespace Scripts.Weapons
{
    public class Bazooka : Explosive
    {
        public int BulletSpeed { get; set; }
        public SwapSpeed SwapSpeed { get; set; }
        public Bazooka() : base()
        {
            WeaponType = WeaponType.Bazooka;
        }
        public Bazooka(int power, int firingRate, int blastRadius, int bulletSpeed) : this()
        {
            Power = power;  // ability_a in cgd  
            FiringRate = firingRate;    // ability_b in cgd  
            BlastRadius = blastRadius;  // ability_c in cgd  
            BulletSpeed = bulletSpeed;  // ability_d in cgd  
        }
        public new string ToUniquePropertyList()
        {
            return "blast_radius, bullet_speed, swap_speed";
        }


        public override void setA(int value)
        {
            Power = value;
        }

        public override int getA()
        {
            return Power;
        }
        

        public override void setB(int value)
        {
            FiringRate = value;
        }

        public override int getB()
        {
            return FiringRate;
        }

        public override void setC(int value)
        {
            BlastRadius = value;
        }

        public override int getC()
        {
            return BlastRadius;
        }

        public override void setD(int value)
        {
            BulletSpeed = value;
        }

        public override int getD()
        {
            return BulletSpeed;
        }

        public new string ToSQLQuery()
        {
            return "INSERT IGNORE INTO bazooka_base_stats (id, " + ToUniquePropertyList() + ") VALUES (" +
                Id + "," +
                BlastRadius + "," +
                BulletSpeed + "," +
                "'" + SwapSpeed.ToString() + "');";
        }
    }
}
