if (!window.CustomSplashScreen.Web)
	window.CustomSplashScreen.Web = {};

CustomSplashScreen.Web.CustomSplashScreen = function() 
{
}

CustomSplashScreen.Web.CustomSplashScreen.prototype =
{
	handleLoad: function(plugIn, userContext, rootElement) 
	{
		this.plugIn = plugIn;
		
		// 範例按鈕事件連結: 尋找按鈕，然後附加事件處理常式
		// this.button = rootElement.children.getItem(0);	
		
		// this.button.addEventListener("MouseDown", Silverlight.createDelegate(this, this.handleMouseDown));
	}
	
	// 範例事件處理常式
	//handleMouseDown: function(sender, eventArgs) 
	//{
	//}
}