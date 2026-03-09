// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.CodeAnalysis.CSharp.CodeStyle;
using Microsoft.CodeAnalysis.CSharp.UseExpressionBody;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.CodeRefactorings;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.UseExpressionBody
{
    public class UseExpressionBodyForConversionOperatorsRefactoringTests : AbstractCSharpCodeActionTest
    {
        protected override CodeRefactoringProvider CreateCodeRefactoringProvider(Workspace workspace, TestParameters parameters)
            => new UseExpressionBodyCodeRefactoringProvider();

        private IDictionary<OptionKey, object> UseExpressionBody =>
            this.Option(CSharpCodeStyleOptions.PreferExpressionBodiedOperators, CSharpCodeStyleOptions.WhenPossibleWithSilentEnforcement);

        private IDictionary<OptionKey, object> UseExpressionBodyDisabledDiagnostic =>
            this.Option(CSharpCodeStyleOptions.PreferExpressionBodiedOperators, new CodeStyleOption<ExpressionBodyPreference>(ExpressionBodyPreference.WhenPossible, NotificationOption.None));

        private IDictionary<OptionKey, object> UseBlockBody =>
            this.Option(CSharpCodeStyleOptions.PreferExpressionBodiedOperators, CSharpCodeStyleOptions.NeverWithSilentEnforcement);

        private IDictionary<OptionKey, object> UseBlockBodyDisabledDiagnostic =>
            this.Option(CSharpCodeStyleOptions.PreferExpressionBodiedOperators, new CodeStyleOption<ExpressionBodyPreference>(ExpressionBodyPreference.Never, NotificationOption.None));

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOfferedIfUserPrefersExpressionBodiesAndInBlockBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    public static implicit operator bool(C c1)
    {
        [||]Bar();
    }
}", parameters: new TestParameters(options: UseExpressionBody));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOfferedIfUserPrefersExpressionBodiesWithoutDiagnosticAndInBlockBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class C
{
    public static implicit operator bool(C c1)
    {
        [||]Bar();
    }
}",
@"class C
{
    public static implicit operator bool(C c1) => Bar();
}", parameters: new TestParameters(options: UseExpressionBodyDisabledDiagnostic));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOfferedIfUserPrefersBlockBodiesAndInBlockBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class C
{
    public static implicit operator bool(C c1)
    {
        [||]Bar();
    }
}",
@"class C
{
    public static implicit operator bool(C c1) => Bar();
}", parameters: new TestParameters(options: UseBlockBody));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOfferedInLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    public static implicit operator bool(C c1)
    {
        return () => { [||] };
    }
}", parameters: new TestParameters(options: UseBlockBody));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOfferedIfUserPrefersBlockBodiesAndInExpressionBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    public static implicit operator bool(C c1) => [||]Bar();
}", parameters: new TestParameters(options: UseBlockBody));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOfferedIfUserPrefersBlockBodiesWithoutDiagnosticAndInExpressionBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class C
{
    public static implicit operator bool(C c1) => [||]Bar();
}",
@"class C
{
    public static implicit operator bool(C c1)
    {
        return Bar();
    }
}", parameters: new TestParameters(options: UseBlockBodyDisabledDiagnostic));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOfferedIfUserPrefersExpressionBodiesAndInExpressionBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class C
{
    public static implicit operator bool(C c1) => [||]Bar();
}",
@"class C
{
    public static implicit operator bool(C c1)
    {
        return Bar();
    }
}", parameters: new TestParameters(options: UseExpressionBody));
        }
    }
}
