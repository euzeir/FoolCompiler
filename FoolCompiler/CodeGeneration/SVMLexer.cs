//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from SVM.g4 by ANTLR 4.8

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419


using FoolCompiler.CodeGeneration;
using System.Collections.Generic;

using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public partial class SVMLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		PUSH=1, POP=2, ADD=3, SUB=4, MULT=5, DIV=6, STOREW=7, LOADW=8, BRANCH=9, 
		BRANCHEQ=10, BRANCHLESSEQ=11, JS=12, LOADRA=13, STORERA=14, LOADRV=15, 
		STORERV=16, LOADFP=17, STOREFP=18, COPYFP=19, LOADHP=20, STOREHP=21, PRINT=22, 
		HALT=23, NEW=24, LOADMETHOD=25, COPYTOPSTACK=26, COL=27, LABEL=28, NUMBER=29, 
		WHITESP=30, ERR=31;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"PUSH", "POP", "ADD", "SUB", "MULT", "DIV", "STOREW", "LOADW", "BRANCH", 
		"BRANCHEQ", "BRANCHLESSEQ", "JS", "LOADRA", "STORERA", "LOADRV", "STORERV", 
		"LOADFP", "STOREFP", "COPYFP", "LOADHP", "STOREHP", "PRINT", "HALT", "NEW", 
		"LOADMETHOD", "COPYTOPSTACK", "COL", "LABEL", "NUMBER", "WHITESP", "ERR"
	};


	public List<string> errors = new List<string>();


	public SVMLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public SVMLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'push'", "'pop'", "'add'", "'sub'", "'mult'", "'div'", "'sw'", 
		"'lw'", "'b'", "'beq'", "'bleq'", "'js'", "'lra'", "'sra'", "'lrv'", "'srv'", 
		"'lfp'", "'sfp'", "'cfp'", "'lhp'", "'shp'", "'print'", "'halt'", "'new'", 
		"'loadm'", "'cts'", "':'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "PUSH", "POP", "ADD", "SUB", "MULT", "DIV", "STOREW", "LOADW", "BRANCH", 
		"BRANCHEQ", "BRANCHLESSEQ", "JS", "LOADRA", "STORERA", "LOADRV", "STORERV", 
		"LOADFP", "STOREFP", "COPYFP", "LOADHP", "STOREHP", "PRINT", "HALT", "NEW", 
		"LOADMETHOD", "COPYTOPSTACK", "COL", "LABEL", "NUMBER", "WHITESP", "ERR"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "SVM.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static SVMLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	public override void Action(RuleContext _localctx, int ruleIndex, int actionIndex) {
		switch (ruleIndex) {
		case 30 : ERR_action(_localctx, actionIndex); break;
		}
	}
	private void ERR_action(RuleContext _localctx, int actionIndex) {
		switch (actionIndex) {
		case 0:  errors.Add("Invalid char: " + Text);  break;
		}
	}

	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x2', '!', '\xCE', '\b', '\x1', '\x4', '\x2', '\t', '\x2', 
		'\x4', '\x3', '\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', 
		'\x5', '\x4', '\x6', '\t', '\x6', '\x4', '\a', '\t', '\a', '\x4', '\b', 
		'\t', '\b', '\x4', '\t', '\t', '\t', '\x4', '\n', '\t', '\n', '\x4', '\v', 
		'\t', '\v', '\x4', '\f', '\t', '\f', '\x4', '\r', '\t', '\r', '\x4', '\xE', 
		'\t', '\xE', '\x4', '\xF', '\t', '\xF', '\x4', '\x10', '\t', '\x10', '\x4', 
		'\x11', '\t', '\x11', '\x4', '\x12', '\t', '\x12', '\x4', '\x13', '\t', 
		'\x13', '\x4', '\x14', '\t', '\x14', '\x4', '\x15', '\t', '\x15', '\x4', 
		'\x16', '\t', '\x16', '\x4', '\x17', '\t', '\x17', '\x4', '\x18', '\t', 
		'\x18', '\x4', '\x19', '\t', '\x19', '\x4', '\x1A', '\t', '\x1A', '\x4', 
		'\x1B', '\t', '\x1B', '\x4', '\x1C', '\t', '\x1C', '\x4', '\x1D', '\t', 
		'\x1D', '\x4', '\x1E', '\t', '\x1E', '\x4', '\x1F', '\t', '\x1F', '\x4', 
		' ', '\t', ' ', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x2', 
		'\x3', '\x2', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x4', '\x3', '\x5', 
		'\x3', '\x5', '\x3', '\x5', '\x3', '\x5', '\x3', '\x6', '\x3', '\x6', 
		'\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\a', '\x3', '\a', '\x3', 
		'\a', '\x3', '\a', '\x3', '\b', '\x3', '\b', '\x3', '\b', '\x3', '\t', 
		'\x3', '\t', '\x3', '\t', '\x3', '\n', '\x3', '\n', '\x3', '\v', '\x3', 
		'\v', '\x3', '\v', '\x3', '\v', '\x3', '\f', '\x3', '\f', '\x3', '\f', 
		'\x3', '\f', '\x3', '\f', '\x3', '\r', '\x3', '\r', '\x3', '\r', '\x3', 
		'\xE', '\x3', '\xE', '\x3', '\xE', '\x3', '\xE', '\x3', '\xF', '\x3', 
		'\xF', '\x3', '\xF', '\x3', '\xF', '\x3', '\x10', '\x3', '\x10', '\x3', 
		'\x10', '\x3', '\x10', '\x3', '\x11', '\x3', '\x11', '\x3', '\x11', '\x3', 
		'\x11', '\x3', '\x12', '\x3', '\x12', '\x3', '\x12', '\x3', '\x12', '\x3', 
		'\x13', '\x3', '\x13', '\x3', '\x13', '\x3', '\x13', '\x3', '\x14', '\x3', 
		'\x14', '\x3', '\x14', '\x3', '\x14', '\x3', '\x15', '\x3', '\x15', '\x3', 
		'\x15', '\x3', '\x15', '\x3', '\x16', '\x3', '\x16', '\x3', '\x16', '\x3', 
		'\x16', '\x3', '\x17', '\x3', '\x17', '\x3', '\x17', '\x3', '\x17', '\x3', 
		'\x17', '\x3', '\x17', '\x3', '\x18', '\x3', '\x18', '\x3', '\x18', '\x3', 
		'\x18', '\x3', '\x18', '\x3', '\x19', '\x3', '\x19', '\x3', '\x19', '\x3', 
		'\x19', '\x3', '\x1A', '\x3', '\x1A', '\x3', '\x1A', '\x3', '\x1A', '\x3', 
		'\x1A', '\x3', '\x1A', '\x3', '\x1B', '\x3', '\x1B', '\x3', '\x1B', '\x3', 
		'\x1B', '\x3', '\x1C', '\x3', '\x1C', '\x3', '\x1D', '\x3', '\x1D', '\a', 
		'\x1D', '\xB1', '\n', '\x1D', '\f', '\x1D', '\xE', '\x1D', '\xB4', '\v', 
		'\x1D', '\x3', '\x1E', '\x3', '\x1E', '\x5', '\x1E', '\xB8', '\n', '\x1E', 
		'\x3', '\x1E', '\x3', '\x1E', '\a', '\x1E', '\xBC', '\n', '\x1E', '\f', 
		'\x1E', '\xE', '\x1E', '\xBF', '\v', '\x1E', '\x5', '\x1E', '\xC1', '\n', 
		'\x1E', '\x3', '\x1F', '\x6', '\x1F', '\xC4', '\n', '\x1F', '\r', '\x1F', 
		'\xE', '\x1F', '\xC5', '\x3', '\x1F', '\x3', '\x1F', '\x3', ' ', '\x3', 
		' ', '\x3', ' ', '\x3', ' ', '\x3', ' ', '\x2', '\x2', '!', '\x3', '\x3', 
		'\x5', '\x4', '\a', '\x5', '\t', '\x6', '\v', '\a', '\r', '\b', '\xF', 
		'\t', '\x11', '\n', '\x13', '\v', '\x15', '\f', '\x17', '\r', '\x19', 
		'\xE', '\x1B', '\xF', '\x1D', '\x10', '\x1F', '\x11', '!', '\x12', '#', 
		'\x13', '%', '\x14', '\'', '\x15', ')', '\x16', '+', '\x17', '-', '\x18', 
		'/', '\x19', '\x31', '\x1A', '\x33', '\x1B', '\x35', '\x1C', '\x37', '\x1D', 
		'\x39', '\x1E', ';', '\x1F', '=', ' ', '?', '!', '\x3', '\x2', '\x5', 
		'\x4', '\x2', '\x43', '\\', '\x63', '|', '\x5', '\x2', '\x32', ';', '\x43', 
		'\\', '\x63', '|', '\x5', '\x2', '\v', '\f', '\xF', '\xF', '\"', '\"', 
		'\x2', '\xD2', '\x2', '\x3', '\x3', '\x2', '\x2', '\x2', '\x2', '\x5', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '\a', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\t', '\x3', '\x2', '\x2', '\x2', '\x2', '\v', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '\r', '\x3', '\x2', '\x2', '\x2', '\x2', '\xF', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x11', '\x3', '\x2', '\x2', '\x2', '\x2', '\x13', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x15', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x17', '\x3', '\x2', '\x2', '\x2', '\x2', '\x19', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x1B', '\x3', '\x2', '\x2', '\x2', '\x2', '\x1D', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x1F', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'!', '\x3', '\x2', '\x2', '\x2', '\x2', '#', '\x3', '\x2', '\x2', '\x2', 
		'\x2', '%', '\x3', '\x2', '\x2', '\x2', '\x2', '\'', '\x3', '\x2', '\x2', 
		'\x2', '\x2', ')', '\x3', '\x2', '\x2', '\x2', '\x2', '+', '\x3', '\x2', 
		'\x2', '\x2', '\x2', '-', '\x3', '\x2', '\x2', '\x2', '\x2', '/', '\x3', 
		'\x2', '\x2', '\x2', '\x2', '\x31', '\x3', '\x2', '\x2', '\x2', '\x2', 
		'\x33', '\x3', '\x2', '\x2', '\x2', '\x2', '\x35', '\x3', '\x2', '\x2', 
		'\x2', '\x2', '\x37', '\x3', '\x2', '\x2', '\x2', '\x2', '\x39', '\x3', 
		'\x2', '\x2', '\x2', '\x2', ';', '\x3', '\x2', '\x2', '\x2', '\x2', '=', 
		'\x3', '\x2', '\x2', '\x2', '\x2', '?', '\x3', '\x2', '\x2', '\x2', '\x3', 
		'\x41', '\x3', '\x2', '\x2', '\x2', '\x5', '\x46', '\x3', '\x2', '\x2', 
		'\x2', '\a', 'J', '\x3', '\x2', '\x2', '\x2', '\t', 'N', '\x3', '\x2', 
		'\x2', '\x2', '\v', 'R', '\x3', '\x2', '\x2', '\x2', '\r', 'W', '\x3', 
		'\x2', '\x2', '\x2', '\xF', '[', '\x3', '\x2', '\x2', '\x2', '\x11', '^', 
		'\x3', '\x2', '\x2', '\x2', '\x13', '\x61', '\x3', '\x2', '\x2', '\x2', 
		'\x15', '\x63', '\x3', '\x2', '\x2', '\x2', '\x17', 'g', '\x3', '\x2', 
		'\x2', '\x2', '\x19', 'l', '\x3', '\x2', '\x2', '\x2', '\x1B', 'o', '\x3', 
		'\x2', '\x2', '\x2', '\x1D', 's', '\x3', '\x2', '\x2', '\x2', '\x1F', 
		'w', '\x3', '\x2', '\x2', '\x2', '!', '{', '\x3', '\x2', '\x2', '\x2', 
		'#', '\x7F', '\x3', '\x2', '\x2', '\x2', '%', '\x83', '\x3', '\x2', '\x2', 
		'\x2', '\'', '\x87', '\x3', '\x2', '\x2', '\x2', ')', '\x8B', '\x3', '\x2', 
		'\x2', '\x2', '+', '\x8F', '\x3', '\x2', '\x2', '\x2', '-', '\x93', '\x3', 
		'\x2', '\x2', '\x2', '/', '\x99', '\x3', '\x2', '\x2', '\x2', '\x31', 
		'\x9E', '\x3', '\x2', '\x2', '\x2', '\x33', '\xA2', '\x3', '\x2', '\x2', 
		'\x2', '\x35', '\xA8', '\x3', '\x2', '\x2', '\x2', '\x37', '\xAC', '\x3', 
		'\x2', '\x2', '\x2', '\x39', '\xAE', '\x3', '\x2', '\x2', '\x2', ';', 
		'\xC0', '\x3', '\x2', '\x2', '\x2', '=', '\xC3', '\x3', '\x2', '\x2', 
		'\x2', '?', '\xC9', '\x3', '\x2', '\x2', '\x2', '\x41', '\x42', '\a', 
		'r', '\x2', '\x2', '\x42', '\x43', '\a', 'w', '\x2', '\x2', '\x43', '\x44', 
		'\a', 'u', '\x2', '\x2', '\x44', '\x45', '\a', 'j', '\x2', '\x2', '\x45', 
		'\x4', '\x3', '\x2', '\x2', '\x2', '\x46', 'G', '\a', 'r', '\x2', '\x2', 
		'G', 'H', '\a', 'q', '\x2', '\x2', 'H', 'I', '\a', 'r', '\x2', '\x2', 
		'I', '\x6', '\x3', '\x2', '\x2', '\x2', 'J', 'K', '\a', '\x63', '\x2', 
		'\x2', 'K', 'L', '\a', '\x66', '\x2', '\x2', 'L', 'M', '\a', '\x66', '\x2', 
		'\x2', 'M', '\b', '\x3', '\x2', '\x2', '\x2', 'N', 'O', '\a', 'u', '\x2', 
		'\x2', 'O', 'P', '\a', 'w', '\x2', '\x2', 'P', 'Q', '\a', '\x64', '\x2', 
		'\x2', 'Q', '\n', '\x3', '\x2', '\x2', '\x2', 'R', 'S', '\a', 'o', '\x2', 
		'\x2', 'S', 'T', '\a', 'w', '\x2', '\x2', 'T', 'U', '\a', 'n', '\x2', 
		'\x2', 'U', 'V', '\a', 'v', '\x2', '\x2', 'V', '\f', '\x3', '\x2', '\x2', 
		'\x2', 'W', 'X', '\a', '\x66', '\x2', '\x2', 'X', 'Y', '\a', 'k', '\x2', 
		'\x2', 'Y', 'Z', '\a', 'x', '\x2', '\x2', 'Z', '\xE', '\x3', '\x2', '\x2', 
		'\x2', '[', '\\', '\a', 'u', '\x2', '\x2', '\\', ']', '\a', 'y', '\x2', 
		'\x2', ']', '\x10', '\x3', '\x2', '\x2', '\x2', '^', '_', '\a', 'n', '\x2', 
		'\x2', '_', '`', '\a', 'y', '\x2', '\x2', '`', '\x12', '\x3', '\x2', '\x2', 
		'\x2', '\x61', '\x62', '\a', '\x64', '\x2', '\x2', '\x62', '\x14', '\x3', 
		'\x2', '\x2', '\x2', '\x63', '\x64', '\a', '\x64', '\x2', '\x2', '\x64', 
		'\x65', '\a', 'g', '\x2', '\x2', '\x65', '\x66', '\a', 's', '\x2', '\x2', 
		'\x66', '\x16', '\x3', '\x2', '\x2', '\x2', 'g', 'h', '\a', '\x64', '\x2', 
		'\x2', 'h', 'i', '\a', 'n', '\x2', '\x2', 'i', 'j', '\a', 'g', '\x2', 
		'\x2', 'j', 'k', '\a', 's', '\x2', '\x2', 'k', '\x18', '\x3', '\x2', '\x2', 
		'\x2', 'l', 'm', '\a', 'l', '\x2', '\x2', 'm', 'n', '\a', 'u', '\x2', 
		'\x2', 'n', '\x1A', '\x3', '\x2', '\x2', '\x2', 'o', 'p', '\a', 'n', '\x2', 
		'\x2', 'p', 'q', '\a', 't', '\x2', '\x2', 'q', 'r', '\a', '\x63', '\x2', 
		'\x2', 'r', '\x1C', '\x3', '\x2', '\x2', '\x2', 's', 't', '\a', 'u', '\x2', 
		'\x2', 't', 'u', '\a', 't', '\x2', '\x2', 'u', 'v', '\a', '\x63', '\x2', 
		'\x2', 'v', '\x1E', '\x3', '\x2', '\x2', '\x2', 'w', 'x', '\a', 'n', '\x2', 
		'\x2', 'x', 'y', '\a', 't', '\x2', '\x2', 'y', 'z', '\a', 'x', '\x2', 
		'\x2', 'z', ' ', '\x3', '\x2', '\x2', '\x2', '{', '|', '\a', 'u', '\x2', 
		'\x2', '|', '}', '\a', 't', '\x2', '\x2', '}', '~', '\a', 'x', '\x2', 
		'\x2', '~', '\"', '\x3', '\x2', '\x2', '\x2', '\x7F', '\x80', '\a', 'n', 
		'\x2', '\x2', '\x80', '\x81', '\a', 'h', '\x2', '\x2', '\x81', '\x82', 
		'\a', 'r', '\x2', '\x2', '\x82', '$', '\x3', '\x2', '\x2', '\x2', '\x83', 
		'\x84', '\a', 'u', '\x2', '\x2', '\x84', '\x85', '\a', 'h', '\x2', '\x2', 
		'\x85', '\x86', '\a', 'r', '\x2', '\x2', '\x86', '&', '\x3', '\x2', '\x2', 
		'\x2', '\x87', '\x88', '\a', '\x65', '\x2', '\x2', '\x88', '\x89', '\a', 
		'h', '\x2', '\x2', '\x89', '\x8A', '\a', 'r', '\x2', '\x2', '\x8A', '(', 
		'\x3', '\x2', '\x2', '\x2', '\x8B', '\x8C', '\a', 'n', '\x2', '\x2', '\x8C', 
		'\x8D', '\a', 'j', '\x2', '\x2', '\x8D', '\x8E', '\a', 'r', '\x2', '\x2', 
		'\x8E', '*', '\x3', '\x2', '\x2', '\x2', '\x8F', '\x90', '\a', 'u', '\x2', 
		'\x2', '\x90', '\x91', '\a', 'j', '\x2', '\x2', '\x91', '\x92', '\a', 
		'r', '\x2', '\x2', '\x92', ',', '\x3', '\x2', '\x2', '\x2', '\x93', '\x94', 
		'\a', 'r', '\x2', '\x2', '\x94', '\x95', '\a', 't', '\x2', '\x2', '\x95', 
		'\x96', '\a', 'k', '\x2', '\x2', '\x96', '\x97', '\a', 'p', '\x2', '\x2', 
		'\x97', '\x98', '\a', 'v', '\x2', '\x2', '\x98', '.', '\x3', '\x2', '\x2', 
		'\x2', '\x99', '\x9A', '\a', 'j', '\x2', '\x2', '\x9A', '\x9B', '\a', 
		'\x63', '\x2', '\x2', '\x9B', '\x9C', '\a', 'n', '\x2', '\x2', '\x9C', 
		'\x9D', '\a', 'v', '\x2', '\x2', '\x9D', '\x30', '\x3', '\x2', '\x2', 
		'\x2', '\x9E', '\x9F', '\a', 'p', '\x2', '\x2', '\x9F', '\xA0', '\a', 
		'g', '\x2', '\x2', '\xA0', '\xA1', '\a', 'y', '\x2', '\x2', '\xA1', '\x32', 
		'\x3', '\x2', '\x2', '\x2', '\xA2', '\xA3', '\a', 'n', '\x2', '\x2', '\xA3', 
		'\xA4', '\a', 'q', '\x2', '\x2', '\xA4', '\xA5', '\a', '\x63', '\x2', 
		'\x2', '\xA5', '\xA6', '\a', '\x66', '\x2', '\x2', '\xA6', '\xA7', '\a', 
		'o', '\x2', '\x2', '\xA7', '\x34', '\x3', '\x2', '\x2', '\x2', '\xA8', 
		'\xA9', '\a', '\x65', '\x2', '\x2', '\xA9', '\xAA', '\a', 'v', '\x2', 
		'\x2', '\xAA', '\xAB', '\a', 'u', '\x2', '\x2', '\xAB', '\x36', '\x3', 
		'\x2', '\x2', '\x2', '\xAC', '\xAD', '\a', '<', '\x2', '\x2', '\xAD', 
		'\x38', '\x3', '\x2', '\x2', '\x2', '\xAE', '\xB2', '\t', '\x2', '\x2', 
		'\x2', '\xAF', '\xB1', '\t', '\x3', '\x2', '\x2', '\xB0', '\xAF', '\x3', 
		'\x2', '\x2', '\x2', '\xB1', '\xB4', '\x3', '\x2', '\x2', '\x2', '\xB2', 
		'\xB0', '\x3', '\x2', '\x2', '\x2', '\xB2', '\xB3', '\x3', '\x2', '\x2', 
		'\x2', '\xB3', ':', '\x3', '\x2', '\x2', '\x2', '\xB4', '\xB2', '\x3', 
		'\x2', '\x2', '\x2', '\xB5', '\xC1', '\a', '\x32', '\x2', '\x2', '\xB6', 
		'\xB8', '\a', '/', '\x2', '\x2', '\xB7', '\xB6', '\x3', '\x2', '\x2', 
		'\x2', '\xB7', '\xB8', '\x3', '\x2', '\x2', '\x2', '\xB8', '\xB9', '\x3', 
		'\x2', '\x2', '\x2', '\xB9', '\xBD', '\x4', '\x33', ';', '\x2', '\xBA', 
		'\xBC', '\x4', '\x32', ';', '\x2', '\xBB', '\xBA', '\x3', '\x2', '\x2', 
		'\x2', '\xBC', '\xBF', '\x3', '\x2', '\x2', '\x2', '\xBD', '\xBB', '\x3', 
		'\x2', '\x2', '\x2', '\xBD', '\xBE', '\x3', '\x2', '\x2', '\x2', '\xBE', 
		'\xC1', '\x3', '\x2', '\x2', '\x2', '\xBF', '\xBD', '\x3', '\x2', '\x2', 
		'\x2', '\xC0', '\xB5', '\x3', '\x2', '\x2', '\x2', '\xC0', '\xB7', '\x3', 
		'\x2', '\x2', '\x2', '\xC1', '<', '\x3', '\x2', '\x2', '\x2', '\xC2', 
		'\xC4', '\t', '\x4', '\x2', '\x2', '\xC3', '\xC2', '\x3', '\x2', '\x2', 
		'\x2', '\xC4', '\xC5', '\x3', '\x2', '\x2', '\x2', '\xC5', '\xC3', '\x3', 
		'\x2', '\x2', '\x2', '\xC5', '\xC6', '\x3', '\x2', '\x2', '\x2', '\xC6', 
		'\xC7', '\x3', '\x2', '\x2', '\x2', '\xC7', '\xC8', '\b', '\x1F', '\x2', 
		'\x2', '\xC8', '>', '\x3', '\x2', '\x2', '\x2', '\xC9', '\xCA', '\v', 
		'\x2', '\x2', '\x2', '\xCA', '\xCB', '\b', ' ', '\x3', '\x2', '\xCB', 
		'\xCC', '\x3', '\x2', '\x2', '\x2', '\xCC', '\xCD', '\b', ' ', '\x2', 
		'\x2', '\xCD', '@', '\x3', '\x2', '\x2', '\x2', '\b', '\x2', '\xB2', '\xB7', 
		'\xBD', '\xC0', '\xC5', '\x4', '\x2', '\x3', '\x2', '\x3', ' ', '\x2',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
