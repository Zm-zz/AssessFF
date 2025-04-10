/********************************************************************************
 [ 文件 ] 公共计算机类
 [ 作者 ] 肖文
 [ 日期 ] 2019/12/24
 [ 描述 ] 公共类库，提供设置和获取计算机软硬件信息的方法
 ********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace WeFrame
{
    /// <summary>
    /// [ 网络适配器类型 ] 有线/无线/任意类型
    /// </summary>
    public enum NetAdapterType
    {
        Wired,
        Wireless,
        AnyType
    }
}

namespace WeFrame
{
    /// <summary>
    /// [ 公共计算机类 ] 公共类库，提供设置和获取计算机软硬件信息的方法
    /// </summary>
	public static class CComputer
	{
        public static string GetPhysicalAddress()
        {
            string address = "";

            foreach (NetworkInterface item in GetNetAdapterList(NetAdapterType.AnyType))
            {
                address = item.GetPhysicalAddress().ToString();

                if (address != "") break;
            }

            return address;
        }

        public static NetworkInterface GetNetAdapter(NetAdapterType type)
        {
            NetworkInterface adapter = null;
            NetworkInterface[] adapterAry = NetworkInterface.GetAllNetworkInterfaces();

            if (type == NetAdapterType.AnyType)
            {
                foreach (NetworkInterface item in adapterAry)
                {
                    adapter = item;
                    break;
                }
            }
            else if (type == NetAdapterType.Wired)
            {
                foreach (NetworkInterface item in adapterAry)
                {
                    if (item.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        adapter = item;
                        break;
                    }
                }
            }
            else if (type == NetAdapterType.Wireless)
            {
                foreach (NetworkInterface item in adapterAry)
                {
                    if (item.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                    {
                        adapter = item;
                        break;
                    }
                }
            }

            return adapter;
        }

        public static List<NetworkInterface> GetNetAdapterList(NetAdapterType type)
        {
            List<NetworkInterface> adapterList = new List<NetworkInterface>();
            NetworkInterface[] adapterAry = NetworkInterface.GetAllNetworkInterfaces();

            if (type == NetAdapterType.AnyType)
            {
                foreach (NetworkInterface item in adapterAry)
                {
                    adapterList.Add(item);
                }
            }
            else if (type == NetAdapterType.Wired)
            {
                foreach (NetworkInterface item in adapterAry)
                {
                    if (item.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        adapterList.Add(item);
                    }
                }
            }
            else if (type == NetAdapterType.Wireless)
            {
                foreach (NetworkInterface item in adapterAry)
                {
                    if (item.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                    {
                        adapterList.Add(item);
                    }
                }
            }

            return adapterList;
        }

        public static DateTime GetUnixTime(int stamp)
        {
            DateTime startTime;
			DateTime unixTime;
		
			startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			unixTime = startTime.AddSeconds(stamp).ToLocalTime();

            return unixTime;
        }
	}
}
