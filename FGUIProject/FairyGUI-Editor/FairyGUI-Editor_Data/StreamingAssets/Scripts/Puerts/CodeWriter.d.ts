interface ICodeWriterConfig {
    blockStart?: string;
    blockEnd?: string;
    blockFromNewLine?: boolean;
    usingTabs?: boolean;
    endOfLine?: string;
    fileMark?: string;
}
export default class CodeWriter {
    private blockStart;
    private blockEnd;
    private blockFromNewLine;
    private indentStr;
    private endOfLine;
    private lines;
    private indent;
    private fileMark;
    constructor(config?: ICodeWriterConfig);
    writeMark(): void;
    writeln(fmt?: string, ...args: any[]): CodeWriter;
    startBlock(): CodeWriter;
    endBlock(): CodeWriter;
    incIndent(): CodeWriter;
    decIndent(): CodeWriter;
    reset(): void;
    toString(): string;
    save(filePath: string): void;
}