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
    public class RadioDisplayWrapper<T> where T : IExtensible
    {
        public const string MACHINE_NAME_PROPERTY = "machine_name"; //Radio_XX类中代表单机名称的属性名
        public const string FLOAT_ARRAY_PROPERTY = "floatarrawy"; //Radio_XX类中代表单机姿态的属性名
        public const string FLOAT_ARRAY_PROPERTY_ALT = "pointtofloat"; //Radio_XX类中代表单机姿态的属性名（备用）
        public const string POSTURE_STATUS_PROPERTY = "XXX"; //Radio_XX类中代表姿态数据传输状态的属性名

        private readonly T _base = default(T); //泛型类基础操作对象
        private readonly bool _available = false;
        private readonly Type _type;
        private readonly PropertyInfo _machine_name_prop, _float_array_prop, _pos_status_prop; //单机名称，单机姿态，姿态数据传输状态属性

        private string _machine_name = string.Empty;
        private bool _pos_status;
        private float _walking, _pitch, _yaw;
        private readonly List<float> _float_list = new List<float>() { 0, 0, 0 };

        /// <summary>
        /// 初始化后内部的泛型实体类
        /// </summary>
        public T Instance { get { return _base; } }

        /// <summary>
        /// 单机名称
        /// </summary>
        public string MachineName
        {
            get { return _machine_name; }
            set
            {
                _machine_name = value;
                if (_machine_name_prop != null)
                    _machine_name_prop.SetValue(_base, _machine_name);
            }
        }

        /// <summary>
        /// 姿态数据传输状态
        /// </summary>
        public bool PostureStatus
        {
            get { return _pos_status; }
            set
            {
                _pos_status = value;
                if (_pos_status_prop != null)
                    _pos_status_prop.SetValue(_base, _pos_status ? 1 : 0);
            }
        }

        /// <summary>
        /// 行走位置
        /// </summary>
        public float Walking
        {
            get { return _walking; }
            set
            {
                _walking = value;
                _float_list[0] = _walking;
            }
        }

        /// <summary>
        /// 俯仰角度
        /// </summary>
        public float PitchAngle
        {
            get { return _pitch; }
            set
            {
                _pitch = value;
                _float_list[2] = _pitch;
            }
        }

        /// <summary>
        /// 回转角度
        /// </summary>
        public float YawAngle
        {
            get { return _yaw; }
            set
            {
                _yaw = value;
                _float_list[1] = _yaw;
            }
        }

        /// <summary>
        /// 单机类型
        /// </summary>
        public Cmd MachineType { get; set; }

        /// <summary>
        /// 默认构造器
        /// </summary>
        public RadioDisplayWrapper() : this(string.Empty) { }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="machine_name">单机名称</param>
        public RadioDisplayWrapper(string machine_name)
        {
            _type = typeof(T);
            //_base = new T();
            _base = (T)Activator.CreateInstance(_type);
            //_base = (T)Assembly.GetAssembly(_type).CreateInstance(_type.ToString());
            _available = _type.Name.Contains("Radio_");
            //获取属性对象
            _machine_name_prop = _type.GetProperty(MACHINE_NAME_PROPERTY);
            _float_array_prop = _type.GetProperty(FLOAT_ARRAY_PROPERTY);
            if (_float_array_prop == null)
                _float_array_prop = _type.GetProperty(FLOAT_ARRAY_PROPERTY_ALT);
            _pos_status_prop = _type.GetProperty(POSTURE_STATUS_PROPERTY);

            //为属性赋初始值
            MachineName = machine_name;
            PostureStatus = false;
            //找到floatarray属性并初始化
            if (_float_array_prop != null && _float_array_prop.GetGetMethod() != null)
                _float_list = (List<float>)_float_array_prop.GetValue(_base);
            _float_list.Clear();
            _float_list.AddRange(new float[] { 0, 0, 0 }); //长度不可小于3
            //_float_array_prop.SetValue(_base, _float_list);
            MachineType = (Cmd)Enum.Parse(typeof(Cmd), "e" + _type.Name);
        }
    }
}
