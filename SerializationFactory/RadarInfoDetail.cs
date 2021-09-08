using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationFactory
{
    /// <summary>
    /// 每台雷达的基础信息
    /// </summary>
    [ProtoContract]
    public class RadarInfoDetail
    {
        /// <summary>
        /// 雷达ID
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }

        /// <summary>
        /// 雷达名称
        /// </summary>
        [ProtoMember(2)]
        public string Name { get; set; }

        /// <summary>
        /// 工作状态，1 收到数据，2 未收到数据
        /// </summary>
        [ProtoMember(3)]
        public int Working { get; set; }

        /// <summary>
        /// 当前距离
        /// </summary>
        [ProtoMember(4)]
        public double CurrentDistance { get; set; }

        /// <summary>
        /// 报警级数
        /// </summary>
        [ProtoMember(5)]
        public int ThreatLevel { get; set; }

        /// <summary>
        /// 报警级数2进制数
        /// </summary>
        [ProtoMember(6)]
        public string ThreatLevelBinary { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        [ProtoMember(7)]
        public string IpAddress { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        [ProtoMember(8)]
        public ushort Port { get; set; }

        /// <summary>
        /// 雷达组类型
        /// </summary>
        [ProtoMember(9)]
        public RadarGroupType GroupType { get; set; }

        /// <summary>
        /// 方向
        /// </summary>
        [ProtoMember(10)]
        public Directions Direction { get; set; }

        /// <summary>
        /// 返回此实例的哈希代码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return CurrentDistance.GetHashCode() | ThreatLevel.GetHashCode();
        }

        #region 对象比较
        #region 是否相等的比较
        /// <summary>
        /// 判断当前实例与某对象是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is RadarInfoDetail && CurrentDistance == ((RadarInfoDetail)obj).CurrentDistance && ThreatLevel == ((RadarInfoDetail)obj).ThreatLevel;
        }

        /// <summary>
        /// 重新定义的相等符号
        /// </summary>
        /// <param name="left">左侧实例</param>
        /// <param name="right">右侧实例</param>
        /// <returns></returns>
        public static bool operator ==(RadarInfoDetail left, RadarInfoDetail right)
        {
            return (object)left == null ? (object)right == null : left.Equals(right);
        }

        /// <summary>
        /// 重新定义的不等符号
        /// </summary>
        /// <param name="left">左侧实例</param>
        /// <param name="right">右侧实例</param>
        /// <returns></returns>
        public static bool operator !=(RadarInfoDetail left, RadarInfoDetail right)
        {
            return !(left == right);
        }
        #endregion

        #region 大小的比较
        /// <summary>
        /// 将当前实例与另一实例相比较，并返回比较结果符号：-1 小于，0 相等，1 大于
        /// </summary>
        /// <param name="other">与当前实例比较的另一实例</param>
        /// <returns></returns>
        public int CompareTo(RadarInfoDetail other)
        {
            int d = CurrentDistance.CompareTo(other.CurrentDistance), a = ThreatLevel.CompareTo(other.ThreatLevel);
            int result;
            if (d == 0 && a == 0)
                result = 0;
            else
                result = (d == -1 && a != -1) || (d != -1 && a == 1) ? -1 : 1;
            return result;
        }

        /// <summary>
        /// 重新定义的小于符号
        /// </summary>
        /// <param name="left">左侧实例</param>
        /// <param name="right">右侧实例</param>
        /// <returns></returns>
        public static bool operator <(RadarInfoDetail left, RadarInfoDetail right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// 重新定义的小于等于符号
        /// </summary>
        /// <param name="left">左侧实例</param>
        /// <param name="right">右侧实例</param>
        /// <returns></returns>
        public static bool operator <=(RadarInfoDetail left, RadarInfoDetail right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// 重新定义的大于符号
        /// </summary>
        /// <param name="left">左侧实例</param>
        /// <param name="right">右侧实例</param>
        /// <returns></returns>
        public static bool operator >(RadarInfoDetail left, RadarInfoDetail right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// 重新定义的大于等于符号
        /// </summary>
        /// <param name="left">左侧实例</param>
        /// <param name="right">右侧实例</param>
        /// <returns></returns>
        public static bool operator >=(RadarInfoDetail left, RadarInfoDetail right)
        {
            return left.CompareTo(right) >= 0;
        }
        #endregion
        #endregion
    }
}
