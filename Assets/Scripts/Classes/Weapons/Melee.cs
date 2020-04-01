using System;
using System.Collections.Generic;
using System.Text;
using Scripts.Classes.Main;

namespace Scripts.Weapons
{
    public class Melee : Weapon
    {
        public int Range { get; set; }
        public int RunSpeed { get; set; }
        public int SecondaryPower { get; set; }
        public Melee()
        {
            WeaponType = WeaponType.Melee;
        }
        public Melee(int id, WeaponType weaponType, string name, string desc) : base(id, weaponType, name, desc)
        {

        }
        public Melee(int power, int range, int runSpeed, int secondaryPower) : this()
        {
            Power = power;  // ability_a in cgd  
            Range = range;  // ability_b in cgd  
            RunSpeed = runSpeed;    // ability_c in cgd  
            SecondaryPower = secondaryPower;    // ability_d in cgd  
        }
        public new string ToUniquePropertyList()
        {
            return "base_secondary_power, base_run_speed, base_range";
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
            Range = value;
        }

        public override int getB()
        {
            return Range;
        }

        public override void setC(int value)
        {
            RunSpeed = value;
        }

        public override int getC()
        {
            return RunSpeed;
        }

        public override void setD(int value)
        {
            SecondaryPower = value;
        }

        public override int getD()
        {
            return SecondaryPower;
        }

        public new string ToSQLQuery()
        {
            return "INSERT IGNORE INTO melee_base_stats (id, " + ToUniquePropertyList() + ") VALUES (" +
                Id + "," +
                SecondaryPower + "," +
                RunSpeed + "," +
                Range + ");";
        }
    }
}
