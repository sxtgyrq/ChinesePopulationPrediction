using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_Shengyulv
{
    public class Shengyulv
    {
        const int countOfEmployerActions = 2000;
        Random rm;
        List<Byte> employerActions;

        List<int> ecducateCost = new List<int>(1000);
        /// <summary>
        /// 育儿基金参数，当每小时获得的资金总量大于60时，调整此参数。主要用于判断过去8小时内玩家获得的收益是否大于8×30
        /// </summary>
        double educateParameterK1 = 1.0;

        double educateParameterK2
        {
            get
            {
                return this.GovementPosition * 0.02 + 0.9;
            }


        }
        double educationParameter
        {
            get { return this.educateParameterK1 * this.educateParameterK2; }
        }

        double baseEdcationCost = 0.5;

        public Shengyulv()
        {

            this.rm = new Random();
            employerActions = new List<byte>();
            List<Byte> employeeActions = new List<byte>();
            for (var i = 0; i < countOfEmployerActions; i++)
            {

                employerActions.Add(Convert.ToByte(rm.Next(6) + 1));
            }
            for (var i = 0; i < 5000; i++)
            {
                employeeActions.Add(Convert.ToByte(rm.Next(8) + 1));
            }
        }
        double _govementPosition = 50;
        public double GovementPosition
        {
            get { return _govementPosition; }
            set
            {
                this._govementPosition = value;
            }
        }

        /// <summary>
        /// 资本家列表
        /// </summary>
        List<string> _capitalists = new List<string>();
        /// <summary>
        /// 教育家列表
        /// </summary>
        List<string> _educator = new List<string>();

        public void capitalistsDo(string capitalist, string action)
        {
            if (_capitalists.Contains(capitalist)) { }
            else
            { }
            switch (action)
            {
                case "996": { }; break;
                case "onlyyourger": { }; break;
                case "notmarriedfemail": { }; break;
                case "newTechnology": { }; break;
                case "goAway": { }; break;
                case "improveWelfare": { }; break;
            }
        }

        internal void DealWithEmployeeResult(byte result)
        {
            throw new NotImplementedException();
        }

        public void educatorDo(Byte action)
        {
            switch (action)
            {
                case 1:
                    {
                        var indexCal = GetIndexOffEmployerActions();
                        var self = action;
                        var employerAction = this.employerActions[indexCal];
                        var s = getLoveSuccess(employerAction);
                        if (s)
                        {
                            Console.WriteLine("恋爱成功");
                        }
                        //love
                    }; break;
                case 2:
                    {
                        var indexCal = GetIndexOffEmployerActions();
                        var employerAction = this.employerActions[indexCal];
                        var s = getMarriedSuccess(employerAction);
                        //marry
                    }; break;
                case 3:
                    {
                        var indexCal = GetIndexOffEmployerActions();
                        var employerAction = this.employerActions[indexCal];
                        var s = getFirstBabySuccess(employerAction);
                        //getFirstBaby
                    }; break;
                case 4:
                    {
                        var indexCal = GetIndexOffEmployerActions();
                        var employerAction = this.employerActions[indexCal];
                        var s = getSecondBabySuccess(employerAction);
                        //getSecondBaby
                    }; break;
                case 5:
                    {
                        //教育成本=教育直接成本+教育间接成本
                        //educateBaby
                        var indexCal = GetIndexOffEmployerActions();
                        var employerAction = this.employerActions[indexCal];
                        var s = getEducateResult(employerAction);
                    }; break;
                case 6:
                    {
                        //struggle
                    }; break;
                case 7:
                    {
                        //parenting
                    }; break;
                case 8:
                    {
                        //fulltime
                    }; break;
            }
        }

        private object getEducateResult(byte employerAction)
        {
            /*
             * 教育分成了5个等级
             * 极好教育 98-100 1000个教育名额中10个名额 
             * 优秀教育 90-97 1000个教育名额中20个名额
             * 良好教育 80-89 1000个教育中200个名额
             * 一般教育 70-79 1000个教育中300个名额
             * 合格教育 60-69 1000个教育中400个名额
             * 不合格教育 40-59 1000个教育中70个名额
             */
            //教育需要的家庭教育和学校教育，学校教育需要花费银子，家庭教育需要消耗家长的精力而且 是7三开。
            //家庭教育消费是浮动的。

            throw new NotImplementedException();
        }

        private bool getSecondBabySuccess(byte employerAction)
        {
            //var k = -0.002;
            double a = 100;
            double b = 0;
            double c = 1;
            double success;
            switch (employerAction)
            {
                case 1:
                    {
                        //996;
                        //996会降低二胎率
                        // k = k * 1.1;
                        success = c * (Math.Sin(this.GovementPosition / a * Math.PI - Math.PI / 2) + 1) / 2 + b;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 2:
                    {
                        //onlyyourger 
                        //这里假设只招年轻人，对二胎影响很大。
                        b = b + 0.5;
                        c = 0.5;
                        success = c * (Math.Sin(this.GovementPosition / a * Math.PI - Math.PI / 2) + 1) / 2 + b;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 3:
                    {
                        //notmarriedfemail，降低了一胎率
                        // 
                        //起始未婚未育女性，也是降低了二胎率。
                        b = b + 0.5;
                        c = 0.5;
                        success = c * (Math.Sin(this.GovementPosition / a * Math.PI - Math.PI / 2) + 1) / 2 + b;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 4:
                    {
                        //newTechnology
                        //我们这里假设，采用新技术，对结婚率有影响。
                        success = c * (Math.Sin(this.GovementPosition / a * Math.PI - Math.PI / 2) + 1) / 2 + b;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 5:
                    {
                        //goAway
                        //我们这里假设，产业转移，对二胎率没有影响。
                        success = c * (Math.Sin(this.GovementPosition / a * Math.PI - Math.PI / 2) + 1) / 2 + b;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 6:
                    {
                        //improveWelfare
                        //我们这里假设，提高福利，对生二胎有促进作用。
                        c = 0.5;
                        b = -0.1;
                        success = c * (Math.Sin(this.GovementPosition / a * Math.PI - Math.PI / 2) + 1) / 2 + b;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                default:
                    {
                        return false;
                    }
            }
        }

        private bool getFirstBabySuccess(byte employerAction)
        {
            var k = -0.002;
            var d = 0.8;
            double success;
            switch (employerAction)
            {
                case 1:
                    {
                        //996;
                        //996会提升一胎率
                        k = k * 1.1;
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 2:
                    {
                        //onlyyourger 
                        //这里假设只招年轻人，对一胎率没有影响。
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 3:
                    {
                        //notmarriedfemail，降低了一胎率
                        //职场歧视未婚女性，对结婚率的影响。是结婚率降低
                        k = k * 0.6;
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 4:
                    {
                        //newTechnology
                        //我们这里假设，采用新技术，对结婚率没有影响。
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 5:
                    {
                        //goAway
                        //我们这里假设，产业转移，对结婚率没有影响。
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 6:
                    {
                        //improveWelfare
                        //我们这里假设，提高福利，对生一胎有促进作用。
                        d = d + 0.15;
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                default:
                    {
                        return false;
                    }
            }
        }

        private bool getMarriedSuccess(byte employerAction)
        {
            var k = 0.01;
            var d = 0.2;
            double success;
            switch (employerAction)
            {
                case 1:
                    {
                        //996;
                        //996能提高结婚率
                        k = k * 1.1;
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 2:
                    {
                        //onlyyourger 
                        //这里假设只招年轻人，对生育率没有影响。
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 3:
                    {
                        //notmarriedfemail 
                        //职场歧视未婚女性，对结婚率的影响。是结婚率降低
                        k = k * 0.5;
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 4:
                    {
                        //newTechnology
                        //我们这里假设，采用新技术，对结婚率没有影响。
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 5:
                    {
                        //goAway
                        //我们这里假设，产业转移，对结婚率没有影响。
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 6:
                    {
                        //improveWelfare
                        //我们这里假设，提高福利，对结婚率有促进作用。
                        d = d + 0.1;
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                default:
                    {
                        return false;
                    }
            }
        }

        private bool getLoveSuccess(byte employerAction)
        {
            //  this.rm.NextDouble();
            var k = -0.7 / 100;
            var d = 0.8;

            var success = k * this.GovementPosition + d;
            switch (employerAction)
            {
                case 1:
                    {
                        //996;
                        k = k * 0.9;
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 2:
                    {
                        //onlyyourger 
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 3:
                    {
                        //notmarriedfemail 
                        k = k * 0.4;
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 4:
                    {
                        //newTechnology
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 5:
                    {
                        //goAway
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                case 6:
                    {
                        d = 0.9;
                        success = k * this.GovementPosition + d;
                        if (success < this.rm.NextDouble())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }; break;
                default:
                    {
                        return false;
                    }
            }
        }

        private int GetIndexOffEmployerActions()
        {
            //下底是上底两倍的概率梯形。
            var x = 2.0 / 3.0 * countOfEmployerActions;
            var d = 2 - Math.Sqrt(4 - 2 * this.rm.NextDouble() / x);
            var indexResult = Convert.ToInt32(Math.Floor(countOfEmployerActions * d));
            if (indexResult < 0)
            {
                indexResult = 0;
            }
            else if (indexResult >= countOfEmployerActions)
            {
                indexResult = countOfEmployerActions - 1;
            }
            return indexResult;
        }

        internal void DealWithEmployerResult(byte result)
        {
            throw new NotImplementedException();
        }

        public void employerDo(Byte actionOfEmployer, Byte actionOf)
        {

        }


    }

    public class employee : employment
    {
    }


    public abstract class employment
    {
        public employment()
        {
            this._step = 0;
        }

        int _step = 0;

        internal bool next()
        {
            this._step++;
            if (this._step >= 20)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public int Step { get { return this._step; } }
    }

    public class employer : employment
    {
        public employer()
        {

        }
    }
}


