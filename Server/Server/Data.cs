using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class Data
    {
        /// <summary>
        /// 24小时内成功的企业
        /// 依据此参数*15为企业能养活的人数
        /// </summary>
        public List<int> employerCountLast24Hour { get; set; }

        /// <summary>
        /// 教育参数1
        /// 此参数受24小时内打工者获得的奖金是否大于24×60进行调节
        /// </summary>
        //public double educationK1 { get; set; }

        /// <summary>
        /// 教育参数2
        /// 此参数受govementPosition影响
        /// </summary>
        //public double educationK2 { get; set; }


        /// <summary>
        /// 过去24小时教育花费记录表
        /// 用于计算教育花费
        /// 24小时内10-1名，接受极好教育
        /// 这个存储的是比例。真实的价格为value*Base
        /// </summary>
        public List<List<double>> educationCost { get; set; }
        /// <summary>
        /// 政府资本倾向,过去24小时内的数据
        /// 0-99
        /// 政府资本走向，决定情商-智商比例，决定人才评分
        /// </summary>
        public List<double> govementPosition { get; set; }

        public List<double> employerSumEarn { get; set; }

        public List<double> employteenSumEarn { get; set; }

        /// <summary>
        /// 人才等级，用于决定什么是合格人才的质数，第一个是数量，第二是平均值。新加入，在取新平均
        /// </summary>
        public List<long[]> rencaiLevel { get; set; }
        //public double employerK1 = 0.1853;
        //public double employerK2 = 1;

        public List<List<Int16>> employerActions { get; set; }
        public List<List<Int16>> employteenActions { get; set; }

        public string state { get; set; }

        /// <summary>
        /// 企业的年营业率，取值范围在1.05~1.20之间
        /// </summary>
        public double businessRate { get; set; }

        /// <summary>
        /// 教育基本支出
        /// 当24小时内，依据打工者收入进行调整
        /// </summary>
        public double educationBasePercent { get; set; }

        /// <summary>
        /// 房价
        /// </summary>
        public double housePrice { get; set; }

        /// <summary>
        /// 教育基本价格
        /// </summary>
        public double educateBasePrice { get; set; }

    }
}
