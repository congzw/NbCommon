namespace NbPilot.Common.Registries
{
    /// <summary>
    /// 注册服务接口，实现此接口的服务必须具有无参构造函数，供注册表实例进行发现和执行
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INbRegistryService<in T> where T : NbRegistry<T>
    {
        /// <summary>
        /// 注册自定义信息
        /// </summary>
        /// <param name="nbRegistry"></param>
        void Register(T nbRegistry);
    }
}
