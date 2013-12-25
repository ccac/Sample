if (!window.Webabcd)
    window.Webabcd = {};

Webabcd.Piano = function() 
{
}

Webabcd.Piano.prototype =
{
    handleLoad: function(control, userContext, rootElement) 
    {
        // plugin（插件）
        this.control = control;
        
        // 根Canvas元素
        this.rootElement = rootElement;
        
        // object.addEventListener(eventName, functionReference)为对象添加一个事件监听器（事件名称，函数名称）
        // Silverlight.createDelegate用来创建一个调用“this”上下文下的特定函数的委托，其被定义在runtime
        // Silverlight.createDelegate = function(instance, method) 
        // {
        //   return function() 
        //   {
        //      return method.apply(instance, arguments);
        //    }
        // }
        this.rootElement.addEventListener("GotFocus", Silverlight.createDelegate(this, this.resetPiano));
        this.rootElement.addEventListener("KeyDown", Silverlight.createDelegate(this, this.handleKeyDown));
        this.rootElement.addEventListener("KeyUp", Silverlight.createDelegate(this, this.handleKeyUp));

        // 鼠标热区的Canvas
        // object.findName(name)
        this.pathCanvas = rootElement.findName("pathCanvas");   

        // 开始录制按钮
        this.startRecord = rootElement.findName("startRecord");   
        this.startRecord.addEventListener("MouseLeftButtonDown", Silverlight.createDelegate(this, this.handleStartRecord));

        // 停止录制按钮
        this.stopRecord = rootElement.findName("stopRecord");   
        this.stopRecord.addEventListener("MouseLeftButtonDown", Silverlight.createDelegate(this, this.handleStopRecord));
        
        // 默认为选中停止录制按钮
        // plugin.content.findName(objectName)
        document.getElementById('SilverlightControl').content.findName("stopRecord").Stroke = "LightBlue";
        document.getElementById('SilverlightControl').content.findName("stopRecord").StrokeThickness = 6;
        
        // 播放按钮
        this.play = rootElement.findName("play");   
        this.play.addEventListener("MouseLeftButtonDown", Silverlight.createDelegate(this, this.handlePlay));

        // object.children对应<Canvas>一个或多个子元素</Canvas>
        // object.getItem(index)获取集合内指定的对象
        // object.count集合内的成员数量
        for(var i = 0; i<this.pathCanvas.children.count; ++i)
        {
            // 为所有鼠标热区添加相关的事件监听器
            this.pathCanvas.children.getItem(i).addEventListener("MouseLeftButtonDown", Silverlight.createDelegate(this, this.handleMouseDown));
            this.pathCanvas.children.getItem(i).addEventListener("MouseLeftButtonUp", Silverlight.createDelegate(this, this.handleMouseUp));
        }
        
        // 是否启用录制
        this._enableRecord = false;
        // 上一次按键的时间
        this._prevTime = new Date().getTime();
    },
    
    // 单击开始录制按钮
    handleStartRecord: function(sender, eventArgs) 
    {
        // 设置开始录制按钮样式
        sender.Stroke = 'LightBlue';
        sender.StrokeThickness = 6;
        
        // 设置停止录制按钮样式
        document.getElementById('SilverlightControl').content.findName("stopRecord").Stroke = "Black";
        document.getElementById('SilverlightControl').content.findName("stopRecord").StrokeThickness = 3;
        
        // 启用录音
        this._enableRecord = true;
        // 记录时间
        this._prevTime = new Date().getTime();
    },
    
    // 单击停止录制按钮
    handleStopRecord: function(sender, eventArgs) 
    {
        // 设置停止录制按钮样式
        sender.Stroke = 'LightBlue';
        sender.StrokeThickness = 6;

        // 设置开始录制按钮样式
        document.getElementById('SilverlightControl').content.findName("startRecord").Stroke = "Black";
        document.getElementById('SilverlightControl').content.findName("startRecord").StrokeThickness = 3;

        // 清空乐谱名称和乐谱内容
        document.getElementById(txtInput).value = '';
        document.getElementById(txtName).value = '';
        
        // 停止录音
        this._enableRecord = false;
    },
    
    // 单击播放按钮
    handlePlay: function(sender, eventArgs) 
    {
        // 根据乐谱自动播放
        autoPlay(this, 0);
    },
    
    // 记录乐谱
    handleRecord: function(currentPianoKeyID)
    {
        if (this._enableRecord)
        {
            // 插入乐谱
            document.getElementById(txtInput).value += new Date().getTime() - this._prevTime + ',';
            document.getElementById(txtInput).value += currentPianoKeyID + ';';

            // 更新时间
            this._prevTime = new Date().getTime();
        }
    },
    
    handleMouseDown: function(sender, eventArgs) 
    {
        // object.captureMouse()为对象启用鼠标捕捉（鼠标离开热区也可触发相关事件，如MouseLeftButtonUp）
        sender.captureMouse();
        
        var currentPianoKeyID = sender.name.substr(0, sender.name.indexOf("Path"));
        var currentImage = sender.findName("img" + currentPianoKeyID);
        currentImage.opacity = 1;
        
        var currentMediaElement = sender.findName(currentPianoKeyID);
        currentMediaElement.stop();
        currentMediaElement.play();
        
        this.handleRecord(currentPianoKeyID);
    },
    
    handleMouseUp: function(sender, eventArgs) 
    {
        // object.releaseMouseCapture()释放对象的鼠标捕捉
        sender.releaseMouseCapture();
        
        var currentPianoKeyID = sender.name.substr(0, sender.name.indexOf("Path"));
        var currentImage = sender.findName("img" + currentPianoKeyID);
        currentImage.opacity = 0;
    },

    pressKey: function (currentPianoKeyID)
    {
        var currentImage = this.control.content.findName("img" + currentPianoKeyID);
        currentImage.opacity = 1;
        
        var currentMediaElement = this.control.content.findName(currentPianoKeyID);
        // object.play()播放，object.pause()暂停，object.stop()停止
        currentMediaElement.stop();
        currentMediaElement.play();
    },

    depressKey: function (currentPianoKeyID)
    {
        var currentImage = this.control.content.findName("img" + currentPianoKeyID);
        currentImage.opacity = 0;
    },

    handleKeyDown: function (sender, eventArgs)
    {
        switch (eventArgs.Key)
        {
            // eventArgs.Key - 获取与该事件相关的键盘按键
            case 55: this.pressKey("C"); this.handleRecord("C"); break;
            case 48: this.pressKey("C2"); this.handleRecord("C2"); break;
            case 53: this.pressKey("D");  this.handleRecord("D"); break;
            case 33: this.pressKey("D2"); this.handleRecord("D2"); break;
            case 32: this.pressKey("E");  this.handleRecord("E"); break;
            case 51: this.pressKey("F");  this.handleRecord("F"); break;
            case 36: this.pressKey("F2"); this.handleRecord("F2"); break;
            case 31: this.pressKey("G");  this.handleRecord("G"); break;
            case 37: this.pressKey("G2"); this.handleRecord("G2"); break;
            case 43: this.pressKey("A");  this.handleRecord("A"); break;
            case 39: this.pressKey("A2"); this.handleRecord("A2"); break;
            case 42: this.pressKey("B");  this.handleRecord("B"); break;
        }
    },

    handleKeyUp: function (sender, eventArgs)
    {
        switch (eventArgs.Key)
        {
            // eventArgs.Key - 获取与该事件相关的键盘按键
            case 55: this.depressKey("C");  break;
            case 48: this.depressKey("C2"); break;
            case 53: this.depressKey("D");  break;
            case 33: this.depressKey("D2"); break;
            case 32: this.depressKey("E");  break;
            case 51: this.depressKey("F");  break;
            case 36: this.depressKey("F2"); break;
            case 31: this.depressKey("G");  break;
            case 37: this.depressKey("G2"); break;
            case 43: this.depressKey("A");  break;
            case 39: this.depressKey("A2"); break;
            case 42: this.depressKey("B");  break;
        }
    },
    
    resetPiano: function(sender, eventArgs) 
    {
        this.depressKey("C");
        this.depressKey("C2");
        this.depressKey("D");
        this.depressKey("D2");
        this.depressKey("E");
        this.depressKey("F");
        this.depressKey("F2");
        this.depressKey("G");
        this.depressKey("G2");
        this.depressKey("A");
        this.depressKey("A2");
        this.depressKey("B");
    }
}


var _obj; // Webabcd.Piano对象
var _index; // 乐谱索引
function autoPlay(obj, index)
{
    _obj = obj;
    _index = index;
    
    execAutoPlay();
}

function execAutoPlay()
{
    _obj.resetPiano();

    var str = document.getElementById(txtInput).value; // 乐谱
    var ary = str.split(';'); // 间隔时间，按键名称
    
    if (_index != 0 && typeof(ary[_index-1]) != 'undefined' && ary[_index-1] != '')
    {
        // 按下当前乐谱索引的上一个索引的按键
        _obj.pressKey(ary[_index-1].split(',')[1]); 
    }
     
    var currentIndex = _index;   
    _index++;
    
    if (typeof(ary[currentIndex]) != 'undefined')
    {
        setTimeout("_obj.resetPiano();", 100)
        // 经过当前乐谱索引的间隔时间则调用execAutoPlay()
        setTimeout("execAutoPlay();", parseInt(ary[currentIndex].split(',')[0], 10)); 
    }
}
