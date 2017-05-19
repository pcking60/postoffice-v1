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
    public interface ITransactionService
    {
        Transaction Add(Transaction transaction);

        void Update(Transaction transaction);

        Transaction Delete(int id);

        IEnumerable<Transaction> GetAll();

        IEnumerable<Transaction> GetAllByUserName(string userName);

        IEnumerable<Transaction> GetAll(string keyword);

        IEnumerable<Transaction> Search(string keyword, int page, int pageSize, string sort, out int totalRow);

        Transaction GetById(int id);

        void Save();
    }
    public class TransactionService : ITransactionService
    {
        private ITransactionRepository _transactionRepository;
        private IUnitOfWork _unitOfWork;
        private IApplicationUserRepository _userRepository;

        public TransactionService(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork, IApplicationUserRepository userRepository)
        {
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public Transaction Add(Transaction transaction)
        {
            return _transactionRepository.Add(transaction);
        }

        public Transaction Delete(int id)
        {
            return _transactionRepository.Delete(id);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _transactionRepository.GetAll();
        }

        public IEnumerable<Transaction> GetAll(string keyword)
        {
            if(!string.IsNullOrEmpty(keyword))
            {
                return _transactionRepository.GetMulti(x => x.MetaDescription.Contains(keyword));
            }
            else
            {
                return _transactionRepository.GetAll();
            }
        }

        public IEnumerable<Transaction> GetAllByUserName(string userName)
        {
            var userId = _userRepository.getByUserName(userName).Id;
            return _transactionRepository.GetMulti(x => x.UserId == userId).ToList();
        }

        public Transaction GetById(int id)
        {
            return _transactionRepository.GetSingleByID(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<Transaction> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var _q = _transactionRepository.GetMulti(x => x.Status && x.MetaDescription.Contains(keyword));

            totalRow = _q.OrderByDescending(x => x.CreatedDate).Count();

            return _q.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public void Update(Transaction transaction)
        {
            _transactionRepository.Update(transaction);
        }
    }
}
