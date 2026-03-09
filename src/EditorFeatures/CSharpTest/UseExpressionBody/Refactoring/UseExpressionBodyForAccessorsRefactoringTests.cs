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
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.UseExpressionBody
{
    public class UseExpressionBodyForAccessorsRefactoringTests : AbstractCSharpCodeActionTest
    {
        protected override CodeRefactoringProvider CreateCodeRefactoringProvider(Workspace workspace, TestParameters parameters)
            => new UseExpressionBodyCodeRefactoringProvider();

        private IDictionary<OptionKey, object> UseExpressionBodyForAccessors_BlockBodyForProperties =>
            OptionsSet(
                this.SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedAccessors, CSharpCodeStyleOptions.WhenPossibleWithSilentEnforcement),
                this.SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedProperties, CSharpCodeStyleOptions.NeverWithSilentEnforcement));

        private IDictionary<OptionKey, object> UseExpressionBodyForAccessors_BlockBodyForProperties_DisabledDiagnostic =>
            OptionsSet(
                this.SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedAccessors, new CodeStyleOption<ExpressionBodyPreference>(ExpressionBodyPreference.WhenPossible, NotificationOption.None)),
                this.SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedProperties, new CodeStyleOption<ExpressionBodyPreference>(ExpressionBodyPreference.Never, NotificationOption.None)));

        private IDictionary<OptionKey, object> UseExpressionBodyForAccessors_ExpressionBodyForProperties =>
            OptionsSet(
                this.SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedAccessors, CSharpCodeStyleOptions.WhenPossibleWithSilentEnforcement),
                this.SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedProperties, CSharpCodeStyleOptions.WhenPossibleWithSilentEnforcement));

        private IDictionary<OptionKey, object> UseExpressionBodyForAccessors_ExpressionBodyForProperties_DisabledDiagnostic =>
            OptionsSet(
                this.SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedAccessors, new CodeStyleOption<ExpressionBodyPreference>(ExpressionBodyPreference.WhenPossible, NotificationOption.None)),
                this.SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedProperties, new CodeStyleOption<ExpressionBodyPreference>(ExpressionBodyPreference.WhenPossible, NotificationOption.None)));

        private IDictionary<OptionKey, object> UseBlockBodyForAccessors_ExpressionBodyForProperties =>
            OptionsSet(
                this.SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedAccessors, CSharpCodeStyleOptions.NeverWithSilentEnforcement),
                this.SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedProperties, CSharpCodeStyleOptions.WhenPossibleWithSilentEnforcement));

        private IDictionary<OptionKey, object> UseBlockBodyForAccessors_ExpressionBodyForProperties_DisabledDiagnostic =>
            OptionsSet(
                this.SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedAccessors, new CodeStyleOption<ExpressionBodyPreference>(ExpressionBodyPreference.Never, NotificationOption.None)),
                this.SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedProperties, new CodeStyleOption<ExpressionBodyPreference>(ExpressionBodyPreference.WhenPossible, NotificationOption.None)));

        private IDictionary<OptionKey, object> UseBlockBodyForAccessors_BlockBodyForProperties =>
            OptionsSet(
                this.SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedAccessors, CSharpCodeStyleOptions.NeverWithSilentEnforcement),
                this.SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedProperties, CSharpCodeStyleOptions.NeverWithSilentEnforcement));

        private IDictionary<OptionKey, object> UseBlockBodyForAccessors_BlockBodyForProperties_DisabledDiagnostic =>
            OptionsSet(
                this.SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedAccessors, new CodeStyleOption<ExpressionBodyPreference>(ExpressionBodyPreference.Never, NotificationOption.None)),
                this.SingleOption(CSharpCodeStyleOptions.PreferExpressionBodiedProperties, new CodeStyleOption<ExpressionBodyPreference>(ExpressionBodyPreference.Never, NotificationOption.None)));

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUpdatePropertyIfPropertyWantsBlockAndAccessorWantsExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class C
{
    int Goo
    {
        get
        {
            [||]return Bar();
        }
    }
}",
@"class C
{
    int Goo => Bar();
}", parameters: new TestParameters(options: UseExpressionBodyForAccessors_BlockBodyForProperties));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOfferedIfUserPrefersExpressionBodiesAndInBlockBody2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    int Goo
    {
        get
        {
            [||]return Bar();
        }
    }
}", parameters: new TestParameters(options: UseExpressionBodyForAccessors_ExpressionBodyForProperties));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOfferedIfUserPrefersExpressionBodiesWithoutDiagnosticAndInBlockBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class C
{
    int Goo
    {
        get
        {
            return [||]Bar();
        }
    }
}",
@"class C
{
    int Goo => Bar();
}", parameters: new TestParameters(options: UseExpressionBodyForAccessors_BlockBodyForProperties_DisabledDiagnostic));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOfferedIfUserPrefersExpressionBodiesWithoutDiagnosticAndInBlockBody2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class C
{
    int Goo
    {
        get
        {
            return [||]Bar();
        }
    }
}",
@"class C
{
    int Goo => Bar();
}", parameters: new TestParameters(options: UseExpressionBodyForAccessors_ExpressionBodyForProperties_DisabledDiagnostic));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOfferedIfUserPrefersBlockBodiesAndInBlockBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class C
{
    int Goo
    {
        get
        {
            return [||]Bar();
        }
    }
}",
@"class C
{
    int Goo
    {
        get => Bar();
    }
}", parameters: new TestParameters(options: UseBlockBodyForAccessors_ExpressionBodyForProperties));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOfferExpressionBodyForPropertyIfPropertyAndAccessorBothPreferExpressions()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class C
{
    int Goo
    {
        get
        {
            return [||]Bar();
        }
    }
}",
@"class C
{
    int Goo => [||]Bar();
}", parameters: new TestParameters(options: UseBlockBodyForAccessors_BlockBodyForProperties));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOfferedIfUserPrefersBlockBodiesAndInExpressionBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    int Goo { get => [||]Bar(); }
}", parameters: new TestParameters(options: UseBlockBodyForAccessors_ExpressionBodyForProperties));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOfferedIfUserPrefersBlockBodiesWithoutDiagnosticAndInExpressionBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class C
{
    int Goo { get => [||]Bar(); }
}",

@"class C
{
    int Goo => Bar();
}", parameters: new TestParameters(options: UseBlockBodyForAccessors_BlockBodyForProperties_DisabledDiagnostic));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOfferedIfUserPrefersBlockBodiesWithoutDiagnosticAndInExpressionBody2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class C
{
    int Goo { get => [||]Bar(); }
}",

@"class C
{
    int Goo => Bar();
}", parameters: new TestParameters(options: UseBlockBodyForAccessors_ExpressionBodyForProperties_DisabledDiagnostic));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOfferedForPropertyIfUserPrefersBlockPropertiesAndHasBlockProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class C
{
    int Goo { get => [||]Bar(); }
}",

@"class C
{
    int Goo => Bar();
}", parameters: new TestParameters(options: UseBlockBodyForAccessors_BlockBodyForProperties));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOfferForPropertyIfPropertyPrefersBlockButCouldBecomeExpressionBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class C
{
    int Goo { get => [||]Bar(); }
}",
@"class C
{
    int Goo => Bar();
}", parameters: new TestParameters(options: UseExpressionBodyForAccessors_BlockBodyForProperties));
        }

        [WorkItem(20350, "https://github.com/dotnet/roslyn/issues/20350")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseExpressionBody)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAccessorListFormatting()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"class C
{
    int Goo { get => [||]Bar(); }
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
}", parameters: new TestParameters(options: UseExpressionBodyForAccessors_ExpressionBodyForProperties));
        }
    }
}
