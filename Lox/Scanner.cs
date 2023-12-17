using System;
using System.Collections.Generic;
using System.Globalization;
using static Lox.TokenType;

namespace Lox;

public class Scanner(string source)
{
    private readonly List<Token> _tokens = [];
    private int _start;
    private int _current;
    private int _line = 1;
    private static readonly Dictionary<string, TokenType> Keywords = new() {
        { "and", AND },
        { "class", CLASS },
        { "else", ELSE },
        { "false", FALSE },
        { "for", FOR },
        { "fun", FUN },
        { "if", IF },
        { "nil", NIL },
        { "or", OR },
        { "print", PRINT },
        { "return", RETURN },
        { "super", SUPER },
        { "this", THIS },
        { "true", TRUE },
        { "var", VAR },
        { "while", WHILE }
    };

    internal List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            _start = _current;
            ScanToken();
        }

        _tokens.Add(new Token(EOF, "", null, _line));
        return _tokens;
    }

    private void ScanToken()
    {
        var c = Advance();

        switch (c)
        {
            case '(': AddToken(LEFT_PAREN); break;
            case ')': AddToken(RIGHT_PAREN); break;
            case '{': AddToken(LEFT_BRACE); break;
            case '}': AddToken(RIGHT_BRACE); break;
            case ',': AddToken(COMMA); break;
            case '.': AddToken(DOT); break;
            case '-': AddToken(MINUS); break;
            case '+': AddToken(PLUS); break;
            case ';': AddToken(SEMICOLON); break;
            case '*': AddToken(STAR); break;
            case '!':
                AddToken(Match('=') ? BANG_EQUAL : BANG);
                break;
            case '=':
                AddToken(Match('=') ? EQUAL_EQUAL : EQUAL);
                break;
            case '<':
                AddToken(Match('=') ? LESS_EQUAL : LESS);
                break;
            case '>':
                AddToken(Match('=') ? GREATER_EQUAL : GREATER);
                break;
            case '/':
                if (Match('/'))
                {
                    // A comment goes until the end of the line.
                    while (Peek() != '\n' && !IsAtEnd()) Advance();
                }
                else
                {
                    AddToken(SLASH);
                }
                break;
            case ' ':
            case '\r':
            case '\t':
                // Ignore whitespace.
                break;
            case '\n':
                _line++;
                break;
            case '"': String(); break;
            default:
                if (IsDigit(c))
                {
                    Number();
                }
                else if (IsAlpha(c))
                {
                    Identifier();
                }
                else
                {
                    Lox.Error(_line, "Unexpected character.");
                }
                break;
        }
    }

    private void Identifier()
    {
        while (IsAlphaNumeric(Peek())) Advance();

        var text = source.Substring(_start, _current - _start);
        var type = Keywords.GetValueOrDefault(text, IDENTIFIER);
        AddToken(type);
    }

    private void Number()
    {
        while (IsDigit(Peek())) Advance();

        if (Peek() == '.' && IsDigit(PeekNext()))
        {
            Advance();

            while (IsDigit(Peek())) Advance();
        }

        AddToken(NUMBER, double.Parse(source.AsSpan(_start, _current - _start), CultureInfo.InvariantCulture));
    }

    private void String()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n') _line++;
            Advance();
        }

        if (IsAtEnd())
        {
            Lox.Error(_line, "Unterminated string.");
            return;
        }

        Advance();

        var value = source.Substring(_start + 1, _current - _start - 2);
        AddToken(STRING, value);
    }

    private bool Match(char expected)
    {
        if (IsAtEnd())
        {
            return false;
        }

        if (source[_current] != expected)
        {
            return false;
        }

        _current++;
        return true;
    }

    private char Peek() => IsAtEnd() ? '\0' : source[_current];
    private char PeekNext() => _current + 1 >= source.Length ? '\0' : source[_current + 1];
    private static bool IsAlpha(char c) => c is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or '_';
    private static bool IsAlphaNumeric(char c) => IsAlpha(c) || IsDigit(c);

    // .NET Char.IsDigit behaves differently from Java, we can safely use the standard library
    // https://docs.microsoft.com/en-us/dotnet/api/system.char.isdigit?view=netcore-3.1
    private static bool IsDigit(char c) => char.IsDigit(c);
    private bool IsAtEnd() => _current >= source.Length;

    private char Advance()
    {
        _current++;
        return source[_current - 1];
    }

    private void AddToken(TokenType type) => AddToken(type, null);

    private void AddToken(TokenType type, object? literal)
    {
        var text = source.Substring(_start, _current - _start);
        _tokens.Add(new Token(type, text, literal, _line));
    }
}