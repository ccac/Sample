var _sl = null, bodyLoadWasFaster = false, upperLimits = [], maxUpperLimit = 0, currentLimitIndex = 0, nodeCount = 0,
    setVisible = [];

function $(id) {
    return document.getElementById(id);
}

function createMySilverlightControl()
{  
    var obj = Silverlight.createObject(
        "JellyGraph.xaml",                        // Source property value.
        parentElement,                      // DOM reference to hosting DIV tag.
        "mySilverlightControl",             // Unique control id value.
        {                                   // Control properties.
            width:'800',                    // Width of rectangular region of control in pixels.
            height:'300',                   // Height of rectangular region of control in pixels.
            inplaceInstallPrompt:false,     // Determines whether to display in-place install prompt if invalid version detected.
            background:'#0d0d0d',           // Background color of control.
            isWindowless:'false',           // Determines whether to display control in Windowless mode.
            framerate:'24',                 // MaxFrameRate property value.
            version:'0.9'                   // Control version to use.
        },
        {
            onError:null,                   // OnError property value -- event handler function name.
            onLoad:null                     // OnLoad property value -- event handler function name.
        },
        null);                              // Context value -- event handler function name.
}

function Animation(type, data) {
    if (data.targetName && data.targetProperty && data.to && data.duration) {
        var properties = '';
        
        properties += ' Storyboard.TargetName="' + data.targetName + '"';
        properties += ' Storyboard.TargetProperty="' + data.targetProperty + '"';
        
        if (data.beginTime)
            properties += ' BeginTime="0:0:' + data.beginTime + '"';

        if (data.from)
            properties += ' From="' + data.from + '"';

        properties += ' To="' + data.to + '"';
        properties += ' Duration="0:0:' + data.duration + '"';
        return '<' + type + 'Animation' + properties + ' />';
    }
    return '';
}

function DoubleAnimation(data) {
    return Animation('Double', data);
}

function PointAnimation(data) {
    return Animation('Point', data);
}

function updateYLabels(limitIndex) {
    for (var i = 1; i < 5; ++i) {
        var labelText = _sl.findName('xaLabelText' + (i+1));
        if (labelText) {
            var value = Math.round(upperLimits[limitIndex] * i / 4)
            labelText.Text = '' + value;
            if (value >= 1000) {
                labelText['Canvas.Left'] = 12;
            } else if (value >= 100) {
                labelText['Canvas.Left'] = 18;
            } else if (value >= 10) {
                labelText['Canvas.Left'] = 24;
            } else {
                labelText['Canvas.Left'] = 32;
            }
        }
    }
}

function changeGraphUpperLimit(limitIndex) {
    if (limitIndex != currentLimitIndex) {
        var oldLimitIndex = currentLimitIndex;
        currentLimitIndex = limitIndex;

        updateYLabels(currentLimitIndex);
        updateYValues(currentLimitIndex, oldLimitIndex);
    }
}

function slLoaded(sender, eventArgs)
{
    _sl = sender;
    if (bodyLoadWasFaster) {
        onLoaded();
    }
}

function nodeAnimations(id, i, limitIndex, value) {
            // Loaded animation (rise from the bottom)

    var newId = function(name, i, limitIndex) { return id(name, i) + '_' + limitIndex; };

    return '    <Storyboard Name="' + newId('showSet1',i,limitIndex) + '" Completed="showSet2">'
         +         DoubleAnimation({
                     'targetName':     id('', i),
                     'targetProperty': '(Canvas.Top)',
                     'beginTime':      (i/50).toFixed(3),
                     'duration':       0.3,
                     'to':             300 - (300 - (value - 4)) * 1.1
                   })
         +      (i-1 > 0 ?
                   DoubleAnimation({
                     'targetName':     id('line', i-1),
                     'targetProperty': 'Y2',
                     'beginTime':      (i/50).toFixed(3),
                     'duration':       0.3,
                     'to':             300 - (300 - value) * 1.1
                   })
                 : '' )
         +      (i + 1 < nodeCount ?
                   DoubleAnimation({
                     'targetName':     id('line', i),
                     'targetProperty': 'Y1',
                     'beginTime':      (i/50).toFixed(3),
                     'duration':       0.3,
                     'to':             300 - (300 - value) * 1.1
                   })
                 : '' )
         + '    </Storyboard>'

         // Loaded animation 2 (first bounce)

         + '    <Storyboard Name="' + newId('showSet2',i,limitIndex) + '" Completed="showSet3">'
         +         DoubleAnimation({
                     'targetName':     id('', i),
                     'targetProperty': '(Canvas.Top)',
                     'duration':       0.2,
                     'to':             300 - (300 - (value - 4)) * 0.98
                   })
         +      (i-1 > 0 ?
                   DoubleAnimation({
                     'targetName':     id('line', i-1),
                     'targetProperty': 'Y2',
                     'duration':       0.2,
                     'to':             300 - (300 - value) * 0.98
                   })
                 : '' )
         +      (i + 1 < nodeCount ?
                   DoubleAnimation({
                     'targetName':     id('line', i),
                     'targetProperty': 'Y1',
                     'duration':       0.2,
                     'to':             300 - (300 - value) * 0.98
                   })
                 : '' )
         + '    </Storyboard>'

         // Loaded animation 3 (last bounce)

         + '    <Storyboard Name="' + newId('showSet3',i,limitIndex) + '">'
         +         DoubleAnimation({
                     'targetName':     id('', i),
                     'targetProperty': '(Canvas.Top)',
                     'duration':       0.06,
                     'to':             value - 4
                   })
         +      (i-1 > 0 ?
                   DoubleAnimation({
                     'targetName':     id('line', i-1),
                     'targetProperty': 'Y2',
                     'duration':       0.06,
                     'to':             value
                   })
                 : '' )
         +      (i + 1 < nodeCount ?
                   DoubleAnimation({
                     'targetName':     id('line', i),
                     'targetProperty': 'Y1',
                     'duration':       0.06,
                     'to':             value
                   })
                 : '' )
         + '    </Storyboard>'

         + '    <Storyboard Name="' + newId('moveTo',i,limitIndex) + '">'
         +         DoubleAnimation({
                     'targetName':     id('', i),
                     'targetProperty': '(Canvas.Top)',
                     'duration':       0.3,
                     'beginTime':      i/50,
                     'to':             value - 4
                   })
         +         DoubleAnimation({
                     'targetName':     id('hs', i),
                     'targetProperty': '(Canvas.Top)',
                     'duration':       0.3,
                     'beginTime':      i/50,
                     'to':             value - 12
                   })
         +         DoubleAnimation({
                     'targetName':     id('t', i),
                     'targetProperty': '(Canvas.Top)',
                     'duration':       0.3,
                     'beginTime':      i/50,
                     'to':             value - 8
                   })
         +         DoubleAnimation({
                     'targetName':     id('r', i),
                     'targetProperty': '(Canvas.Top)',
                     'duration':       0.3,
                     'beginTime':      i/50,
                     'to':             parseInt(value) - 10.5
                   })
         +      (i-1 > 0 ?
                   DoubleAnimation({
                     'targetName':     id('line', i-1),
                     'targetProperty': 'Y2',
                     'duration':       0.3,
                     'beginTime':      i/50,
                     'to':             value
                   })
                 : '' )
         +      (i + 1 < nodeCount ?
                   DoubleAnimation({
                     'targetName':     id('line', i),
                     'targetProperty': 'Y1',
                     'duration':       0.3,
                     'beginTime':      i/50,
                     'to':             value
                   })
                 : '' )
         + '    </Storyboard>'

         // Hide animation 1

         + '    <Storyboard Name="' + newId('hideSet1',i,limitIndex) + '" Completed="hideSet2">'
         +         DoubleAnimation({
                     'targetName':     id('', i),
                     'targetProperty': '(Canvas.Top)',
                     'beginTime':      (i/50).toFixed(3),
                     'duration':       0.06,
                     'to':             300 - (300 - (value - 4)) * 0.98
                   })
         +      (i-1 > 0 ?
                   DoubleAnimation({
                     'targetName':     id('line', i-1),
                     'targetProperty': 'Y2',
                     'beginTime':      (i/50).toFixed(3),
                     'duration':       0.06,
                     'to':             300 - (300 - value) * 0.98
                   })
                 : '' )
         +      (i + 1 < nodeCount ?
                   DoubleAnimation({
                     'targetName':     id('line', i),
                     'targetProperty': 'Y1',
                     'beginTime':      (i/50).toFixed(3),
                     'duration':       0.06,
                     'to':             300 - (300 - value) * 0.98
                   })
                 : '' )
         + '    </Storyboard>'

         // Hide animation 2

         + '    <Storyboard Name="' + newId('hideSet2',i,limitIndex) + '" Completed="hideSet3">'
         +         DoubleAnimation({
                     'targetName':     id('', i),
                     'targetProperty': '(Canvas.Top)',
                     'duration':       0.1,
                     'to':             300 - (300 - (value - 4)) * 1.1
                   })
         +      (i-1 > 0 ?
                   DoubleAnimation({
                     'targetName':     id('line', i-1),
                     'targetProperty': 'Y2',
                     'duration':       0.1,
                     'to':             300 - (300 - value) * 1.1
                   })
                 : '' )
         +      (i + 1 < nodeCount ?
                   DoubleAnimation({
                     'targetName':     id('line', i),
                     'targetProperty': 'Y1',
                     'duration':       0.1,
                     'to':             300 - (300 - value) * 1.1
                   })
                 : '' )
         + '    </Storyboard>'

         // Hide animation 3

         + '    <Storyboard Name="' + newId('hideSet3',i,limitIndex) + '">'
         +         DoubleAnimation({
                     'targetName':     id('', i),
                     'targetProperty': '(Canvas.Top)',
                     'duration':       0.3,
                     'to':             302
                   })
         +      (i-1 > 0 ?
                   DoubleAnimation({
                     'targetName':     id('line', i-1),
                     'targetProperty': 'Y2',
                     'duration':       0.3,
                     'to':             306
                   })
                 : '' )
         +      (i + 1 < nodeCount ?
                   DoubleAnimation({
                     'targetName':     id('line', i),
                     'targetProperty': 'Y1',
                     'duration':       0.3,
                     'to':             306
                   })
                 : '' )
         + '    </Storyboard>';
}

function onLoaded()
{
  if (!_sl) {
    bodyLoadWasFaster = true;
    return;
  }

  // Retrieve a reference to the control.
  var control = _sl.getHost();

  var colours = [ '444444', '0046a6', 'b50c0c', '0ad500' ]; //, '444444' ];
  
  var dataSets = [ [], [], [], [], ];
  
  for (var i = 0; i < 40; ++i) {
    dataSets[0][i] = Math.sin(i / 20) * 100 + 5;
  }
  for (var i = 0; i < 40; ++i) {
    dataSets[1][i] = Math.sin(i / 10) * 40 + 40;
  }
  for (var i = 0; i < 40; ++i) {
    dataSets[2][i] = Math.sin(i / 5 + 0.7) * 40 + 100 + Math.cos(i * 1.5) * 5;
  }
  for (var i = 0; i < 40; ++i) {
    dataSets[3][i] = Math.sin(i) * 20 + 20 + i * 5;
  }

  var maxGroupLength = 0;

  for (var group = 0; group < colours.length; ++group) {
    if (dataSets[group].length > maxGroupLength)
      maxGroupLength = dataSets[group].length;

    upperLimits[group] = 0;
    for (var i = 0; i < dataSets[group].length; ++i) {
      if (dataSets[group][i] > upperLimits[group]) {
        upperLimits[group] = dataSets[group][i];
      }
    }
    upperLimits[group] = parseInt((upperLimits[group] * 1.1 + 10) / 20) * 20;

    if (upperLimits[group] > upperLimits[maxUpperLimit]) {
      maxUpperLimit = group;
    }
  }
  
  currentLimitIndex = maxUpperLimit;
  updateYLabels(maxUpperLimit);

  var nodeSpacing = (770 / maxGroupLength).toFixed(1);

  for (var group = 0; group < colours.length; ++group) {

    setVisible[group] = true;

    var id = function(name, id) { return name + '_' + group + '_' + id; }
    var values = [ [] ];

    for (var limitIndex = 0; limitIndex < upperLimits.length; ++limitIndex) {
        values[0][limitIndex] = 273 - dataSets[group][0] / upperLimits[limitIndex] * 260;
    }

    var graphArea = _sl.findName('graphArea');

    nodeCount = dataSets[group].length;
    for (i = 1; i < nodeCount; ++i)
    {
        values[i] = [];
        for (var limitIndex = 0; limitIndex < upperLimits.length; ++limitIndex) {
            values[i][limitIndex] = 273 - dataSets[group][i] / upperLimits[limitIndex] * 260;
        }

        var x = i * nodeSpacing + 10;

        // Draw the line
        if (i + 1 < nodeCount) {
            var line = control.content.createFromXaml(
                '<Line Name="' + id('line',i) + '" X1="' + (x - nodeSpacing) + '" X2="' + x + '"'
                + ' Y1="305" Y2="305"'
                + ' StrokeThickness="3" Stroke="#' + colours[group] + '" />'
            );
            graphArea.children.add(line);
        }

        // The ugly part.
        // Creates a circle, with a render transform so the circle can be resized later.
        // Also create two storyboard animations. One for growing the circle, and one for shrinking it.
        var circle = control.content.createFromXaml(

            //////////////////////

            // Start the Ellipse tag

            '<Ellipse Name="' + id('', i) + '" Canvas.ZIndex="1"'
            + '       Canvas.Left="' + (x - nodeSpacing - 4) + '"'
            + '       Canvas.Top="301" StrokeThickness="8"'
            + '       Width="1" Height="1">'

            + '  <Ellipse.Stroke>'
            + '    <SolidColorBrush Name="' + id('vertexFill',i) + '" Color="#' + colours[group] + '" />'
            + '  </Ellipse.Stroke>'

            //////////////////////

            // Transform used for scaling up/down later

            + '  <Ellipse.RenderTransform>'
            + '    <ScaleTransform Name="' + id('est',i) + '" CenterX="4" CenterY="4" ScaleX="1.0" ScaleY="1.0"/>'
            + '  </Ellipse.RenderTransform>'

            //////////////////////

            // Animations

            + '  <Ellipse.Resources>'

            + nodeAnimations(id, i, 0, values[i-1][0])
            + nodeAnimations(id, i, 1, values[i-1][1])
            + nodeAnimations(id, i, 2, values[i-1][2])
            + nodeAnimations(id, i, 3, values[i-1][3])

            + '  </Ellipse.Resources>'

            ///////////////////////

            + '</Ellipse>'
        );

        graphArea.children.add(circle);


        var hotspot = control.content.createFromXaml(
            '<Rectangle Name="' + id('hs',i) + '" Canvas.ZIndex="5"'
            + '       Canvas.Left="' + (x - nodeSpacing - 8) + '"'
            + '       Canvas.Top="' + (values[i-1][maxUpperLimit]-12) + '"'
            + '       Width="16" Height="24"'
            + '       Fill="#00000000"'
            + '       MouseEnter="mouseenter" MouseLeave="mouseleave">'

            //////////////////////

            // Animations

            + '  <Rectangle.Resources>'

            // Growing animation

            + '    <Storyboard Name="' + id('esg',i) + '">'
            + '      <DoubleAnimation Storyboard.TargetName="' + id('est',i) + '" Storyboard.TargetProperty="ScaleX"'
            + '                       To="1.5" Duration="0:0:0.07" />'
            + '      <DoubleAnimation Storyboard.TargetName="' + id('est',i) + '" Storyboard.TargetProperty="ScaleY"'
            + '                       To="1.5" Duration="0:0:0.07" />'
            + '      <DoubleAnimation Storyboard.TargetName="' + id('t',i) + '" Storyboard.TargetProperty="Opacity"'
            + '                       To="1" Duration="0:0:0.07" />'
            + '      <DoubleAnimation Storyboard.TargetName="' + id('r',i) + '" Storyboard.TargetProperty="Opacity"'
            + '                       To="1" Duration="0:0:0.07" />'
            + '      <DoubleAnimation Storyboard.TargetName="' + id('r',i) + '" Storyboard.TargetProperty="(Canvas.Left)"'
            + '                       To="' + (x-2) + '" Duration="0:0:0.1" />'
            + '      <DoubleAnimation Storyboard.TargetName="' + id('t',i)+ '" Storyboard.TargetProperty="(Canvas.Left)"'
            + '                       To="' + (x+1) + '" Duration="0:0:0.1" />'
            + '      <ColorAnimation Storyboard.TargetName="' + id('vertexFill',i)+ '" Storyboard.TargetProperty="Color"'
            + '                       To="#00baff" Duration="0:0:0.1" />'
            + '    </Storyboard>'

            // Shrinking animation

            + '    <Storyboard Name="' + id('ess',i) + '" Completed="doneShrink">'
            + '      <DoubleAnimation Storyboard.TargetName="' + id('est',i) + '" Storyboard.TargetProperty="ScaleX"'
            + '                       To="1.0" Duration="0:0:0.2" />'
            + '      <DoubleAnimation Storyboard.TargetName="' + id('est',i) + '" Storyboard.TargetProperty="ScaleY"'
            + '                       To="1.0" Duration="0:0:0.2" />'
            + '      <DoubleAnimation Storyboard.TargetName="' + id('t',i) + '" Storyboard.TargetProperty="Opacity"'
            + '                       To="0" Duration="0:0:0.2" />'
            + '      <DoubleAnimation Storyboard.TargetName="' + id('r',i) + '" Storyboard.TargetProperty="Opacity"'
            + '                       To="0" Duration="0:0:0.2" />'
            + '      <DoubleAnimation Storyboard.TargetName="' + id('r',i) + '" Storyboard.TargetProperty="(Canvas.Left)"'
            + '                       To="' + (x+12) + '" Duration="0:0:0.1" />'
            + '      <DoubleAnimation Storyboard.TargetName="' + id('t',i) + '" Storyboard.TargetProperty="(Canvas.Left)"'
            + '                       To="' + (x+15) + '" Duration="0:0:0.1" />'
            + '      <ColorAnimation Storyboard.TargetName="' + id('vertexFill',i)+ '" Storyboard.TargetProperty="Color"'
            + '                       To="#' + colours[group] + '" Duration="0:0:0.1" />'
            + '    </Storyboard>'

            + '  </Rectangle.Resources>'

            ///////////////////////

            + '</Rectangle>'
        );

        graphArea.children.add(hotspot);

        ////////////////
        // This is the bubble that contains the number, showing up on MouseEnter
        //
        var rect = control.content.createFromXaml(
            '<Path Name="' + id('r',i) + '" Opacity="0" Canvas.ZIndex="2"'
            + '    Stroke="#3a3a3a" StrokeThickness="1" Fill="#202020"'
            + '    Canvas.Left="' + (x - 5) + '" Canvas.Top="' + (parseInt(values[i-1][maxUpperLimit]) - 10.5) + '"'
            + '    Data="M 0,2'
            + '          c 0,-2 -2,0 2,-2'
            + '          h 24'
            + '          c 2,0 0,-2, 2,2'
            + '          v 16'
            + '          c 0,2 2,0 -2,2'
            + '          h -24'
            + '          c -2,0 0,2 -2,-2'
            + '          v -4'
            + '          l -6,-4'
            + '          l 6,-4'
            + '          v -4'
            + '">'
//            + '    <Path.Fill>'
//            + '        <LinearGradientBrush StartPoint="0,0" EndPoint="0,1">'
//            + '            <GradientStop Color="#FFFFFF" Offset="0.0"/>'
//            + '            <GradientStop Color="#F0F0F0" Offset="1.0"/>'
//            + '        </LinearGradientBrush>'
//            + '    </Path.Fill>'
            + '</Path>'
        );
        graphArea.children.add(rect);


        ////////////////
        // This is the text block that shows up inside the bubble
        //
        var yInt = parseInt(values[i-1][maxUpperLimit]);
        var textBlock = control.content.createFromXaml(
            '<TextBlock Name="' + id('t',i) + '" FontSize="11" Text="' + parseInt(dataSets[group][i-1]) + '" Foreground="#ffffff"'
            + '         Canvas.Left="' + (x - 2) + '" Canvas.Top="' + (yInt - 8) + '"'
            + '         Canvas.ZIndex="3" Opacity="0">'
            + '</TextBlock>'
        );
        graphArea.children.add(textBlock);
    }

  }
  
  for (var group = 0; group < colours.length; ++group) {
    for (i = 1; i < nodeCount; ++i) {
      setTimeout('_sl.findName(\'showSet1_' + group + '_' + i + '_' + currentLimitIndex + '\').begin();', 333*group);
    }
  }
}

function mouseenter(sender, eventArgs)
{
    var id = sender.Name.substr(2,6);
    sender.findName('esg'+id).begin();
}

function mouseleave(sender, eventArgs)
{
    var id = sender.Name.substr(2,6);
    sender.findName('ess'+id).begin();
}

function findUpperLimit(afterShowSetId)
{
    var maxIndex = -1;
    for (var i = 0; i < upperLimits.length; ++i) {
        if (setVisible[i] || afterShowSetId == i) {
            if (-1 == maxIndex || upperLimits[i] > upperLimits[maxIndex]) {
                maxIndex = i;
            }
        }
    }
    if (maxIndex > -1) {
        changeGraphUpperLimit(maxIndex);
    }
}

function showSet(setId)
{
    var nodeCount = 40;

    var oldLimitIndex = currentLimitIndex;

    findUpperLimit(setId);

    setVisible[setId] = true;

    for (i = 1; i < nodeCount; ++i) {
        _sl.findName('moveTo_' + setId + '_' + i + '_' + oldLimitIndex).pause();
        _sl.findName('hideSet1_' + setId + '_' + i + '_' + oldLimitIndex).pause();
        _sl.findName('hideSet2_' + setId + '_' + i + '_' + oldLimitIndex).pause();
        _sl.findName('hideSet3_' + setId + '_' + i + '_' + oldLimitIndex).pause();
        _sl.findName('showSet1_' + setId + '_' + i + '_' + currentLimitIndex).begin();
    }
}


function showSet2(sender, eventArgs)
{
    sender.findName('showSet2'+sender.Name.substr(8,7)).begin();
}

function showSet3(sender, eventArgs)
{
    sender.findName('showSet3'+sender.Name.substr(8,7)).begin();
}

function hideSet(setId)
{
    var nodeCount = 40;

    setVisible[setId] = false;

    for (i = 1; i < nodeCount; ++i) {
        _sl.findName('moveTo_' + setId + '_' + i + '_' + currentLimitIndex).pause();
        _sl.findName('showSet1_' + setId + '_' + i + '_' + currentLimitIndex).pause();
        _sl.findName('showSet2_' + setId + '_' + i + '_' + currentLimitIndex).pause();
        _sl.findName('showSet3_' + setId + '_' + i + '_' + currentLimitIndex).pause();
        _sl.findName('hideSet1_' + setId + '_' + i + '_' + currentLimitIndex).begin();
    }

    findUpperLimit();
}

function hideSet2(sender, eventArgs)
{
    sender.findName('hideSet2'+sender.Name.substr(8,7)).begin();
}

function hideSet3(sender, eventArgs)
{
    sender.findName('hideSet3'+sender.Name.substr(8,7)).begin();
}

function updateYValues(limitIndex, oldLimitIndex)
{
    for (var setId = 0; setId < 4; ++setId) {
        if (setVisible[setId]) {
            var nodeCount = 40;
            for (i = 1; i < nodeCount; ++i) {
                _sl.findName('showSet1_' + setId + '_' + i + '_' + oldLimitIndex).pause();
                _sl.findName('showSet2_' + setId + '_' + i + '_' + oldLimitIndex).pause();
                _sl.findName('showSet3_' + setId + '_' + i + '_' + oldLimitIndex).pause();
                _sl.findName('moveTo_' + setId + '_' + i + '_' + oldLimitIndex).pause();
                _sl.findName('moveTo_' + setId + '_' + i + '_' + limitIndex).begin();
            }
        }
    }
}

var _showStarted = false;
function setShowMode(flag)
{
    if (flag) {
        if (!_showStarted) {
            _showStarted = true;
            showStep();
        }
    } else {
        _showStarted = false;
    }
}
function showStep() {
    var setId = parseInt(Math.random() * 4);
    if (setVisible[setId]) {
        hideSet(setId);
        $('hide' + (setId+1)).style.display = 'none';
        $('show' + (setId+1)).style.display = '';
    } else {
        showSet(setId);
        $('hide' + (setId+1)).style.display = '';
        $('show' + (setId+1)).style.display = 'none';
    }
    if (_showStarted) {
        setTimeout('showStep()', 500);
    }
}