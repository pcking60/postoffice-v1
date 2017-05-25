using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PostOfiice.DAta.Repositories
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        int getNoUserByPoID(int PoID);

        IEnumerable<ApplicationUser> GetAllByPoId(int id);

        int getNoUserByGroup(int GroupId);

        ApplicationUser getByUserName(string userName);
    }

    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<ApplicationUser> GetAllByPoId(int id)
        {
            return DbContext.Users.Where(x => x.POID == id).ToList();
        }

        public ApplicationUser getByUserName(string userName)
        {
            var user = this.DbContext.Users.Where(x => x.UserName.Contains(userName)).FirstOrDefault();
            return user;
        }

        public int getNoUserByGroup(int GroupId)
        {
            var query = from ug in this.DbContext.ApplicationUserGroups
                        join g in this.DbContext.ApplicationGroups
                        on ug.GroupId equals g.ID
                        join u in this.DbContext.Users
                        on ug.UserId equals u.Id
                        where g.ID == GroupId
                        select u;
            return query.Count();
        }

        public int getNoUserByPoID(int PoID)
        {
            return this.DbContext.Users.Where(x => x.POID == PoID).Count();
        }
    }
}