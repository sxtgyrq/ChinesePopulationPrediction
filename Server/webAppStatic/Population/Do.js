var pMain;
var musicManger = {
    musicBackground: null,
    loop: function () {
        if (musicManger.checkIsPlay(musicManger.musicBackground)) {
        }
        else {
            musicManger.musicBackground.play();
        }
        //console.log('currentTime', musicManger.musicBackground.currentTime);

    },
    load: function () {
        musicManger.musicBackground = document.getElementById("musicBackground");
        if (musicManger.musicBackground != null) {
            musicManger.musicBackground.load();
        };

    },
    checkIsPlay: function togglePause(audioC) {
        return !!(audioC.currentTime > 0 && !audioC.paused && !audioC.ended && audioC.readyState > 2);
    },
    showWord: function () {
        requestAnimationFrame(musicManger.showWord);
        if (musicManger.checkIsPlay(musicManger.musicBackground)) {
            //console.log('currentTime', musicManger.musicBackground.currentTime);
            var t = musicManger.musicBackground.currentTime;

            var marginLeft = t % 50 / 50.0 * 300;
            document.getElementById('imgOfIcon').style.marginLeft = `calc(${marginLeft}%)`;

            if (t <= 0) {
                setNotify('');
            }
            else if (t < 6.266) {
                setNotify('真的很难想象二十年过得这么快');
            }
            else if (t < 9.6) {
                setNotify('这的确太疯狂了');
            }
            else if (t < 13.7) {
                setNotify('能够站在这里和你们讲话');
            }
            else if (t < 16.46) {
                setNotify('背后站着我曾经的队友');
            }
            else if (t < 20.9) {
                setNotify('我们一起感恩回味走过的日子');
            }
            else if (t < 23.1) {
                setNotify('曾经我们经历过起起伏伏');
            }
            else if (t < 26.4) {
                setNotify('我绝得最重要的是');
            }
            else if (t < 28.7) {
                setNotify('我们荣辱与共坚持了过来');
            }
            else if (t < 33) {
                setNotify('');
            }
            else if (t < 37) {
                setNotify('从小我就是湖人的死忠球迷');
            }
            else if (t < 40.5) {
                setNotify('在这里打过球的人');
            }
            else if (t < 44) {
                setNotify('我都知道他们的一切');
            }
            else if (t < 45.9) {
                setNotify('能够被选中并被交易到湖人');
            }
            else if (t < 50) {
                setNotify('并整整效力20年');
            }
            else if (t < 53.2) {
                setNotify('没有比这更好的故事了');
            }
            else if (t < 55.7) {
                setNotify('');
            }
            else if (t < 57.6) {
                setNotify('但让我感到自豪的');
            }
            else if (t < 61.4) {
                setNotify('并不是总冠军');
            }
            else if (t < 61.4) {
                setNotify('并不是总冠军');
            }
            else if (t < 63.7) {
                setNotify('而是球队处于低谷那些年');
            }
            else if (t < 66.5) {
                setNotify('因为我们没有逃避');
            }
            else if (t < 69) {
                setNotify('我们客服了那些困难并拿到了总冠军');
            }
            else if (t < 71) {
                setNotify('我们用正确的方式完成了目标');
            }
            else if (t < 74) {
                setNotify('');
            }
            else if (t < 76.9) {
                setNotify('我现在能做的就是感谢你们');
            }
            else if (t < 81.1) {
                setNotify('谢谢你们这些年来的鼓励和支持');
            }
            else if (t < 82.5) {
                setNotify('谢谢你们的鼓励');
            }
            else if (t < 88) {
                setNotify('');
            }
            else if (t < 89.2) {
                setNotify('我知道你们觉得好笑的是什么');
            }
            else if (t < 92.6) {
                setNotify('今天一整晚我都笑不停是因为');
            }
            else if (t < 97.6) {
                setNotify('这二十年来每个人都曾经要让我传球');
            }
            else if (t < 100.2) {
                setNotify('但今晚却大喊不要传');
            }
            else if (t < 105.2) {
                setNotify('');
            }
            else if (t < 108) {
                setNotify('这是一个精彩的夜晚');
            }
            else if (t < 109.8) {
                setNotify('真的不敢相信这一切都结束了');
            }
            else if (t < 113) {
                setNotify('你们的支持我会一直铭记于心');
            }
            else if (t < 116.3) {
                setNotify('真的非常非常感激');
            }
            else if (t < 118.5) {
                setNotify('对你们的感情无法用三言两语形容');
            }
            else if (t < 122) {
                setNotify('非常感谢你们');
            }
            else if (t < 124) {
                setNotify('我爱你们');
            }
            else if (t < 131) {
                setNotify('');
            }
            else if (t < 134.5) {
                setNotify('还有我要感谢我的妻子瓦妮莎');
            }
            else if (t < 135.9) {
                setNotify('我的女儿娜塔莉亚和吉安娜');
            }
            else if (t < 138.6) {
                setNotify('谢谢你们为我的付出');
            }
            else if (t < 142.2) {
                setNotify('当我在球馆专心训练时');
            }
            else if (t < 144.8) {
                setNotify('瓦妮莎你一手撑起了这个家');
            }
            else if (t < 146.8) {
                setNotify('你对我的这份恩情');
            }
            else if (t < 148.4) {
                setNotify('或许一生都感谢不完');
            }
            else if (t < 150.8) {
                setNotify('所以我从心里感激你');
            }
            else if (t < 154) {
                setNotify('');
            }
            else if (t < 155.8) {
                setNotify('我还能说什么呢？');
            }
            else if (t < 157.2) {
                setNotify('黑曼巴要走了');
            }
            else if (t < 157.2) {
                setNotify('');
            }
        }
    }
};
var setNotify = function (msg) {
    document.getElementById('notifyMsg').innerText = msg;
}
$(document).ready(function () {
    pMain = new Program();

    pMain.dealWithDataFunction = function (evt) {
        dealWithData(evt.data);
    }

    pMain.functionAfterSocketConnected = function () {

        //$.get('https://www.nyrq123.com/api/gettoken/', { dt: Date.now(), action: 'Population' }, function (data) {
        //    var obj = JSON.parse(data);
        //    pMain.socket.send(obj.actonTime + '_' + obj.actionCommand + '_' + obj.sign);
        //    // draw();

        //    //musicManger.musicBackground = document.getElementById("musicBackground");
        //    //if (musicManger.musicBackground != null) {
        //    //    musicManger.musicBackground.load();
        //    //    musicManger.showWord();
        //    //};
        //});
        pMain.socket.send('20200712000000' + '_' + 'Population' + '_' + 'sign');
    }
    pMain.connectWebsocket();

    //MathJax.Hub.Config({
    //    tex2jax: { inlineMath: [['$', '$'], ['\\(', '\\)']] }
    //});
});
var employeeState = null;
var employeeActions = [];
var employeeEducateAction = '';
var dealWithData = function (strData) {

    var objGet = JSON.parse(strData);
    console.log('Command', strData);
    switch (objGet.ObjType) {
        case 'selectRole':
            {
                selectRole();
            }; break;
        case 'employee-notify':
            {
                showBtn(objGet.showContinue, objGet.showIsError, false);
                var objPass = JSON.parse(objGet.msg);
                {
                    var divCreateNew = document.createElement('div');
                    divCreateNew.innerHTML = objPass.ageDisplay;
                    divCreateNew.id = objGet.ObjID + '_a';

                    // MathJax.Hub.Queue(["Typeset", MathJax.Hub, divCreateNew]);
                    var askAndAnswer = document.getElementById('askAndAnswer');
                    divCreateNew.classList.add('msg');
                    askAndAnswer.appendChild(divCreateNew);
                    askAndAnswer.scrollTop = askAndAnswer.scrollHeight;
                }
                {
                    var divCreateNew = document.createElement('div');
                    divCreateNew.innerHTML = objPass.yearDisplay;
                    divCreateNew.id = objGet.ObjID + '_b';
                    divCreateNew.classList.add('msg');
                    // MathJax.Hub.Queue(["Typeset", MathJax.Hub, divCreateNew]);
                    var askAndAnswer = document.getElementById('askAndAnswer');

                    askAndAnswer.appendChild(divCreateNew);
                    askAndAnswer.scrollTop = askAndAnswer.scrollHeight;
                }
                for (var i = 0; i < objPass.childrenInfo.length; i++) {

                    var divCreateNew = document.createElement('div');
                    divCreateNew.innerHTML = objPass.childrenInfo[i];
                    divCreateNew.id = objGet.ObjID + '_c' + i;
                    divCreateNew.classList.add('msg');
                    var askAndAnswer = document.getElementById('askAndAnswer');

                    askAndAnswer.appendChild(divCreateNew);
                    askAndAnswer.scrollTop = askAndAnswer.scrollHeight;
                }
                employeeState = objPass.state;
                employeeActions = objPass.actions;
                employeeEducateAction = objPass.educateAction;
            }; break;
        case 'employee-notify-next':
            {
                showBtn(objGet.showContinue, objGet.showIsError, false);
                var objPass = JSON.parse(objGet.msg);
                {
                    var divCreateNew = document.createElement('div');
                    divCreateNew.innerHTML = objPass.ageDisplay;
                    divCreateNew.id = objGet.ObjID + '_a';

                    // MathJax.Hub.Queue(["Typeset", MathJax.Hub, divCreateNew]);
                    var askAndAnswer = document.getElementById('askAndAnswer');
                    divCreateNew.classList.add('msg');
                    askAndAnswer.appendChild(divCreateNew);
                    askAndAnswer.scrollTop = askAndAnswer.scrollHeight;
                }
                {
                    var divCreateNew = document.createElement('div');
                    divCreateNew.innerHTML = objPass.yearDisplay;
                    divCreateNew.id = objGet.ObjID + '_b';
                    divCreateNew.classList.add('msg');
                    // MathJax.Hub.Queue(["Typeset", MathJax.Hub, divCreateNew]);
                    var askAndAnswer = document.getElementById('askAndAnswer');

                    askAndAnswer.appendChild(divCreateNew);
                    askAndAnswer.scrollTop = askAndAnswer.scrollHeight;
                }
                for (var i = 0; i < objPass.childrenInfo.length; i++) {

                    var divCreateNew = document.createElement('div');
                    divCreateNew.innerHTML = objPass.childrenInfo[i];
                    divCreateNew.id = objGet.ObjID + '_c' + i;
                    divCreateNew.classList.add('msg');
                    var askAndAnswer = document.getElementById('askAndAnswer');

                    askAndAnswer.appendChild(divCreateNew);
                    askAndAnswer.scrollTop = askAndAnswer.scrollHeight;
                }
                for (var i = 0; i < objPass.childrenInfo.length; i++) {

                    var divCreateNew = document.createElement('div');
                    divCreateNew.innerHTML = objPass.childrenInfo[i];
                    divCreateNew.id = objGet.ObjID + '_c' + i;
                    divCreateNew.classList.add('msg');
                    var askAndAnswer = document.getElementById('askAndAnswer');

                    askAndAnswer.appendChild(divCreateNew);
                    askAndAnswer.scrollTop = askAndAnswer.scrollHeight;
                }
                for (var i = 0; i < objPass.notifyMsgs.length; i++)
                {
                    var divCreateNew = document.createElement('div');
                    divCreateNew.innerHTML = objPass.notifyMsgs[i];
                    divCreateNew.id = objGet.ObjID + '_n_' + i;
                    divCreateNew.classList.add('msg');
                    var askAndAnswer = document.getElementById('askAndAnswer');

                    askAndAnswer.appendChild(divCreateNew);
                    askAndAnswer.scrollTop = askAndAnswer.scrollHeight;
                }
                employeeState = objPass.state;
                employeeActions = objPass.actions;
                employeeEducateAction = objPass.educateAction;
            }; break;
        case 'employee-action':
            {
                employeeState = objGet.state;
                employeeActions = objGet.actions;
                employeeEducateAction = objGet.educateAction;
                selectStrategy();
            }; break;
    };
}

var showBtn = function (showContinue, showIsError, isEnd) {
    document.getElementById('btns').innerHTML = '';
    if (showContinue) {
        var btnContinue = document.createElement('div');
        btnContinue.id = 'btnContinue';
        btnContinue.className = 'button';
        btnContinue.innerText = '下一年';

        btnContinue.onclick = function () {
            pMain.socket.send('Next');
            //musicManger.loop();
        };
        document.getElementById('btns').appendChild(btnContinue);
    }
    if (showIsError) {
        var btnErrorRecovery = document.createElement('div');
        btnErrorRecovery.id = 'btnErrorRecovery';
        btnErrorRecovery.className = 'button';
        btnErrorRecovery.innerText = '策略';
        btnErrorRecovery.style.backgroundColor = 'orangered';
        btnErrorRecovery.onclick = function () {
            //pMain.socket.send('2');
            //musicManger.loop();
            if (employeeState != null) {
                selectStrategy();
            }
        };
        document.getElementById('btns').appendChild(btnErrorRecovery);
    }
    //if (isEnd) {
    //    pMain.socket.close();
    //}
}

var drawLine = function (ctx, width, height, startPercentX, startPercentY, endPercentX, endPercentY, color, linewidth) {
    if (color === null) { color = 'rgba(255,0,0,0.5)'; }
    if (linewidth === null) { linewidth = 1 };
    ctx.beginPath();
    ctx.moveTo(startPercentX * width, (1 - startPercentY) * height);
    ctx.lineTo(endPercentX * width, (1 - endPercentY) * height);
    ctx.strokeStyle = color;
    ctx.lineWidth = linewidth;
    ctx.stroke();
    //ctx.closePath();
}

var drawDot = function (ctx, width, height, x, y, r, color) {
    if (color === null) { color = 'rgba(255,0,0,0.5)'; }
    ctx.beginPath();
    var x2 = x * width;
    var y2 = (1 - y) * height;
    ctx.lineWidth = r;
    ctx.arc(x2, y2, r, 0, Math.PI * 2, color);
    ctx.stroke();
}

var promptF = function (msg1, msg2, f) {
    document.getElementById('dialog').hidden = false;
    document.getElementById('dialog').innerHTML = '';
    var div1 = document.createElement('div');
    div1.innerText = msg1;
    div1.className = 'dialogPromtfirstchild';

    var input1 = document.createElement('input');
    input1.value = msg2;
    input1.type = 'text';
    input1.className = 'dialogPromtsecondchild';
    input1.id = 'btcAddressInput'

    var input2 = document.createElement('input');
    input2.value = '提交';
    input2.type = 'button';
    input2.className = 'dialogPromtthirdchild';

    document.getElementById('dialog').appendChild(div1);
    document.getElementById('dialog').appendChild(input1);
    document.getElementById('dialog').appendChild(input2);

    input2.onclick = function () {
        var address = document.getElementById('btcAddressInput').value;
        pMain.socket.send(address);
        localStorage.setItem('btcAddress', address);
        document.getElementById('dialog').innerHTML = '';
        document.getElementById('dialog').hidden = true;
    }
}


function KeyboardInputManager() {
    this.events = {};

    if (window.navigator.msPointerEnabled) {
        //Internet Explorer 10 style
        this.eventTouchstart = "MSPointerDown";
        this.eventTouchmove = "MSPointerMove";
        this.eventTouchend = "MSPointerUp";
    } else {
        this.eventTouchstart = "touchstart";
        this.eventTouchmove = "touchmove";
        this.eventTouchend = "touchend";
    }

    this.listen();
}

KeyboardInputManager.prototype.on = function (event, callback) {
    if (!this.events[event]) {
        this.events[event] = [];
    }
    this.events[event].push(callback);
};

KeyboardInputManager.prototype.emit = function (event, data) {
    var callbacks = this.events[event];
    if (callbacks) {
        callbacks.forEach(function (callback) {
            callback(data);
        });
    }
};

KeyboardInputManager.prototype.listen = function () {
    var self = this;

    var map = {
        38: 0, // Up
        39: 1, // Right
        40: 2, // Down
        37: 3, // Left
        75: 0, // Vim up
        76: 1, // Vim right
        74: 2, // Vim down
        72: 3, // Vim left
        87: 0, // W
        68: 1, // D
        83: 2, // S
        65: 3  // A
    };

    // Respond to direction keys
    document.addEventListener("keydown", function (event) {
        var modifiers = event.altKey || event.ctrlKey || event.metaKey ||
            event.shiftKey;
        var mapped = map[event.which];


        if (!modifiers) {
            if (mapped !== undefined) {
                switch (mapped) {
                    //  case 0: { pMain.socket.send('top'); }; break;
                    case 1: {
                        if (document.getElementById('btnContinue')) {
                            pMain.socket.send('1');
                        }
                        //pMain.socket.send('right');
                    }; break;
                    //case 2: { pMain.socket.send('bottom'); }; break;
                    case 3: {
                        if (document.getElementById('btnErrorRecovery')) {
                            pMain.socket.send('2');
                        }
                    }; break;
                }
                //event.preventDefault();
                //self.emit("move", mapped);
            }
        }

        // R key restarts the game
        //if (!modifiers && event.which === 82) {
        //    self.restart.call(self, event);
        //}
    });

    // Respond to button presses
    //this.bindButtonPress(".retry-button", this.restart);
    //this.bindButtonPress(".restart-button", function () { alert('滑动屏幕或者用键盘输入↑↓←→WASD') });
    //this.bindButtonPress(".keep-playing-button", this.keepPlaying);

    // Respond to swipe events
    var touchStartClientX, touchStartClientY;
    //askAndAnswer
    var gameContainer = document.body;
    //var gameContainer = document.getElementById("btns");
    // var gameContainer = document.getElementsByClassName("game-container")[0];
    gameContainer.addEventListener(this.eventTouchstart, function (event) {
        if ((!window.navigator.msPointerEnabled && event.touches.length > 1) ||
            event.targetTouches.length > 1) {
            return; // Ignore if touching with more than 1 finger
        }

        if (window.navigator.msPointerEnabled) {
            touchStartClientX = event.pageX;
            touchStartClientY = event.pageY;
        } else {
            touchStartClientX = event.touches[0].clientX;
            touchStartClientY = event.touches[0].clientY;
        }

        event.preventDefault();
    });

    gameContainer.addEventListener(this.eventTouchmove, function (event) {
        //event.preventDefault();
    });

    gameContainer.addEventListener(this.eventTouchend, function (event) {
        if ((!window.navigator.msPointerEnabled && event.touches.length > 0) ||
            event.targetTouches.length > 0) {
            return; // Ignore if still touching with one or more fingers
        }

        var touchEndClientX, touchEndClientY;

        if (window.navigator.msPointerEnabled) {
            touchEndClientX = event.pageX;
            touchEndClientY = event.pageY;
        } else {
            touchEndClientX = event.changedTouches[0].clientX;
            touchEndClientY = event.changedTouches[0].clientY;
        }

        var dx = touchEndClientX - touchStartClientX;
        var absDx = Math.abs(dx);

        var dy = touchEndClientY - touchStartClientY;
        var absDy = Math.abs(dy);

        if (Math.max(absDx, absDy) > 10) {
            // (right : left) : (down : up)

            // if (sysState.state > 0)
            if (absDx > absDy) {
                if (dx > 0) {

                    if (document.getElementById('btnContinue')) {
                        pMain.socket.send('1');
                    }
                }
                else {
                    if (document.getElementById('btnErrorRecovery')) {
                        pMain.socket.send('2');
                    }
                }

            }
            else {
                //    gameContainer.scrollTop += dy; 
            }
            //self.emit("move", absDx > absDy ? (dx > 0 ? 1 : 3) : (dy > 0 ? 2 : 0));
        }
    });
};

KeyboardInputManager.prototype.restart = function (event) {
    //event.preventDefault();
    //this.emit("restart");
};

KeyboardInputManager.prototype.keepPlaying = function (event) {
    //event.preventDefault();
    //this.emit("keepPlaying");
};

KeyboardInputManager.prototype.bindButtonPress = function (selector, fn) {
    var button = document.querySelector(selector);
    button.addEventListener("click", fn.bind(this));
    button.addEventListener(this.eventTouchend, fn.bind(this));
};


var kim = null;

var selectRole = function () {
    var innerHtml = `<div>
            <div onclick="pMain.socket.send('1');selectHtmlHide();">
                <a href="#" class="myButton">成为悲催打工仔</a>
            </div>
            <div onclick="pMain.socket.send('1');selectHtmlHide();">
                <a href="#" class="myButton">成为万恶资本家</a>
            </div> 
        </div>`;
    selectHtmlShow(innerHtml);
}
var selectStrategy = function () {
    var innerHtml = '<div>';

    var divDesign = function (id, passCommand, classStr, btnLabel, selected) {
        if (selected) {
            return `<div id="${id}" onclick="pMain.socket.send('${passCommand}');"><a class="${classStr} selected">${btnLabel}</a></div>`;
        }
        else {
            return `<div id="${id}" onclick="pMain.socket.send('${passCommand}');"><a class="${classStr}">${btnLabel}</a></div>`;
        }
    }
    var educateDivDesign = function () {
        var position1 = (employeeEducateAction == "Employee-Educate-Perfect" ? "selected" : "");
        var position2 = (employeeEducateAction == "Employee-Educate-Good" ? "selected" : "");
        var position3 = (employeeEducateAction == "Employee-Educate-Common" ? "selected" : "");
        return `<div>
            <a id="educateTopBtn" class="myButton education ${position1}"  onclick="pMain.socket.send('J');">顶级教育</a>
            <a id="educateGoodBtn" class="myButton education ${position2}"  onclick="pMain.socket.send('K');">良好教育</a>
            <a id="educateCommonBtn" class="myButton education ${position3}" onclick="pMain.socket.send('L');">普通教育</a>
        </div>`;
    }
    if (employeeState.canLove) innerHtml += divDesign('loveBtn', 'A', 'myButton', '恋爱', employeeActions.indexOf('Employee-Love') >= 0);
    if (employeeState.canBeMarried) innerHtml += divDesign('marryBtn', 'B', 'myButton', '结婚', employeeActions.indexOf('Employee-Marry') >= 0);
    if (employeeState.canGetFirstBaby) innerHtml += divDesign('boreFirstBabyBtn', 'C', 'myButton', '生一胎', employeeActions.indexOf('Employee-BirthFirstBaby') >= 0);
    if (employeeState.canGetSecondBaby) innerHtml += divDesign('boreSecondBabyBtn', 'D', 'myButton', '生二胎', employeeActions.indexOf('Employee-BirthSecondBaby') >= 0);
    if (employeeState.canGetThirdBaby) innerHtml += divDesign('boreThirdBabyBtn', 'E', 'myButton', '生三胎', employeeActions.indexOf('Employee-BirthThirdBaby') >= 0);
    if (employeeState.canGetFourthBaby) innerHtml += divDesign('boreFourthBabyBtn', 'F', 'myButton', '生四胎', employeeActions.indexOf('Employee-BirthFourthBaby') >= 0);

    if (employeeState.canEducate) innerHtml += educateDivDesign();

    if (employeeState.canPlayWithChildren) innerHtml += divDesign('marryBtn', 'G', 'myButton', '亲子', false);
    if (employeeState.canSingleWork) innerHtml += divDesign('marryBtn', 'H', 'myButton', '单职', false);
    if (employeeState.canStrugle) innerHtml += divDesign('marryBtn', 'I', 'myButton', '奋斗', employeeActions.indexOf('Employee-Strive') >= 0);


    innerHtml += '<div onclick="selectHtmlHide();"><a class="myButton BtnHideWindow">隐藏</a></div>'
    innerHtml += '</div>';

    selectHtmlShow(innerHtml);
}
var selectHtmlShow = function (html) {
    if (document.getElementById('selection').classList.contains('hide')) {
        document.getElementById('selection').classList.remove('hide');
        document.getElementById('selection').classList.add('show');
    }
    document.getElementById('selection').innerHTML = html;
}

var selectHtmlHide = function () {
    if (document.getElementById('selection').classList.contains('show')) {
        document.getElementById('selection').classList.remove('show');
        document.getElementById('selection').classList.add('hide');
    }
    document.getElementById('selection').innerHTML = '';
}