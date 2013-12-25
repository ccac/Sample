    // Is this object a shape, have to do in JavaScript so we can use "instanceof"
    function IsShape(obj)
    {    
        return (typeof obj == "object" && obj instanceof VEShape);    
    }
    
    // Is this object the map, have to do in JavaScript so we can use "instanceof"
    function IsMap(obj)
    {    
        return (typeof obj == "object" && obj instanceof VEMap);    
    }
    
    // support function for Find API, uses a callback that can't be replaced by a delegate
	function FindJS(veMap, what, where, veFindType, shapeLayer, startIndex, numberOfResults, showResults, createResults, useDefaultDisambiguation, setBestMapView)
	{
	    veMap.Find(what, where, veFindType, shapeLayer, startIndex, numberOfResults, showResults, createResults, useDefaultDisambiguation, setBestMapView, FindResults);
	}

    // find callback
	function FindResults(layer, arrFindResult, arrPlace, moreResults, error) {
	    silverlight.content.SLMAP.FindManagedCallback(layer, arrFindResult, arrPlace, moreResults, error);
	}
    
    // support function for GetDirections API, uses a callback that can't be replaced by a delegate
    function GetDirectionsJS(veMap, locations, options)
    {
        // modify the callback to a local Javascript function
        options.SetRouteCallback(RouteResults);
        veMap.GetDirections(locations, options);
    }

    // GetDirections Callback
	function RouteResults(route) {
	    silverlight.content.SLMAP.RouteManagedCallback(route);
	}

    // support function for ImportShapeLayerData API, uses a callback that can't be replaced by a delegate
    function ImportShapeLayerDataJS(veMap, shapeSource, setBestView)
    {
        veMap.ImportShapeLayerData(shapeSource, ImportResults, setBestView);
    }
    
    // ImportShapeLayerData callback
	function ImportResults(layer){
	    silverlight.content.SLMAP.ImportManagedCallback(layer);
	}

    // support function for SetBirdseyeScene API, uses a callback that can't be replaced by a delegate
	function SetBirdseyeSceneJS(veMap, veLatLong, orientation, zoomLevel)
	{
	    veMap.SetBirdseyeScene(veLatLong, orientation, zoomLevel, BirdseyeComplete);
	}
	
    // SetBirdseyeScene callback
	function BirdseyeComplete(scene) {
	    silverlight.content.SLMAP.BirdseyeCompleteManagedCallback(scene);
	}