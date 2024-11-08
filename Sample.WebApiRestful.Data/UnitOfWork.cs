using Sample.WebApiRestful.Data.Abstract;
using Sample.WebApiRestful.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WebApiRestful.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        SampleWebApiContext _sampleWebApiContext;

        Repository<User> _repositoryUser;
        Repository<UserToken> _repositoryUserToken;
        Repository<Products> _repositoryProducts;
        Repository<Categories> _repositoryCategories;

        public UnitOfWork(SampleWebApiContext sampleWebApiContext)
        {
            _sampleWebApiContext = sampleWebApiContext;
        }

        // ??= : nếu null nó sẽ thực thi sau dấu =
        public Repository<User> RepositoryUser { get { return _repositoryUser ??= new Repository<User>(_sampleWebApiContext); } }
        public Repository<UserToken> RepositoryUserToken { get { return _repositoryUserToken ??= new Repository<UserToken>(_sampleWebApiContext); } }
        public Repository<Products> RepositoryProducts { get { return _repositoryProducts ??= new Repository<Products>(_sampleWebApiContext); } }
        public Repository<Categories> RepositoryCategories { get { return _repositoryCategories ??= new Repository<Categories>(_sampleWebApiContext); } }

        public async Task CommitAsync()
        {
            await _sampleWebApiContext.SaveChangesAsync();
        }
    }
}
