namespace DRG.Math
{

    /// <summary>
    /// Fast math utils
    /// </summary>
    public static class MathUtils
    {

        public static bool Approximately(float a, float b, float maxDelta = float.Epsilon)
        {
            float delta = a - b;

            if (delta > 0)
            {
                return delta < maxDelta;
            }

            if (delta < 0)
            {
                return delta > -maxDelta;
            }

            return true;
        }

        public static float Abs(float f)
        {
            if (f > 0)
            {
                return f;
            }

            return -f;
        }
    }
}
