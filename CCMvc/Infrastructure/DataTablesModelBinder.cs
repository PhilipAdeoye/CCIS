using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCMvc.ViewModels;

namespace CCMvc.Infrastructure
{
    // More information can be found at
    //  Datatable serverside v1.10 API http://datatables.net/manual/server-side
    //  Stack Overflow answer for model binding Datatables v1.10 API http://stackoverflow.com/a/28223358/3669953

    /// <summary>
    /// Model Binder for DataTablesParameterModel
    /// </summary>
    public class DataTablesModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            base.BindModel(controllerContext, bindingContext);
            HttpRequestBase request = controllerContext.HttpContext.Request;
            // Retrieve request data
            int draw = Convert.ToInt32(request["draw"]);
            int start = Convert.ToInt32(request["start"]);
            int length = Convert.ToInt32(request["length"]);
            // Search
            DataTablesSearch search = new DataTablesSearch
            {
                Value = request["search[value]"],
                Regex = Convert.ToBoolean(request["search[regex]"])
            };
            // Order
            var o = 0;
            var order = new List<DataTablesOrder>();
            while (request["order[" + o + "][column]"] != null)
            {
                order.Add(new DataTablesOrder()
                {
                    Column = Convert.ToInt32(request["order[" + o + "][column]"]),
                    Dir = request["order[" + o + "][dir]"]
                });
                o++;
            }
            // Columns
            var c = 0;
            var columns = new List<DataTablesColumn>();
            while (request["columns[" + c + "][name]"] != null)
            {
                columns.Add(new DataTablesColumn
                {
                    Data = request["columns[" + c + "][data]"],
                    Name = request["columns[" + c + "][name]"],
                    Orderable = Convert.ToBoolean(request["columns[" + c + "][orderable]"]),
                    Search = new DataTablesSearch
                    {
                        Value = request["columns[" + c + "][search][value]"],
                        Regex = Convert.ToBoolean(request["columns[" + c + "][search][regex]"])
                    }
                });
                c++;
            }

            return new DataTablesParameterModel
            {
                Draw = draw,
                Start = start,
                Length = length,
                Search = search,
                Order = order,
                Columns = columns
            };
        }
    }
}