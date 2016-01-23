namespace DRG.Constants
{
    using System;
    using System.Reflection;

    public abstract class ApplicationConfigs
    {
        protected ACHashCode HashCode = new ACHashCode();

        public T Get<T>() where T : ApplicationConfig
        {
            Type constantType = typeof(T);
            Type instanceType = GetType();
            FieldInfo[] fields = instanceType.GetFields();

            for(int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
            {
                FieldInfo field = fields[fieldIndex];
                if (constantType == field.FieldType)
                    return (T)field.GetValue(this);
            }

            return default(T);
        }

        public void SaveToCache()
        {
            Type instanceType = GetType();
            FieldInfo[] fields = instanceType.GetFields();

            for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
            {
                FieldInfo field = fields[fieldIndex];
                ApplicationConfig applicationConstant = field.GetValue(this) as ApplicationConfig;

                if (applicationConstant != null)
                {
                    applicationConstant.SaveToCache();
                }
            }

            HashCode.SaveToCache();
        }

        public void LoadFromCache()
        {
            Type instanceType = GetType();
            FieldInfo[] fields = instanceType.GetFields();

            for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
            {
                FieldInfo field = fields[fieldIndex];
                ApplicationConfig applicationConstant = field.GetValue(this) as ApplicationConfig;

                if (applicationConstant != null)
                {
                    applicationConstant.LoadFromCache();
                }
            }

            HashCode.LoadFromCache();
        }

        public override int GetHashCode()
        {
            return HashCode.Value;
        }
    }
}


