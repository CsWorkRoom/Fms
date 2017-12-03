using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Easyman.Common;
using Easyman.Domain;
using Easyman.Dto;
using EasyMan;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Easyman.Import
{
    /// <summary>
    /// 外导表管理
    /// </summary>
    public class ImpTbAppService : ApplicationService, IImpTbAppService
    {
        #region 初始化

        private readonly IRepository<ImpTb, long> _impTbRepository;
        private readonly IRepository<DbServer, long> _dbServerRepository;
        private readonly IRepository<DefaultField, long> _defaultFieldRepository;
        private readonly IRepository<ImpType, long> _impTypeRepository;
        private readonly IRepository<DbType, long> _dbTypeRepository;

        public ImpTbAppService(IRepository<ImpTb, long> impTbRepository, IRepository<DbServer, long> dbServerRepository, 
            IRepository<DefaultField,long> defaultFieldRepository, IRepository<ImpType, long> impTypeRepository,
            IRepository<DbType, long> dbTypeRepository)
        {
            _impTbRepository = impTbRepository;
            _dbServerRepository = dbServerRepository;
            _defaultFieldRepository = defaultFieldRepository;
            _impTypeRepository = impTypeRepository;
            _dbTypeRepository = dbTypeRepository;
        }

        #endregion

        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="input">输入的实体</param>
        /// <returns></returns>
        public ImpTbSearchOutput GetAll(ImpTbSearchInput input)
        {
            int reordCount;
            var dataList = _impTbRepository.GetAll() 
              .SearchByInputDto(input, out reordCount)
              .Select(a => new ImpTbOutput()
              {
                  Id = a.Id,
                  Code = a.Code,
                  CnTableName = a.CnTableName,
                  EnTableName = a.EnTableName,
                  Rule = a.Rule == "1" ? "单独创建跟随时间戳" : "沿用外导表名创建表",
                  DbServerName = a.DbServer.ByName,
                  ImpTypeName = a.ImpType.Name
              }).ToList();
            input.Page.TotalCount = reordCount;
            return new ImpTbSearchOutput() { Datas = dataList, Page = input.Page};
        }
        /// <summary>
        /// 新增或者修改
        /// </summary>
        /// <param name="input">输入的实体</param>
        public void AddOrUpdate(ImpTbInput input)
        {
            var dbServer = _dbServerRepository.Get(input.DbServerId);
            var impType = _impTypeRepository.Get(input.ImpTypeId);
            var list = _impTbRepository.GetAllList(a => a.Id != input.Id);
            foreach(var item in list)
            {
                if(item.Code == input.Code) "编码已存在，请重新输入新的编码！".ErrorMsg();
                if(item.CnTableName.ToLower() == input.CnTableName.ToLower()) "表名称已存在，请重新输入新的表名称！".ErrorMsg();
            }
            var data = input.MapTo<ImpTb>();
            if(input.Id > 0)
            {
                var item = _impTbRepository.Get(input.Id);
                data = input.MapTo<ImpTbInput, ImpTb>(item);
            }
            if(data.DefaultField != null)
            {
                data.DefaultField.Clear();
                if (!string.IsNullOrEmpty(input.DefaultFields))
                {
                    foreach (var idStr in input.DefaultFields.Split(','))
                    {
                        if (idStr == "")
                            continue;
                        var id = long.Parse(idStr);
                        var defaultField = _defaultFieldRepository.Get(id);
                        data.DefaultField.Add(defaultField);
                    }
                }
            }
            data.DbServer = dbServer;
            data.ImpType = impType; 
            _impTbRepository.InsertOrUpdate(data);
            if(input.Id > 0)
            {
                CreateSqlScript(input.Id);
            }
        }
        /// <summary>
        /// 获取下拉列表
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetDropDownList()
        {
            var dataList = _impTbRepository.GetAll()
                .Select(x => new SelectListItem() { Text = x.CnTableName, Value = x.Id.ToString() })
                .ToList();
            return dataList;
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id">实体Id</param>
        /// <returns></returns>
        public ImpTbInput Get(long id)
        {
            var item = _impTbRepository.Get(id);
            var data = item.MapTo<ImpTbInput>();
            return data;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">实体</param>
        public void Del(EntityDto<long> input)
        {
            _impTbRepository.Delete(a => a.Id == input.Id);
        }
        /// <summary>
        /// 获取内置字段id串
        /// </summary>
        /// <param name="id">外导表id</param>
        /// <returns></returns>
        public string GetIds(long id)
        {
            var data = _impTbRepository.Get(id);
            var idList = data.DefaultField.Select(a => a.Id).ToList();
            return string.Join(",", idList);
        }
        /// <summary>
        /// 获取外导表执行库所属数据库类型
        /// </summary>
        /// <param name="id">外导表Id</param>
        /// <returns></returns>
        public long GetDbTypeId(long id)
        {
            try
            {
                var impTb = _impTbRepository.Get(id);
                return impTb.DbServer.DbType.Id;
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 保存配置信息Sql
        /// </summary>
        /// <param name="id">外导表Id</param>
        public bool CreateSqlScript(long id)
        {
            var fieldStr = string.Empty;
            var sqlScript = "  CREATE TABLE {0} ( {1} )";
            var impTb = _impTbRepository.Get(id);
            var impTbFieldList = impTb.ImpTbField.ToList();

            foreach(var item in impTbFieldList)
            {
                var str = string.Format(" {0} {1} ", item.FieldCode, item.DataType);
                fieldStr += string.IsNullOrEmpty(fieldStr)
                    ? str
                    : string.Format(",{0}", str);
            }

            if (string.IsNullOrEmpty(fieldStr)) return false;

            var defaultFieldList = impTb.DefaultField.ToList();
            foreach (var item in defaultFieldList)
            {
                fieldStr += string.Format(" ,{0} {1} ", item.FieldCode, item.DataType);
            }

            //默认在表中添加固定字段用做数据是否完整区分
            var dataType = string.Empty;
            string intDataType = "int";
            var dbType = impTb.DbServer.DbType.Name.ToLower().Trim();
            switch (dbType)
            {
                case "oracle":
                    dataType = "NCLOB";
                    intDataType = "NUMBER";
                    break;
                case "sqlserver":
                    dataType = "TEXT";
                    break;
                case "mysql":
                    dataType = "LONGTEXT";
                    break;
                case "db2":
                    dataType = "CLOB";
                    break;
            }
            fieldStr += string.Format(" ,{0} {1} ", "IMP_DATA_COMPLETE_DEFAULT", "VARCHAR(50)");
            fieldStr += string.Format(" ,{0} {1} ", "IMP_ERROR_MSG_DEFAULT", dataType);
            fieldStr += string.Format(" ,{0} {1} ", "createUser", intDataType);
            fieldStr += string.Format(" ,{0} {1} ", "createImport", "varchar(1000)");
            fieldStr += string.Format(" ,{0} {1} ", "createDate", "date");
            impTb.Sql = string.Format(sqlScript, "{0}", fieldStr);
            try
            {
                _impTbRepository.Update(impTb);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 模板下载
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte[] GetbytesByCode(string code, out string name)
        {
            var impTb = _impTbRepository.GetAllList(a => a.Code == code).FirstOrDefault();
            name = impTb.CnTableName;
            var impTbField = impTb.ImpTbField;
            var dic = new Dictionary<string, string>();
            if (impTbField.Count > 0)
            {
                foreach(var item in impTbField)
                {
                    dic.Add(item.FieldCode, item.FieldName);
                }
            }

            var sheet = new List<MultiSheet>();
            sheet.Add(new MultiSheet { SheetName = "ss", Data = null, Description = null, TopTitle = null, DicTitle = dic });
            var ss = ExcelHelper.CreateExcel(sheet, "ssss", name);
            return ss.GetBuffer();
        }
    }
}
