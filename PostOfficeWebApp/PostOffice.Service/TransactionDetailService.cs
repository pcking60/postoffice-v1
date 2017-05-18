﻿using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using PostOfiice.DAta.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostOffice.Service
{
    public interface ITransactionDetailService {
        TransactionDetail Add(TransactionDetail transactionDetail);

        void Update(TransactionDetail transactionDetail);

        TransactionDetail Delete(int id);

        IEnumerable<TransactionDetail> GetAll();

        IEnumerable<TransactionDetail> GetAll(string keyword);

        IEnumerable<TransactionDetail> GetAllByTransactionId(int transactionId);

        IEnumerable<TransactionDetail> Search(string keyword, int page, int pageSize, string sort, out int totalRow);
        
        TransactionDetail GetById(int id);

        decimal? GetTotalMoneyByTransactionId(int id);

        decimal? GetTotalEarnMoneyByTransactionId(int id);

        void Save();
    }
    public class TransactionDetailService : ITransactionDetailService
    {
        private ITransactionDetailRepository _transactionDetailRepository;
        private ITransactionRepository _transactionRepository;
        private IPropertyServiceRepository _propertyServiceRepository;
        private IUnitOfWork _unitOfWork;

        public TransactionDetailService(ITransactionDetailRepository transactionDetailRepository, IPropertyServiceRepository propertyServiceRepository, ITransactionRepository transactionRepository, IUnitOfWork unitOfWork) {
            this._transactionDetailRepository = transactionDetailRepository;
            _propertyServiceRepository = propertyServiceRepository;
            _transactionRepository = transactionRepository;
            this._unitOfWork = unitOfWork;
        }
        public TransactionDetail Add(TransactionDetail transactionDetail)
        {
            return _transactionDetailRepository.Add(transactionDetail);
        }

        public TransactionDetail Delete(int id)
        {
            return _transactionDetailRepository.Delete(id);
        }

        public IEnumerable<TransactionDetail> GetAll()
        {
            return _transactionDetailRepository.GetAll();
        }

        public IEnumerable<TransactionDetail> GetAll(string keyword)
        {
            if(!string.IsNullOrEmpty(keyword))
            {
                return _transactionDetailRepository.GetMulti(x => x.MetaDescription.Contains(keyword));
            }
            else
            {
                return _transactionDetailRepository.GetAll();
            }
        }

        public TransactionDetail GetById(int id)
        {
            return _transactionDetailRepository.GetSingleByID(id);
        }

        public decimal? GetTotalEarnMoneyByTransactionId(int id)
        {
            decimal? earnTotal = 0;
            var listTransactionDetail = _transactionDetailRepository.GetMulti(x => x.TransactionId == id).ToList();
            foreach (var item in listTransactionDetail)
            {
                decimal? percent = _propertyServiceRepository.GetSingleByID(item.PropertyServiceId).Percent;
                earnTotal = earnTotal + percent * item.Money;
            }
            int? quantity = _transactionRepository.GetSingleByID(id).Quantity;
            return earnTotal * quantity;
        }

        public decimal? GetTotalMoneyByTransactionId(int id)
        {
            return _transactionDetailRepository.GetMulti(x => x.TransactionId == id).Sum(x => x.Money);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<TransactionDetail> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _transactionDetailRepository.GetMulti(x => x.Status && x.MetaDescription.Contains(keyword));

            totalRow = query.OrderByDescending(x => x.CreatedDate).Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public void Update(TransactionDetail transactionDetail)
        {
            _transactionDetailRepository.Update(transactionDetail);
        }

        public IEnumerable<TransactionDetail> GetAllByTransactionId(int transactionId)
        {
           return _transactionDetailRepository.GetMulti(x => x.TransactionId == transactionId);
        }
    }
}
