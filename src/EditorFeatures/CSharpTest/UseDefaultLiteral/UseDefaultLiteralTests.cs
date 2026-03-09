// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.UseDefaultLiteral;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.UseDefaultLiteral
{
    public class UseDefaultLiteralTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (new CSharpUseDefaultLiteralDiagnosticAnalyzer(), new CSharpUseDefaultLiteralCodeFixProvider());

        private static readonly CSharpParseOptions s_parseOptions = 
            CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp7_1);

        private static readonly TestParameters s_testParameters =
            new TestParameters(parseOptions: s_parseOptions);

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInCSharp7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"
class C
{
    void Goo(string s = [||]default(string))
    {
    }
}", parameters: new TestParameters(
    parseOptions: CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp7)));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInParameterList()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
class C
{
    void Goo(string s = [||]default(string))
    {
    }
}",
@"
class C
{
    void Goo(string s = default)
    {
    }
}", parseOptions: s_parseOptions);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInIfCheck()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
class C
{
    void Goo(string s)
    {
        if (s == [||]default(string)) { }
    }
}",
@"
class C
{
    void Goo(string s)
    {
        if (s == default) { }
    }
}", parseOptions: s_parseOptions);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInReturnStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
class C
{
    string Goo()
    {
        return [||]default(string);
    }
}",
@"
class C
{
    string Goo()
    {
        return default;
    }
}", parseOptions: s_parseOptions);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInReturnStatement2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"
class C
{
    string Goo()
    {
        return [||]default(int);
    }
}", parameters: s_testParameters);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInLambda1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
using System;

class C
{
    void Goo()
    {
        Func<string> f = () => [||]default(string);
    }
}",
@"
using System;

class C
{
    void Goo()
    {
        Func<string> f = () => [||]default;
    }
}", parseOptions: s_parseOptions);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInLambda2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"
using System;

class C
{
    void Goo()
    {
        Func<string> f = () => [||]default(int);
    }
}", parameters: s_testParameters);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInLocalInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
class C
{
    void Goo()
    {
        string s = [||]default(string);
    }
}",
@"
class C
{
    void Goo()
    {
        string s = default;
    }
}", parseOptions: s_parseOptions);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInLocalInitializer2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"
class C
{
    void Goo()
    {
        string s = [||]default(int);
    }
}", parameters: s_testParameters);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotForVar()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"
class C
{
    void Goo()
    {
        var s = [||]default(string);
    }
}",  parameters: s_testParameters);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInInvocationExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
class C
{
    void Goo()
    {
        Bar([||]default(string));
    }

    void Bar(string s) { }
}",
@"
class C
{
    void Goo()
    {
        Bar(default);
    }

    void Bar(string s) { }
}", parseOptions: s_parseOptions);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotWithMultipleOverloads()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"
class C
{
    void Goo()
    {
        Bar([||]default(string));
    }

    void Bar(string s) { }
    void Bar(int i);
}", parameters: s_testParameters);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLeftSideOfTernary()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
class C
{
    void Goo(bool b)
    {
        var v = b ? [||]default(string) : default(string);
    }
}",
@"
class C
{
    void Goo(bool b)
    {
        var v = b ? default : default(string);
    }
}", parseOptions: s_parseOptions);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRightSideOfTernary()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
class C
{
    void Goo(bool b)
    {
        var v = b ? default(string) : [||]default(string);
    }
}",
@"
class C
{
    void Goo(bool b)
    {
        var v = b ? default(string) : default;
    }
}", parseOptions: s_parseOptions);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAll1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
class C
{
    void Goo()
    {
        string s1 = {|FixAllInDocument:default|}(string);
        string s2 = default(string);
    }
}",
@"
class C
{
    void Goo()
    {
        string s1 = default;
        string s2 = default;
    }
}", parseOptions: s_parseOptions);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAll2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
class C
{
    void Goo(bool b)
    {
        string s1 = b ? {|FixAllInDocument:default|}(string) : default(string);
    }
}",
@"
class C
{
    void Goo(bool b)
    {
        string s1 = b ? default : default(string);
    }
}", parseOptions: s_parseOptions);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAll3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
class C
{
    void Goo()
    {
        string s1 = {|FixAllInDocument:default|}(string);
        string s2 = default(int);
    }
}",
@"
class C
{
    void Goo()
    {
        string s1 = default;
        string s2 = default(int);
    }
}", parseOptions: s_parseOptions);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDoNotOfferIfTypeWouldChange()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
struct S
{
    void M()
    {
        var s = new S();
        s.Equals([||]default(S));
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }
}", new TestParameters(parseOptions: s_parseOptions));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDoNotOfferIfTypeWouldChange2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
struct S<T>
{
    void M()
    {
        var s = new S<int>();
        s.Equals([||]default(S<int>));
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }
}", new TestParameters(parseOptions: s_parseOptions));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnShadowedMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
struct S
{
    void M()
    {
        var s = new S();
        s.Equals([||]default(S));
    }

    public new bool Equals(S s) => true;
}",

@"
struct S
{
    void M()
    {
        var s = new S();
        s.Equals(default);
    }

    public new bool Equals(S s) => true;
}", parseOptions: s_parseOptions);
        }

        [WorkItem(25456, "https://github.com/dotnet/roslyn/issues/25456")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInSwitchCase()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        switch (true)
        {
            case [||]default(bool):
        }
    }
}", s_testParameters);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInSwitchCase_InsideParentheses()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        switch (true)
        {
            case ([||]default(bool)):
        }
    }
}", s_testParameters);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInSwitchCase_InsideCast()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    void M()
    {
        switch (true)
        {
            case (bool)[||]default(bool):
        }
    }
}",
@"
class C
{
    void M()
    {
        switch (true)
        {
            case (bool)[||]default:
        }
    }
}", parameters: s_testParameters);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInPatternSwitchCase()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        switch (true)
        {
            case [||]default(bool) when true:
        }
    }
}", s_testParameters);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInPatternSwitchCase_InsideParentheses()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        switch (true)
        {
            case ([||]default(bool)) when true:
        }
    }
}", s_testParameters);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInPatternSwitchCase_InsideCast()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    void M()
    {
        switch (true)
        {
            case (bool)[||]default(bool) when true:
        }
    }
}",
@"
class C
{
    void M()
    {
        switch (true)
        {
            case (bool)[||]default when true:
        }
    }
}", parameters: s_testParameters);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInPatternSwitchCase_InsideWhenClause()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    void M()
    {
        switch (true)
        {
            case default(bool) when [||]default(bool):
        }
    }
}",
@"
class C
{
    void M()
    {
        switch (true)
        {
            case default(bool) when default:
        }
    }
}", parameters: s_testParameters);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInPatternIs()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        if (true is [||]default(bool));
    }
}", s_testParameters);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInPatternIs_InsideParentheses()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
class C
{
    void M()
    {
        if (true is ([||]default(bool)));
    }
}", s_testParameters);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseDefaultLiteral)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInPatternIs_InsideCast()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
class C
{
    void M()
    {
        if (true is (bool)[||]default(bool));
    }
}",
@"
class C
{
    void M()
    {
        if (true is (bool)default);
    }
}", parameters: s_testParameters);
        }
    }
}
