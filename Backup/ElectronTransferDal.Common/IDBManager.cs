using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ElectronTransferModel.Base;

namespace ElectronTransferDal.Common
{
    public interface IDBManager
    {
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        void Delete(DBEntity entity);

        /// <summary>
        /// 获取一系列实体
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        IEnumerable<DBEntity> GetEntities(Type type);

        /// <summary>
        /// 获取一系列实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        IEnumerable<T> GetEntities<T>() where T : DBEntity, new();

        /// <summary>
        /// 获取一系列实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="expr">条件</param>
        /// <returns></returns>
        IEnumerable<T> GetEntities<T>(Expression<Func<T, bool>> expr) where T : DBEntity, new();

        IEnumerable<DBEntity> GetEntities(Type type,Expression<Func<DBEntity, bool>> expr);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="expr">条件</param>
        /// <returns></returns>
        T GetEntity<T>(Expression<Func<T, bool>> expr) where T : DBEntity, new();


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="key">键值</param>
        /// <returns></returns>
        T GetEntity<T>(object key) where T : DBEntity, new();

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="expr">条件</param>
        /// <returns></returns>
        DBEntity GetEntity(Type type, Expression<Func<DBEntity, bool>> expr);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <param name="key">键值</param>
        /// <returns></returns>
        DBEntity GetEntity(Type type, object key);

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool Insert(DBEntity entity);

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="typename">类名</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool Insert(string typename, object entity);

        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="entities">一系列的实体</param>
        /// <returns></returns>
        bool InsertBulk(IEnumerable<DBEntity> entities);// where T : DBEntity, new();

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        void Update(DBEntity entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="condition">条件</param>
        void Update(DBEntity entity, object condition);

        /// <summary>
        /// 提交
        /// </summary>
        void Submit();
    }
}
