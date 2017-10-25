﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public struct Byte
    {
        public const byte MaxValue = 255;
        public const byte MinValue = 0;


        public extern static Byte Parse(string value);

        public extern override string ToString();

        public static bool TryParse(string value, out Byte v)
        {
            try
            {
                v = Parse(value);
                return true;
            }
            catch (Exception e)
            {
                v = 0;
                return false;
            }
        }
    }
}
