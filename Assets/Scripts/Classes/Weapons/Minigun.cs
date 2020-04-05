using Scripts.Classes.Main;

namespace Scripts.Weapons
{
    public class Minigun : Gun
    {
        public enum FireType
        {
            BULLETS,
            FIRE,
            PLASMA
        }

        public int WarmUpTime { get; set; }
        public int Weight { get; set; }
        public FireType FType { get; set; }

        public Minigun()
        {
            WeaponType = WeaponType.Minigun;
        }

        public Minigun(int power, int warmUpTime, int runSpeed, int accuracy) : this()
        {
            Power = power;  // ability_a in cgd
            WarmUpTime = warmUpTime;    // ability_b in cgd
            Weight = runSpeed;    // ability_c in cgd
            Accuracy = accuracy;    // ability_d in cgd
        }

        public new string ToUniquePropertyList()
        {
            return "fire_type, warm_up, weigth";
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
            WarmUpTime = value;
        }

        public override int getB()
        {
            return WarmUpTime;
        }

        public override void setC(int value)
        {
            Weight = value;
        }

        public override int getC()
        {
            return Weight;
        }

        public override void setD(int value)
        {
            Accuracy = value;
        }

        public override int getD()
        {
            return Accuracy;
        }

        public new string ToSQLQuery()
        {
            return "INSERT IGNORE INTO minigun_base_stats (id, " + ToUniquePropertyList() + ") VALUES (" +
                Id + "," +
                "'" + FType.ToString() + "'" +
                WarmUpTime + "," +
                Weight + ");";
        }
    }
}