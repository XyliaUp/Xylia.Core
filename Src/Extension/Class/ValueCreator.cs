namespace Xylia.Extension.Class;
public class ValueCreator
{
    public virtual object Construct(Type type, params object[] param)
    {
        var constructors = type.GetConstructors();
        if (constructors.Length == 0) throw new InvalidOperationException($"{type} no constrctors!");

        var types = new Type[param.Length];
        for (int i = 0; i < param.Length; i++) types[i] = param[i].GetType();

        var lst = param.ToList();
        foreach (var constructor in constructors)
        {
            var paramsInfos = constructor.GetParameters();
            if (paramsInfos.Length == 0 && param.Length != 0) continue;

            for (int i = 0; i < paramsInfos.Length; i++)
            {
                if (types.Length <= i)
                {
                    if (paramsInfos[i].HasDefaultValue) lst.Add(paramsInfos[i].DefaultValue);
                    else continue;
                }
                else if (paramsInfos[i].ParameterType != types[i]) continue;
            }

            return constructor.Invoke(lst.ToArray());
        }


        throw new NotSupportedException();
    }
}