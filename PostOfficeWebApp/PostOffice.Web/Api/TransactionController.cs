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
        private IApplicationUserService _userService;
        private IErrorService _errorService;

        public TransactionController(IErrorService errorService, ITransactionService transactionService, IApplicationUserService userService) : base(errorService)
        {
            this._transactionService = transactionService;
            this._errorService = errorService;
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

        [Route("getall")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _transactionService.GetAll();
                totalRow = model.Count();
                var query = model.OrderBy(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Transaction>, IEnumerable<TransactionViewModel>>(query);

             
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

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, TransactionViewModel propVM)
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
                    var dbTransaction = _transactionService.GetById(propVM.ID);
                    dbTransaction.UpdateTransaction(propVM);
                    _transactionService.Update(dbTransaction);
                    _transactionService.Save();
                    var responseData = Mapper.Map<Transaction, TransactionViewModel>(dbTransaction);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _transactionService.GetById(id);

                var responseData = Mapper.Map<Transaction, TransactionViewModel>(model);

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
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var newTransaction = new Transaction();
                    newTransaction.UpdateTransaction(transactionVM);
                    newTransaction.UserId = _userService.getByUserName(User.Identity.Name).Id;
                    _transactionService.Add(newTransaction);
                    _transactionService.Save();
                    var responseData = Mapper.Map<Transaction, TransactionViewModel>(newTransaction);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
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
                    var oldTransaction = _transactionService.Delete(id);
                    _transactionService.Save();
                    var responseData = Mapper.Map<Transaction, TransactionViewModel>(oldTransaction);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
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