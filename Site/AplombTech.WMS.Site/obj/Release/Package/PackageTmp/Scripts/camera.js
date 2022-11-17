
    var flashVars = {
        streamer: 'rtmp://123.49.33.103:1935/wasalive/myapp',
        file:'test'
    };
var params = {};
params.allowfullscreen = "true";
var attributes = {};
swfobject.embedSWF($('swfUrl').val(), "rtmp-publisher", "200", "150", "9.0.0", null, flashVars, params, attributes);

