using ACUServer;
using ISafe_Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ISafe_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //启动客户端服务
            ServiceHost User_host = new ServiceHost(typeof(ACUServer.UserClientServer));
            User_host.Open();
            Console.WriteLine("客户端服务已启动");

            //启动OPCClient服务
            ServiceHost OPCClient_host = new ServiceHost(typeof(ACUServer.OPCClientProxyServer));
            OPCClient_host.Open();
            Console.WriteLine("OPCClient代理端服务已启动");

            //读取系统配置
            XmlHelper.ReadGraphic();

            XmlHelper.InitialPreSensorCache();

            XmlHelper.IsIntial = true;

            //启动误报屏蔽
            SCADAAlarmManager.Instance.StartSCADAPointRead();

            Console.Read();

            

        }

    }
}
