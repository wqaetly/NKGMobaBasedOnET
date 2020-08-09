local genModelCode = require(PluginPath..'/NKG_GenerateModelCode')
local genHotfixCode = require(PluginPath..'/NKG_GenerateHotfixCode')

function onPublish(handler)
    if not handler.genCode then 
        handler.genCode = false
        genHotfixCode(handler)
		fprint("使用NKG插件生成ET Hotfix代码完成")
    else
        handler.genCode = false
        genModelCode(handler)
		fprint("使用NKG插件生成ET Model层代码完成")
    end
end