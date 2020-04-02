using System;
using System.Collections.Generic;
using System.Text;
using Scripts.Classes.Main;

namespace Scripts.Weapons
{
    public enum SwapSpeed
    {
        Slow,
        Medium,
        Fast
    }
    public abstract class Weapon : Item
    {
        public int Power { get; set; } 
        public int FiringRate { get; set; } 
        public int ReloadSpeed { get; set; }
        public int AmmoClip { get; set; } //wi_bullet_capacity
        public int TotalAmmo { get; set; } //wi_bullet_total
        public int ChangeTime { get; set; }
//        public int ChangeSkip { get; set; } always 500
//        public int ChangeDelay { get; set; } always 0
        public WeaponType WeaponType { get; set; }
        public Weapon(int id, WeaponType weaponType, string name, string desc)
        {
            Id = id;
            Name = name;
            Description = desc;
            WeaponType = weaponType;
        }
        public Weapon()
        {        
        }
        public Weapon(int abilityA, int abilityB, int abilityC, int abilityD)
        {
        }

        public Weapon shallowCopy()
        {
            return (Weapon) this.MemberwiseClone();
        }


        public abstract void setA(int value);

        public abstract int getA();
        public abstract void setB(int value);
        public abstract int getB();
        public abstract void setC(int value);
        public abstract int getC();
        public abstract void setD(int value);
        public abstract int getD();
        

       

        public new string ToSQLQuery()
        {
            return "INSERT IGNORE INTO weapons (id, " + ToUniquePropertyList() + ") VALUES (" +
                Id + "," +
                "'" + WeaponType.ToString() + "'," +
                Power + "," +
                ReloadSpeed + "," +
                FiringRate + "," +
                AmmoClip + "," +
                TotalAmmo + ");";
        }
        public new string ToUniquePropertyList()
        {
            return "type, base_power, reload_speed, firing_rate, ammo_clip, ammo_amount";
        }
    }
}
