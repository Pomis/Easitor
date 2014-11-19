using System;
using System.Reflection;

namespace Easitor
{
    public class Singleton<T> :INPC where T : class
    {
        /// Защищённый конструктор необходим для того, чтобы предотвратить создание экземпляра класса Singleton. 
        /// Он будет вызван из закрытого конструктора наследственного класса.
        protected Singleton() { }

        /// Фабрика используется для отложенной инициализации экземпляра класса
        private sealed class SingletonCreator<S> where S : class
        {
            //Используется Reflection для создания экземпляра класса без публичного конструктора
            private static readonly S instance = (S)typeof(S).GetConstructor(
                        BindingFlags.Instance | BindingFlags.NonPublic,
                        null,
                        new Type[0],
                        new ParameterModifier[0]).Invoke(null);

            public static S CreatorInstance
            {
                get { return instance; }
            }
        }

        public static T Instance
        {
            get { return SingletonCreator<T>.CreatorInstance; }
        }

    }
}
