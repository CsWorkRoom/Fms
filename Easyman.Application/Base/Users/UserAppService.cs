using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Easyman.Authorization;
using Easyman.Users.Dto;
using Microsoft.AspNet.Identity;
using Easyman.Dto;
using EasyMan.Export;
using EasyMan;
using Abp.UI;
using System;
using System.Linq;
using Abp.Domain.Uow;
using EasyMan.Common.Data;
using EasyMan.Dtos;
using Easyman.Common;
using Easyman.App.Dto;
using System.Text.RegularExpressions;

namespace Easyman.Users
{
    /* THIS IS JUST A SAMPLE. */
    //[AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserAppService : EasymanAppServiceBase, IUserAppService
    {

        #region 初始化

        private readonly IRepository<User, long> _userRepository;
        private readonly IPermissionManager _permissionManager;
        private readonly UserManager _userManager;
        private readonly UserStore _userStore;
        private readonly IExportProvider _exportProvider;

        public UserAppService(UserManager userManager, UserStore userStore, IExportProvider exportProvider, IRepository<User, long> userRepository, IPermissionManager permissionManager)
        {
            _userRepository = userRepository;
            _permissionManager = permissionManager;
            _userManager = userManager;
            _userStore = userStore;
            _exportProvider = exportProvider;
        }

        #endregion

        #region 公有方法

        public async Task ProhibitPermission(ProhibitPermissionInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            var permission = _permissionManager.GetPermission(input.PermissionName);

            await UserManager.ProhibitPermissionAsync(user, permission);
        }

        //Example for primitive method parameters.
        public async Task RemoveFromRole(long userId, string roleName)
        {
            CheckErrors(await UserManager.RemoveFromRoleAsync(userId, roleName));
        }

        public async Task<ListResultDto<UserListDto>> GetUsers()
        {
            var users = await _userRepository.GetAllListAsync();

            return new ListResultDto<UserListDto>(
                users.MapTo<List<UserListDto>>()
                );
        }

        public async Task CreateUser(CreateUserInput input)
        {
            var user = input.MapTo<User>();

            user.TenantId = AbpSession.TenantId;
            user.Password = new PasswordHasher().HashPassword(input.Password);
            user.IsEmailConfirmed = true;

            CheckErrors(await UserManager.CreateAsync(user));
        }

        public void DeleteUser(EntityDto<long> input)
        {
            _userRepository.Delete(x => x.Id == input.Id);
        }

        public List<User> GetAllUser()
        {
            return _userRepository.GetAll().ToList();
        }

        public User AddUser(User user)
        {
            return _userRepository.Insert(user);
        }

        public UserSearchOutput Search(UserSearchInput input)
        {
            //throw new UserFriendlyException("test");
            int rowCount;
            var data = _userRepository.GetAll().SearchByInputDto(input, out rowCount);
            var output = new UserSearchOutput
            {
                Datas = data.ToList().Select(s => {
                    var temp = s.MapTo<UserOutput>();
                    if (s.IsDeleted)
                        temp.State = "注销";
                    else
                        temp.State = s.IsActive ? "正常" : "锁定";
                    return temp;
                }).ToList(),
                Page = new Pager(input.Page) { TotalCount = rowCount }
            };
            return output;
        }

        public User GetUser(long id)
        {
            return _userManager.FindByIdAsync(id).Result;
        }

        public void Delete(EntityDto<string> input)
        {
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    var ids = Array.ConvertAll<string, long>(input.Id.Split(','), delegate (string s) { return long.Parse(s); });
                    _userRepository.Delete(x => ids.Any(i => i == x.Id));
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public void UpdateOrInserUser(UserInput input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                
                if (input.Id == 0)
                {
                    //新增，查询所有同工号用户
                    var userCount = _userRepository.GetAll().Count(a => a.UserName == input.UserName);
                    if (userCount >= 1)
                    {
                        throw new UserFriendlyException("工号已存在");
                    }
                }else
               {
                    //修改，查询除自己外所有工号用户
                    var userCounts = _userRepository.GetAll().Where(a => a.Id != input.Id).Count(b => b.UserName == input.UserName);
                    if (userCounts >= 1)
                    {
                        throw new UserFriendlyException("工号已存在");
                    }
                }
                
                var user = GetUser(input.Id) ?? new User();

                user.Name = input.Name;
                user.UserName = input.UserName;
                user.Surname = input.UserName;
               // user.EmailAddress = input.EmailAddress;
                user.PhoneNo = input.PhoneNo;
                user.IsActive = input.IsActive;
                user.IsDeleted = input.IsDeleted;
                user.DistrictId = input.DistrictId;
                user.DepartmentId = input.DepartmentId;

                #region 20180111由cs添加
                user.IsLockoutEnabled = false;//不启用自动锁定（如果为true，密码输入4次错误会启用锁定）
                if(input.IsActive)
                {
                    user.LoginFailCount = 0;//如果启用用户，记录的登录失败的次数改为0
                }
                #endregion

                if (input.EmailAddress == "")
                {
                    user.EmailAddress = "{0}@139.com".FormatWith(input.UserName);
                }
                else
                {
                    user.EmailAddress = input.EmailAddress;
                }
                user.TenantId = input.TenantId;

                if (input.Id == 0)
                {
                    _userStore.CreateUserAsync(user);
                    CurrentUnitOfWork.SaveChanges();
                    user = _userStore.Query.FirstOrDefault(f => f.UserName == input.UserName);
                }
                else
                    _userStore.UpdateUserAsync(user);

                _userManager.SetRoles(user, input.RoleNames.HasValue() ? input.RoleNames.Split(',') : new string[] { });
            }
            
        }

        public void Unlock(EntityDto<long> inout)
        {
            try
            {
                var user = GetUser(inout.Id);
                if (user != null)
                {
                    if (user.IsDeleted == true || !user.IsActive)
                    {
                        user.IsDeleted = false;
                        user.IsActive = true;
                        _userManager.UpdateAsync(user);
                    }
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public void Lock(EntityDto<long> inout)
        {
            try
            {
                var user = GetUser(inout.Id);
                if (user != null)
                {
                    if (user.IsDeleted || !user.IsActive)
                    {
                        throw new UserFriendlyException("工号状态异常，无法锁定");
                    }
                    user.IsActive = false;
                    _userManager.UpdateAsync(user);
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        public void Cannel(EntityDto<long> inout)
        {
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    var user = GetUser(inout.Id);

                    if (user != null)
                    {
                        if (user.IsDeleted || user.IsActive == false)
                        {
                            throw new UserFriendlyException("工号状态不正常，不能注销");
                        }
                        _userManager.DeleteAsync(user);
                    }
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }


        /// <summary>
        /// 重置密码功能
        /// </summary>
        /// <param name="id">用户ID</param>
        public string ResetPwd(int id)
        {
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    var user = GetUser(id);

                    if (user != null)
                    {
                        //判断是否启用随机密码
                        //获取配置节点信息
                        var userSe = OperateSection.GetPwdRuleSection();
                        if (userSe.IsRandomPwd)
                        {
                            
                            //密码复杂度验证
                            if (userSe.IsValidatecComplex)
                            {
                                //重置密码，返回通过验证随机密码
                                var pwdNow = RandomPasswords();//生成通过验证随机密码
                                user.Password = new PasswordHasher().HashPassword(pwdNow);
                                user.ModifyPwd = 0;
                                 _userManager.UpdateAsync(user);
                                //throw new UserFriendlyException(pwdNow);
                                return pwdNow;
                            }
                            else
                            {
                                //重置密码，返回随机密码
                                var pwdNow = RandomPassword();//生成随机密码
                                user.Password = new PasswordHasher().HashPassword(pwdNow);
                                user.ModifyPwd = 0;
                                 _userManager.UpdateAsync(user);
                                //throw new UserFriendlyException(pwdNow);
                                return pwdNow;
                                //abp.message.info("重置密码成功！新密码："+pwdNow, "提示");

                            }
                        }
                        else
                        {
                            //重置密码，返回默认密码
                            var pwdNow = userSe.DefualtPwd;//获取默认密码
                            user.Password = new PasswordHasher().HashPassword(pwdNow);
                            user.ModifyPwd = 0;
                            _userManager.UpdateAsync(user);
                            //throw new UserFriendlyException(pwdNow);
                            return pwdNow;
                        }

                    }
                    else
                    {
                        throw new UserFriendlyException("查询出错，用户不存在！");
                    }
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        

        /// <summary>
        /// 简单生成12位数，生成随机密码
        /// </summary>
        /// <returns></returns>
        public string RandomPassword()
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%&*.?_";
            Random randrom = new Random((int)DateTime.Now.Ticks);
            string str = "";
            for (int i = 0; i < 12; i++)
            {
                str += chars[randrom.Next(chars.Length)];
            }
            return str;

        }
        /// <summary>
        /// 生成通过，密码复杂度验证的随机密码
        /// </summary>
        /// <returns></returns>
        private string RandomPasswords()
        {
            var newPwd = getRandomizer(12);
            
            while (!RandomBoolPwd(newPwd))
            {
                newPwd = getRandomizer(12);
            }
            return newPwd;
        }
        /// <summary>
        /// 密码复杂度验证
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private bool RandomBoolPwd(string pwd)
        {
            string result = "";

            //进行密码复杂度的校验
            var pwdRule = OperateSection.GetPwdRuleSection();
            if (pwdRule.IsValidatecComplex)//是否启用复杂度校验
            {
                var complexList = OperateSection.GetPwdComplexSetList();
                if (complexList != null && complexList.Count > 0)
                {
                    foreach (var complex in complexList)
                    {
                        if (Regex.IsMatch(pwd, complex.Regular))
                        {
                            result += complex.ErrorMsg + "\r\n";
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(result) && result.Length > 0)
            {
                //密码复杂度不通过验证
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 生成指定长度包含数字字符大小写字母的复杂随机密码
        /// </summary>
        /// <param name="intLength"></param>
        /// <returns></returns>
        public string getRandomizer(int intLength)
        {
            //定义
            Random ranA = new Random();
            int intResultRound = 0;
            int intA = 0;
            string strB = "";
            
             while (intResultRound<intLength)
             {
                //生成随机数A，表示生成类型
                 //1=数字，2=符号，3=小写字母，4=大写字母
                 intA = ranA.Next(1, 5);
                //如果随机数A=1，则运行生成数字
                //生成随机数A，范围在0-10
                //把随机数A，转成字符
                 //生成完，位数+1，字符串累加，结束本次循环
                if (intA == 1)
                {
                    intA = ranA.Next(0, 10);
                    strB = intA.ToString() + strB;
                     intResultRound = intResultRound + 1;
                     continue;
                 }
                 //如果随机数A=2，则运行生成符号
                 //生成随机数A，表示生成值域
                //1：33-47值域，2：58-64值域，3：91-96值域，4：123-126值域
                if (intA == 2)
                 {
                     intA = ranA.Next(1, 5);
                    //如果A=1
                     //生成随机数A，33-47的Ascii码
                     //把随机数A，转成字符
                     //生成完，位数+1，字符串累加，结束本次循环
                     if (intA == 1)
                     {
                         intA = ranA.Next(33, 48);
                         strB = ((char)intA).ToString() + strB;
                         intResultRound = intResultRound + 1;
                         continue;
                     }

                     //如果A=2
                     //生成随机数A，58-64的Ascii码
                     //把随机数A，转成字符
                     //生成完，位数+1，字符串累加，结束本次循环
                     if (intA == 2)
                     {
                         intA = ranA.Next(58, 65);
                         strB = ((char)intA).ToString() + strB;
                        intResultRound = intResultRound + 1;
                        continue;
                     }
 
                     //如果A=3
                     //生成随机数A，91-96的Ascii码
                    //把随机数A，转成字符
                    //生成完，位数+1，字符串累加，结束本次循环
                     if (intA == 3)
                     {
                        intA = ranA.Next(91, 97);
                        strB = ((char)intA).ToString() + strB;
                         intResultRound = intResultRound + 1;
                         continue;
                     }
 
                     //如果A=4
                     //生成随机数A，123-126的Ascii码
                    //把随机数A，转成字符
                     //生成完，位数+1，字符串累加，结束本次循环
                    if (intA == 4)
                     {
                        intA = ranA.Next(123, 127);
                         strB = ((char)intA).ToString() + strB;
                        intResultRound = intResultRound + 1;
                        continue;
                    }
                }

                //如果随机数A=3，则运行生成小写字母
                 //生成随机数A，范围在97-122
                 //把随机数A，转成字符
                 //生成完，位数+1，字符串累加，结束本次循环
                 if (intA == 3 )
                 {
                     intA = ranA.Next(97, 123);
                     strB = ((char)intA).ToString() + strB;
                     intResultRound = intResultRound + 1;
                     continue;
                }
                 //如果随机数A=4，则运行生成大写字母
                 //生成随机数A，范围在65-90
                 //把随机数A，转成字符
                 if (intA == 4 )
                {
                     intA = ranA.Next(65, 89);
                     strB = ((char)intA).ToString() + strB;
                    intResultRound = intResultRound + 1;
                     continue;
                }
            }
             return strB;
         }

        public byte[] ExportUserBytes(UserSearchInput input)
        {

            using (var session = DatabaseSession.OpenSession())
            {
                var sql = @"SELECT A.""Id"" 标识,
                               A.""UserName"" 工号,
                              ""Name"" 姓名,
                               C.GROUP_NAME 地区,
                               B.NAME 部门,
                               CASE
                                  WHEN A.""IsDeleted"" = 1 THEN '注销'
                                  WHEN A.""IsActive"" = 0 THEN '锁定'
                                  ELSE '正常'
                               END
                                  状态,
                               a.""CreationTime"" 创建时间,
                               a.""LastLoginTime"" 最后登录时间
                          FROM ""AbpDepartment"" B,
                              ""AbpUsers"" A
                               LEFT JOIN ""AbpDistrict"" C ON A.""GroupId"" = C.ID
                         WHERE ""TenantId"" = {0} AND A.""DepartmentId"" = B.ID".FormatWith(AbpSession.TenantId);

                const string whereString = "";
                var orderString = input.Order != null
                    ? @" ORDER BY A.""{0}"" {1}".FormatWith(input.Order.Name, input.Order.Type)
                    : "";

                var reader = session.ExecuteReader(new DataCommandDefinition(sql + whereString + orderString));

                return _exportProvider.ExportBig(reader, ExportFileType.Excel);
            }
        }

        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <returns></returns>
        public long GetCurrentUserId()
        {
            var user = GetCurrentUserAsync();

            if (user != null)
            {
                return user.Result.Id;
            }

            return 0;
        }

        /// <summary>
        /// 获取当前用户的用户名
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUserName()
        {
            var user = GetCurrentUserAsync();

            if (user != null)
            {
                return user.Result.UserName;
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取当前系统名
        /// </summary>
        /// <returns></returns>
        public string GetCurrentSysName()
        {
            return System.Configuration.ConfigurationManager.AppSettings["SysName"];
        }

        #endregion

        #region 私有方法

        #endregion
    }
}