using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TouresCommon;
using TouresCommon.Middleware;
using TouresRepository;
using TouresRestOrder.Model;

namespace TouresRestOrder.Service
{
    public class ReportService
    {
        private string connString = "";
        public ReportService(string ConnectionString)
        {
            connString = ConnectionString;
        }

        public async Task<ResponseBase<List<ReportOrdenModel>>> GetReportOrders(int tipobusqueda, string fecha1, string fecha2)
        {
            var response = new ResponseBase<List<ReportOrdenModel>>();

            if (tipobusqueda > 0)
            {
                IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "C_DATASET");
                var order = new ReportOrdenModel();
                var lOrder = new List<ReportOrdenModel>();

                repository.Parameters.Add("P_TIPO_INFORME", OracleDbType.Int32).Value = tipobusqueda;
                repository.Parameters.Add("P_FECHA1", OracleDbType.Date).Value = DateTime.Parse(fecha1);
                repository.Parameters.Add("P_FECHA2", OracleDbType.Date).Value = DateTime.Parse(fecha2);
                repository.Parameters.Add("C_DATASET", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                var result = repository.Get("PKG_B2C_REPORT.B2C_ORDERS_SELECT");
                if (repository.Status.Code == Status.Ok)
                {
                    foreach (var item in result)
                    {
                        order = new ReportOrdenModel();
                        order.ordid = long.Parse(item["ORDID"].ToString());
                        order.comments = item["COMMENTS"].ToString();
                        order.fname = item["FNAME"].ToString();
                        order.ordendate = DateTime.Parse(item["ORDENDATE"].ToString());
                        order.nombre_estado = item["NOMBRE_ESTADO"].ToString();
                        order.price =  double.Parse(item["PRICE"].ToString());
                        lOrder.Add(order);
                    }

                    response.Data = lOrder;
                }
                else
                {
                    response.Message = repository.Status.Message;
                }
                response.Code = repository.Status.Code;
            }
            else
            {
                response.Code = Status.InvalidData;
                response.Message = "The field CustId is zero(0)";
            }

            return await Task.Run(() => response);
        }

        public async Task<ResponseBase<List<ReportClienteModel>>> GetReportClientes(int tipobusqueda, string fecha1, string fecha2)
        {
            var response = new ResponseBase<List<ReportClienteModel>>();

            if (tipobusqueda > 0)
            {
                IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "C_DATASET");
                var order = new ReportClienteModel();
                var lOrder = new List<ReportClienteModel>();

                repository.Parameters.Add("P_TIPO_INFORME", OracleDbType.Int32).Value = tipobusqueda;
                repository.Parameters.Add("P_FECHA1", OracleDbType.Date).Value = DateTime.Parse(fecha1);
                repository.Parameters.Add("P_FECHA2", OracleDbType.Date).Value = DateTime.Parse(fecha2);
                repository.Parameters.Add("C_DATASET", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                var result = repository.Get("PKG_B2C_REPORT.B2C_ORDERS_SELECT");
                if (repository.Status.Code == Status.Ok)
                {
                    foreach (var item in result)
                    {
                        order = new ReportClienteModel();
                        order.fname = item["FNAME"].ToString();
                        order.Total = double.Parse(item["TOTAL"].ToString());
                        order.custid = int.Parse(item["CUSTID"].ToString());
                        lOrder.Add(order);
                    }

                    response.Data = lOrder;
                }
                else
                {
                    response.Message = repository.Status.Message;
                }
                response.Code = repository.Status.Code;
            }
            else
            {
                response.Code = Status.InvalidData;
                response.Message = "The field CustId is zero(0)";
            }

            return await Task.Run(() => response);
        }

        public async Task<ResponseBase<List<ReportOrdenModel>>> GetReportRankingClientes(int cusid, string fecha1, string fecha2)
        {
            var response = new ResponseBase<List<ReportOrdenModel>>();

            if (cusid > 0)
            {
                IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "C_DATASET");
                var order = new ReportOrdenModel();
                var lOrder = new List<ReportOrdenModel>();

                repository.Parameters.Add("P_CUSID", OracleDbType.Int32).Value = cusid;
                repository.Parameters.Add("P_FECHA1", OracleDbType.Date).Value = DateTime.Parse(fecha1);
                repository.Parameters.Add("P_FECHA2", OracleDbType.Date).Value = DateTime.Parse(fecha2);
                repository.Parameters.Add("C_DATASET", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                var result = repository.Get("PKG_B2C_REPORT.B2C_CLIENTE_RANKING_SELECT");
                if (repository.Status.Code == Status.Ok)
                {
                    foreach (var item in result)
                    {
                        order = new ReportOrdenModel();
                        order.ordid = long.Parse(item["ORDID"].ToString());
                        order.comments = item["COMMENTS"].ToString();
                        order.fname = item["FNAME"].ToString();
                        order.ordendate = DateTime.Parse(item["ORDENDATE"].ToString());
                        order.nombre_estado = item["NOMBRE_ESTADO"].ToString();
                        order.price = double.Parse(item["PRICE"].ToString());
                        lOrder.Add(order);
                    }

                    response.Data = lOrder;
                }
                else
                {
                    response.Message = repository.Status.Message;
                }
                response.Code = repository.Status.Code;
            }
            else
            {
                response.Code = Status.InvalidData;
                response.Message = "The field CustId is zero(0)";
            }

            return await Task.Run(() => response);
        }

        public async Task<ResponseBase<List<ReportProductModel>>> GetReportProducto(int tipobusqueda, string fecha1, string fecha2)
        {
            var response = new ResponseBase<List<ReportProductModel>>();

            if (tipobusqueda > 0)
            {
                IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "C_DATASET");
                var order = new ReportProductModel();
                var lOrder = new List<ReportProductModel>();

                repository.Parameters.Add("P_TIPO_INFORME", OracleDbType.Int32).Value = tipobusqueda;
                repository.Parameters.Add("P_FECHA1", OracleDbType.Date).Value = DateTime.Parse(fecha1);
                repository.Parameters.Add("P_FECHA2", OracleDbType.Date).Value = DateTime.Parse(fecha2);
                repository.Parameters.Add("C_DATASET", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                var result = repository.Get("PKG_B2C_REPORT.B2C_ORDERS_SELECT");
                if (repository.Status.Code == Status.Ok)
                {
                    foreach (var item in result)
                    {
                        order = new ReportProductModel();
                        order.productname = item["PRODUCTNAME"].ToString();
                        order.Cantidad = int.Parse(item["CANTIDAD"].ToString());
                        order.proid = int.Parse(item["PRODID"].ToString());
                        order.valor = double.Parse(item["VALOR"].ToString());
                        lOrder.Add(order);
                    }

                    response.Data = lOrder;
                }
                else
                {
                    response.Message = repository.Status.Message;
                }
                response.Code = repository.Status.Code;
            }
            else
            {
                response.Code = Status.InvalidData;
                response.Message = "The field CustId is zero(0)";
            }

            return await Task.Run(() => response);
        }

        public async Task<ResponseBase<List<ReportOrderMonth>>> GetReportOrderMonth(int tipobusqueda)
        {
            var response = new ResponseBase<List<ReportOrderMonth>>();

            if (tipobusqueda > 0)
            {
                IRepository<OracleParameterCollection> repository = new OracleRepository(connString, "C_DATASET");
                var order = new ReportOrderMonth();
                var lOrder = new List<ReportOrderMonth>();

                repository.Parameters.Add("P_TIPO_INFORME", OracleDbType.Int32).Value = tipobusqueda;
                repository.Parameters.Add("P_FECHA1", OracleDbType.Date).Value = DateTime.Now;
                repository.Parameters.Add("P_FECHA2", OracleDbType.Date).Value = DateTime.Now;
                repository.Parameters.Add("C_DATASET", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                var result = repository.Get("PKG_B2C_REPORT.B2C_ORDERS_SELECT");
                if (repository.Status.Code == Status.Ok)
                {
                    foreach (var item in result)
                    {
                        order = new ReportOrderMonth();
                        order.month = item["PRODUCTNAME"].ToString();
                        order.cantidad = int.Parse(item["CANTIDAD"].ToString());
                        order.valor = double.Parse(item["VALOR"].ToString());
                        lOrder.Add(order);
                    }

                    response.Data = lOrder;
                }
                else
                {
                    response.Message = repository.Status.Message;
                }
                response.Code = repository.Status.Code;
            }
            else
            {
                response.Code = Status.InvalidData;
                response.Message = "The field CustId is zero(0)";
            }

            return await Task.Run(() => response);
        }
    }
}
