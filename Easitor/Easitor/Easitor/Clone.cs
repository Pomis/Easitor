using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easitor
{
    public static class CloneClass
    {
        /// <summary>
        /// Чистая копия объекта
        /// </summary>
        /// <typeparam name="T">Тип копируемого объекта</typeparam>
        /// <param name="obj">Копируемный объект</param>
        /// <returns>Ссылка на новый объект</returns>
        public static T CloneObject<T>(this T obj) where T : class
        {
            if (obj == null) return null;
            System.Reflection.MethodInfo inst = obj.GetType().GetMethod("MemberwiseClone",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (inst != null)
                return (T)inst.Invoke(obj, null);
            else
                return null;
        }
    }
}
