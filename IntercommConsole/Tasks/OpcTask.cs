﻿using CommonLib.Function;
using OpcLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.Tasks
{
    public class OpcTask : Task
    {
        private OpcUtilHelper opcHelper = new OpcUtilHelper(1000, true);

        /// <summary>
        /// 构造器
        /// </summary>
        public OpcTask() : base() { }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            OpcInit();
            SetOpcGroupsDataSource();
        }

        /// <summary>
        /// 循环体内容
        /// </summary>
        public override void LoopContent()
        {
            OpcReadValues();
            OpcWriteValues();
        }

        /// <summary>
        /// OPC初始化
        /// </summary>
        private void OpcInit()
        {
            if (!Config.OpcEnabled)
                return;

            Const.WriteConsoleLog(string.Format("开始连接IP地址为{0}的OPC SERVER {1}...", Config.OpcServerIp, Config.OpcServerName));
            DataService_Opc dataService_Opc = new DataService_Opc();
            opcHelper = new OpcUtilHelper(1000, true);
            string[] servers = opcHelper.ServerEnum(Config.OpcServerIp, out _errorMessage);
            if (!string.IsNullOrWhiteSpace(_errorMessage))
            {
                Const.WriteConsoleLog(string.Format("枚举过程中出现问题：{0}", _errorMessage));
                goto END_OF_OPC;
            }
            if (servers == null || !servers.Contains(Config.OpcServerName))
            {
                Const.WriteConsoleLog(string.Format("无法找到指定OPC SERVER：{0}", Config.OpcServerName));
                goto END_OF_OPC;
            }
            DataTable table = dataService_Opc.GetOpcInfo();
            if (table == null || table.Rows.Count == 0)
            {
                Const.WriteConsoleLog(string.Format("在表中未找到任何OPC记录，将不进行读取或写入", Config.OpcServerName));
                goto END_OF_OPC;
            }
            List<OpcGroupInfo> groups = new List<OpcGroupInfo>();
            List<DataRow> dataRows = table.Rows.Cast<DataRow>().ToList();
            List<OpcItemInfo> items = null;
            int id = 0;
            foreach (var row in dataRows)
            {
                string itemId = row["item_id"].ConvertType<string>();
                if (string.IsNullOrWhiteSpace(itemId))
                    continue;
                int groupId = row["group_id"].ConvertType<int>(), clientHandle = row["record_id"].ConvertType<int>();
                string groupName = row["group_name"].ConvertType<string>(), fieldName = row["field_name"].ConvertType<string>();
                GroupType type = (GroupType)row["group_type"].ConvertType<int>();
                if (groupId != id)
                {
                    id = groupId;
                    groups.Add(new OpcGroupInfo(null, groupName/*, OpcDatasource*/) { GroupType = type, ListItemInfo = new List<OpcItemInfo>() });
                    OpcGroupInfo groupInfo = groups.Last();
                    items = groupInfo.ListItemInfo;
                }
                items.Add(new OpcItemInfo(itemId, clientHandle, fieldName));
            }
            opcHelper.ListGroupInfo = groups;
            opcHelper.ConnectRemoteServer(Config.OpcServerIp, Config.OpcServerName, out _errorMessage);
            Const.WriteConsoleLog(string.Format("OPC连接状态：{0}", opcHelper.OpcConnected));
            if (!string.IsNullOrWhiteSpace(_errorMessage))
                Const.WriteConsoleLog(string.Format("连接过程中出现问题：{0}", _errorMessage));
            END_OF_OPC:;
        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        private void SetOpcGroupsDataSource()
        {
            if (opcHelper != null && opcHelper.ListGroupInfo != null)
                opcHelper.ListGroupInfo.ForEach(group => group.DataSource = Const.OpcDatasource);
        }

        /// <summary>
        /// 读取值
        /// </summary>
        private void OpcReadValues()
        {
            if (!Config.OpcEnabled)
                return;

            opcHelper.ListGroupInfo.ForEach(group =>
            {
                if (group.GroupType != GroupType.READ)
                    return;

                if (!group.ReadValues(out _errorMessage))
                    Const.WriteConsoleLog(string.Format("读取PLC失败，读取过程中出现问题：{0}", _errorMessage));
            });
        }

        /// <summary>
        /// 写入值
        /// </summary>
        private void OpcWriteValues()
        {
            if (!Config.OpcEnabled && !Config.Write2Plc)
                return;

            //OpcDatasource.WalkingPosition = 542.492;
            //OpcDatasource.PitchAngle = 11.2;
            //OpcDatasource.YawAngle = 15.12;
            //OpcDatasource.PileHeight = 15.23;
            opcHelper.ListGroupInfo.ForEach(group =>
            {
                if (group.GroupType != GroupType.WRITE)
                    return;

                if (!group.WriteValues(out _errorMessage))
                    Const.WriteConsoleLog(string.Format("写入PLC失败，写入过程中出现问题：{0}", _errorMessage));
            });
        }
    }
}