using System;
using NbPilot.Common.Internal;

namespace NbPilot.Common
{
    /// <summary>
    /// Resolve As Singleton
    /// </summary>
    public interface IResolveAsSingleton
    {
        /// <summary>
        /// Resolve As Singleton
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        TInterface Resolve<T, TInterface>() where T : TInterface, new();
        /// <summary>
        /// Resolve As Singleton
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>() where T : new();
    }

    //注意，仅供Common内部使用
    public class ResolveAsSingleton : IResolveAsSingleton
    {
        TInterface IResolveAsSingleton.Resolve<T, TInterface>()
        {
            return AutoResolveAsSingletonHelper<T>.Lazy.Value;
        }

        T IResolveAsSingleton.Resolve<T>()
        {
            return AutoResolveAsSingletonHelper<T>.Lazy.Value;
        }

        #region for di extensions

        private static readonly Lazy<IResolveAsSingleton> Lazy = new Lazy<IResolveAsSingleton>(() => new ResolveAsSingleton());
        private static Func<IResolveAsSingleton> _resolve = () => Lazy.Value;
        public static Func<IResolveAsSingleton> Factory
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion

        #region for simple use

        public static TInterface Resolve<T, TInterface>() where T : TInterface, new()
        {
            return Factory().Resolve<T, TInterface>();
        }

        public static T Resolve<T>() where T : new()
        {
            return Factory().Resolve<T>();
        }

        #endregion
    }
}
