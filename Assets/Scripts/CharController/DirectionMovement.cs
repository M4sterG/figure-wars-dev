
using System;
using System.Collections.Generic;

namespace DefaultNamespace.CharController
{
    
    public static class DirectionMovement
    {
        private static Dictionary<PlayerMovement.Direction, Tuple<float, float>> directionMoves = new 
            Dictionary<PlayerMovement.Direction, Tuple<float, float>>
            {
                {PlayerMovement.Direction.Front, new Tuple<float, float>(1,1)},
                {PlayerMovement.Direction.Back, new Tuple<float, float>(1,-1)},
                {PlayerMovement.Direction.Right, new Tuple<float, float>(1,1)},
                {PlayerMovement.Direction.Left, new Tuple<float, float>(-1, 1)},
                {PlayerMovement.Direction.BackLeft, new Tuple<float, float>(-1, -1)},
                {PlayerMovement.Direction.BackRight, new Tuple<float, float>(1, -1)},
                {PlayerMovement.Direction.FrontLeft, new Tuple<float, float>(-1,1)},
                {PlayerMovement.Direction.FrontRight, new Tuple<float, float>(1, 1)},
                {PlayerMovement.Direction.None, new Tuple<float, float>(0,0)}
            };

        internal static float getXMultiplier(PlayerMovement.Direction dir)
        {
            var touple = directionMoves[dir];
            return touple.Item1;
        }

        internal static float getZMultiplier(PlayerMovement.Direction dir)
        {
            var touple = directionMoves[dir];
            return touple.Item2;
        }
    }
    
    
}