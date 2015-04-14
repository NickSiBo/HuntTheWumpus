﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuntTheWumpus.SharedCode.Helpers
{
    static class MathUtils
    {
        public static double PolygonWidth(int NumSides, double Apothem)
        {
            double Theta = (Math.Floor(NumSides / 2d) * 2d * Math.PI) / NumSides;
            double Radius = PolygonRadius(NumSides, Apothem);
            return 2d * Math.Sin(Theta / 2) * Radius;
        }

        public static double PolygonRadius(int NumSides, double Apothem)
        {
            return Apothem * Sec(Math.PI / NumSides);
        }

        public static double PolygonHeight(int NumSides, double Apothem)
        {
            if (IsEven(NumSides))
                return Apothem * 2d;
            else
            {
                double Radius = Apothem * Sec(Math.PI / NumSides);
                return Radius + Apothem;
            }
        }

        public static Vector2 PolarToCart(double Angle, double Radius)
        {
            return new Vector2(
                (float)Math.Cos(Angle) * (float)Radius,
                (float)Math.Sin(Angle) * (float)Radius * -1f);
        }

        public static bool IsEven(double d)
        {
            return d % 2 == 0;
        }
        /// <summary>
        /// Returns the secant.
        /// </summary>
        /// <param name="Angle">Measured in radians.</param>
        /// <returns></returns>
        public static double Sec(double Angle)
        {
            return 1d / Math.Cos(Angle);
        }
    }
}
