using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Easyman.Users.Dto;
using Easyman.Dto;
using System.Collections.Generic;
using Easyman.App.Dto;

namespace Easyman.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task ProhibitPermission(ProhibitPermissionInput input);

        Task RemoveFromRole(long userId, string roleName);

        Task<ListResultDto<UserListDto>> GetUsers();

        Task CreateUser(CreateUserInput input);

        void DeleteUser(EntityDto<long> input);

        List<User> GetAllUser();

        User AddUser(User user);

        UserSearchOutput Search(UserSearchInput input);

        User GetUser(long id);

        /// <summary>
        /// ����ע���˺�
        /// </summary>
        /// <param name="input"></param>
        void Delete(EntityDto<string> input);

        /// <summary>
        /// ����/�༭�û� Id 0������ 1���༭
        /// </summary>
        /// <param name="userInput"></param>
        void UpdateOrInserUser(UserInput userInput);

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="userId"></param>
        void Unlock(EntityDto<long> inout);

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="userId"></param>
        void Lock(EntityDto<long> inout);

        /// <summary>
        /// ע���˺�
        /// </summary>
        /// <param name="inout"></param>
        void Cannel(EntityDto<long> inout);

        /// <summary>
        /// �����û�����
        /// </summary>
        /// <param name="id"></param>
        string ResetPwd(int id);

        

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        byte[] ExportUserBytes(UserSearchInput input);

        /// <summary>
        /// ��ȡ��ǰ�û�ID
        /// </summary>
        /// <returns></returns>
        long GetCurrentUserId();


        /// <summary>
        /// ��ȡ��ǰ�û����û���
        /// </summary>
        /// <returns></returns>
        string GetCurrentUserName();

        /// <summary>
        /// ��ȡ��ǰϵͳ��
        /// </summary>
        /// <returns></returns>
        string GetCurrentSysName();
    }
}