import { System } from 'csharp';
import { format } from 'format';

interface ICodeWriterConfig {
    blockStart?: string;
    blockEnd?: string;
    blockFromNewLine?: boolean;
    usingTabs?: boolean;
    endOfLine?: string;
    fileMark?: string;
}

export default class CodeWriter {
    private blockStart: string;
    private blockEnd: string;
    private blockFromNewLine: boolean;
    private indentStr: string;
    private endOfLine: string;
    private lines: Array<string>;
    private indent: number;
    private fileMark: string;

    public constructor(config?: ICodeWriterConfig) {
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

    public writeMark(): void {
        this.lines.push(this.fileMark, '');
    }

    public writeln(fmt?: string, ...args: any[]): CodeWriter {
        if (!fmt) {
            this.lines.push('');
            return;
        }

        let str: string = '';
        for (let i: number = 0; i < this.indent; i++) {
            str += this.indentStr;
        }
        str += format(fmt, ...args);
        this.lines.push(str);

        return this;
    }

    public startBlock(): CodeWriter {
        if (this.blockFromNewLine || this.lines.length == 0)
            this.writeln(this.blockStart);
        else {
            let str = this.lines[this.lines.length - 1];
            this.lines[this.lines.length - 1] = str + ' ' + this.blockStart;
        }
        this.indent++;

        return this;
    }

    public endBlock(): CodeWriter {
        this.indent--;
        this.writeln(this.blockEnd);

        return this;
    }

    public incIndent(): CodeWriter {
        this.indent++;

        return this;
    }

    public decIndent(): CodeWriter {
        this.indent--;

        return this;
    }

    public reset(): void {
        this.lines.length = 0;
        this.indent = 0;

        this.writeMark();
    }

    public toString(): string {
        return this.lines.join(this.endOfLine);
    }

    public save(filePath: string): void {
        let str = this.toString();

        System.IO.File.WriteAllText(filePath, str);
    }
}