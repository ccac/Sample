function createSilverlight()
{
	var scene = new Clock.Page();
	Silverlight.createObjectEx({
		source: "Clock.xaml",
		parentElement: document.getElementById("SilverlightControlHost"),
		id: "SilverlightControl",
		inplaceInstallPrompt:true,
		properties: {
			width: "100%",
			height: "100%",
			version: "1.0"
		},
		events: {
			onLoad: Silverlight.createDelegate(scene, scene.handleLoad)
		}
	});
}


if (!window.Silverlight) 
	window.Silverlight = {};

Silverlight.createDelegate = function(instance, method) {
	return function() {
		return method.apply(instance, arguments);
	}
}