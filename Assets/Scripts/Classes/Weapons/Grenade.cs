using System;
using System.Collections.Generic;
using System.Text;
using Scripts.Classes.Main;

namespace Scripts.Weapons
{
    
    public class Grenade : Explosive
    {
        private const int MAX_BOUNCE_COUNT = 2;
        public SwapSpeed SwapSpeed { get; set; }
        public Grenade() 
        {
            WeaponType = WeaponType.Grenade;
        }
        public Grenade(int power, int firingRate, int blastRadius, int reloadSpeed) : this()
        {
            Power = power;  // ability_a in cgd  
            FiringRate = firingRate;    // ability_b in cgd  
            BlastRadius = blastRadius;  // ability_c in cgd  
            ReloadSpeed = reloadSpeed;  // ability_d in cgd  
        }

        public new string ToUniquePropertyList()
        {
            return "blast_radius, swap_speed";
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
            ReloadSpeed = value;
        }

        public override int getD()
        {
            return ReloadSpeed;
        }

        public new string ToSQLQuery()
        {
            return "INSERT IGNORE INTO grenade_base_stats (id, " + ToUniquePropertyList() + ") VALUES (" +
                Id + "," +
                BlastRadius + "," +
                "'" + SwapSpeed.ToString() + "');";
        }
    }
}
