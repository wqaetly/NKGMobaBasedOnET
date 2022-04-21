local NKGCodeGenEntry = {}

---@type HotfixCodeGenHandler
NKGCodeGenEntry.HotfixCodeGenHandler = require(PluginPath .. '/HotfixCodeGenHandler')
---@type ModelCodeGenHandler
NKGCodeGenEntry.ModelCodeGenHandler = require(PluginPath .. '/ModelCodeGenHandler')
---@type CodeGenConfig
NKGCodeGenEntry.CodeGenConfig = require(PluginPath .. "/CodeGenConfig")

--- 点击发布工程时的回调
---@param handler CS.FairyEditor.PublishHandler 发布处理者
function onPublish(handler)
    --- 不勾选生成代码时，将为其生成热更层代码
    if not handler.genCode then
        NKGCodeGenEntry.HotfixCodeGenHandler.Do(handler, NKGCodeGenEntry.CodeGenConfig)
    else
        --- 勾选生成代码时，将为其生成非热更层代码
        NKGCodeGenEntry.ModelCodeGenHandler.Do(handler, NKGCodeGenEntry.CodeGenConfig)
    end
end

return NKGCodeGenEntry