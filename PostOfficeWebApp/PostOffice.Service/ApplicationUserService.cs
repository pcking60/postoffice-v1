﻿using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using PostOfiice.DAta.Repositories;
using System.Collections.Generic;

namespace PostOffice.Service
{
    public interface IApplicationUserService
    {
        int getNoUserByPoID(int PoID);

        IEnumerable<ApplicationUser> GetAllByPOID(int id);

        ApplicationUser getByUserName(string userName);
    }

    public class ApplicationUserService : IApplicationUserService
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

        public IEnumerable<ApplicationUser> GetAllByPOID(int id)
        {
            return _userRepository.GetAllByPoId(id);
        }
    }
}