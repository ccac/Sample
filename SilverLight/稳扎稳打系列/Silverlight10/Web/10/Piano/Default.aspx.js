function createSilverlight()
{
	var scene = new Webabcd.Piano();
	Silverlight.createObjectEx({
		source: "Piano.xaml",
		parentElement: document.getElementById("SilverlightControlHost"),
		id: "SilverlightControl",
		properties: {
			width: "100%",
			height: "100%",
			version: "1.0",
			isWindowless: "true"
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