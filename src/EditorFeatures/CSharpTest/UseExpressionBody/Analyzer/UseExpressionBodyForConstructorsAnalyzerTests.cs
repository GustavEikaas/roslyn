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
    public class UseExpressionBodyForConstructorsAnalyzerTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (new UseExpressionBodyDiagnosticAnalyzer(), new UseExpressionBodyCodeFixProvider());

        private IDictionary<OptionKey, object> UseExpressionBody =>
            Option(CSharpCodeStyleOptions.PreferExpressionBodiedConstructors, CSharpCodeStyleOptions.WhenPossibleWithSilentEnforcement);

        private IDictionary<OptionKey, object> UseBlockBody =>
            Option(CSharpCodeStyleOptions.PreferExpressionBodiedConstructors, CSharpCodeStyleOptions.NeverWithSilentEnforcement);

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseExpressionBody1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public C()
    {
        [|Bar|]();
    }
}",
@"class C
{
    public C() => Bar();
}", options: UseExpressionBody);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseExpressionBody2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public C()
    {
        a = [|Bar|]();
    }
}",
@"class C
{
    public C() => a = Bar();
}", options: UseExpressionBody);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseExpressionBody3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public C()
    {
        [|throw|] new NotImplementedException();
    }
}",
@"class C
{
    public C() => throw new NotImplementedException();
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
    public C()
    {
        [|throw|] new NotImplementedException(); // comment
    }
}",
@"class C
{
    public C() => throw new NotImplementedException(); // comment
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
    public C() [|=>|] Bar();
}",
@"class C
{
    public C()
    {
        Bar();
    }
}", options: UseBlockBody);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseBlockBody2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public C() [|=>|] a = Bar();
}",
@"class C
{
    public C()
    {
        a = Bar();
    }
}", options: UseBlockBody);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUseBlockBody3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public C() [|=>|] throw new NotImplementedException();
}",
@"class C
{
    public C()
    {
        throw new NotImplementedException();
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
    public C() [|=>|] throw new NotImplementedException(); // comment
}",
@"class C
{
    public C()
    {
        throw new NotImplementedException(); // comment
    }
}", options: UseBlockBody);
        }

        [WorkItem(20362, "https://github.com/dotnet/roslyn/issues/20362")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOfferToConvertToBlockEvenIfExpressionBodyPreferredIfPriorToCSharp7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
using System;
class C
{
    public C() [|=>|] throw new NotImplementedException();
}",
@"
using System;
class C
{
    public C()
    {
        throw new NotImplementedException();
    }
}", options: UseExpressionBody, parseOptions: CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp6));
        }

        [WorkItem(20362, "https://github.com/dotnet/roslyn/issues/20362")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOfferToConvertToBlockEvenIfExpressionBodyPreferredIfPriorToCSharp7_FixAll()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
using System;
class C
{
    public C() {|FixAllInDocument:=>|} throw new NotImplementedException();
    public C(int i) => throw new NotImplementedException();
}",
@"
using System;
class C
{
    public C()
    {
        throw new NotImplementedException();
    }

    public C(int i)
    {
        throw new NotImplementedException();
    }
}", options: UseExpressionBody, parseOptions: CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp6));
        }
    }
}
