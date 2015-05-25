using BLL.Interfaces.Services;
using BLL.Services;
using DAL.Concrete;
using DAL.Interfaces.Repository;
using Ninject;
using ORM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Web.Common;
using DAL.Interfaces.DTO;

namespace DependencyResolver
{
    public static class ResolverConfig
    {
        public static void ConfigurateResolver(this IKernel kernel)
        {
            kernel.Bind<DbContext>().To<SiteFileModel>().InRequestScope();
            kernel.Bind<IRepository<DalUser>>().To<UserRepository>();
            kernel.Bind<IRepository<DalRole>>().To<RoleRepository>();
            kernel.Bind<IProfileRepository>().To<ProfileRepository>();
            kernel.Bind<IPostRepository>().To<PostRepository>();
            kernel.Bind<IVoteRepository>().To<VoteRepository>();
            kernel.Bind<ICommentRepository>().To<CommentRepository>();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IRoleService>().To<RoleService>();
            kernel.Bind<IProfileService>().To<ProfileService>();
            kernel.Bind<IPostService>().To<PostService>();
            kernel.Bind<IVoteService>().To<VoteService>();
            kernel.Bind<ICommentService>().To<CommentService>();
        }
    }
}
