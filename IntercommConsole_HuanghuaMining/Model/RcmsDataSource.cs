using IntercommConsole.Core;
using SerializationFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntercommConsole.Model
{
    public class RcmsDataSource
    {
        /// <summary>
        /// 单机名称
        /// </summary>
        public string MachineName { get { return Config.MachineName; } }

        /// <summary>
        /// 雷达信息
        /// </summary>
        public List<RadarInfoDetail> RadarDetails { get { return Const.RadarInfo == null ? null : Const.RadarInfo.RadarList; } }

        /// <summary>
        /// 北斗信息
        /// </summary>
        public GnssProtoInfo GnssInfo { get { return Const.GnssInfo; } }

        /// <summary>
        /// OPC数据源
        /// </summary>
        public OpcDataSource OpcDataSource { get { return Const.OpcDatasource; } }
    }
}
