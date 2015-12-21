namespace DRG.Constants
{

    public class ACHashCode : ApplicationConstant
    {

        private const int DEFAULT_VALUE = 0;

        public int Value { get; private set; }

        public ACHashCode()
        {
            Value = DEFAULT_VALUE;
        }

        // set as static to prevent uncontrolled modification
        public static void SetValue(ACHashCode constant, int val)
        {
            constant.Value = val;
        }
    }
}
