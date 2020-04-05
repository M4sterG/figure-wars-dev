﻿using Scripts.Classes.Icons;
using Scripts.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;

namespace Scripts.Classes.Main
{
    internal static class WeaponGetter
    {
        private const string WEAPON_INFO_PATH = "Assets/Resources/CGD/MV/weaponinfo.json";
        private const string ITEM_WEAPON_INFO_PATH = "Assets/Resources/CGD/MV/itemweaponsinfo.json";
        private const string WEAPON_INFO_PATH_TW = "Assets/Resources/CGD/TW/weaponinfo.json";
        private const string ITEM_WEAPON_INFO_PATH_TW = "Assets/Resources/CGD/TW/itemweaponsinfo.json";
        private const string MV_ICONS_PATH = "Assets/Resources/CGD/MV/iconsinfo_descr.json";
        private const string URL = "https://figurewars.000webhostapp.com/api/dbpush.php?key=switnub&query=";

        // static StyleSheet styleSheet = new StyleSheet(Color.WhiteSmoke);
        private static List<PrimitiveIcon> primIcons = new SortedSet<PrimitiveIcon>(IconGetter.getPrimIcons(MV_ICONS_PATH)).ToList<PrimitiveIcon>();

        public static List<Weapon> getWeapons(string weaponsPath, string weaponInfoPath)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Debug.Log("!!! Debugger looks for the files in: " + Environment.CurrentDirectory);

            CgdDataReader<PrimitiveWeapon> PrimitiveWeaponDataList = new CgdDataReader<PrimitiveWeapon>(weaponsPath);
            List<PrimitiveWeapon> unsortedweapons = PrimitiveWeaponDataList.GetDataList();

            CgdDataReader<PrimitiveItemWeaponInfo> PrimitiveWeaponInfoDataList = new CgdDataReader<PrimitiveItemWeaponInfo>(weaponInfoPath);
            List<PrimitiveItemWeaponInfo> unsortedweaponinfo = PrimitiveWeaponInfoDataList.GetDataList();

            unsortedweapons = unsortedweapons.FindAll(w_mv => lastTwoDigitsAreGood(w_mv.wi_id));
            unsortedweaponinfo = unsortedweaponinfo.FindAll(info_mv => lastTwoDigitsAreGood(info_mv.ii_weaponinfo));

            SortedSet<PrimitiveWeapon> weapons_mv = new SortedSet<PrimitiveWeapon>(unsortedweapons);
            SortedSet<PrimitiveItemWeaponInfo> weaponinfo_mv = new SortedSet<PrimitiveItemWeaponInfo>(unsortedweaponinfo);
            //Debugger.Log(weaponinfo_mv.ToList<PrimitiveItemWeaponInfo>());
            List<int> missingIDS = new List<int>();

            List<Weapon> weapons = new List<Weapon>();
            getMVWeapons(weapons_mv.ToList<PrimitiveWeapon>(), weaponinfo_mv.ToList<PrimitiveItemWeaponInfo>(), missingIDS, weapons);
            return weapons;
        }

        public static List<Weapon> getWeapons()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Debug.Log("!!! Debugger looks for the files in: " + Environment.CurrentDirectory);

            List<PrimitiveWeapon> weapons_mv = new JSONToCSharpParser<PrimitiveWeapon>().parse(WEAPON_INFO_PATH);
            List<PrimitiveItemWeaponInfo> weaponInfos_mv = new JSONToCSharpParser<PrimitiveItemWeaponInfo>().parse(ITEM_WEAPON_INFO_PATH);

            List<PrimitiveWeapon> weapons_tw = new JSONToCSharpParser<PrimitiveWeapon>().parse(WEAPON_INFO_PATH_TW);
            List<PrimitiveItemWeaponInfo> weaponInfos_tw = new JSONToCSharpParser<PrimitiveItemWeaponInfo>().parse(ITEM_WEAPON_INFO_PATH_TW);

            weapons_mv = weapons_mv.FindAll(w_mv => lastTwoDigitsAreGood(w_mv.wi_id));
            weaponInfos_mv = weaponInfos_mv.FindAll(info_mv => lastTwoDigitsAreGood(info_mv.ii_weaponinfo));

            weapons_tw = weapons_tw.FindAll(w_tw => lastTwoDigitsAreGood(w_tw.wi_id));
            weaponInfos_tw = weaponInfos_tw.FindAll(info_tw => lastTwoDigitsAreGood(info_tw.ii_weaponinfo));

            List<int> missingIDS = new List<int>();

            List<Weapon> weapons = new List<Weapon>();

            getMVWeapons(weapons_mv, weaponInfos_mv, missingIDS, weapons);
            getTWWeaponsAndCompare(weapons_tw, weaponInfos_tw, weapons, missingIDS);

            int missing = missingIDS.Count;
            Console.WriteLine(missing + " items are missing in database");
            Console.WriteLine("Still " + missingIDS.Count + " items are missing in database");

            //   WebClient web = new WebClient();
            //  foreach (Weapon wep in weapons)
            //  {
            //     ProcessQuery(getWeaponInfo(wep), web);
            //    System.Threading.Thread.Sleep(50);
            // }

            Console.WriteLine("File in: " + Environment.CurrentDirectory);
            return weapons;
        }

        private static void getMVWeapons(List<PrimitiveWeapon> mvWeapons,
            List<PrimitiveItemWeaponInfo> infoWeps,
            List<int> missingIDS, List<Weapon> weapons)
        {
            foreach (PrimitiveWeapon w_mv in mvWeapons)
            {
                bool found = false;
                foreach (PrimitiveItemWeaponInfo info_mv in infoWeps)
                {
                    if (info_mv.ii_weaponinfo == w_mv.wi_id)
                    {
                        weapons.Add(getActualWeapon(w_mv, info_mv));
                        string msg = "id : " + w_mv.wi_id + " | name: " + info_mv.ii_name + " | Type " +
                                     w_mv.wi_weapon_type.ToString();
                        Console.WriteLine(msg);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    missingIDS.Add(w_mv.wi_id);
                    string errorMsg = "Couldn't find match for id: " + w_mv.wi_id;
                    Console.WriteLine(errorMsg);
                }
            }
        }

        private static void getTWWeaponsAndCompare(List<PrimitiveWeapon> TWWeapons,
                                    List<PrimitiveItemWeaponInfo> infoTW,
                                    List<Weapon> weapons,
                                    List<int> missingIDS)
        {
            foreach (PrimitiveWeapon w_tw in TWWeapons)
            {
                foreach (PrimitiveItemWeaponInfo info_tw in infoTW)
                {
                    if (missingIDS.Contains(w_tw.wi_id))
                    {
                        missingIDS.Remove(w_tw.wi_id);
                        weapons.Add(getActualWeapon(w_tw, info_tw));
                        String msg = "id : " + w_tw.wi_id + " | name: " + info_tw.ii_name + " | Type " +
                                     w_tw.wi_weapon_type.ToString();
                        Console.WriteLine("Found in TW :" + msg);
                    }
                }
            }
        }

        private static void ProcessQuery(List<string> queryList, WebClient web)
        {
            for (int i = 0; i < queryList.Count; i++)
            {
                Console.WriteLine("Executing on remote host: " + queryList[i]);
                var response = web.DownloadString(URL + queryList[i]);
                Console.WriteLine(response);
                System.Threading.Thread.Sleep(50);
            }
        }

        private static Weapon getActualWeapon(PrimitiveWeapon primWep, PrimitiveItemWeaponInfo info)
        {
            switch (primWep.wi_weapon_type)
            {
                case WeaponType.Melee:
                    return handleMeleeCase(primWep, info);

                case WeaponType.Rifle:
                    return handleCaseRifle(primWep, info);

                case WeaponType.Shotgun:
                    return handleCaseShotgun(primWep, info);

                case WeaponType.Sniper:
                    return handleCaseSniper(primWep, info);

                case WeaponType.Minigun:
                    return handleCaseMinigun(primWep, info);

                case WeaponType.Bazooka:
                    return handleCaseBazooka(primWep, info);

                case WeaponType.Grenade:
                    return handleCaseGrenade(primWep, info);

                default:
                    throw new ArgumentException("Illegal weapon type");
            }
        }

        private static List<string> getWeaponInfo(Weapon wep)
        {
            switch (wep.WeaponType)
            {
                case WeaponType.Melee:
                    return handleCaseMelee(wep);

                case WeaponType.Rifle:
                    return handleCaseRifle(wep);

                case WeaponType.Shotgun:
                    return handleCaseShotgun(wep);

                case WeaponType.Sniper:
                    return handleCaseSniper(wep);

                case WeaponType.Minigun:
                    return handleCaseMinigun(wep);

                case WeaponType.Bazooka:
                    return handleCaseBazooka(wep);

                case WeaponType.Grenade:
                    return handleCaseGrenade(wep);

                default:
                    throw new ArgumentException("Illegal weapon type");
            }
        }

        private static List<string> handleCaseMelee(Weapon wep)
        {
            List<string> queryList = new List<string>();
            queryList.Add(((Item)wep).ToSQLQuery());
            queryList.Add(wep.ToSQLQuery());
            queryList.Add(((Melee)wep).ToSQLQuery());

            return queryList;
        }

        private static List<string> handleCaseRifle(Weapon wep)
        {
            List<string> queryList = new List<string>();
            queryList.Add(((Item)wep).ToSQLQuery());
            queryList.Add(wep.ToSQLQuery());
            queryList.Add(((Rifle)wep).ToSQLQuery());

            return queryList;
        }

        private static List<string> handleCaseShotgun(Weapon wep)
        {
            List<string> queryList = new List<string>();
            queryList.Add(((Item)wep).ToSQLQuery());
            queryList.Add(wep.ToSQLQuery());
            queryList.Add(((Shotgun)wep).ToSQLQuery());

            return queryList;
        }

        private static List<string> handleCaseSniper(Weapon wep)
        {
            List<string> queryList = new List<string>();
            queryList.Add(((Item)wep).ToSQLQuery());
            queryList.Add(wep.ToSQLQuery());
            queryList.Add(((Sniper)wep).ToSQLQuery());

            return queryList;
        }

        private static List<string> handleCaseMinigun(Weapon wep)
        {
            List<string> queryList = new List<string>();
            queryList.Add(((Item)wep).ToSQLQuery());
            queryList.Add(wep.ToSQLQuery());
            queryList.Add(((Minigun)wep).ToSQLQuery());

            return queryList;
        }

        private static List<string> handleCaseBazooka(Weapon wep)
        {
            List<string> queryList = new List<string>();
            queryList.Add(((Item)wep).ToSQLQuery());
            queryList.Add(wep.ToSQLQuery());
            queryList.Add(((Bazooka)wep).ToSQLQuery());

            return queryList;
        }

        private static List<string> handleCaseGrenade(Weapon wep)
        {
            List<string> queryList = new List<string>();
            queryList.Add(((Item)wep).ToSQLQuery());
            queryList.Add(wep.ToSQLQuery());
            queryList.Add(((Grenade)wep).ToSQLQuery());

            return queryList;
        }

        private static Weapon handleCaseGrenade(PrimitiveWeapon primWep, PrimitiveItemWeaponInfo info)
        {
            Weapon wep = new Grenade(primWep.wi_ability_a, primWep.wi_ability_b,
                                        primWep.wi_ability_c, primWep.wi_ability_d);
            setWeaponStats(wep, primWep, info);
            return wep;
        }

        private static Weapon handleCaseBazooka(PrimitiveWeapon primWep, PrimitiveItemWeaponInfo info)
        {
            Weapon wep = new Bazooka(primWep.wi_ability_a, primWep.wi_ability_b,
                                        primWep.wi_ability_c, primWep.wi_ability_d);
            setWeaponStats(wep, primWep, info);
            return wep;
        }

        private static Weapon handleCaseMinigun(PrimitiveWeapon primWep, PrimitiveItemWeaponInfo info)
        {
            Weapon wep = new Minigun(primWep.wi_ability_a, primWep.wi_ability_b,
                                        primWep.wi_ability_c, primWep.wi_ability_d);
            setWeaponStats(wep, primWep, info);
            return wep;
        }

        private static Weapon handleCaseSniper(PrimitiveWeapon primWep, PrimitiveItemWeaponInfo info)
        {
            Weapon wep = new Sniper(primWep.wi_ability_a, primWep.wi_ability_b,
                                        primWep.wi_ability_c, primWep.wi_ability_d);
            setWeaponStats(wep, primWep, info);
            return wep;
        }

        private static Weapon handleCaseShotgun(PrimitiveWeapon primWep, PrimitiveItemWeaponInfo info)
        {
            Weapon wep = new Shotgun(primWep.wi_ability_a, primWep.wi_ability_b,
                                        primWep.wi_ability_c, primWep.wi_ability_d);
            setWeaponStats(wep, primWep, info);
            return wep;
        }

        private static Weapon handleCaseRifle(PrimitiveWeapon primWep, PrimitiveItemWeaponInfo info)
        {
            Weapon wep = new Rifle(primWep.wi_ability_a, primWep.wi_ability_b,
                                    primWep.wi_ability_c, primWep.wi_ability_d);
            setWeaponStats(wep, primWep, info);
            return wep;
        }

        private static void setWeaponStats(Weapon wep, PrimitiveWeapon primWep, PrimitiveItemWeaponInfo info)
        {
            wep.Id = primWep.wi_id;
            wep.Description = info.ii_desc;
            wep.Name = info.ii_name;
            wep.MeshPath = info.ii_meshfilename;
            wep.AmmoClip = primWep.wi_bullet_capacity;
            wep.TotalAmmo = primWep.wi_bullet_total;
            wep.ChangeTime = primWep.wi_change_time;
            //int iconDDSExtension = 4;

            foreach (var icon in primIcons)
            {
                if (icon.ii_id == info.ii_icon)
                {
                    string fileName = icon.ii_filename;
                    // back is an extra word in the filename
                    // in old CGD files
                    int indexOfBack = fileName.IndexOf("back");
                    fileName = fileName.Remove(indexOfBack, 4);
                    fileName = fileName.Substring(0, fileName.Length - 4);
                    //fileName += ".dds";
                    wep.IconFile = fileName;
                    wep.IconOffset = icon.ii_offset;
                    break;
                }
            }
        }

        private static Weapon handleMeleeCase(PrimitiveWeapon primWep, PrimitiveItemWeaponInfo info)
        {
            Weapon wep = new Melee(primWep.wi_ability_a, primWep.wi_ability_b,
                                   primWep.wi_ability_c, primWep.wi_ability_d);
            setWeaponStats(wep, primWep, info);
            return wep;
        }

        private static bool lastTwoDigitsAreGood(int id)
        {
            int[] digits = new int[7];
            int pos = 0;
            while (id > 0)
            {
                digits[pos] = id % 10;
                id /= 10;
                pos++;
            }
            Array.Reverse(digits);
            return digits[5] == 0 && digits[6] == 0;
        }

        /*
		public static void consoleFormat(StyleSheet styleSheet)
		{
			styleSheet.AddStyle("INSERT", Color.Magenta);
			styleSheet.AddStyle("INTO", Color.Magenta);
			styleSheet.AddStyle("IGNORE", Color.Magenta);
			styleSheet.AddStyle("VALUES", Color.Magenta);
			styleSheet.AddStyle("OK", Color.Green);
			styleSheet.AddStyle("'.*'", Color.LightCoral);
			styleSheet.AddStyle("[0-9]", Color.LightCoral);
		}
		*/
    }
}