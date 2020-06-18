using gprotocol;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConnectServerWrapper
{
    /// <summary>
    /// Radio_XXAlarm单机实体类的包裹类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RadioAlarmWrapper<T> where T : IExtensible
    {
        public const string THREAT_LEVEL_PROPERTY = "radio_{0}"; //Radio_XXAlarm类中代表各雷达报警级别的属性名，{0}代表此类属性的序号
        public const string THREAT_STATUS_PROPERTY = "radio_status"; //Radio_XXAlarm类中代表雷达报警状态的属性名

        private readonly T _base = default(T); //泛型类基础操作对象
        private readonly Type _type;
        private readonly List<PropertyInfo> _threat_level_props = new List<PropertyInfo>(); //雷达报警级别属性列表
        private readonly PropertyInfo _threat_status_prop; //报警状态属性

        private List<int> _threat_levels = new List<int>();
        private int _threat_status;

        /// <summary>
        /// 初始化后内部的泛型实体类
        /// </summary>
        public T Instance { get { return this._base; } }

        /// <summary>
        /// 单机雷达报警级别列表
        /// </summary>
        public List<int> ThreatLevels
        {
            //get
            //{
            //    this.SyncThreatLevels();
            //    return this._threat_levels;
            //}
            set
            {
                this._threat_levels = value;
                this.SyncThreatLevels();
                //假如报警级别列表不为空且具有至少1个报警级别大于0的，则报警状态为1，否则为0
                this.ThreatStatus = this._threat_levels != null && this._threat_levels.Count(level => level > 0) > 0 ? 1 : 0;
            }
        }

        /// <summary>
        /// 报警状态
        /// </summary>
        public int ThreatStatus
        {
            get { return this._threat_status; }
            set
            {
                this._threat_status = value;
                if (this._threat_status_prop != null)
                    this._threat_status_prop.SetValue(this._base, this._threat_status);
            }
        }

        /// <summary>
        /// 单机类型
        /// </summary>
        public Cmd MachineType { get; set; }

        ///// <summary>
        ///// 默认构造器
        ///// </summary>
        //public RadioAlarmWrapper() : this(string.Empty) { }

        /// <summary>
        /// 构造器
        /// </summary>
        public RadioAlarmWrapper(/*string machine_name*/)
        {
            this._type = typeof(T);
            this._base = (T)Activator.CreateInstance(this._type);
            //获取属性对象
            int i = 1;
            while (true)
            {
                PropertyInfo prop = this._type.GetProperty(string.Format(THREAT_LEVEL_PROPERTY, i++));
                if (prop == null)
                    break;
                this._threat_level_props.Add(prop);
            }
            this._threat_status_prop = this._type.GetProperty(THREAT_STATUS_PROPERTY);

            //为属性赋初始值
            this.ThreatLevels = new List<int>();
            this.ThreatStatus = 0;
            this.MachineType = (Cmd)Enum.Parse(typeof(Cmd), "e" + this._type.Name);
        }

        /// <summary>
        /// 同步雷达报警级别
        /// </summary>
        private void SyncThreatLevels()
        {
            if (this._threat_levels == null || this._threat_level_props == null)
                return;
            for (int i = 0; i < this._threat_levels.Count && i < this._threat_level_props.Count; i++)
            {
                PropertyInfo prop = this._threat_level_props[i];
                if (prop != null)
                    prop.SetValue(this._base, this._threat_levels[i]);
            }
        }
    }
}
