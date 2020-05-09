using gprotocol;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectServerWrapper
{
    /// <summary>
    /// 单机对象集合类
    /// </summary>
    public class Machines
    {
        #region Adundant Properties
        public RadioWrapper<Radio_S1> S1 { get; private set; }
        public RadioWrapper<Radio_S2> S2 { get; private set; }
        public RadioWrapper<Radio_S3> S3 { get; private set; }
        public RadioWrapper<Radio_S4> S4 { get; private set; }
        public RadioWrapper<Radio_S5> S5 { get; private set; }
        public RadioWrapper<Radio_R1> R1 { get; private set; }
        public RadioWrapper<Radio_R2> R2 { get; private set; }
        public RadioWrapper<Radio_R3> R3 { get; private set; }
        public RadioWrapper<Radio_R4> R4 { get; private set; }
        public RadioWrapper<Radio_R5> R5 { get; private set; }
        public RadioWrapper<Radio_R6> R6 { get; private set; }
        public RadioWrapper<Radio_R7> R7 { get; private set; }
        #endregion

        /// <summary>
        /// 单机名称与单机包裹类的键值对
        /// </summary>
        public Dictionary<string, object> DictMachine { get; set; }

        public Machines()
        {
            this.DictMachine = new Dictionary<string, object>();
            List<string> names = new List<string>() { "S1", "R1" };
            Type classType = Type.GetType("ConnectServerWrapper.RadioWrapper`1"); //获取泛型类类型，命名空间+类名
            foreach (var name in names)
            {
                Type T = Type.GetType(string.Format("gprotocol.Radio_{0}, connect_server", name)); //获取类型参数的类型，引用的类库需指明程序集名称
                Type genericType = classType.MakeGenericType(T); //替代由当前泛型类型定义的类型参数组成的类型数组的元素，并返回表示结果构造类型的 System.Type 对象
                //报错，无法强制将RadioWrapper<gprotocol.Radio_S1>转为RadioWrapper<IExtensible>
                this.DictMachine.Add(name, Activator.CreateInstance(genericType));
            }

            #region Adundant Initializations
            this.S1 = new RadioWrapper<Radio_S1>("S1");
            this.S2 = new RadioWrapper<Radio_S2>("S2");
            this.S3 = new RadioWrapper<Radio_S3>("S3");
            this.S4 = new RadioWrapper<Radio_S4>("S4");
            this.S5 = new RadioWrapper<Radio_S5>("S5");
            this.R1 = new RadioWrapper<Radio_R1>("R1");
            this.R2 = new RadioWrapper<Radio_R2>("R2");
            this.R3 = new RadioWrapper<Radio_R3>("R3");
            this.R4 = new RadioWrapper<Radio_R4>("R4");
            this.R5 = new RadioWrapper<Radio_R5>("R5");
            this.R6 = new RadioWrapper<Radio_R6>("R6");
            this.R7 = new RadioWrapper<Radio_R7>("R7");
            #endregion
        }
    }
}
