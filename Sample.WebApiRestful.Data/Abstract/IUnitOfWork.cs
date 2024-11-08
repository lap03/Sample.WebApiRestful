using Sample.WebApiRestful.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WebApiRestful.Data.Abstract
{
    public interface IUnitOfWork
    {
        Repository<User> RepositoryUser { get; }
        Repository<UserToken> RepositoryUserToken { get; }
        Repository<Products> RepositoryProducts { get; }
        Repository<Categories> RepositoryCategories { get; }
    }
}
