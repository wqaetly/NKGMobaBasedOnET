
var global = global || (function () { return this; }());
(function (global) {
    "use strict";

    let App = puerts.loadType('FairyEditor.App');
    let UnityEngine_Debug = puerts.loadType('UnityEngine.Debug');

    if (!UnityEngine_Debug) return;

    var console = {}

    function toString(args) {
        return Array.prototype.map.call(args, x => x === null ? "null" : x === undefined ? 'undefined' : x.toString()).join(',');
    }

    console.log = function (msg) {
        if (App.consoleView)
            App.consoleView.Log(toString(arguments));
        else
            UnityEngine_Debug.Log(toString(arguments));
    }

    console.info = function (msg) {
        if (App.consoleView)
            App.consoleView.Log(toString(arguments));
        else
            UnityEngine_Debug.Log(toString(arguments));
    }

    console.warn = function (msg) {
        if (App.consoleView)
            App.consoleView.LogWarning(toString(arguments));
        else
            UnityEngine_Debug.LogWarning(toString(arguments));
    }

    console.error = function (msg) {
        if (App.consoleView)
            App.consoleView.LogError(toString(arguments));
        else
            UnityEngine_Debug.LogError(toString(arguments));
    }

    global.console = console;
    puerts.console = console;

}(global));

require('./CodeWriter');