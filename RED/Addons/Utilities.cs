using System;

namespace RED.Addons
{
    public static class Utilities
    {
        public static double ParseCoordinate(string coord)
        {
            string[] input = coord.Trim().Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
            switch (input.Length)
            {
                case 1:
                    {
                        double value0;
                        if (!Double.TryParse(input[0], out value0))
                            throw new ArgumentException();
                        return value0;
                    }
                case 2:
                    {
                        int value0;
                        double value1;
                        if (!Int32.TryParse(input[0], out value0) || !Double.TryParse(input[1], out value1))
                            throw new ArgumentException();
                        return (value0) + Math.Sign(value0) * (value1 * 1 / 60d);
                    }
                case 3:
                    {
                        int value0, value1;
                        double value2;
                        if (!Int32.TryParse(input[0], out value0) || !Int32.TryParse(input[1], out value1) || !Double.TryParse(input[2], out value2))
                            throw new ArgumentException();
                        return (value0) + Math.Sign(value0) * ((value1 * 1 / 60d) + (value2 * 1 / 60d / 60d));
                    }
                default:
                    throw new ArgumentException();
            }
        }
    }
}
