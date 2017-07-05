﻿using AutoMapper;
using OfficeOpenXml;
using PostOffice.Model.Models;
using PostOffice.Service;
using PostOffice.Web.Infrastructure.Core;
using PostOffice.Web.Infrastructure.Extensions;
using PostOffice.Web.Models;
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
    [RoutePrefix("api/tkbd")]
    [Authorize]
    public class TKBDController : ApiControllerBase
    {
        private ITKBDService _tkbdService;
        private ITKBDHistoryService _tkbdHistoryService;
        private IApplicationUserService _applicationUserService;

        public TKBDController(IErrorService errorService, ITKBDService tkbdService, ITKBDHistoryService tkbdHistoryService, IApplicationUserService applicationUserService) : base(errorService)
        {
            this._tkbdService = tkbdService;
            this._tkbdHistoryService = tkbdHistoryService;
            _applicationUserService = applicationUserService;
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

        [Route("import")]
        [HttpPost]
        public async Task<HttpResponseMessage> Import()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Định dạng không được server hỗ trợ");
            }

            var root = HttpContext.Current.Server.MapPath("~/UploadedFiles/Excels");
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            var provider = new MultipartFormDataStreamProvider(root);
            Stream reqStream = Request.Content.ReadAsStreamAsync().Result;
            MemoryStream tempStream = new MemoryStream();
            reqStream.CopyTo(tempStream);

            tempStream.Seek(0, SeekOrigin.End);
            StreamWriter writer = new StreamWriter(tempStream);
            writer.WriteLine();
            writer.Flush();
            tempStream.Position = 0;

            StreamContent streamContent = new StreamContent(tempStream);
            foreach (var header in Request.Content.Headers)
            {
                streamContent.Headers.Add(header.Key, header.Value);
            }
            try
            {
                // Read the form data.
                streamContent.LoadIntoBufferAsync().Wait();
                //This is where it bugs out
                var result = await streamContent.ReadAsMultipartAsync(provider);
                //Upload files
                int addedCount = 0;
                foreach (MultipartFileData fileData in result.FileData)
                {
                    if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Yêu cầu không đúng định dạng");
                    }
                    string fileName = fileData.Headers.ContentDisposition.FileName;
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }
                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }

                    var fullPath = Path.Combine(root, fileName);
                    File.Copy(fileData.LocalFileName, fullPath, true);

                    //insert to DB
                    //var 
                    List<TKBDHistory> listItem = new List<TKBDHistory>();
                    listItem = this.ReadTKBDFromExcel(fullPath);
                    if (listItem.Count > 0)
                    {
                        foreach (var product in listItem)
                        {
                            _tkbdHistoryService.Add(product);
                            addedCount++;
                        }
                        _tkbdHistoryService.Save();
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Đã nhập thành công " + addedCount + " sản phẩm thành công.");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }            
            
        }

        private List<TKBDHistory> ReadTKBDFromExcel(string fullPath)
        {
            using (var package = new ExcelPackage(new FileInfo(fullPath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                List<TKBDHistory> listTKBD = new List<TKBDHistory>();
                TKBDHistoryViewModel tkbdViewModel;
                TKBDHistory tkbdHistory;

                DateTimeOffset transactionDate;
                DateTimeOffset tranDate;
                decimal money;
                decimal rate;  

                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    tkbdViewModel = new TKBDHistoryViewModel();
                    tkbdHistory = new TKBDHistory();

                    tkbdViewModel.Name = workSheet.Cells[i, 1].Value.ToString();
                    tkbdViewModel.CustomerId = workSheet.Cells[i, 2].Value.ToString();
                    tkbdViewModel.Account = workSheet.Cells[i, 3].Value.ToString();
                    if (DateTimeOffset.TryParse(workSheet.Cells[i, 4].Value.ToString(), out transactionDate))
                    {
                        string temp = transactionDate.ToString("yyyy-MM-dd");
                        DateTimeOffset.TryParse(temp, out tranDate);
                        tkbdViewModel.TransactionDate = tranDate;

                    }
                    decimal.TryParse(workSheet.Cells[i, 5].Value.ToString().Replace(",", ""), out money);
                    tkbdViewModel.Money = money;
                    decimal.TryParse(workSheet.Cells[i, 6].Value.ToString().Replace(",", ""), out rate);
                    tkbdViewModel.Rate = rate;
                    tkbdViewModel.UserId = _applicationUserService.getByUserName(User.Identity.Name).Id;
                    tkbdViewModel.CreatedBy = "Admin";                  
                    tkbdViewModel.Status = true;
                    tkbdHistory.UpdateTKBDHistory(tkbdViewModel);
                    listTKBD.Add(tkbdHistory);
                }
                return listTKBD;
            }
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
                        //TimeSpan s = DateTimeOffset.UtcNow.Subtract(item.TransactionDate);
                        //days = (int)s.TotalDays;
                        days = 6;
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