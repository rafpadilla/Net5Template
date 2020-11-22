//using AutoMapper;
//using Net5Template.Core.Domain;
//using Net5Template.Core.Repository;
//using Net5Template.Infrastructure.DataContext;
//using MongoDB.Driver;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Net5Template.Infrastructure.Persistence.Repository
//{
//    public class RepositoryMongo<TEntity> : RepositoryMongo<TEntity, Guid> where TEntity : class, IEntity
//    {
//        public RepositoryMongo(IDataContextMongo context, IMapper mapper)
//            : base(context, mapper)
//        {
//        }
//    }
//    public class RepositoryMongo<TEntity, TKeyEntity> : IRepository<TEntity, TKeyEntity>
//        where TEntity : class, IEntity<TKeyEntity>
//        where TKeyEntity : struct
//    {
//        protected readonly IDataContextMongo _context;
//        protected readonly IMongoCollection<TEntity> _collection;
//        protected readonly IMapper _mapper;

//        public RepositoryMongo(IDataContextMongo context, IMapper mapper)
//        {
//            _mapper = mapper;
//            _context = context;

//            var dbContext = context?.Context as IMongoDatabase;

//            if (dbContext != null)
//            {
//                _collection = dbContext.GetCollection<TEntity>(typeof(TEntity).Name);
//            }
//        }

//        public IMongoCollection<TEntity> GetCollection()
//        {
//            return _collection;
//        }

//        public virtual async Task<IEnumerable<TEntity>> GetAll(int pageSize = 10, int pageIndex = 0)
//        {
//            return await _collection.Find(a => true).Skip(pageIndex * pageSize).Limit(pageSize).ToListAsync();
//        }

//        public virtual Task Add(TEntity entity)
//        {
//            return _collection.InsertOneAsync(entity);
//        }

//        public virtual Task AddRange(IEnumerable<TEntity> entities)
//        {
//            return _collection.InsertManyAsync(entities);
//        }

//        public virtual Task Update(TEntity entity)
//        {
//            return _collection.FindOneAndReplaceAsync(a => a.Id.Equals(entity.Id), entity);
//        }

//        public virtual async Task UpdateRange(IEnumerable<TEntity> entities)
//        {
//            foreach (var entity in entities)
//            {
//                await _collection.FindOneAndReplaceAsync(a => a.Id.Equals(entity.Id), entity);
//            }
//        }
//        public virtual Task Remove(TEntity entity)
//        {
//            return _collection.FindOneAndDeleteAsync(a => a.Id.Equals(entity.Id));
//        }

//        public virtual async Task RemoveRange(IEnumerable<TEntity> entities)
//        {
//            foreach (var entity in entities)
//            {
//                await _collection.FindOneAndDeleteAsync(a => a.Id.Equals(entity.Id));
//            }
//        }

//        public Task<TEntity> GetById(TKeyEntity Id)
//        {
//            return _collection.Find(a => a.Id.Equals(Id)).FirstOrDefaultAsync();
//        }
//    }
//}
