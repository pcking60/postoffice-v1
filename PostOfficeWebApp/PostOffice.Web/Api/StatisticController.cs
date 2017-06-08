using PostOffice.Common;
using PostOffice.Common.ViewModels;
using PostOffice.Model.Models;
using PostOffice.Service;
using PostOffice.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PostOffice.Web.Api
{
    [RoutePrefix("api/statistic")]
    public class StatisticController : ApiControllerBase
    {
        private IStatisticService _statisticService;
        private IDistrictService _districtService;

        public StatisticController(IErrorService errorService, IStatisticService statisticService, IDistrictService districtService) : base(errorService)
        {
            _statisticService = statisticService;
            _districtService = districtService;
        }

        [Route("getrevenue")]
        [HttpGet]
        public HttpResponseMessage GetRevenueStatistic(HttpRequestMessage request, string fromDate, string toDate)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _statisticService.GetRevenueStatistic(fromDate, toDate);                
               
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
               
            });
        }

        [Route("getunit")]
        [HttpGet]
        public HttpResponseMessage GetUnitStatistic(HttpRequestMessage request, string fromDate, string toDate)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _statisticService.GetUnitStatistic(fromDate, toDate);

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;

            });
        }
        [HttpGet]
        [Route("reportFunction1")]
        public async Task<HttpResponseMessage> ReportFunction1(HttpRequestMessage request, string fromDate, string toDate, int functionId, int unitId)
        {
            string fileName = string.Concat("Money_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");
            var folderReport = ConfigHelper.GetByKey("ReportFolder");
            string filePath = HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);
            ReportTemplate vm = new ReportTemplate();
            try
            {
                #region customFill Test
                vm.FromDate = DateTime.Parse(fromDate);
                vm.ToDate = DateTime.Parse(toDate);
                District obj = new District();
                if (unitId != 0)
                {
                   obj = _districtService.GetById(unitId);
                }
                switch (functionId)
                {
                    case 1:
                        vm.FunctionName = "Bảng kê thu tiền tại đơn vị";
                        break;
                    default:
                        vm.FunctionName = "Chức năng khác";
                        break; 

                }
                vm.Unit = obj.Name;
                vm.CreatedBy = User.Identity.Name;

                #endregion
                
                IEnumerable<ReportFunction1> rp = _statisticService.ReportFunction1(fromDate, toDate);
                List<ReportFunction1> listData = rp.ToList();                

                //test medthod customFill
                await ReportHelper.RP1(listData, fullPath, vm);

                return request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

    }
}