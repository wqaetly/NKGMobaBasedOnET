"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const csharp_1 = require("csharp");
const format_1 = require("format");
class CodeWriter {
    constructor(config) {
        config = config || {};
        this.blockStart = config.blockStart || '{';
        this.blockEnd = config.blockEnd || '}';
        this.blockFromNewLine = config.blockFromNewLine != undefined ? config.blockFromNewLine : true;
        if (config.usingTabs)
            this.indentStr = '\t';
        else
            this.indentStr = '    ';
        this.endOfLine = config.endOfLine || '\n';
        this.fileMark = config.fileMark ||
            '/** This is an automatically generated class by FairyGUI. Please do not modify it. **/';
        this.lines = [];
        this.indent = 0;
        this.writeMark();
    }
    writeMark() {
        this.lines.push(this.fileMark, '');
    }
    writeln(fmt, ...args) {
        if (!fmt) {
            this.lines.push('');
            return;
        }
        let str = '';
        for (let i = 0; i < this.indent; i++) {
            str += this.indentStr;
        }
        str += format_1.format(fmt, ...args);
        this.lines.push(str);
        return this;
    }
    startBlock() {
        if (this.blockFromNewLine || this.lines.length == 0)
            this.writeln(this.blockStart);
        else {
            let str = this.lines[this.lines.length - 1];
            this.lines[this.lines.length - 1] = str + ' ' + this.blockStart;
        }
        this.indent++;
        return this;
    }
    endBlock() {
        this.indent--;
        this.writeln(this.blockEnd);
        return this;
    }
    incIndent() {
        this.indent++;
        return this;
    }
    decIndent() {
        this.indent--;
        return this;
    }
    reset() {
        this.lines.length = 0;
        this.indent = 0;
        this.writeMark();
    }
    toString() {
        return this.lines.join(this.endOfLine);
    }
    save(filePath) {
        let str = this.toString();
        csharp_1.System.IO.File.WriteAllText(filePath, str);
    }
}
exports.default = CodeWriter;
