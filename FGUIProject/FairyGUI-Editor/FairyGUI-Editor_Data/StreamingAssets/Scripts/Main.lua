local util = require 'xlua.util'

App = CS.FairyEditor.App
ProjectType = CS.FairyEditor.ProjectType
fprint = function(msg)
    App.consoleView:Log(tostring(msg))
end

function printLeakingReferences()
    local str = 'These references from Lua to C# have not been released: \n'
    local registry = debug.getregistry()
    for k, v in pairs(registry) do
        if type(k) == 'number' and type(v) == 'function' and registry[v] == k then
            local info = debug.getinfo(v)
            str = str..string.format('%s:%d', info.short_src, info.linedefined)..'\n'
        end
    end
    str = str..'Try to solve it in a function with name \'onDestroy\'.'
    App.consoleView:LogWarning(str)
end

function fclass()
    local cls = {}
    cls.__index=cls
 
    function cls.new(...)
        local inst=setmetatable({}, cls)
        if inst.ctor then
			inst.ctor(inst, ...)
		end
        return inst
    end
 
    return cls
end

local codeGenerators = {
    [ProjectType.Flash] = require('GenCode_AS3'),
    [ProjectType.Starling] = require('GenCode_AS3'),
    [ProjectType.Layabox] = require('GenCode_TS'),
    [ProjectType.Egret] = require('GenCode_Egret'),
    [ProjectType.PIXI] = require('GenCode_PIXI'),
    [ProjectType.Unity] = require('GenCode_CSharp'),
    [ProjectType.CryEngine] = require('GenCode_CSharp'),
    [ProjectType.MonoGame] = require('GenCode_CSharp'),
    [ProjectType.Haxe] = require('GenCode_Haxe'),
    [ProjectType.Cocos2dx] = require('GenCode_CPP'),
    [ProjectType.Vision] = require('GenCode_CPP'),
    [ProjectType.CocosCreator] = require('GenCode_TS'),
    [ProjectType.ThreeJS] = require('GenCode_TS'),
    [ProjectType.CreateJS] = require('GenCode_TS')
}

function genCodeDefault(handler)
    local func = codeGenerators[handler.project.type]
    if func~=nil then
        func(handler)
    end
end

CodeWriter = fclass()

function CodeWriter:ctor(config)
    config = config or {}
    self.blockStart = config.blockStart or '{'
    self.blockEnd = config.blockEnd or '}'
    self.blockFromNewLine = config.blockFromNewLine
    if self.blockFromNewLine==nil then self.blockFromNewLine = true end
    if config.usingTabs then
        self.indentStr = '\t'
    else
        self.indentStr = '    '
    end
    self.usingTabs = config.usingTabs
    self.endOfLine = config.endOfLine or '\n'
    self.fileMark = config.fileMark or '/** This is an automatically generated class by FairyGUI. Please do not modify it. **/'
    self.lines = {}
    self.indent = 0

    self:writeMark()
end

function CodeWriter:writeMark()
    table.insert(self.lines, self.fileMark)
    table.insert(self.lines, '')
end

function CodeWriter:writeln(format, ...)
    if not format then
        table.insert(self.lines, '')
        return
    end

    local str = ''
    for i=0,self.indent-1 do
        str = str..self.indentStr
    end
    str = str..string.format(format, ...) 
    table.insert(self.lines, str)

    return self
end

function CodeWriter:startBlock()
    if self.blockFromNewLine or #self.lines==0 then
        self:writeln(self.blockStart)
    else
        local str = self.lines[#self.lines]
        self.lines[#self.lines] = str..' '..self.blockStart
    end
    self.indent = self.indent + 1

    return self
end

function CodeWriter:endBlock()
    self.indent = self.indent - 1
    self:writeln(self.blockEnd)

    return self
end

function CodeWriter:incIndent()
    self.indent = self.indent + 1

    return self
end

function CodeWriter:decIndent()
    self.indent = self.indent - 1

    return self
end

function CodeWriter:reset()
    if #self.lines>0 then self.lines = {} end
    self.indent = 0

    self:writeMark()
end

function CodeWriter:tostring()
    return table.concat(self.lines, self.endOfLine)
end

function CodeWriter:save(filePath)
    local str = table.concat(self.lines, self.endOfLine)
    
    CS.System.IO.File.WriteAllText(filePath, str)
    -- local file = io.open(filePath, 'w+b')
    -- io.output(file)
    -- io.write(str)
    -- io.close(file)
end