﻿using Scripts.Classes.Main;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Scripts.Weapons
{
    public class PrimitiveItemWeaponInfo 
    {
        public int ii_id { get; set; }
        public string ii_name { get; set; }
        public string ii_name_option { get; set; }
        public string ii_name_time { get; set; }
        public int ii_type { get; set; }
        public int ii_type_inven { get; set; }
        public bool ii_inven_usable { get; set; }
        public int ii_type_pcbang { get; set; }
        public int ii_package_result { get; set; }
        public bool ii_dress { get; set; }
        public bool ii_hide_hair { get; set; }
        public bool ii_hide_face { get; set; }
        public bool ii_hide_back { get; set; }
        public bool ii_class_a { get; set; }
        public bool ii_class_b { get; set; }
        public bool ii_class_c { get; set; }
        public bool ii_class_d { get; set; }
        public bool ii_class_e { get; set; }
        public bool ii_class_f { get; set; }
        public bool ii_class_g { get; set; }
        public bool ii_class_h { get; set; }
        public bool ii_class_i { get; set; }
        public bool ii_class_j { get; set; }
        public bool ii_class_k { get; set; }
        public bool ii_class_l { get; set; }
        public bool ii_class_m { get; set; }
        public bool ii_class_n { get; set; }
        public bool ii_class_o { get; set; }
        public bool ii_class_p { get; set; }
        public int ii_grade { get; set; }
        public int ii_stocks { get; set; }
        public bool ii_usable { get; set; }
        public bool ii_upgradable { get; set; }
        public bool ii_consumable { get; set; }
        public int ii_weaponinfo { get; set; }
        public int ii_fullseteffectinfo { get; set; }
        public int ii_durable_value { get; set; }
        public int ii_durable_factor { get; set; }
        public int ii_durable_repair_type { get; set; }
        public bool ii_limited_mod { get; set; }
        public int ii_limited_time { get; set; }
        public int ii_buy_coupon { get; set; }
        public int ii_buy_cash { get; set; }
        public int ii_buy_point { get; set; }
        public int ii_sell_point { get; set; }
        public int ii_bonus_point { get; set; }
        public int ii_bonus_pack { get; set; }
        public int ii_dioramainfo { get; set; }
        public int ii_dummyinfo { get; set; }
        public int ii_icon { get; set; }
        public int ii_iconsmall { get; set; }
        public string ii_meshfilename { get; set; }
        public string ii_nodename { get; set; }
        public string ii_color_ambient { get; set; }
        public string ii_color_diffuse { get; set; }
        public string ii_color_specular { get; set; }
        public string ii_color_emittance { get; set; }
        public int ii_sfx { get; set; }
        public int ef_effect_1 { get; set; }
        public int ef_target_1 { get; set; }
        public int ef_effect_2 { get; set; }
        public int ef_target_2 { get; set; }
        public int ef_effect_3 { get; set; }
        public int ef_target_3 { get; set; }
        public string ii_desc { get; set; }
        public bool ii_immediately_set { get; set; }
        public bool ii_is_trade { get; set; }
        public int ii_ei_exp { get; set; }
        public bool ii_evolution_type { get; set; }

        public PrimitiveItemWeaponInfo(CgdDescriptor descr, Byte[] datablock)
        {
            PropertyInfo[] listProperties = this.GetType().GetProperties();

            int cursorpos = 0;
            for (int i = 0; i < listProperties.Length; i++)
            {
                Byte[] x = new Byte[descr.fieldsizes[i]];
                Buffer.BlockCopy(datablock, cursorpos, x, 0, x.Length);
                if (descr.fieldsizes[i] > 4)
                {
                    var value = Encoding.UTF8.GetString(x, 0, x.Length);
                    listProperties[i].SetValue(this, value);
                }
                else if (descr.fieldsizes[i] < 4)
                {
                    listProperties[i].SetValue(this, BitConverter.ToBoolean(x, 0));
                }
                else
                {
                    var value = BitConverter.ToInt32(x, 0);
                    listProperties[i].SetValue(this, value);
                }
                cursorpos += descr.fieldsizes[i];
            }
        }
    }

    public class PrimitiveItemWeaponInfoComparer : IComparer<PrimitiveItemWeaponInfo>
    {
        public int Compare(PrimitiveItemWeaponInfo x, PrimitiveItemWeaponInfo y)
        {
            if (x.ii_id > y.ii_id)
            {
                return 1;
            }
            else if (y.ii_id < x.ii_id)
            {
                return -1;
            }
            else return 0;
        }
    }
}