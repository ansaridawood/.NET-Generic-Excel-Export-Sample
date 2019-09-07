using GenericExcelExport.ExcelExport;
using GenericExcelExport.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace GenericExcelExport.Controllers
{
    public class ValuesController : ApiController
    {
        private IExcelExport _excelExport { get; set; }

        public ValuesController()
        {
            _excelExport = new GenerateExcel();
        }

        // GET api/values/excelExport
        [HttpGet]
        public HttpResponseMessage ExcelExport()
        {
            var ObjectToExcel = new List<DummyExternalLoginViewModel>
            {
                new DummyExternalLoginViewModel { Name = "Mohammed", FamilyName= "Ansari", State = "CA"},
                new DummyExternalLoginViewModel { Name = "Harvey", FamilyName= "Spectre", State = "NY"},
                new DummyExternalLoginViewModel { Name = "Mike", FamilyName= "Ross", State = "NY"},
                new DummyExternalLoginViewModel { Name = "Donald", FamilyName= "Trump", State = "AL"},
                new DummyExternalLoginViewModel { Name = "Spencer", FamilyName= "Mike", State = "AK"},
                new DummyExternalLoginViewModel { Name = "Trump", FamilyName= "Donald", State = "AZ"},
                new DummyExternalLoginViewModel { Name = "Bill", FamilyName= "Gates", State = "AR"}
            };


            var resultContent = _excelExport.Export(ObjectToExcel, "ExcelExport", true);

            return resultContent;
        }
    }
}
