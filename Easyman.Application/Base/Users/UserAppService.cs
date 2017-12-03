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

        #region ��ʼ��

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

        #region ���з���

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
                        temp.State = "ע��";
                    else
                        temp.State = s.IsActive ? "����" : "����";
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
                    //��������ѯ����ͬ�����û�
                    var userCount = _userRepository.GetAll().Count(a => a.UserName == input.UserName);
                    if (userCount >= 1)
                    {
                        throw new UserFriendlyException("�����Ѵ���");
                    }
                }else
               {
                    //�޸ģ���ѯ���Լ������й����û�
                    var userCounts = _userRepository.GetAll().Where(a => a.Id != input.Id).Count(b => b.UserName == input.UserName);
                    if (userCounts >= 1)
                    {
                        throw new UserFriendlyException("�����Ѵ���");
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
                        throw new UserFriendlyException("����״̬�쳣���޷�����");
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
                            throw new UserFriendlyException("����״̬������������ע��");
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
        /// �������빦��
        /// </summary>
        /// <param name="inout"></param>
        public string ResetPwd(EntityDto<long> inout)
        {
            try
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    var user = GetUser(inout.Id);

                    if (user != null)
                    {
                        //�ж��Ƿ������������
                        //��ȡ���ýڵ���Ϣ
                        var userSe = OperateSection.GetPwdRuleSection();
                        if (userSe.IsRandomPwd)
                        {
                            
                            //���븴�Ӷ���֤
                            if (userSe.IsValidatecComplex)
                            {
                                //�������룬����ͨ����֤�������
                                var pwdNow = RandomPasswords();//����ͨ����֤�������
                                user.Password = new PasswordHasher().HashPassword(pwdNow);
                                user.ModifyPwd = 0;
                                 _userManager.UpdateAsync(user);
                                //throw new UserFriendlyException(pwdNow);
                                return pwdNow;
                            }
                            else
                            {
                                //�������룬�����������
                                var pwdNow = RandomPassword();//�����������
                                user.Password = new PasswordHasher().HashPassword(pwdNow);
                                user.ModifyPwd = 0;
                                 _userManager.UpdateAsync(user);
                                //throw new UserFriendlyException(pwdNow);
                                return pwdNow;
                                //abp.message.info("��������ɹ��������룺"+pwdNow, "��ʾ");

                            }
                        }
                        else
                        {
                            //�������룬����Ĭ������
                            var pwdNow = userSe.DefualtPwd;//��ȡĬ������
                            user.Password = new PasswordHasher().HashPassword(pwdNow);
                            user.ModifyPwd = 0;
                            _userManager.UpdateAsync(user);
                            //throw new UserFriendlyException(pwdNow);
                            return pwdNow;
                        }

                    }
                    else
                    {
                        throw new UserFriendlyException("��ѯ�����û������ڣ�");
                    }
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        /// <summary>
        /// ����GUID��ȡ16λ��Ψһ�ַ���
        /// </summary>
        /// <returns></returns>
        public static string GuidTo16String()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        public string RandomPassword()
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890~!@#$%^&*.?";
            Random randrom = new Random((int)DateTime.Now.Ticks);
            string str = "";
            for (int i = 0; i < 12; i++)
            {
                str += chars[randrom.Next(chars.Length)];
            }
            return str;

        }
        /// <summary>
        /// ����ͨ�������븴�Ӷ���֤���������
        /// </summary>
        /// <returns></returns>
        private string RandomPasswords()
        {
            var newPwd = RandomPassword();
            
            while (!RandomBoolPwd(newPwd))
            {
                newPwd = RandomPassword();
            }
            return newPwd;
        }
        /// <summary>
        /// ���븴�Ӷ���֤
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private bool RandomBoolPwd(string pwd)
        {
            string result = "";

            //�������븴�Ӷȵ�У��
            var pwdRule = OperateSection.GetPwdRuleSection();
            if (pwdRule.IsValidatecComplex)//�Ƿ����ø��Ӷ�У��
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
                //���븴�ӶȲ�ͨ����֤
                return false;
            }
            else
            {
                return true;
            }
        }

        public byte[] ExportUserBytes(UserSearchInput input)
        {

            using (var session = DatabaseSession.OpenSession())
            {
                var sql = @"SELECT A.""Id"" ��ʶ,
                               A.""UserName"" ����,
                              ""Name"" ����,
                               C.GROUP_NAME ����,
                               B.NAME ����,
                               CASE
                                  WHEN A.""IsDeleted"" = 1 THEN 'ע��'
                                  WHEN A.""IsActive"" = 0 THEN '����'
                                  ELSE '����'
                               END
                                  ״̬,
                               a.""CreationTime"" ����ʱ��,
                               a.""LastLoginTime"" ����¼ʱ��
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
        /// ��ȡ��ǰ�û�ID
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
        /// ��ȡ��ǰ�û����û���
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
        /// ��ȡ��ǰϵͳ��
        /// </summary>
        /// <returns></returns>
        public string GetCurrentSysName()
        {
            return System.Configuration.ConfigurationManager.AppSettings["SysName"];
        }

        #endregion

        #region ˽�з���

        #endregion
    }
}