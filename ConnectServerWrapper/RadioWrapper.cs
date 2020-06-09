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
    /// Radio_XX单机实体类的包裹类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RadioWrapper<T> where T : IExtensible
    {
        public const string MACHINE_NAME_PROPERTY = "machine_name"; //Radio_XX类中代表单机名称的属性名
        public const string FLOAT_ARRAY_PROPERTY = "floatarrawy"; //Radio_XX类中代表单机姿态的属性名

        private readonly T _base = default(T); //泛型类基础操作对象
        private readonly bool _available = false;
        private readonly Type _type;
        private readonly PropertyInfo _machine_name_prop, _float_array_prop; //单机名称，单机姿态属性

        private string _machine_name = string.Empty;
        private float _walking, _pitch, _yaw;
        private readonly List<float> _float_list = new List<float>() { 0, 0, 0 };

        /// <summary>
        /// 初始化后内部的泛型实体类
        /// </summary>
        public T Instance { get { return this._base; } }

        /// <summary>
        /// 单机名称
        /// </summary>
        public string MachineName
        {
            get { return this._machine_name; }
            set
            {
                this._machine_name = value;
                if (this._machine_name_prop != null)
                    this._machine_name_prop.SetValue(this._base, this._machine_name);
            }
        }

        /// <summary>
        /// 行走位置
        /// </summary>
        public float Walking
        {
            get { return this._walking; }
            set
            {
                this._walking = value;
                this._float_list[0] = this._walking;
            }
        }

        /// <summary>
        /// 俯仰角度
        /// </summary>
        public float PitchAngle
        {
            get { return this._pitch; }
            set
            {
                this._pitch = value;
                this._float_list[2] = this._pitch;
            }
        }

        /// <summary>
        /// 回转角度
        /// </summary>
        public float YawAngle
        {
            get { return this._yaw; }
            set
            {
                this._yaw = value;
                this._float_list[1] = this._yaw;
            }
        }

        /// <summary>
        /// 单机类型
        /// </summary>
        public Cmd MachineType { get; set; }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public RadioWrapper() : this(string.Empty) { }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="machine_name">单机名称</param>
        public RadioWrapper(string machine_name)
        {
            this._type = typeof(T);
            //this._base = new T();
            this._base = (T)Activator.CreateInstance(this._type);
            //this._base = (T)Assembly.GetAssembly(this._type).CreateInstance(this._type.ToString());
            this._available = this._type.Name.Contains("Radio_");
            //获取属性对象
            this._machine_name_prop = this._type.GetProperty(MACHINE_NAME_PROPERTY);
            this._float_array_prop = this._type.GetProperty(FLOAT_ARRAY_PROPERTY);

            //为属性赋初始值
            this.MachineName = machine_name;
            //找到floatarray属性并初始化
            if (this._float_array_prop != null && this._float_array_prop.GetGetMethod() != null)
                this._float_list = (List<float>)this._float_array_prop.GetValue(this._base);
            this._float_list.Clear();
            this._float_list.AddRange(new float[] { 0, 0, 0 }); //长度不可小于3
            //this._float_array_prop.SetValue(this._base, this._float_list);
            this.MachineType = (Cmd)Enum.Parse(typeof(Cmd), "e" + this._type.Name);
        }
    }
}
