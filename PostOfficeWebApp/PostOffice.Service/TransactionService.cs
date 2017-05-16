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
    public interface ITransactionService {
        Transaction Add(Transaction transaction);

        void Update(Transaction transaction);

        Transaction Delete(int id);

        IEnumerable<Transaction> GetAll();

        IEnumerable<Transaction> GetAll(string keyword);

        IEnumerable<Transaction> Search(string keyword, int page, int pageSize, string sort, out int totalRow);

        Transaction GetById(int id);

        void Save();
    }
    public class TransactionService : ITransactionService
    {
        ITransactionRepository _transactionRepository;
        IUnitOfWork _unitOfWork;
        public TransactionService(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
        {
            this._transactionRepository = transactionRepository;
            this._unitOfWork = unitOfWork;
        }

        public Transaction Add(Transaction transaction)
        {
           return  _transactionRepository.Add(transaction);
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
            if (!string.IsNullOrEmpty(keyword)) {
                return _transactionRepository.GetMulti(x => x.MetaDescription.Contains(keyword));
            }
            else
            {
                return _transactionRepository.GetAll();
            }
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
            var query = _transactionRepository.GetMulti(x => x.Status && x.MetaDescription.Contains(keyword));

            totalRow = query.OrderByDescending(x => x.CreatedDate).Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public void Update(Transaction transaction)
        {
            _transactionRepository.Update(transaction);
        }
    }
}
