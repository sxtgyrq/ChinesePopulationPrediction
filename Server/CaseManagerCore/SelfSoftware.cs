using System;

namespace CaseManagerCore
{
    public class SelfSoftware : CaseManager
    {
        public string jiangzhangName = "";
        public string randomChinese = "";
        public SelfSoftware()
        {
            this.idPrevious = "ssw";
            this.rm = new Random(DateTime.Now.GetHashCode());
            this.rm.Next();
            this.rm.Next();
            this.step = 0;
            PassObj p = new PassObj()
            {
                ObjType = "html",
                msg = $"老师您好，现在是{DateTime.Now.ToString("yyyy年MM月dd日 HH:mm")}。我给您介绍一下本软件。",
                showContinue = true,
                showIsError = false,
                isEnd = false,
                ObjID = $"{idPrevious}{startId++}",
                styleStr = "msg"

            };
            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
            this.jiangzhangName = "身份证奖章";
        }

        public void Continue()
        {
            if (error)
            {
                //if (!this.isEnd)
                //{
                //    this.isEnd = true;
                //}
                this.msg = this.rightMsg;
                this.error = !error;
                if (this.score > 0)
                    this.score -= 10;
                if (this.score < 0)
                {
                    this.score = 0;
                }
                this.AddScore = 0;
                return;
            }
            else
            {
                this.score += this.AddScore;
                if (this.score > 100)
                {
                    this.score = 100;
                }
                this.AddScore = 0;
            }
            if (this.isEnd)
            {
                PassObj p = new PassObj()
                {
                    ObjType = "html_Error",
                    msg = $"重来",// $"<div style=\"text-align:center;border:1px solid #4cff00;padding:5px;margin:5px;background-color:yellow;color:red;\">老师，我讲错了，刷新网页我重新给你讲吧！！！</div>",
                    showContinue = false,
                    showIsError = false,
                    isEnd = true,
                    styleStr = "error"
                };

                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
                return;
            }
            else
            {
                this.step++;
                switch (step)
                {
                    case 1:
                        {
                            var rightMsg = "本软件是套问答系统";

                            string wrongMsg;
                            switch (this.rm.Next(2))
                            {
                                case 0:
                                    {
                                        wrongMsg = "本软件是套搜索引擎";
                                    }; break;
                                case 1:
                                    {
                                        wrongMsg = "本软件是门户网站";
                                    }; break;
                                default:
                                    {
                                        wrongMsg = "本软件是购物网站";
                                    }; break;
                            }
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 2:
                        {
                            switch (this.rm.Next(7))
                            {
                                case 0:
                                    {
                                        if (this.rm.Next(2) == 0)
                                        {

                                            var rightMsg = this.Important("如登录https://www.nyrq123.com,点击摩斯密码。最后答题得分大于等于98，可获得{0}。", "摩斯密码奖章");
                                            var wrongMsg = this.Important("如登录https://www.nyrq123.com,点击摩斯密码。最后答题得分大于等于98，可获得{0}。", "摩斯密码勋章");
                                            DealWithRightAndWrong(rightMsg, wrongMsg);
                                        }
                                        else
                                        {
                                            var rightMsg = this.Important("如登录https://www.nyrq123.com,点击摩斯密码。{0}", "最后答题得分大于等于98，可获得摩斯密码奖章。");
                                            var wrongMsg = this.Important("如登录https://www.nyrq123.com,点击摩斯密码。{0}", "只要答完题就可获得摩斯密码奖章。");
                                            DealWithRightAndWrong(rightMsg, wrongMsg);
                                        }
                                        this.jiangzhangName = "摩斯密码";
                                    }; break;
                                case 1:
                                    {
                                        if (this.rm.Next(2) == 0)
                                        {
                                            var rightMsg = Important("如登录https://www.nyrq123.com,点击{0}。最后答题得分大于等于98，可获得椭圆加密奖章。", "画曲线的那个图标");
                                            var wrongMsg = Important("如登录https://www.nyrq123.com,点击{0}。最后答题得分大于等于98，可获得椭圆加密奖章。", "颜色搭配最丰富的那个图标");
                                            DealWithRightAndWrong(rightMsg, wrongMsg);
                                        }
                                        else
                                        {
                                            var rightMsg = Important("如登录https://www.nyrq123.com,点击{0}。最后答题得分大于等于98，可获得椭圆加密奖章。", "第二行第二列的那个图标");
                                            var wrongMsg = Important("如登录https://www.nyrq123.com,点击{0}。最后答题得分大于等于98，可获得椭圆加密奖章。", "第三行第一列的那个图标");
                                            DealWithRightAndWrong(rightMsg, wrongMsg);
                                        }
                                        this.jiangzhangName = "椭圆加密";
                                    }; break;
                                case 2:
                                    {
                                        if (this.rm.Next(2) == 0)
                                        {
                                            var rightMsg = Important("如登录https://www.nyrq123.com,点击{0}。最后答题得分大于等于98，可获得杨钱奖章。", "带两个人头像的那个图标");
                                            var wrongMsg = Important("如登录https://www.nyrq123.com,点击{0}。最后答题得分大于等于98，可获得杨钱奖章。", "带只有一人的那个图标");
                                            DealWithRightAndWrong(rightMsg, wrongMsg);
                                        }
                                        else
                                        {
                                            var rightMsg = Important("如登录https://www.nyrq123.com,点击带两个人头像的那个图标。最后答题得分大于等于98，可获得{0}。", "杨钱奖章");
                                            var wrongMsg = Important("如登录https://www.nyrq123.com,点击带两个人头像的那个图标。最后答题得分大于等于98，可获得{0}。", "杨钱勋章");
                                            DealWithRightAndWrong(rightMsg, wrongMsg);
                                        }
                                        this.jiangzhangName = "杨钱";
                                    }; break;
                                case 3:
                                    {
                                        if (this.rm.Next(2) == 0)
                                        {
                                            var rightMsg = Important("如登录https://www.nyrq123.com,{0}。最后答题得分大于等于98，可获得黑曼巴奖章。", "点击黑曼巴图标，缅怀科比");
                                            var wrongMsg = Important("如登录https://www.nyrq123.com,{0}。最后答题得分大于等于90，可获得黑曼巴奖章。", "然后随便点");
                                            DealWithRightAndWrong(rightMsg, wrongMsg);
                                        }
                                        else
                                        {
                                            var rightMsg = Important("如登录https://www.nyrq123.com,点击{0}，缅怀科比。最后答题得分大于等于98，可获得黑曼巴奖章。", "黑曼巴图标");
                                            var wrongMsg = Important("如登录https://www.nyrq123.com,点击{0}，缅怀科比。最后答题得分大于等于98，可获得黑曼巴奖章。", "勋章按钮");
                                            DealWithRightAndWrong(rightMsg, wrongMsg);
                                        }
                                        this.jiangzhangName = "黑曼巴";
                                    }; break;
                                case 4:
                                    {
                                        var rightMsg = Important("如登录https://www.nyrq123.com,点击人造小太阳图标。最后答题得分大于等于{0}，可获得人造小太阳奖章。", "98");
                                        var wrongMsg = Important("如登录https://www.nyrq123.com,点击人造小太阳图标。最后答题得分大于等于{0}，可获得人造小太阳奖章。", "60");
                                        DealWithRightAndWrong(rightMsg, wrongMsg);
                                        this.jiangzhangName = "人造小太阳";
                                    }; break;
                                case 5:
                                    {
                                        var rightMsg = Important("如登录https://www.nyrq123.com,点击那个写着{0}的按钮。最后答题得分大于等于98，可获得元素周期表奖章。", "Ba");
                                        var wrongMsg = Important("如登录https://www.nyrq123.com,点击那个写着{0}的按钮。最后答题得分大于等于98，可获得元素周期表奖章。", "Fe");
                                        DealWithRightAndWrong(rightMsg, wrongMsg);
                                        this.jiangzhangName = "元素周期表";
                                    }; break;
                                default:
                                    {
                                        var rightMsg = Important("如登录https://www.nyrq123.com,点击身份证。{0}，可获得身份证奖章。", "完成一道题");
                                        var wrongMsg = Important("如登录https://www.nyrq123.com,点击身份证。{0}，可获得身份证奖章。", "最后答题得分大于等于98");
                                        this.jiangzhangName = "身份证";
                                    }; break;
                            }
                            //step = 1;
                        }; break;
                    case 3:
                        {
                            DealWithMsg("完成一些任务，可以获得金币但并不是所有任务完成后都能获得金币奖励。获得金币的多少，由总体上任务完成的频率决定。");
                            switch (rm.Next(2))
                            {
                                case 0:
                                    {
                                        this.step = 3000;
                                    }; break;
                                case 1:
                                    {
                                        this.step = 4000;
                                    }; break;
                                default:
                                    {
                                        throw new Exception("错误的step");
                                    }
                            }
                        }; break;
                    case 4:
                        {
                            var rightMsg = this.Important("可以通过在市场里收购别人的奖章和勋章。进行奖章交易的媒介是{0}。", "金币");
                            var wrongMsg = this.Important("可以通过在市场里收购别人的奖章和勋章。进行奖章交易的媒介是{0}。", "稀土");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                            this.step = 5000;
                        }; break;
                    case 5:
                        {
                            var rightMsg = this.Important("勋章是由{0}和奖章合成的", "稀土");
                            var wrongMsg = this.Important("勋章是由{0}和奖章合成的", "金币");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                            this.step = 6000;
                        }; break;
                    case 6:
                        {
                            var rightMsg = this.Important("获得的勋章、奖章、金币可以用来申请匿贝尔奖学金，匿贝尔奖学金是实打实的{0}", "比特币");
                            var wrongMsg = this.Important("获得的勋章、奖章、金币可以用来申请匿贝尔奖学金，匿贝尔奖学金是实打实的{0}", "美元");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 7:
                        {
                            var rightMsg = this.Important("打开匿贝尔界面，如果状态是{0}，继续。", "可申请");
                            var wrongMsg = this.Important("打开匿贝尔界面，如果状态是{0}，继续。", "已结束");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 8:
                        {
                            var rightMsg = this.Important("对{0}签名并执行就可以申请奖学金", "申请材料表格中的某一内容");
                            var wrongMsg = this.Important("对{0}签名并执行就可以申请奖学金", "奖学金存放的比特币地址");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;

                    case 9:
                        {
                            if (this.getRewardSuccess)
                            {
                                this.step = 500;
                            }
                            else
                            {
                                if (this.score >= 98)
                                {
                                    InputAddress();
                                    this.step = 500;
                                }
                                else
                                {
                                    var msg = $"您的得分为{this.score}。小于{98}将不能获得勋章，继续努力吧！少年！！！";
                                    DealWithMsg(msg);
                                    this.step = 500;
                                }
                            }
                        }; break;

                    case 500:
                        {
                            this.CloseSocket();
                        }; break;
                    /*
                     * 3000
                     * 奖章可送给别人
                     */
                    case 3001:
                        {
                            var rightMsg = Important("获得的奖章{0}", "可以送给别人");
                            var wrongMsg = Important("获得的奖章{0}", "只能查看");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                            switch (rm.Next(2))
                            {
                                case 0:
                                    {
                                        this.step = 3100;
                                    }; break;
                                default:
                                    {
                                        this.step = 3200;
                                    }; break;
                            }
                        }; break;
                    case 3101:
                        {
                            DealWithMsg("所谓热令，是将私钥在web端进行展示和签名，在浏览器端使用理论上有泄露的风险。（作者这里呼吁大家直接使用safiri或chrome）。其他有可能有泄露私钥的危险。私钥即权限。丢了私钥就丢了全部。不过在使用safiri或chrome情况下，这种概率很低。");

                        }; break;
                    case 3102:
                        {
                            var rightMsg = Important($"用热令将获得的{this.jiangzhangName}奖章送给别人（如1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg），第一步打开{{0}}", "https://www.nyrq123.com");
                            var wrongMsg = Important($"用热令将获得的{this.jiangzhangName}奖章送给别人（如1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg），第一步打开{{0}}", "https://www.google.com");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 3103:
                        {
                            var rightMsg = Important("第二步点击{0}", "权限");
                            var wrongMsg = Important("第二步点击{0}", "勋章");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 3104:
                        {
                            var rightMsg = Important("第三步如果您有私钥，那么，请在私钥栏输入；如果没有私钥，那么，请点击{0}按钮，并牢记私钥。最好拿本记住，丢了找不回来的。", "获取新私钥");
                            var wrongMsg = Important("第三步如果您有私钥，那么，请在私钥栏输入；如果没有私钥，那么，请点击{0}按钮，并牢记私钥。最好拿本记住，丢了找不回来的。", "执行");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 3105:
                        {
                            var xx = $"将{this.rm.Next(1, 200)}枚{this.jiangzhangName}奖章送给1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg";
                            var rightMsg = Important("第四步，在{0}输入{1}", "命令正文输入框", xx);
                            var wrongMsg = Important("第四步，在{0}输入{1}", "消息签名输入框", xx);
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 3106:
                        {
                            DealWithMsg("第五步，点击签名按钮");
                        }; break;
                    case 3107:
                        {
                            DealWithMsg(this.Important("第六步，点击执行按钮。那么，{0},赠予过程结束。", "就将奖章送给了1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg"));
                            this.step = 3;
                        }; break;

                    case 3201:
                        {
                            DealWithMsg("所谓冷令，是将私钥用安全的钱包软件生成和打开。私钥不需要出现在web端，只要您操作得当，别人永远没有机会知道您的私钥是什么。通过往钱包中输入消息，用钱包对消息(utf-8编码)进行签名。将签名后的消息发送至网络执行即可");
                        }; break;
                    case 3202:
                        {
                            var rightMsg = Important($"用冷令将获得的{this.jiangzhangName}奖章送给别人（如1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg），第一步打开{{0}}", "https://www.nyrq123.com");
                            var wrongMsg = Important($"用冷令将获得的{this.jiangzhangName}奖章送给别人（如1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg），第一步打开{{0}}", "https://www.google.com");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 3203:
                        {
                            var rightMsg = Important("第二步点击{0}", "权限");
                            var wrongMsg = Important("第二步点击{0}", "勋章");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 3204:
                        {
                            var rightMsg = Important("第三步点击{0}", "冷令");
                            var wrongMsg = Important("第三步点击{0}", "主页");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 3205:
                        {
                            var rightMsg = Important("第四步在第一个输入框输入{0}", "比特币地址");
                            var wrongMsg = Important("第四步在第一个输入框输入{0}", "比特币私钥");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 3206:
                        {
                            var rightMsg = Important("第五步点击{0}按钮", "获取命令号");
                            var wrongMsg = Important("第五步点击{0}按钮", "执行");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 3207:
                        {
                            var xx = $"将{this.rm.Next(1, 200)}枚{this.jiangzhangName}奖章送给1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg";
                            var msg = Important("第六步，当命令正文输入框由灰转白后，在命令输入框中输入{0}", xx);
                            DealWithMsg(msg);
                        }; break;
                    case 3208:
                        {
                            var msg = $"第七步，用钱包对命令中的文字进行签名,要支持utf-8格式的签名。";
                            DealWithMsg(msg);
                        }; break;
                    case 3209:
                        {
                            var rightMsg = Important("第八步,{0}。", "按照消息签名内格式进行填写签名");
                            var wrongMsg = Important("第八步,{0}。", "只是将签好名的Base64Sign输入即可。");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 3210:
                        {
                            DealWithMsg("最后一步点击执行。那么，奖章赠予完成。");
                            this.step = 3;
                        }; break;

                    /*
                   * 4000
                   * 奖章可进入市场进行交易
                   * 
                   */
                    case 4001:
                        {
                            var rightMsg = Important("获得的奖章{0}", "可进入市场交易");
                            var wrongMsg = Important("获得的奖章{0}", "只能查看");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                            switch (rm.Next(2))
                            {
                                case 0:
                                    {
                                        this.step = 4100;
                                    }; break;
                                default:
                                    {
                                        this.step = 4200;
                                    }; break;
                            }
                        }; break;
                    case 4101:
                        {
                            DealWithMsg("所谓热令，是将私钥在web端进行展示和签名，在浏览器（作者这里呼吁大家使用safiri或chrome）不安全的情况下，有可能泄露私钥的危险。私钥即权限。丢了私钥就丢了全部。不过在使用safiri或chrome情况下，这种概率很低。");

                        }; break;
                    case 4102:
                        {
                            var rightMsg = Important($"用热令将获得的{this.jiangzhangName}奖章在市场内进行定价交易，第一步打开{{0}}", "https://www.nyrq123.com");
                            var wrongMsg = Important($"用热令将获得的{this.jiangzhangName}奖章在市场内进行定价交易，第一步打开{{0}}", "https://www.google.com");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 4103:
                        {
                            var rightMsg = Important("第二步点击{0}", "权限");
                            var wrongMsg = Important("第二步点击{0}", "勋章");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 4104:
                        {
                            var rightMsg = Important("第三步，如果您有私钥，那么，请在私钥栏输入；如果没有私钥，那么，请点击{0}按钮，并牢记私钥。最好拿本记住，丢了找不回来的。", "获取新私钥");
                            var wrongMsg = Important("第三步，如果您有私钥，那么，请在私钥栏输入；如果没有私钥，那么，请点击{0}按钮，并牢记私钥。最好拿本记住，丢了找不回来的。", "执行");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 4105:
                        {
                            // "^[将]{0,1}"+regexParameter.countofxunzhang+"枚"+regexParameter.xunzhangType.regex+ "章以单价"+regexParameter.goldValue+"金币出售"
                            string xx = $"{(this.rm.Next(2) == 0 ? "" : "将")}{this.rm.Next(1, 200)}枚{this.jiangzhangName}奖章以单价{(this.rm.Next(5) == 0 ? (this.rm.Next(1, 4).ToString()) : (this.rm.NextDouble() * 4 + 0.01).ToString("f2"))}金币出售";
                            var rightMsg = Important("第四步，在{0}输入{1}", "命令正文输入框", xx);
                            var wrongMsg = Important("第四步，在{0}输入{1}", "消息签名输入框", xx);
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 4106:
                        {
                            DealWithMsg("第五步，点击签名按钮");
                        }; break;
                    case 4107:
                        {
                            DealWithMsg("第六步，点击执行按钮。那么，奖章送给了1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg");
                            this.step = 3;
                        }; break;

                    case 4201:
                        {
                            DealWithMsg("所谓冷令，是将私钥用安全的钱包软件生成和打开。私钥不需要出现在web端，只要您操作得当，别人永远没有机会知道您的私钥是什么。通过往钱包中输入消息，用钱包对消息(utf-8编码)进行签名。签名后,将消息与签名按格式发送至网络即可。");
                        }; break;
                    case 4202:
                        {
                            var rightMsg = Important($"用冷令将获得的{this.jiangzhangName}奖章在市场内进行定价交易，第一步打开{{0}}", "https://www.nyrq123.com");
                            var wrongMsg = Important($"用冷令将获得的{this.jiangzhangName}奖章在市场内进行定价交易，第一步打开{{0}}", "https://www.google.com");
                            //var rightMsg = Important($"用冷令将获得的{this.jiangzhangName}奖章送给别人（如1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg），第一步打开{{0}}", "https://www.nyrq123.com");
                            //var wrongMsg = Important($"用冷令将获得的{this.jiangzhangName}奖章送给别人（如1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg），第一步打开{{0}}", "https://www.google.com");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 4203:
                        {
                            var rightMsg = Important("第二步点击{0}", "权限");
                            var wrongMsg = Important("第二步点击{0}", "勋章");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 4204:
                        {
                            var rightMsg = Important("第三步点击{0}", "冷令");
                            var wrongMsg = Important("第三步点击{0}", "主页");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 4205:
                        {
                            var rightMsg = Important("第四步，在第一个输入框输入{0}", "比特币地址");
                            var wrongMsg = Important("第四步，在第一个输入框输入{0}", "比特币私钥");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 4206:
                        {
                            var rightMsg = Important("第五步，点击{0}按钮", "获取命令号");
                            var wrongMsg = Important("第五步，点击{0}按钮", "执行");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 4207:
                        {
                            string xx = $"{(this.rm.Next(2) == 0 ? "" : "将")}{this.rm.Next(1, 200)}枚{this.jiangzhangName}奖章以单价{(this.rm.Next(5) == 0 ? (this.rm.Next(1, 4).ToString()) : (this.rm.NextDouble() * 4 + 0.01).ToString("f2"))}金币出售";
                            var msg = this.Important("第六步，当命令正文输入框由灰转白后，在命令输入框中输入{0}", xx);
                            DealWithMsg(msg);
                        }; break;
                    case 4208:
                        {
                            var msg = $"第七步，用钱包对命令中的文字进行签名,要支持utf-8格式的签名。";
                            DealWithMsg(msg);
                        }; break;
                    case 4209:
                        {
                            var rightMsg = Important("第八步,{0}。", "按照消息签名内格式进行填写签名");
                            var wrongMsg = Important("第八步,{0}。", "只是将签好名的Base64Sign输入即可。");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 4210:
                        {
                            DealWithMsg("最后一步点击执行。即完成了赠予。");
                            this.step = 3;
                        }; break;

                    //case 5001: { }; break;

                    case 5001:
                        {
                            var msg = "以单价" + (this.rm.NextDouble() * 4 + 0.01).ToString("f2") + "金币收购" + this.rm.Next(1, 333) + "枚" + this.jiangzhangName + (this.rm.Next(2) == 0 ? "勋" : "奖") + "章";
                            DealWithMsg(Important("想要收购市场里出售的勋章和奖章，无论用热令还是冷令，执行命令{0}即可", msg));
                            this.step = 4;
                        }; break;

                    case 6001:
                        {
                            var msg = $"将{this.rm.Next(1, 1000)}枚" + this.jiangzhangName + "奖章转化为勋章";
                            DealWithMsg(Important("想要合成勋章，，无论用热令还是冷令，执行命令{0}即可", msg));
                        }; break;
                    case 6002:
                        {
                            var rightMsg = Important("奖章转化勋章时，要消耗稀土元素，1枚勋章需要{0}稀土。", "1kg");
                            var wrongMsg = Important("奖章转化勋章时，要消耗稀土元素，1枚勋章需要{0}稀土。", $"{this.rm.Next(2, 5)}kg");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 6003:
                        {
                            var msg = "通过微信打赏，可以获得稀土。如何获得稀土？";
                            DealWithMsg(msg);
                        }; break;
                    case 6004:
                        {
                            var rightMsg = Important($"第一步打开{{0}}", "https://www.nyrq123.com");
                            var wrongMsg = Important($"第一步打开{{0}}", "https://www.google.com");
                            //var rightMsg = Important($"用冷令将获得的{this.jiangzhangName}奖章送给别人（如1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg），第一步打开{{0}}", "https://www.nyrq123.com");
                            //var wrongMsg = Important($"用冷令将获得的{this.jiangzhangName}奖章送给别人（如1NyrqNVai7uCP4CwZoDfQVAJ6EbdgaG6bg），第一步打开{{0}}", "https://www.google.com");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 6005:
                        {
                            var rightMsg = Important("第二步点击{0}", "帮助");
                            var wrongMsg = Important("第二步点击{0}", "匿贝尔");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 6006:
                        {
                            var rightMsg = Important("第三步，找到生成{0}功能", "md5");
                            var wrongMsg = Important("第三步，找到生成{0}功能", "RSA-256");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 6007:
                        {
                            this.randomChinese = getRandomStr(ref this.rm);
                            var rightMsg = Important("第四步，在中文暗语栏输入{0}，如{1}。", "10至20个汉字", this.randomChinese);
                            var wrongMsg = Important("第四步，在中文暗语栏输入{0}，如{1}", "10至20个英文单词", "Hello Nyrq Hello World Hello evyryone Hello Bill Gates Hell");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 6008:
                        {
                            DealWithMsg(Important("再点击校验，确认此码未被占用或者被绑定。如已绑定，确认是自己想要充值的地址，那么此暗语可用。如未绑定，请牢记此暗语明文{0}", this.randomChinese));
                        }; break;
                    case 6009:
                        {
                            var rightMsg = Important("微信扫码打赏，备注为{0}", "中文暗语的utf-8格式的32位md5加密的中间30位");
                            var wrongMsg = Important("微信扫码打赏，备注为{0}", "中文暗语的utf-8格式的32位md5加密的前30位");
                            DealWithRightAndWrong(rightMsg, wrongMsg);
                        }; break;
                    case 6010:
                        {
                            DealWithMsg("一般情况下，白天最多等候4小时，晚上最多等候12小时，您的暗语密文即会被管理员入库！");
                        }; break;
                    case 6011:
                        {
                            DealWithMsg("待管理人员入库后，");
                        }; break;
                    case 6012:
                        {
                            //   static string rexText1 = "^用暗号的明文(?<plaintext>[\\u4e00-\\u9fa5]{10,20})换稀土$";
                            DealWithMsg("点击权限");
                        }; break;
                    case 6013:
                        {
                            var chineseStyle = $"<span style =\"color: orange\">{randomChinese}</span>";
                            var msg = $"第{this.rm.Next(1, 300)}号主人令是用暗号的明文{chineseStyle}换稀土";
                            string rexText1 = Important("执行命令{0},执行命令的地址将和该暗号绑定。并且，将获得相应的稀土。", msg);
                            DealWithMsg(rexText1);
                            this.step = 5;
                        }; break;

                    default:
                        {
                            //throw new Exception($"{step}");
                            CloseSocket();
                        }; break;

                }

                //if (this.step <= this.orderGet.Count)
                //{
                //    var indexValue = this.orderGet[this.step - 1];
                //    XmlDocument xmlDoc = new XmlDocument();
                //    xmlDoc.Load(filePath);
                //    var rightMsg = string.Format(xmlDoc.ChildNodes[2].ChildNodes[0].ChildNodes[indexValue]["prmblem"].InnerText,
                //      styleText(xmlDoc.ChildNodes[2].ChildNodes[0].ChildNodes[indexValue]["answer"]["right"].InnerText));
                //    var wrongMsg = string.Format(xmlDoc.ChildNodes[2].ChildNodes[0].ChildNodes[indexValue]["prmblem"].InnerText,
                //                      styleText(xmlDoc.ChildNodes[2].ChildNodes[0].ChildNodes[indexValue]["answer"]["wrong"].InnerText));

                //    DealWithRightAndWrong(rightMsg, wrongMsg);
                //    xmlDoc = null;

                //}
                //else if (this.step == this.orderGet.Count + 1)
                //{
                //    DealWithMsg($"你的最终得分是{this.score}.");
                //}
                //else if (this.step > this.orderGet.Count + 1 && this.step < 500)
                //{
                //    if (this.getRewardSuccess)
                //    {
                //        this.step = 500;
                //    }
                //    else
                //    {
                //        if (this.score >= minScore)
                //        {
                //            InputAddress();
                //            this.step = 500;
                //        }
                //        else
                //        {
                //            var msg = $"您的得分为{this.score}。小于{minScore}将不能获得勋章，继续努力吧！少年！！！";
                //            DealWithMsg(msg);
                //            this.step = 500;
                //        }
                //    }
                //}
                //else if (this.step >= 500)
                //{

                //    CloseSocket();
                //}

            }
        }


        //public void generateRandomChinese()
        //{
        //    var common = this.rm.Next(0x4e00, 0xa000);
        //    var rare = this.rm.Next(0x3400, 0x4e00);

        //}
        public static string getRandomStr(ref Random rm)
        {
            var length = rm.Next(10, 21);
            var result = "";
            for (var i = 0; i < length; i++)
            {
                var charV = Convert.ToChar(rm.Next(0x4e00, 0x9fa5));
                result += charV;
            }
            return result;
        }

        //MySql.BaseItem b = new MySql.BaseItem("identitycard");
        //    return this.SaveAddress(address, b.xunzhangName, 0, b.AddAddressValue);
        const int minScore = 98;
        public string SaveAddress(string address)
        {
            MysqlCore.BaseItem b = new MysqlCore.BaseItem("selfsoftware");
            //b.AddAddressValue(address, out moneycountAddV);
            return this.SaveAddress(address, b.xunzhangName, minScore, b.AddAddressValue);
        }
    }


}
