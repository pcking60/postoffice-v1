﻿using AutoMapper;
using PostOffice.Model.Models;
using PostOffice.Service;
using PostOffice.Web.Infrastructure.Core;
using PostOffice.Web.Infrastructure.Extensions;
using PostOffice.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace PostOffice.Web.Api
{
    [RoutePrefix("api/transactions")]
    [Authorize]
    public class TransactionController : ApiControllerBase
    {
        private ITransactionService _transactionService;
        private ITransactionDetailService _transactionDetailService;
        private IApplicationUserService _userService;
        private IServiceService _serviceService;
        private IErrorService _errorService;

        public TransactionController(IErrorService errorService, IServiceService serviceService, ITransactionDetailService transactionDetailService, ITransactionService transactionService, IApplicationUserService userService) : base(errorService)
        {
            this._transactionService = transactionService;
            _transactionDetailService = transactionDetailService;
            this._errorService = errorService;
            _serviceService = serviceService;
            _userService = userService;
        }

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAllParentID(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _transactionService.GetAll();
                var responseData = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(model);
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("getEarnMoneyByUserName")]
        [HttpGet]
        public decimal? GetEarnMoneyByUserName(string userName)
        {
            
            decimal? totalEarn = _transactionDetailService.GetTotalEarnMoneyByUsername(userName);
            return totalEarn;    
        }

        [Route("getall")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var userName = User.Identity.Name;
                var model = _transactionService.GetAllByUserName(userName);
                totalRow = model.Count();
                var query = model.OrderBy(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(query);

                foreach (var item in responseData)
                {
                    item.ServiceName = _serviceService.GetById(item.ServiceId).Name;
                    item.TotalMoney = _transactionDetailService.GetTotalMoneyByTransactionId(item.ID);
                    item.EarnMoney = _transactionDetailService.GetTotalEarnMoneyByTransactionId(item.ID);
                }

                var paginationSet = new PaginationSet<TransactionViewModel>
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
        public IHttpActionResult Delete(HttpRequestMessage request, int id)
        {
            if (!ModelState.IsValid)
            {
                return Json("302");
            }
            else
            {
                var oldTransaction = _transactionService.GetById(id);
                oldTransaction.Status = false;
                var transactionDetails = _transactionDetailService.GetAllByTransactionId(oldTransaction.ID);                
                _transactionService.Update(oldTransaction);
                foreach (var item in transactionDetails)
                {
                    item.Status = false;
                }
                _transactionService.Save();
                return Json(oldTransaction.ID);                     
            }            
        }

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, TransactionViewModel transactionVM)
        {
           
            if (!ModelState.IsValid)
            {
                return request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                //return Json("301");
            }
            else
            {
                var currentDate = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                var transactionDate = transactionVM.TransactionDate.Ticks / TimeSpan.TicksPerMillisecond;

                bool isValid = (currentDate - transactionDate) > 86400 * 1000;
               
                if (isValid)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest);
                }
                else
                {
                    var dbTransaction = _transactionService.GetById(transactionVM.ID);
                    ICollection<TransactionDetail> transactionDetails = transactionVM.TransactionDetails;
                    var responseTransactionDetail = Mapper.Map<IEnumerable<TransactionDetail>, IEnumerable<TransactionDetailViewModel>>(transactionDetails);
                    transactionVM.UpdatedBy = User.Identity.Name;
                    dbTransaction.UpdateTransaction(transactionVM);
                    _transactionService.Update(dbTransaction);
                    _transactionService.Save();
                    foreach (var item in responseTransactionDetail)
                    {
                        item.UpdatedDate = DateTime.Now;
                        item.UpdatedBy = User.Identity.Name;
                        var transactionDetail = new TransactionDetail();
                        transactionDetail.UpdateTransactionDetail(item);
                        _transactionDetailService.Update(transactionDetail);
                        _transactionDetailService.Save();
                    }
                    //var responseData = Mapper.Map<Transaction, TransactionViewModel>(dbTransaction);
                    //response = request.CreateResponse(HttpStatusCode.OK, responseData);

                    //update total earn money by username

                    //ApplicationUser user = _userService.getByUserName(User.Identity.Name);
                    //var responseData = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user);
                    //responseData.TotalEarn = _transactionDetailService.GetTotalEarnMoneyByUsername(user.UserName);                
                    return request.CreateResponse(HttpStatusCode.OK, responseTransactionDetail);
                   // return Json(dbTransaction.ID);
                }
                
            }
                

        }

        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _transactionService.GetById(id);                

                var responseData = Mapper.Map<Transaction, TransactionViewModel>(model);

                responseData.ServiceName = _serviceService.GetById(model.ServiceId).Name;
                
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, TransactionViewModel transactionVM)
        {
            return CreateHttpResponse(request, () =>
            {
                if (!ModelState.IsValid)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var transaction = new Transaction();
                    //var transactionDetails = transactionVM.TransactionDetails;                
                    ICollection<TransactionDetail> transactionDetails = transactionVM.TransactionDetails;
                    var responseData = Mapper.Map<IEnumerable<TransactionDetail>, IEnumerable<TransactionDetailViewModel>>(transactionDetails);
                    //transactionVM.TransactionDetails = new List<TransactionDetail>();
                    transactionVM.UserId = _userService.getByUserName(User.Identity.Name).Id;
                    transactionVM.CreatedBy = User.Identity.Name;
                    transaction.UpdateTransaction(transactionVM);
                    _transactionService.Add(transaction);
                    _transactionService.Save();
                    foreach (var item in responseData)
                    {
                        item.TransactionID = transaction.ID;
                        item.CreatedBy = User.Identity.Name;
                        item.CreatedDate = DateTime.Now;
                        var dbTransactionDetail = new TransactionDetail();
                        dbTransactionDetail.UpdateTransactionDetail(item);
                        _transactionDetailService.Add(dbTransactionDetail);
                        _transactionDetailService.Save();

                    }

                    //foreach (var item in transactionDetails)
                    //{
                    //    item.TransactionId = transactionVM.ID;
                    //    transactionVM.TransactionDetails.Add(item);
                    //}

                    //foreach (var item in responseData)
                    //{
                    //    var dbTransactionDetail = new TransactionDetail();
                    //    dbTransactionDetail.UpdateTransactionDetail(item);                    
                    //    dbTransactionDetail.TransactionId = item.ID;
                    //    //_transactionDetailService.Add(dbTransactionDetail);
                    //}

                    return request.CreateErrorResponse(HttpStatusCode.OK, transaction.ID.ToString());
                }
            });
            
            
        }

        

        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedTransactions)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var listTransactions = new JavaScriptSerializer().Deserialize<List<int>>(checkedTransactions);
                    foreach (var item in listTransactions)
                    {
                        _transactionService.Delete(item);
                    }

                    _transactionService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listTransactions.Count);
                }

                return response;
            });
        }
    }
}