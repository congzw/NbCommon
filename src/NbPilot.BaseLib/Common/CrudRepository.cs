using System.Linq;
using ZQNB.Common.Data.Model;

namespace ZQNB.Common
{
    /// <summary>
    /// 简单模型的增删改查等数据服务接口
    /// 为典型的增删改查类型的数据服务提供一个一致性的命名接口，简单数据的服务，建议从此继承但不做强制要求
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TPk"></typeparam>
    public interface ICrudRepository<T, in TPk> where T : INbEntity<TPk>
    {
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Query();

        /// <summary>
        /// 根据id查找实体
        /// </summary>
        /// <param name="id">实体的Id</param>
        /// <returns></returns>
        T Get(TPk id);

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Add(T entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Remove(T entity);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Edit(T entity);
    }
}
