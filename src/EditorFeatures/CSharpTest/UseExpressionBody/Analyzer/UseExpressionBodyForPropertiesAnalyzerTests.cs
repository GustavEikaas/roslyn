// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.CodeStyle;
using Microsoft.CodeAnalysis.CSharp.UseExpressionBody;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.UseExpressionBody
{
    public class UseExpressionBodyForPropertiesAnalyzerTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (new UseExpressionBodyDiagnosticAnalyzer(), new UseExpressionBodyCodeFixProvider());

        private IDictionary<OptionKey, object> UseExpressionBody =>
            OptionsSet(
                SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedProperties, CSharpCodeStyleOptions.WhenPossibleWithSilentEnforcement),
                SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedAccessors, CSharpCodeStyleOptions.NeverWithSilentEnforcement));

        private IDictionary<OptionKey, object> UseBlockBody =>
            OptionsSet(
                SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedProperties, CSharpCodeStyleOptions.NeverWithSilentEnforcement),
                SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedAccessors, CSharpCodeStyleOptions.NeverWithSilentEnforcement));

        private IDictionary<OptionKey, object> UseBlockBodyExceptAccessor =>
            OptionsSet(
                SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedProperties, CSharpCodeStyleOptions.NeverWithSilentEnforcement),
                SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedAccessors, CSharpCodeStyleOptions.WhenPossibleWithSilentEnforcement));

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseExpressionBody1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int Goo
    {
        get
        {
            [|return|] Bar();
        }
    }
}",
@"class C
{
    int Goo => Bar();
}", options: UseExpressionBody);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingWithSetter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    int Goo
    {
        get
        {
            [|return|] Bar();
        }

        set
        {
        }
    }
}", new TestParameters(options: UseExpressionBody));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingWithAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    int Goo
    {
        [A]
        get
        {
            [|return|] Bar();
        }
    }
}", new TestParameters(options: UseExpressionBody));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnSetter1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    int Goo
    {
        set
        {
            [|Bar|]();
        }
    }
}", new TestParameters(options: UseExpressionBody));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseExpressionBody3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int Goo
    {
        get
        {
            [|throw|] new NotImplementedException();
        }
    }
}",
@"class C
{
    int Goo => throw new NotImplementedException();
}", options: UseExpressionBody);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseExpressionBody4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int Goo
    {
        get
        {
            [|throw|] new NotImplementedException(); // comment
        }
    }
}",
@"class C
{
    int Goo => throw new NotImplementedException(); // comment
}", options: UseExpressionBody);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseBlockBody1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int Goo [|=>|] Bar();
}",
@"class C
{
    int Goo
    {
        get
        {
            return Bar();
        }
    }
}", options: UseBlockBody);
        }

        [WorkItem(20363, "https://github.com/dotnet/roslyn/issues/20363")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseBlockBodyForAccessorEventWhenAccessorWantExpression1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int Goo [|=>|] Bar();
}",
@"class C
{
    int Goo
    {
        get
        {
            return Bar();
        }
    }
}", options: UseBlockBodyExceptAccessor);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseBlockBody3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int Goo [|=>|] throw new NotImplementedException();
}",
@"class C
{
    int Goo
    {
        get
        {
            throw new NotImplementedException();
        }
    }
}", options: UseBlockBody);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseBlockBody4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int Goo [|=>|] throw new NotImplementedException(); // comment
}",
@"class C
{
    int Goo
    {
        get
        {
            throw new NotImplementedException(); // comment
        }
    }
}", options: UseBlockBody);
        }

        [WorkItem(16386, "https://github.com/dotnet/roslyn/issues/16386")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseExpressionBodyKeepTrailingTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    private string _prop = ""HELLO THERE!"";
    public string Prop { get { [|return|] _prop; } }

    public string OtherThing => ""Pickles"";
}",
@"class C
{
    private string _prop = ""HELLO THERE!"";
    public string Prop => _prop;

    public string OtherThing => ""Pickles"";
}", options: UseExpressionBody);
        }

        [WorkItem(19235, "https://github.com/dotnet/roslyn/issues/19235")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDirectivesInBlockBody1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class C
{
    int Goo
    {
        get
        {
#if true
            [|return|] Bar();
#else
            return Baz();
#endif
        }
    }
}",

@"class C
{
    int Goo =>
#if true
            Bar();
#else
            return Baz();
#endif

}",
    parameters: new TestParameters(options: UseExpressionBody));
        }

        [WorkItem(19235, "https://github.com/dotnet/roslyn/issues/19235")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDirectivesInBlockBody2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class C
{
    int Goo
    {
        get
        {
#if false
            return Bar();
#else
            [|return|] Baz();
#endif
        }
    }
}",

@"class C
{
    int Goo =>
#if false
            return Bar();
#else
            Baz();
#endif

}",
    parameters: new TestParameters(options: UseExpressionBody));
        }

        [WorkItem(19235, "https://github.com/dotnet/roslyn/issues/19235")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingWithDirectivesInExpressionBody1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    int Goo [|=>|]
#if true
            Bar();
#else
            Baz();
#endif
}", parameters: new TestParameters(options: UseBlockBody));
        }

        [WorkItem(19235, "https://github.com/dotnet/roslyn/issues/19235")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingWithDirectivesInExpressionBody2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    int Goo [|=>|]
#if false
            Bar();
#else
            Baz();
#endif
}", parameters: new TestParameters(options: UseBlockBody));
        }

        [WorkItem(19193, "https://github.com/dotnet/roslyn/issues/19193")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMoveTriviaFromExpressionToReturnStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int Goo(int i) [|=>|]
        //comment
        i * i;
}",
@"class C
{
    int Goo(int i)
    {
        //comment
        return i * i;
    }
}",
    options: UseBlockBody);
        }

        [WorkItem(20362, "https://github.com/dotnet/roslyn/issues/20362")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOfferToConvertToBlockEvenIfExpressionBodyPreferredIfHasThrowExpressionPriorToCSharp7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
using System;
class C
{
    int Goo [|=>|] throw new NotImplementedException();
}",
@"
using System;
class C
{
    int Goo
    {
        get
        {
            throw new NotImplementedException();
        }
    }
}", options: UseExpressionBody, parseOptions: CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp6));
        }

        [WorkItem(20362, "https://github.com/dotnet/roslyn/issues/20362")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOfferToConvertToBlockEvenIfExpressionBodyPreferredIfHasThrowExpressionPriorToCSharp7_FixAll()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
using System;
class C
{
    int Goo {|FixAllInDocument:=>|} throw new NotImplementedException();
    int Bar => throw new NotImplementedException();
}",
@"
using System;
class C
{
    int Goo
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    int Bar
    {
        get
        {
            throw new NotImplementedException();
        }
    }
}", options: UseExpressionBody, parseOptions: CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp6));
        }
    }
}
