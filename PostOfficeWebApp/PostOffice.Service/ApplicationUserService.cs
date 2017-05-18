using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using PostOfiice.DAta.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostOffice.Service
{
    public interface IApplicationUserService {
        int getNoUserByPoID(int PoID);

        ApplicationUser getByUserName(string userName);
    }
    public class ApplicationUserService: IApplicationUserService
    {
        private IApplicationUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;

        public ApplicationUserService(IUnitOfWork unitOfWork,
           IApplicationUserRepository user)
        {
            this._userRepository = user;
            this._unitOfWork = unitOfWork;
        }
        public int getNoUserByPoID(int PoID)
        {
            return _userRepository.getNoUserByPoID(PoID);
        }

        public ApplicationUser getByUserName(string userName)
        {
            return _userRepository.getByUserName(userName);
        }
    }
}
