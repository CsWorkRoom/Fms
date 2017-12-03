using Abp.Domain.Repositories;
using Easyman.Common;
using Easyman.Common.Helper;
using Easyman.Domain;
using Easyman.Service;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Easyman.Web.Rdlc
{
    public partial class rdlc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 以内存流形式返回rdlc报表配置信息
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns></returns>
        public MemoryStream GenerateRdlc(string inStr)
        {
            byte[] b = Encoding.UTF8.GetBytes(inStr);
            MemoryStream ms = new MemoryStream(b);
            return ms;
        }
        protected void SearchBtn_Click(object sender, EventArgs e)
        {
            string code = this.code.Value;
            string queryParams = this.queryParams.Value;
            string xmlStr = EncryptHelper.AesDecrpt(this.xmlStr.Value);
            string rpName = this.rpName.Value;
            DataTable dt = new DataTable();
            EasyMan.Dtos.ErrorInfo err = new EasyMan.Dtos.ErrorInfo();
            //从ioc容器中获取当前需要的接口实例
            var _reportApp = Abp.Dependency.IocManager.Instance.Resolve<IReportAppService>();

            #region 避免调用接口方法嵌套了多个复杂类型的方法
            //string sql = _reportApp.GetSqlByCode(code, queryParams);
            //var _dbServerApp = Abp.Dependency.IocManager.Instance.Resolve<IDbServerAppService>();
            //dt= _dbServerApp.ExecuteGetTable(1, sql);
            #endregion

            //GetDataTableFromCode方法中调用_dbServerApp.ExecuteGetTable(dbserver.Id, sql),不能传复杂类型dbserver
            dt = _reportApp.GetDataTableFromCode(code, queryParams, ref err);

            reportViewer1.LocalReport.DataSources.Clear();//清理原rdlc数据
            reportViewer1.LocalReport.DisplayName = rpName;
            reportViewer1.LocalReport.LoadReportDefinition(GenerateRdlc(xmlStr));
            ReportDataSource reportDataSource = new ReportDataSource("DataSet1",dt);
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);//赋值新数据
            reportViewer1.LocalReport.Refresh();
            
        }
    }
}