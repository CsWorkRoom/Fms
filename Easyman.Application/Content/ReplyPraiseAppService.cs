using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Easyman.App.Dto;
using Easyman.Authorization;
using Easyman.Authorization.Roles;
using Easyman.Base.Content.Dto;
using Easyman.Content.Dto;
using Easyman.Domain;
using Easyman.Users;
using EasyMan;
using EasyMan.Dtos;
using Newtonsoft.Json;
using Abp.Application.Services.Dto;

namespace Easyman.Content
{
    /// <summary>
    /// 评论和点赞管理
    /// </summary>
    public class ReplyPraiseAppService : EasymanAppServiceBase, IReplyPraiseAppService
    {
        #region 初始化
        private readonly IRepository<Define, long> _defineRepository;
        private readonly IRepository<DefineConfig, long> _defineConfigRepository;
        private readonly IRepository<ContentType, long> _contentTypeRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Domain.Content, long> _contentRepository;
        private readonly IRepository<ContentTag, long> _contentTagRepository;
        private readonly IRepository<ContentRefTag, long> _contentRefTagRepository;
       
        private readonly IRepository<Files, long> _filesRepository;
        private readonly IRepository<ContentReplyFile, long> _contentReplyFileRepository;

        private readonly IRepository<ContentReadLog, long> _contentReadLogRepository;
        private readonly IRepository<ContentPraiseLog, long> _contentPraiseLogRepository;
        private readonly IRepository<ContentReply, long> _contentReplyRepository;
        private readonly IRepository<ReplyPraiseLog, long> _replyPraiseLogRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defineRepository"></param>
        /// <param name="contentRepository"></param>
        /// <param name="defineConfigRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="contentTypeRepository"></param>
        /// <param name="contentTagRepository"></param>
        /// <param name="contentRefTagRepository"></param>
        /// <param name="fileRepository"></param>
        /// <param name="contentReplyFileRepository"></param>
        /// <param name="contentReadLogRepository"></param>
        /// <param name="contentPraiseLogRepository"></param>
        /// <param name="contentReplyRepository"></param>
        /// <param name="replyPraiseLogRepository"></param>
        public ReplyPraiseAppService(
            IRepository<Define, long> defineRepository, 
            IRepository<Domain.Content, long> contentRepository,
            IRepository<DefineConfig, long> defineConfigRepository,
            IRepository<User, long> userRepository,
            IRepository<ContentType, long> contentTypeRepository,
            IRepository<ContentTag, long> contentTagRepository, 
            IRepository<ContentRefTag, long> contentRefTagRepository,
            IRepository<Files, long> fileRepository,
            IRepository<ContentReplyFile, long> contentReplyFileRepository,
            IRepository<ContentReadLog, long> contentReadLogRepository,
            IRepository<ContentPraiseLog, long> contentPraiseLogRepository, 
            IRepository<ContentReply, long> contentReplyRepository, 
            IRepository<ReplyPraiseLog, long> replyPraiseLogRepository
            )
        {
            _defineRepository = defineRepository;
            _contentRepository = contentRepository;
            _defineConfigRepository = defineConfigRepository;
          
            _contentTagRepository = contentTagRepository;
            _contentRefTagRepository = contentRefTagRepository;
            _userRepository = userRepository;
            _contentTypeRepository = contentTypeRepository;
            _filesRepository = fileRepository;
            _contentReplyFileRepository = contentReplyFileRepository;

            _contentReadLogRepository = contentReadLogRepository;
            _contentPraiseLogRepository = contentPraiseLogRepository;
            _contentReplyRepository = contentReplyRepository;
            _replyPraiseLogRepository = replyPraiseLogRepository;

        }
        #endregion

        #region 内容功能管理
        /// <summary>
        /// 阅读日志
        /// </summary>
        /// <param name="contentId"></param>
        public void ContentReadLog(int contentId)
        {
            if (AbpSession.UserId != null)
            {
                ContentReadLog readLog = new Domain.ContentReadLog
                {
                    ContentId = contentId,
                    CreationTime = DateTime.Now,
                    UserId = (long)AbpSession.UserId
                };
                _contentReadLogRepository.Insert(readLog);
            }
        }
        /// <summary>
        /// 查询阅读次数
        /// </summary>
        /// <returns></returns>
        public int ContentReadContent()
        {
            return _contentReadLogRepository.GetAll().Count();
        }
        /// <summary>
        /// 根据内容ID查询评论
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public List<ReplyModel> GetContentReplyListId(int contentId)
        {
            //查询顶级数量
            var ReplyNumber = GetContentReply(contentId);
            foreach (var item in ReplyNumber)
            {
                if (item.ChildContentReply != null)
                    foreach (var tem in item.ChildContentReply)
                    {
                        if (tem.ParentId != null)
                        {
                            var reply = _contentReplyRepository.FirstOrDefault(a => a.Id == tem.ParentId);
                            tem.ParentName = reply.ReplyUser.Name;
                        }
                        tem.IsReolyOrLike = IsReplyOrLike(Convert.ToInt32(tem.Id));
                    }
            }
            if (ReplyNumber.Count > 12)
            {
                return ReplyNumber.Take(12).ToList();
            }
            else
            {
                return ReplyNumber;
            }
        }
         
        /// <summary>
        /// 根据内容ID查询评论
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public List<ReplyModel> GetContentReply(int contentId)
        {
            List<ReplyModel> result = new List<ReplyModel>();

            var replyAll = _contentReplyRepository.GetAll().Select(item => new ReplyModel()
            {
                Content = item.Content,
                ContentId = item.ContentId,
                CreationTime = item.CreationTime,
                DeleteReason = item.DeleteReason,
                DeleteTime = item.DeleteTime,
                DeleteUid = item.DeleteUid,
                DeleteUser = item.DeleteUser,
                Id = item.Id,
               
                Info = item.Info,
                IpAddr = item.IpAddr,
                IpRomise = item.IpRomise,
                IsDelete = item.IsDelete,
                ParentId = (long)item.ParentId,
                ReolyUId = item.ReolyUId,
                ReplyUser = item.ReplyUser
            }).OrderByDescending(x => x.CreationTime).ToList();
           
            var reply = replyAll.Where(a => a.ContentId == contentId && a.ParentId ==null).OrderByDescending(a => a.CreationTime).ToList();
            return GetChildList(reply, replyAll);
        }

        private List<ReplyModel> GetChildList(List<ReplyModel> re, List<ReplyModel> all)
        {
            return (from item in re
                    let temp = all.Where(a => a.ParentId == item.Id).ToList()
                    select new ReplyModel()
                    {
                        ChildContentReply = temp.Count > 0 ? GetChild(null, temp, all) : null,
                        Content = item.Content,
                        ContentId = item.ContentId,
                        CreationTime = item.CreationTime,
                        DeleteReason = item.DeleteReason,
                        DeleteTime = item.DeleteTime,
                        DeleteUid = item.DeleteUid,
                        DeleteUser = item.DeleteUser,
                        Id = item.Id,
                        IsFileNumber = _contentReplyFileRepository.GetAll().Count(x => x.ContentReplyId == item.Id),
                        Info = item.Info,
                        IpAddr = item.IpAddr,
                        IpRomise = item.IpRomise,
                        IsDelete = item.IsDelete,
                        ParentId = item.ParentId,
                        ReolyUId = item.ReolyUId,
                        ReplyUser = item.ReplyUser,
                        ReplyPraiseCount = _replyPraiseLogRepository.GetAll().Count(a => a.ContentReplyId == item.Id),
                        IsReolyOrLike = IsReplyOrLike(Convert.ToInt32(item.Id))
                    }).ToList();
        }

        public List<ReplyModel> GetChild(List<ReplyModel> list, List<ReplyModel> re, List<ReplyModel> all)
        {
            if (list == null)
                list = new List<ReplyModel>();
            foreach (var item in re)
            {
                ReplyModel rm = new ReplyModel()
                {
                    Content = item.Content,
                    ContentId = item.ContentId,
                    CreationTime = item.CreationTime,
                    DeleteReason = item.DeleteReason,
                    DeleteTime = item.DeleteTime,
                    DeleteUid = item.DeleteUid,
                    DeleteUser = item.DeleteUser,
                    Id = item.Id,
                    IsFileNumber = _contentReplyFileRepository.GetAll().Count(x=>x.ContentReplyId==item.Id),
                    Info = item.Info,
                    IpAddr = item.IpAddr,
                    IpRomise = item.IpRomise,
                    IsDelete = item.IsDelete,
                    ParentId = item.ParentId,
                    ReolyUId = item.ReolyUId,
                    ReplyUser = item.ReplyUser,
                    ReplyPraiseCount = _replyPraiseLogRepository.GetAll().Count(a => a.ContentReplyId == item.Id)
                };
                list.Add(rm);
                var childList = all.Where(a => a.ParentId == item.Id).ToList();
                if (childList.Count > 0)
                    list = GetChild(list, childList, all);
            }
            return list;
        }

        /// <summary>
        /// 新增评论
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="replyInfo"></param>
        /// <param name="fileIds">评论上传附件集合</param>
        /// <param name="replyId"></param>
        /// <returns></returns>
        public object CerateReply(int contentId, string replyInfo,string fileIds, int? replyId)
        {

            ContentReply reply = new ContentReply();
            reply.Info = replyInfo;
            reply.ContentId = contentId;
            reply.CreationTime = DateTime.Now;
            if (replyId != null)
            {
                reply.ParentId = (long)replyId;
            }
            else
            {
                reply.ParentId = null;
            }
            if (AbpSession.UserId != null)
            {
                reply.DeleteUid = (long)AbpSession.UserId;//因有约束条件FK_EM_CONTENT_REPLY_DELETE_UID 则删除人默认等于创建人
                reply.ReolyUId = (long)AbpSession.UserId;
                //reply.ReplyUser = _userRepository.Get(reply.ReolyUId);
            }
            var rId = _contentReplyRepository.InsertAndGetId(reply);
            //保存评论附件上传文件
            //上传附件ID集合
            var fileIdList = fileIds.Split(',');
            int fileNumber = 0;
            if (fileIdList.Count() > 0)
            {
                //保存上传附件和评论ID
                foreach (var item in fileIdList)
                {
                    if (item != "" && item != "删除")
                    {
                        var fileId = item.ToInt32();
                        if (fileId != 0)
                        {
                            ContentReplyFile rfiles = new ContentReplyFile
                            {
                                ContentReplyId = rId,
                                FileId = fileId
                            };
                            _contentReplyFileRepository.Insert(rfiles);
                            fileNumber++;
                        }
                    }
                }
            }
            var result = _contentReplyRepository.Get(rId);
            ReplyModel rm = new ReplyModel
            {
                Info = result.Info,
                ContentId = result.ContentId,
                Id = result.Id,
                CreationTime = result.CreationTime,
                ReplyUserName = _userRepository.Get(result.ReolyUId).Name,
                //返回上传附件数量,刚刚上传，查询不出来
                //IsFileNumber=_contentReplyFileRepository.GetAll().Count(x=>x.ContentReplyId== result.Id)
                IsFileNumber=fileNumber
            };
            
            if (result.ParentId != null)
                rm.ParentName = _contentReplyRepository.FirstOrDefault((long)result.ParentId).ReplyUser.Name;
            var content = _contentRepository.Get(contentId);
            var defineType = _contentTypeRepository.Get(content.DefineTypeId);
            var defineId = Convert.ToInt32(defineType.DefineId);
            //获取内容权限
            var defineConfig = _defineConfigRepository.GetAll().SingleOrDefault(a => a.DefineId == defineId);
            if (defineConfig != null) rm.IsReolyFloor = Convert.ToBoolean(defineConfig.IsReolyFloor);
            if (defineConfig != null) rm.IsLike = Convert.ToBoolean(defineConfig.IsLike);
            if (defineConfig != null) rm.IsReolyFile = Convert.ToBoolean(defineConfig.IsReolyFile);
            if (defineConfig != null) rm.IsReolyFloorFile = Convert.ToBoolean(defineConfig.IsReolyFloorFile);
            return rm;
        }

        public int ContentReadContent(int contentId)
        {
            return _contentReadLogRepository.GetAll().Count(a => a.ContentId == contentId);
        }

        public int ReplyContent(int contentId)
        {
            return _contentReplyRepository.GetAll().Count(a => a.ContentId == contentId);
        }

        public int ContentPraiseCount(int contentId)
        {
            return _contentPraiseLogRepository.GetAll().Count(a => a.ContentId == contentId);
        }

        public int ReplyPraiseCount(int replyId)
        {
            return _replyPraiseLogRepository.GetAll().Count(a => a.ContentReplyId == replyId);
        }

        /// <summary>
        /// 保存评论的上传附件，返回关联文件
        /// </summary>
        /// <param name="repid"></param>
        /// <param name="filesid"></param>
        public List<FileTemp> ContentReplyFiles(long repid,string filesid)
        {
            //返回文件集合
            List<FileTemp> listFileTemp = new List<FileTemp>();

            var listFile = filesid.Split(',');
            //var reapFile = _contentReplyFileRepository.GetAll().Where(a => a.ContentReplyId==repid);
           
            //删除原来的评论上传文件
            //foreach (var item in reapFile)
            //{
            //    _contentReplyFileRepository.Delete(item);
            //}
            //新增上传文件
            if (listFile.Count() > 0)
            {
                foreach (var item in listFile)
                {
                    if (item != "" && item != "删除")
                    {
                        var fileId = item.ToInt32();
                        if (fileId != 0)
                        {
                            ContentReplyFile rfiles = new ContentReplyFile
                            {
                                ContentReplyId = repid,
                                FileId= fileId
                            };
                            _contentReplyFileRepository.Insert(rfiles);
                            //根据文件ID查询出上传文件，并加入返回集合
                            var File_Id = _filesRepository.Get(fileId);
                            var filetp = new FileTemp();
                            filetp.Id = File_Id.Id;
                            filetp.Name = File_Id.TrueName;
                            filetp.Length = File_Id.Length;
                            if (File_Id.Length > 1024 * 1024)
                            {
                                filetp.LengthKb = ((double)(File_Id.Length * 100 / (1024 * 1024)) / 100) + "MB";
                            }
                            else
                            {
                                filetp.LengthKb = ((double)(File_Id.Length * 100 / 1024) / 100) + "KB";
                            }
                            filetp.Uptime = File_Id.UploadTime;
                            filetp.Upurl = File_Id.Url;

                            listFileTemp.Add(filetp);
                        }
                    }
                }
            }
            return listFileTemp;

        }

        /// <summary>
        /// 根据评论ID获取上传附件集合
        /// </summary>
        /// <param name="repid"></param>
        /// <returns></returns>
        public List<FileTemp> ReplyFilesList(long repid)
        {
            //返回文件集合
            List<FileTemp> listFileTemp = new List<FileTemp>();
            var replyFile = _contentReplyFileRepository.GetAll().Where(x => x.ContentReplyId == repid).ToList();
            List<long> fileList = new List<long>();
            if (replyFile.Count > 0)
            {
                for(int i = 0; i < replyFile.Count; i++)
                {
                    fileList.Add(replyFile[i].FileId);
                }
            }
            if (fileList.Count > 0)
            {
               foreach(var item in fileList)
                {
                    //根据文件ID查询出上传文件，并加入返回集合
                    var File_Id = _filesRepository.Get(item);
                    var filetp = new FileTemp();
                    filetp.Id = File_Id.Id;
                    filetp.Name = File_Id.TrueName;
                    filetp.Length = File_Id.Length;
                    if (File_Id.Length > 1024 * 1024)
                    {
                        filetp.LengthKb = ((double)(File_Id.Length * 100 / (1024 * 1024)) / 100) + "MB";
                    }
                    else
                    {
                        filetp.LengthKb = ((double)(File_Id.Length * 100 / 1024) / 100) + "KB";
                    }
                    filetp.Uptime = File_Id.UploadTime;
                    filetp.Upurl = File_Id.Url;

                    listFileTemp.Add(filetp);
                }
            }
           

            return listFileTemp;

        }

        /// <summary>
        /// 根据评论ID，查询关联上传附件
        /// </summary>
        /// <param name="repid"></param>
        public List<FileTemp> ContentReplyFileList(string repid)
        {

            //新建一列，把评论ID保存。
            //返回文件集合
            List<FileTemp> listFileTemp = new List<FileTemp>();

            var listRealy = repid.Split(',');

          
            if (listRealy.Count() > 0)
            {
                foreach (var item in listRealy)
                {
                    //根据评论ID查询该评论上传文件
                    var itemId = item.ToInt32();
                    var ListFileId = _contentReplyFileRepository.GetAll().Where(x => x.ContentReplyId == itemId).ToList();
                    for (int i = 0; i < ListFileId.Count; i++)
                    {
                        //根据文件ID查询出上传文件，并加入返回集合
                        var File_Id = _filesRepository.Get(ListFileId[i].FileId);
                        var filetp = new FileTemp();
                        filetp.Id = File_Id.Id;
                        filetp.Name = File_Id.TrueName;
                        filetp.Length = File_Id.Length;
                        if (File_Id.Length > 1024 * 1024)
                        {
                            filetp.LengthKb = ((double)(File_Id.Length * 100 / (1024 * 1024)) / 100) + "MB";
                        }
                        else
                        {
                            filetp.LengthKb = ((double)(File_Id.Length * 100 / 1024) / 100) + "KB";
                        }
                        filetp.Uptime = File_Id.UploadTime;
                        filetp.Upurl = File_Id.Url;
                        filetp.RealyId = itemId;
                        listFileTemp.Add(filetp);
                    }
                }
            }
            return listFileTemp;

        }

        /// <summary>
        /// 内容点赞
        /// </summary>
        /// <param name="contentId"></param>
        public int ContentPraise(int contentId)
        {
            var num = -1;
            int userId = 0;
            if (AbpSession.UserId != null)
            {
                userId = (int)AbpSession.UserId;
                var praiseLog = _contentPraiseLogRepository.GetAll().Where(a => a.ContentId == contentId && a.UserId == userId);
                if (praiseLog.Count() == 1)
                {
                    foreach (var item in praiseLog)
                    {
                        _contentPraiseLogRepository.Delete(item.Id);
                    }
                    num = _contentPraiseLogRepository.GetAllList(a => a.ContentId == contentId).Count();
                }
                else
                {
                    ContentPraiseLog cpl = new ContentPraiseLog
                    {
                        ContentId = contentId,
                        CreationTime = DateTime.Now,
                        UserId = userId
                    };
                    var model = _contentPraiseLogRepository.Insert(cpl);
                    if (model != null) num = _contentPraiseLogRepository.GetAllList(a => a.ContentId == contentId).Count();
                }
            }
            return num;//_contentPraiseLogRepository.GetAllList(a => a.ContentId == contentId).Count();
        }

        /// <summary>
        /// 评论点赞
        /// </summary>
        /// <param name="replyId"></param>
        public void ReplyPraise(int replyId)
        {
            int userId = 0;
            if (AbpSession.UserId != null)
            {
                userId = (int)AbpSession.UserId;
                var praiseLog = _replyPraiseLogRepository.GetAll().Where(a => a.ContentReplyId == replyId && a.UserId == userId);
                if (praiseLog.Count() == 1)
                {
                    foreach (var item in praiseLog)
                    {
                        _replyPraiseLogRepository.Delete(item.Id);
                    }
                }
                else
                {
                    ReplyPraiseLog rpl = new ReplyPraiseLog
                    {
                        ContentReplyId = replyId,
                        CreationTime = DateTime.Now,
                        UserId = userId

                    };
                    _replyPraiseLogRepository.Insert(rpl);
                }
            }
        }

      

        /// <summary>
        /// 内容是否点赞
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public bool IsOrLike(int contentId)
        {
            var result = _contentPraiseLogRepository.GetAll().Where(a => a.ContentId == contentId && a.UserId == AbpSession.UserId);
            if (result.Any())
                return true;
            else
                return false;
        }

        /// <summary>
        /// 评论是否点赞
        /// </summary>
        /// <param name="replyId"></param>
        /// <returns></returns>
        public bool IsReplyOrLike(int replyId)
        {

            var result = _replyPraiseLogRepository.GetAll().Where(a => a.ContentReplyId == replyId && a.UserId == AbpSession.UserId);
            if (result.Any())
                return true;
            else
                return false;
        }

       
        #endregion

    }
}
