﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xigadee
{
    public class PersistenceMessageHandlerRedisCache<K,E>: PersistenceManagerHandlerJsonBase<K,E, PersistenceStatistics, PersistenceCommandPolicy>
        where K : IEquatable<K>
    {
        #region Constructor
        /// <summary>
        /// This is the default constructor.
        /// </summary>
        /// <param name="entityName">The entity name, derived from E if left null.</param>
        /// <param name="versionPolicy">The optional version and locking policy.</param>
        /// <param name="defaultTimeout">The default timeout when making requests.</param>
        /// <param name="retryPolicy">The retry policy</param>
        protected PersistenceMessageHandlerRedisCache(string redisConnection
            , Func<E, K> keyMaker
            , Func<string, K> keyDeserializer
            , string entityName = null
            , VersionPolicy<E> versionPolicy = null
            , TimeSpan? defaultTimeout = null
            , PersistenceRetryPolicy persistenceRetryPolicy = null
            , ResourceProfile resourceProfile = null
            , Func<E, IEnumerable<Tuple<string, string>>> referenceMaker = null
            , Func<K, string> keySerializer = null
            )
            : base( entityName: entityName
                  , versionPolicy: versionPolicy
                  , defaultTimeout: defaultTimeout
                  , persistenceRetryPolicy: persistenceRetryPolicy
                  , resourceProfile: resourceProfile
                  , cacheManager: RedisCacheHelper.Default<K,E>(redisConnection)
                  , keyMaker:keyMaker
                  , referenceMaker:referenceMaker
                  , keySerializer: keySerializer
                  , keyDeserializer: keyDeserializer)
        {
        }
        #endregion

        protected async override Task<IResponseHolder<E>> InternalCreate(PersistenceRequestHolder<K, E> holder)
        {          
            if (await mCacheManager.Write(mTransform, holder.rq.Entity))
                return new PersistenceResponseHolder<E> { IsSuccess = true, StatusCode = 201, Entity = holder.rq.Entity };

            return new PersistenceResponseHolder<E> { IsSuccess = false, StatusCode = 409 };
        }

        protected async override Task<IResponseHolder<E>> InternalUpdate(PersistenceRequestHolder<K, E> holder)
        {
            if (await mCacheManager.Write(mTransform, holder.rq.Entity))
                return new PersistenceResponseHolder<E> { IsSuccess = true, StatusCode = 200, Entity = holder.rq.Entity };

            return new PersistenceResponseHolder<E> { IsSuccess = false, StatusCode = 409 };
        }

        protected async override Task<IResponseHolder<E>> InternalRead(K key, PersistenceRequestHolder<K, E> holder)
        {
            return await mCacheManager.Read(mTransform, key);
        }

        protected async override Task<IResponseHolder<E>> InternalReadByRef(Tuple<string, string> reference, PersistenceRequestHolder<K, E> holder)
        {
            return await mCacheManager.Read(mTransform, reference);
        }

        protected async override Task<IResponseHolder> InternalVersion(K key, PersistenceRequestHolder<K, Tuple<K, string>> holder)
        {
            return await mCacheManager.VersionRead(mTransform, key);
        }

        protected async override Task<IResponseHolder> InternalVersionByRef(Tuple<string, string> reference, PersistenceRequestHolder<K, Tuple<K, string>> holder)
        {
            return await mCacheManager.VersionRead(mTransform, reference);
        }

        protected async override Task<IResponseHolder> InternalDelete(K key, PersistenceRequestHolder<K, Tuple<K, string>> holder)
        {
            if (await mCacheManager.Delete(mTransform, key))
                return new PersistenceResponseHolder<E> { IsSuccess = true, StatusCode = 200 };

            return new PersistenceResponseHolder<E> { IsSuccess = false, StatusCode = 404 };
        }

        protected async override Task<IResponseHolder> InternalDeleteByRef(Tuple<string, string> reference, PersistenceRequestHolder<K, Tuple<K, string>> holder)
        {
            if (await mCacheManager.Delete(mTransform, reference))
                return new PersistenceResponseHolder<E> { IsSuccess = true, StatusCode = 200 };

            return new PersistenceResponseHolder<E> { IsSuccess = false, StatusCode = 404 };
        }
    }
}
