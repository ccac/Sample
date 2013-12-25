// date.getDay()索引转文字
var aryWeek = new Array(7)
aryWeek[0]="Sun"
aryWeek[1]="Mon"
aryWeek[2]="Tue"
aryWeek[3]="Wed"
aryWeek[4]="Thu"
aryWeek[5]="Fri"
aryWeek[6]="Sat"

// date.getMonth()索引转文字
var aryMonth = new Array(12)
aryMonth[0]="01"
aryMonth[1]="02"
aryMonth[2]="03"
aryMonth[3]="04"
aryMonth[4]="05"
aryMonth[5]="06"
aryMonth[6]="07"
aryMonth[7]="08"
aryMonth[8]="09"
aryMonth[9]="10"
aryMonth[10]="11"
aryMonth[11]="12"

// Canvas的Loaded调用的方法
function enableClock() 
{
	var date = new Date();

	var SilverlightControl = document.getElementById("SilverlightControl");

    // plugin.content.findName(objectName)
	var hour = SilverlightControl.content.findName("txtHour");
	var minute = SilverlightControl.content.findName("txtMinute");
	var second = SilverlightControl.content.findName("txtSecond");
	var month = SilverlightControl.content.findName("txtMonth");
	var day = SilverlightControl.content.findName("txtDay");
	var week = SilverlightControl.content.findName("txtWeek");

    // TextBlock.text
    if (date.getHours() > 9)
	    hour.text = date.getHours().toString();
	else
	    hour.text = "0" + date.getHours().toString();

    if (date.getMinutes() > 9)
	    minute.text = date.getMinutes().toString();
	else
	    minute.text = "0" + date.getMinutes().toString();
	
    if (date.getSeconds() > 9)
	    second.text = date.getSeconds().toString();
	else
	    second.text = "0" + date.getSeconds().toString();
	    
	month.text = aryMonth[date.getMonth()];
	    
	if (date.getDate() > 9)
	    day.text = date.getDate().toString();
	else
	    day.text = "0" + date.getDate().toString();
	
	week.text = aryWeek[date.getDay()];
	
	setTimeout("enableClock()",1000);	
}

// 全屏（TextBlock的MouseLeftButtonDown调用的方法）
function toggle_fullScreen(sender, args)
{
    // 当前元素所属的Silverlight插件
    var silverlightPlugin = sender.getHost();
    silverlightPlugin.content.fullScreen = !silverlightPlugin.content.fullScreen;    
}