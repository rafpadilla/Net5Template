//using AutoMapper;
//using Net5Template.Core.Domain;
//using Net5Template.Core.Repository;
//using Net5Template.Infrastructure.DataContext;
//using MongoDB.Driver;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Net5Template.Infrastructure.Persistence.Repository
//{
//    public class RepositoryValueObjectMongo<TEntityValueObject> : IRepositoryValueObject<TEntityValueObject> where TEntityValueObject : class, IValueObject
//    {
//        protected readonly IDataContextMongo _context;
//        protected readonly IMongoCollection<TEntityValueObject> _collection;
//        protected readonly IMapper _mapper;

//        public RepositoryValueObjectMongo(IDataContextMongo context, IMapper mapper)
//        {
//            _mapper = mapper;
//            _context = context;

//            var dbContext = context?.Context as IMongoDatabase;

//            if (dbContext != null)
//            {
//                _collection = dbContext.GetCollection<TEntityValueObject>(typeof(TEntityValueObject).Name);
//            }
//        }

//        public IMongoCollection<TEntityValueObject> GetCollection()
//        {
//            return _collection;
//        }
//    }
//}
