using AutoMapper;
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
    [RoutePrefix("api/service")]
    [Authorize]
    public class ServiceController : ApiControllerBase
    {
        private IServiceService _serviceService;
        private IServiceGroupService _serviceGroupService;

        public ServiceController(IErrorService errorService, IServiceService service, IServiceGroupService serviceGroupService) : base(errorService)
        {
            this._serviceService = service;
            this._serviceGroupService = serviceGroupService;
        }

        [Route("edit")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, ServiceViewModel serviceVm)
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
                    var dbService = _serviceService.GetById(serviceVm.ID);
                    dbService.UpdateService(serviceVm);
                    _serviceService.Update(dbService);
                    _serviceService.Save();
                    var responseData = Mapper.Map<PostOffice.Model.Models.Service, ServiceViewModel>(dbService);
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
                    var oldServie = _serviceService.Delete(id);
                    _serviceService.Save();
                    var responseData = Mapper.Map<PostOffice.Model.Models.Service, ServiceViewModel>(oldServie);
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
                var model = _serviceService.GetById(id);

                var responseData = Mapper.Map<PostOffice.Model.Models.Service, ServiceViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("add")]
        [HttpPost]
        public HttpResponseMessage Add(HttpRequestMessage request, ServiceViewModel serviceVM)
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
                    var newService = new PostOffice.Model.Models.Service();
                    newService.UpdateService(serviceVM);
                    _serviceService.Add(newService);
                    _serviceService.Save();
                    var responseData = Mapper.Map<PostOffice.Model.Models.Service, ServiceViewModel>(newService);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _serviceService.Getall(keyword);
                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);
                var responseData = Mapper.Map<IEnumerable<PostOffice.Model.Models.Service>, IEnumerable<ServiceViewModel>>(query);

                foreach (var item in responseData)
                {
                    var sv = _serviceGroupService.GetById(item.GroupID);
                    item.GroupName = sv.Name;
                }

                var paginationSet = new PaginationSet<ServiceViewModel>
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

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAllParentID(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {//ham nay la sao ban
                var model = _serviceService.Getall();
                var responseData = Mapper.Map<IEnumerable<Model.Models.Service>, IEnumerable<ServiceViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }
    }
}