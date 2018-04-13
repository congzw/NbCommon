using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NbPilot.Common.Trees
{
    /// <summary>
    /// 调整树排序
    /// </summary>
    public class TreeItemHolder<T> where T : IHaveRelationCode
    {
        /// <summary>
        /// CTOR
        /// </summary>
        public TreeItemHolder()
        {
            Children = new List<TreeItemHolder<T>>();
        }

        /// <summary>
        /// 节点值
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public IList<TreeItemHolder<T>> Children { get; set; }

        /// <summary>
        /// 转化成二维列表
        /// </summary>
        /// <param name="topPosition"></param>
        /// <returns></returns>
        public List<T> ToRelations(int topPosition = 1)
        {
            var convertNamePositions = SetRelations(this, topPosition.ToString());
            var items = convertNamePositions.ToList();
            return items;
        }
        
        /// <summary>
        /// 树转换成列表，并计算好Position
        /// </summary>
        /// <param name="treeItemHolder"></param>
        /// <param name="relationCode"></param>
        /// <returns></returns>
        private static IList<T> SetRelations(TreeItemHolder<T> treeItemHolder, string relationCode)
        {
            var relations = new List<T>();
            if (relations.Count == 0)
            {
                //root
                if (treeItemHolder.Value == null)
                {
                    throw new InvalidOperationException("必须对Value进行赋值，然后使用");
                }
                treeItemHolder.Value.RelationCode = relationCode;
                relations.Add(treeItemHolder.Value);
            }

            if (treeItemHolder.Children != null && treeItemHolder.Children.Count > 0)
            {
                for (int i = 1; i <= treeItemHolder.Children.Count; i++)
                {
                    var item = treeItemHolder.Children[i - 1];
                    string currentTreePosition = relationCode + "." + i;
                    var childrenList = SetRelations(item, currentTreePosition);
                    relations.AddRange(childrenList);
                }
            }

            return relations;
        }

    }

    ///// <summary>
    ///// 计算过排序码的节点值
    ///// </summary>
    //internal class TreeItemHolderWithRelation
    //{
    //    /// <summary>
    //    /// 排序码
    //    /// </summary>
    //    public string RelationCode { get; set; }

    //    /// <summary>
    //    /// 节点值
    //    /// </summary>
    //    public object Value { get; set; }

    //    /// <summary>
    //    /// Factory
    //    /// </summary>
    //    /// <param name="relationCode"></param>
    //    /// <param name="value"></param>
    //    /// <returns></returns>
    //    public static TreeItemHolderWithRelation Create<T>(string relationCode, T value) where T : IHaveRelationCode
    //    {
    //        return new TreeItemHolderWithRelation() { RelationCode = relationCode, Value = value };
    //    }

    //    /// <summary>
    //    /// 树转换成列表，并计算好Position
    //    /// </summary>
    //    /// <param name="treeItemHolder"></param>
    //    /// <param name="relationCode"></param>
    //    /// <returns></returns>
    //    public static List<TreeItemHolderWithRelation> ConvertNameRelations<T>(TreeItemHolder<T> treeItemHolder, string relationCode) where T : IHaveRelationCode
    //    {
    //        var relations = new List<TreeItemHolderWithRelation>();
    //        if (relations.Count == 0)
    //        {
    //            //root
    //            treeItemHolder.Value.RelationCode = relationCode;
    //            var withRelation = Create(relationCode, treeItemHolder.Value);
    //            relations.Add(withRelation);
    //        }

    //        if (treeItemHolder.Children != null && treeItemHolder.Children.Count > 0)
    //        {
    //            for (int i = 1; i <= treeItemHolder.Children.Count; i++)
    //            {
    //                var item = treeItemHolder.Children[i - 1];
    //                string currentTreePosition = relationCode + "." + i;
    //                var childrenList = ConvertNameRelations(item, currentTreePosition);
    //                relations.AddRange(childrenList);
    //            }
    //        }

    //        return relations;
    //    }
    //}
}
