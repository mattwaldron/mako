namespace Mako.Common
{
    public static class MapTo<TTarget>
    {
        public static TTarget From<TSource>(TSource source)
        {
            var target = Activator.CreateInstance<TTarget>();
            foreach (var prop in typeof(TTarget).GetProperties())
            {
                var sourceProp = typeof(TSource).GetProperty(prop.Name);
                if (sourceProp != null && sourceProp.PropertyType == prop.PropertyType)
                {
                    prop.SetValue(target, sourceProp.GetValue(source));
                }
            }
            foreach (var field in typeof(TTarget).GetFields())
            {
                var sourceField = typeof(TSource).GetField(field.Name);
                if (sourceField != null && sourceField.FieldType == field.FieldType)
                {
                    field.SetValue(target, sourceField.GetValue(source));
                }
            }

            return target;
        }
    }
}
