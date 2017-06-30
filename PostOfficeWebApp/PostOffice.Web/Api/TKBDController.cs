using AutoMapper;
using PostOffice.Model.Models;
using PostOffice.Service;
using PostOffice.Web.Infrastructure.Core;
using PostOffice.Web.Infrastructure.Extensions;
using PostOffice.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PostOffice.Web.Api
{
    [RoutePrefix("api/tkbd")]
    [Authorize]
    public class TKBDController : ApiControllerBase
    {
        private ITKBDService _tkbdService;
        private ITKBDHistoryService _tkbdHistoryService;

        public TKBDController(IErrorService errorService, ITKBDService tkbdService, ITKBDHistoryService tkbdHistoryService) : base(errorService)
        {
            this._tkbdService = tkbdService;
            this._tkbdHistoryService = tkbdHistoryService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _tkbdService.GetAll();
                totalRow = model.Count();
                var query = model.OrderBy(x => x.Id).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<TKBDAmount>, IEnumerable<TKBDAmountViewModel>>(query);

                foreach (var item in responseData)
                {
                }
                //ban test lai thu
                var paginationSet = new PaginationSet<TKBDAmountViewModel>//sai ne ban.
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
        [HttpGet]       
        public HttpResponseMessage Update(HttpRequestMessage request)
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
                    int days = 0;
                    var tkbdHistories = _tkbdHistoryService.GetAllDistinct();
                    int c = tkbdHistories.Count();             
                    foreach (var item in tkbdHistories)
                    {
                        TimeSpan s = DateTimeOffset.UtcNow.Subtract(item.TransactionDate);
                        days = (int)s.TotalDays;
                        TKBDAmountViewModel vm = new TKBDAmountViewModel();
                        vm.Status = true;
                        vm.Account = item.Account;
                        vm.CreatedBy = User.Identity.Name;
                        vm.UserId = item.UserId;
                        vm.Month = DateTime.Now.Month;
                        decimal money = _tkbdHistoryService.GetByAccount(item.Account).Sum(x => x.Money) ?? 0;

                        if (money > 0)
                        {
                            if (days > 30)
                            {
                                vm.Amount = money * item.Rate * 20 / 120 ?? 0;
                            }
                            else
                            {
                                vm.Amount = money * item.Rate * 20 * days / 120 / 30 ?? 0;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        TKBDAmount tkbd = new TKBDAmount();
                        tkbd.UpdateTKBD(vm);
                        if(_tkbdService.CheckExist(vm.Account, vm.Month))
                        {
                            continue;
                        }
                        _tkbdService.Add(tkbd);   
                    }
                    _tkbdService.Save();

                    response = request.CreateResponse(HttpStatusCode.Created, tkbdHistories.Count());
                }
                return response;
            });
        }
    }
}