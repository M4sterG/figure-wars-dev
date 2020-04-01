using System;
using System.Collections.Generic;
using System.Text;
using Scripts.Classes.Main;

namespace Scripts.Weapons
{
    public class Shotgun : Gun
    {   
        public SwapSpeed SwapSpeed { get; set; }
        public Shotgun()
        {
            WeaponType = WeaponType.Shotgun;
        }
        public Shotgun(int power, int firingRate, int accuracy, int reloadSpeed) : this()
        {
            Power = power;  // ability_a in cgd
            FiringRate = firingRate; // ability_b in cgd
            Accuracy = accuracy;    // ability_c in cgd
            ReloadSpeed = reloadSpeed; // ability_d in cgd
        }
        public new string ToUniquePropertyList()
        {
            return "swap_speed";
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
            Accuracy = value;
        }

        public override int getC()
        {
            return Accuracy;
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
            return "INSERT IGNORE INTO shotgun_base_stats (id, " + ToUniquePropertyList() + ") VALUES (" +
                Id + "," +
                "'" + SwapSpeed.ToString() + "');";
        }
    }
}
