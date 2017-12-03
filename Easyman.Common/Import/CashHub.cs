using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using EasyMan.Dtos;
using Microsoft.AspNet.SignalR;


namespace EasyMan.Import
{
   public class CashHub:Hub
    {
   

       public CashHub()
       {
         
       }
       public void Send(CashHubDto msg)
       {
           Clients.All.getMessage(msg);
       }

    }
}
