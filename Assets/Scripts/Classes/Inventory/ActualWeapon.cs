using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Classes.Main;
using Scripts.Weapons;
//using Console = Colorful.Console;

namespace Scripts.Classes.Inventory
{
    public class ActualWeapon : Item
    {
        
        public static int MAX_LEVEL = 5;
        private static Dictionary<int, int> upgradeLevelChances = createBaseRates();
        private static Dictionary<int, int> energyLevels = createEnergyLevels();
        private static HashSet<Tuple<WeaponType, Dictionary<UpgradeStat, int>>> upgradeBonuses
                = createUpgadeBonuses();

        
        private Random rng = new Random();


        private readonly Weapon baseWeapon;
        private Weapon actualWeapon;
        private int Level { get; set; }
        private int currUpgradeRate;
        private int CurrentUpgradeRate
        {
            get { return currUpgradeRate;}
            set { currUpgradeRate = value; }
        }
        private UpgradeStat upgradeStat;
        private int Energy { get; set; }

        public ActualWeapon(Weapon baseWeapon)
        {
            this.baseWeapon = baseWeapon;
            this.actualWeapon = baseWeapon.shallowCopy();
            CurrentUpgradeRate = upgradeLevelChances[1];
            Level = 0;
            Energy = 0;
        }

        public string getName()
        {
            return baseWeapon.Name;
        }

        public string IconFile
        {
            get => baseWeapon.IconFile;
        }

        public int IconOffset
        {
            get => baseWeapon.IconOffset;
        }

        public WeaponType getType()
        {
            return baseWeapon.WeaponType;
        }

        public Weapon getBaseWeapon()
        {
            return baseWeapon;
        }



        public void upgrade(int addedEnergy)
        {
            if (Level == MAX_LEVEL)
            {
                return;
            }
            int requiredEnergy;
            energyLevels.TryGetValue(Level + 1, out requiredEnergy);
            int remainingEnergy = addedEnergy + Energy - requiredEnergy;
            if (Level == 0 && remainingEnergy >= 0)
            {
               // make user choose upgrade
               upgradeStat = UpgradeStat.A;
               bool succeeded = tryUpgrade(CurrentUpgradeRate);
               if (!succeeded)
               {
                   upgradeStat = UpgradeStat.None;
               }
               return;
            }
            if (remainingEnergy >= 0)
            {
                tryUpgrade(CurrentUpgradeRate);
            }
            else
            {
                Energy += addedEnergy;
            }
        }

        private void failUpgrade()
        {
            CurrentUpgradeRate += 15;
            Energy = 0;
        }

        private bool tryUpgrade(int chance)
        {
            int guess = 1 + rng.Next(100);
            if (guess <= CurrentUpgradeRate)
            {
                Level++;
                Energy = 0;
                setActualWeaponStat(Level);
                if (Level != 5)
                {
                    upgradeLevelChances.TryGetValue(Level, out currUpgradeRate);
                }
                return true;
            }
            failUpgrade();
            return false;
        }

        private void setActualWeaponStat(int level)
        {
            if (upgradeStat == null)
            {
                throw new NotSupportedException("Upgrade Stat is null");
            }

            Dictionary<UpgradeStat, int> bonuses = getBonusesByWeaponType(baseWeapon.WeaponType);
            actualWeapon = baseWeapon.shallowCopy();
            switch (upgradeStat)
            {
                case UpgradeStat.A:
                    int offsetA;
                    bonuses.TryGetValue(UpgradeStat.A, out offsetA);
                    actualWeapon.setA(actualWeapon.getA() + level * offsetA);
                    break;
                case UpgradeStat.B:
                    int offsetB;
                    bonuses.TryGetValue(UpgradeStat.B, out offsetB);
                    actualWeapon.setB(actualWeapon.getB() + level * offsetB);
                    break;
                case UpgradeStat.C:
                    int offsetC;
                    bonuses.TryGetValue(UpgradeStat.C, out offsetC);
                    actualWeapon.setB(actualWeapon.getB() + level * offsetC);
                    break;
                case UpgradeStat.D:
                    int offsetD;
                    bonuses.TryGetValue(UpgradeStat.D, out offsetD);
                    actualWeapon.setB(actualWeapon.getB() + level * offsetD);
                    break;
            }
        }

        private static Dictionary<UpgradeStat, int> getBonusesByWeaponType(WeaponType type)
        {
            switch (type)
            {
                case WeaponType.Melee:
                    Tuple<WeaponType, Dictionary<UpgradeStat, int>> tuple1;
                    tuple1 = upgradeBonuses.ElementAt(0);
                    return tuple1.Item2;
                case WeaponType.Rifle:
                    Tuple<WeaponType, Dictionary<UpgradeStat, int>> tuple2;
                    tuple2 = upgradeBonuses.ElementAt(1);
                    return tuple2.Item2;
                case WeaponType.Shotgun:
                    Tuple<WeaponType, Dictionary<UpgradeStat, int>> tuple3;
                    tuple3 = upgradeBonuses.ElementAt(2);
                    return tuple3.Item2;
                case WeaponType.Sniper:
                    Tuple<WeaponType, Dictionary<UpgradeStat, int>> tuple4;
                    tuple4 = upgradeBonuses.ElementAt(3);
                    return tuple4.Item2;
                case WeaponType.Minigun:
                    Tuple<WeaponType, Dictionary<UpgradeStat, int>> tuple5;
                    tuple5 = upgradeBonuses.ElementAt(4);
                    return tuple5.Item2;
                case WeaponType.Bazooka:
                    Tuple<WeaponType, Dictionary<UpgradeStat, int>> tuple6;
                    tuple6 = upgradeBonuses.ElementAt(5);
                    return tuple6.Item2;
                case WeaponType.Grenade:
                    Tuple<WeaponType, Dictionary<UpgradeStat, int>> tuple7;
                    tuple7 = upgradeBonuses.ElementAt(6);
                    return tuple7.Item2;
            }

            return null;
        }
            
        
        
        private static HashSet<Tuple<WeaponType, Dictionary<UpgradeStat, int>>> createUpgadeBonuses()
        {
            var meleeUpgrades = getMeleeUpgrades();
            var rifleUpgrades = getRifleUpgrades();
            var shotgunUpgrades = getShotgunUpgrades();
            var sniperUpgrades = getSniperUpgrades();
            var minigunUpgrades = getMinigunUpgrades();
            var bazookaUpgrades = getBazookaUpgrades();
            var grenadeUpgrades = getGrenadeUpgrades();
            HashSet<Tuple<WeaponType, Dictionary<UpgradeStat, int>>> upgrades 
                = new HashSet<Tuple<WeaponType, Dictionary<UpgradeStat, int>>>();
            upgrades.Add(meleeUpgrades);
            upgrades.Add(rifleUpgrades);
            upgrades.Add(shotgunUpgrades);
            upgrades.Add(sniperUpgrades);
            upgrades.Add(minigunUpgrades);
            upgrades.Add(bazookaUpgrades);
            upgrades.Add(grenadeUpgrades);
            return upgrades;
        }
        
        private static Tuple<WeaponType, Dictionary<UpgradeStat, int>> getGrenadeUpgrades()
        {
            Dictionary<UpgradeStat, int> dic = new Dictionary<UpgradeStat, int>();
            dic.Add(UpgradeStat.A, 15);
            dic.Add(UpgradeStat.B, 15);
            dic.Add(UpgradeStat.C, 15);
            dic.Add(UpgradeStat.D, 15);
            return new Tuple<WeaponType, Dictionary<UpgradeStat, int>>(WeaponType.Grenade, dic);
        } 

        
        private static Tuple<WeaponType, Dictionary<UpgradeStat, int>> getBazookaUpgrades()
        {
            Dictionary<UpgradeStat, int> dic = new Dictionary<UpgradeStat, int>();
            dic.Add(UpgradeStat.A, 15);
            dic.Add(UpgradeStat.B, 15);
            dic.Add(UpgradeStat.C, 15);
            dic.Add(UpgradeStat.D, 15);
            return new Tuple<WeaponType, Dictionary<UpgradeStat, int>>(WeaponType.Bazooka, dic);
        } 

        
        private static Tuple<WeaponType, Dictionary<UpgradeStat, int>> getMinigunUpgrades()
        {
            Dictionary<UpgradeStat, int> dic = new Dictionary<UpgradeStat, int>();
            dic.Add(UpgradeStat.A, 15);
            dic.Add(UpgradeStat.B, 15);
            dic.Add(UpgradeStat.C, 15);
            dic.Add(UpgradeStat.D, 15);
            return new Tuple<WeaponType, Dictionary<UpgradeStat, int>>(WeaponType.Minigun, dic);
        } 

        
        
        
        private static Tuple<WeaponType, Dictionary<UpgradeStat, int>> getSniperUpgrades()
        {
            Dictionary<UpgradeStat, int> dic = new Dictionary<UpgradeStat, int>();
            dic.Add(UpgradeStat.A, 15);
            dic.Add(UpgradeStat.B, 15);
            dic.Add(UpgradeStat.C, 15);
            dic.Add(UpgradeStat.D, 25);
            return new Tuple<WeaponType, Dictionary<UpgradeStat, int>>(WeaponType.Sniper, dic);
        } 
        
        private static Tuple<WeaponType, Dictionary<UpgradeStat, int>> getShotgunUpgrades()
        {
            Dictionary<UpgradeStat, int> dic = new Dictionary<UpgradeStat, int>();
            dic.Add(UpgradeStat.A, 15);
            dic.Add(UpgradeStat.B, 15);
            dic.Add(UpgradeStat.C, 15);
            dic.Add(UpgradeStat.D, 15);
            return new Tuple<WeaponType, Dictionary<UpgradeStat, int>>(WeaponType.Shotgun, dic);
        } 
        
        private static Tuple<WeaponType, Dictionary<UpgradeStat, int>> getRifleUpgrades()
        {
            Dictionary<UpgradeStat, int> dic = new Dictionary<UpgradeStat, int>();
            dic.Add(UpgradeStat.A, 15);
            dic.Add(UpgradeStat.B, 3);
            dic.Add(UpgradeStat.C, 4);
            dic.Add(UpgradeStat.D, 15);
            return new Tuple<WeaponType, Dictionary<UpgradeStat, int>>(WeaponType.Rifle, dic);
        } 

        private static Tuple<WeaponType, Dictionary<UpgradeStat, int>> getMeleeUpgrades()
        {
            Dictionary<UpgradeStat, int> dic = new Dictionary<UpgradeStat, int>();
            dic.Add(UpgradeStat.A, 15);
            dic.Add(UpgradeStat.B, 15);
            dic.Add(UpgradeStat.C, 15);
            dic.Add(UpgradeStat.D, 15);
            return new Tuple<WeaponType, Dictionary<UpgradeStat, int>>(WeaponType.Melee, dic);
        } 
        
        private static Dictionary<int, int> createEnergyLevels()
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            dic.Add(1, 1000);
            dic.Add(2, 2000);
            dic.Add(3, 3000);
            dic.Add(4, 4000);
            dic.Add(5, 500);
            return dic;
        }
        
        
        private static Dictionary<int, int> createBaseRates()
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            dic.Add(1, 100);
            dic.Add(2, 60);
            dic.Add(3, 40);
            dic.Add(4, 22);
            dic.Add(5, 10);
            return dic;
        }

        public enum UpgradeStat
        {
            None,
            A,B,C,D // which of the stats the weapon is upgraded on
        }
    }
}