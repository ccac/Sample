if (!window.Piano)
	window.Piano = {};

Piano.Page = function() 
{
}

Piano.Page.prototype =
{
	handleLoad: function(control, userContext, rootElement) 
	{
		this.control = control;
		
		// Sample event hookup:	
		rootElement.addEventListener("MouseLeftButtonDown", Silverlight.createDelegate(this, this.handleMouseDown));
	},
	
	// Sample event handler
	handleMouseDown: function(sender, eventArgs) 
	{
		// The following line of code shows how to find an element by name and call a method on it.
		// this.control.content.findName("Timeline1").Begin();
	}
}