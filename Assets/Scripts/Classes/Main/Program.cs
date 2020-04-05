using Scripts.Classes.Parts;
using Scripts.Weapons;
using System;
using System.Collections.Generic;

namespace Scripts.Classes.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<Weapon> weapons = WeaponGetter.getWeapons();
            List<Part> parts = PartGetter.getParts();
            List<Weapon> wepsWithNoIcons = weapons.FindAll(w => w.IconFile == null);
            List<Part> partsWithNoIcons = parts.FindAll(w => w.IconFile == null);
            partsWithNoIcons.ForEach(p => Console.WriteLine(p.Name));
        }
    }
}