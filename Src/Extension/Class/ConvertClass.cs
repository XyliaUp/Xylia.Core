using System;
using System.Linq;

namespace Xylia.Extension
{
	public class ConvertClass
    {
        /// <summary>
        /// 构造实例对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public virtual object Construct(Type type, params object[] param)
        {
            var constructors = type.GetConstructors();
            if (constructors.Length == 0) throw new Exception($"{type} 不存在构造函数");

            //类型数组
            var types = new Type[param.Length];
            for (int i = 0; i < param.Length; i++) types[i] = param[i].GetType();

            var lst = param.ToList();
            foreach (var constructor in constructors)
            {
                //核验参数类型
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

            throw new Exception($"{type} 没有匹配的重载");
        }
    }
}