using Abp.Dependency;
using Abp.RealTime;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyman.Common.SignalRExtension
{
    public class SignalRHelper : Hub, ITransientDependency
    {
        #region 初始化

        private readonly IOnlineClientManager _onlineclientManager;
        public SignalRHelper(IOnlineClientManager onlineclientManager)
        {
            _onlineclientManager = onlineclientManager;
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 取所有当前建立连接用户
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<IOnlineClient> GetAllClients()
        {
            return _onlineclientManager.GetAllClients();
        }

        /// <summary>
        /// 取当前建立连接用户数量
        /// </summary>
        /// <returns></returns>
        public int GetAllClientsCount()
        {
            return _onlineclientManager.GetAllClients().Count;
        }

        /// <summary>
        /// 发送通知给指定用户
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="userId">用户Id</param>
        public void SendMessageToUser(string message,int userId)
        {
            //
        }

        /// <summary>
        /// 发送通知给指定角色
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="roleId">角色id</param>
        public void SendMessageToRole(string message, int roleId)
        {

        }

        /// <summary>
        /// 发送消息给多个用户
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="userIds">接收消息的用户</param>
        public void SendMessageToUserList(string message, List<int> userIds)
        { }

        #endregion

    }
}
