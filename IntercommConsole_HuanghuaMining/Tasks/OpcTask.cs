using CommonLib.Clients;
using CommonLib.Clients.Tasks;
using CommonLib.Function;
using IntercommConsole.Core;
using IntercommConsole.DataUtil;
using OpcLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace IntercommConsole.Tasks
{
    public class OpcTask : Task
    {
        private OpcUtilHelper _opcHelper = new OpcUtilHelper(1000, true);

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
            Const.OpcDatasource.RadarStatus = Const.RadarInfo.RadarList == null ? 0 : Convert.ToInt32(new string(string.Join(string.Empty, Const.RadarInfo.RadarList.Select(r => r.Working)).Reverse().ToArray()), 2);
            Const.OpcDatasource.WalkingPositionCorr = Posture.WalkingPosition;
            Const.OpcDatasource.PitchAngleCorr = Posture.PitchAngle;
            Const.OpcDatasource.YawAngleCorr = Posture.YawAngle;
            //代表单击姿态可用性的状态
            string temp = (Posture.WalkingValid ? "1" : "0") + (Posture.PitchValid ? "1" : "0") + (Posture.YawValid ? "1" : "0") + Convert.ToString((int)Posture.WalkingInvalidResource, 2) + Convert.ToString((int)Posture.PitchInvalidResource, 2) + Convert.ToString((int)Posture.YawInvalidResource, 2);
            Const.OpcDatasource.PostureStates = Convert.ToInt32(new string(temp.Reverse().ToArray()), 2);
            OpcReadValues();
            OpcWriteValues();
            Const.OpcDatasource.WalkingQueue_Plc.Push(Const.OpcDatasource.WalkingPosition_Plc * 1000 / this.Interval); //按照任务执行间隔对PLC行走位置进行放大，相邻数据相减即可得到速度
            Const.OpcDatasource.YawQueue_Plc.Push(Const.OpcDatasource.YawAngle_Plc * 1000 / this.Interval); //按照任务执行间隔对PLC行走位置进行放大，相邻数据相减即可得到速度
            this.CalculateMovements();
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
            _opcHelper = new OpcUtilHelper(1000, true);
            string[] servers = _opcHelper.ServerEnum(Config.OpcServerIp, out _errorMessage);
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
            _opcHelper.ListGroupInfo = groups;
            _opcHelper.ConnectRemoteServer(Config.OpcServerIp, Config.OpcServerName, out _errorMessage);
            Const.WriteConsoleLog(string.Format("OPC连接状态：{0}", _opcHelper.OpcConnected));
            if (!string.IsNullOrWhiteSpace(_errorMessage))
                Const.WriteConsoleLog(string.Format("连接过程中出现问题：{0}", _errorMessage));
            END_OF_OPC:;
        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        private void SetOpcGroupsDataSource()
        {
            if (_opcHelper != null && _opcHelper.ListGroupInfo != null)
                _opcHelper.ListGroupInfo.ForEach(group => group.DataSource = Const.OpcDatasource);
        }

        /// <summary>
        /// 读取值
        /// </summary>
        private void OpcReadValues()
        {
            Const.OpcDatasource.PlcReadable = true;
            if (!Config.OpcEnabled)
                return;

            _opcHelper.ListGroupInfo.ForEach(group =>
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
            Const.OpcDatasource.PlcWritable = true;
            if (!Config.OpcEnabled || !Config.Write2Plc)
                return;

            _opcHelper.ListGroupInfo.ForEach(group =>
            {
                if (group.GroupType != GroupType.WRITE)
                    return;

                if (!group.WriteValues(out _errorMessage))
                    Const.WriteConsoleLog(string.Format("写入PLC失败，写入过程中出现问题：{0}", _errorMessage));
            });
        }

        private void CalculateMovements()
        {
            GenericStorage<double> wqueue = Const.OpcDatasource.WalkingQueue_Plc, yqueue = Const.OpcDatasource.YawQueue_Plc;
            //Const.OpcDatasource.WalkingSpeed_Plc = wqueue == null ? 0 : Math.Round(wqueue.ElementAt(2) - wqueue.ElementAt(1), 3);
            Const.OpcDatasource.WalkingAcce_Plc = wqueue == null ? 0 : Math.Round(wqueue.ElementAt(0) - 2 * wqueue.ElementAt(1) + wqueue.ElementAt(2), 3);
            //Const.OpcDatasource.YawSpeed_Plc = yqueue == null ? 0 : Math.Round(yqueue.ElementAt(1) - yqueue.ElementAt(0), 3);
        }
    }
}
