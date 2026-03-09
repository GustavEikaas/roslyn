// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.CodeFixes.ConditionalExpressionInStringInterpolation;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics.ConditionalExpressionInStringInterpolation
{
    public class CSharpAddParenthesesAroundConditionalExpressionInInterpolatedStringCodeFixProviderTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (null, new CSharpAddParenthesesAroundConditionalExpressionInInterpolatedStringCodeFixProvider());

        private async Task TestInMethodAsync(string initialMethodBody, string expectedMethodBody)
        {
            var template = @"
class Application
{{
    public void M()
    {{
        {0}
    }}
}}";
            await TestInRegularAndScriptAsync(
                string.Format(template, initialMethodBody), 
                string.Format(template, expectedMethodBody)).ConfigureAwait(false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesSimpleConditionalExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"var s = $""{ true ? 1 [|:|] 2}"";",
                @"var s = $""{ (true ? 1 : 2)}"";");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesMultiLineConditionalExpression1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(@"
var s = $@""{ true
            [|? 1|]
            : 2}"";
", @"
var s = $@""{ (true
            ? 1
            : 2)}"";
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesMultiLineConditionalExpression2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(@"
var s = $@""{
            true
            ?
            [|1|]
            : 
            2
            }"";
", @"
var s = $@""{
            (true
            ?
            1
            : 
            2
)            }"";
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesWithTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"var s = $""{ /* Leading1 */ true /* Leading2 */ ? /* TruePart1 */ 1 /* TruePart2 */[|:|] /* FalsePart1 */ 2 /* FalsePart2 */ }"";",
                @"var s = $""{ /* Leading1 */ (true /* Leading2 */ ? /* TruePart1 */ 1 /* TruePart2 */: /* FalsePart1 */ 2 /* FalsePart2 */ )}"";");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesClosingBracketInFalseCondition()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"var s = $""{ true ? new int[0] [|:|] new int[] {} }"";",
                @"var s = $""{ (true ? new int[0] : new int[] {} )}"";");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesStringLiteralInFalseCondition()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"var s = $""{ true ? ""1"" [|:|] ""2"" }"";",
                @"var s = $""{ (true ? ""1"" : ""2"" )}"";");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesVerbatimStringLiteralInFalseCondition()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"var s = $""{ true ? ""1"" [|:|] @""""""2"""""" }"";",
                @"var s = $""{ (true ? ""1"" : @""""""2"""""" )}"";");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesStringLiteralInFalseConditionWithClosingParenthesisInLiteral()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"var s = $""{ true ? ""1"" [|:|] ""2)"" }"";",
                @"var s = $""{ (true ? ""1"" : ""2)"" )}"";");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesStringLiteralInFalseConditionWithEscapedDoubleQuotes()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"var s = $""{ true ? ""1"" [|:|] ""2\"""" }"";",
                @"var s = $""{ (true ? ""1"" : ""2\"""" )}"";");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesStringLiteralInFalseConditionWithCodeLikeContent()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"var s = $""{ true ? ""1"" [|:|] ""M(new int[] {}, \""Parameter\"");"" }"";",
                @"var s = $""{ (true ? ""1"" : ""M(new int[] {}, \""Parameter\"");"" )}"";");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesNestedConditionalExpression1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"var s2 = $""{ true ? ""1"" [|:|] (false ? ""2"" : ""3"") };",
                @"var s2 = $""{ (true ? ""1"" : (false ? ""2"" : ""3"") )};");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesNestedConditionalExpression2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"var s2 = $""{ true ? ""1"" [|:|] false ? ""2"" : ""3"" };",
                @"var s2 = $""{ (true ? ""1"" : false ? ""2"" : ""3"" )};");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesNestedConditionalWithNestedInterpolatedString()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"var s2 = $""{ (true ? ""1"" : false ? $""{ true ? ""2"" [|:|] ""3""}"" : ""4"") }""",
                @"var s2 = $""{ (true ? ""1"" : false ? $""{ (true ? ""2"" : ""3"")}"" : ""4"") }""");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesMultipleInterpolatedSections1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"var s3 = $""Text1 { true ? ""Text2"" [|:|] ""Text3""} Text4 { (true ? ""Text5"" : ""Text6"")} Text7"";",
                @"var s3 = $""Text1 { (true ? ""Text2"" : ""Text3"")} Text4 { (true ? ""Text5"" : ""Text6"")} Text7"";");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesMultipleInterpolatedSections2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"var s3 = $""Text1 { (true ? ""Text2"" : ""Text3"")} Text4 { true ? ""Text5"" [|:|] ""Text6""} Text7"";",
                @"var s3 = $""Text1 { (true ? ""Text2"" : ""Text3"")} Text4 { (true ? ""Text5"" : ""Text6"")} Text7"";");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesMultipleInterpolatedSections3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"var s3 = $""Text1 { true ? ""Text2"" [|:|] ""Text3""} Text4 { true ? ""Text5"" : ""Text6""} Text7"";",
                @"var s3 = $""Text1 { (true ? ""Text2"" : ""Text3"")} Text4 { true ? ""Text5"" : ""Text6""} Text7"";");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesWhileTyping1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"
                PreviousLineOfCode();
                var s3 = $""Text1 { true ? ""Text2"" [|:|]
                NextLineOfCode();",
                @"
                PreviousLineOfCode();
                var s3 = $""Text1 { (true ? ""Text2"" :)
                NextLineOfCode();");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesWhileTyping2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"
                PreviousLineOfCode();
                var s3 = $""Text1 { true ? ""Text2"" [|:|] ""
                NextLineOfCode();",
                @"
                PreviousLineOfCode();
                var s3 = $""Text1 { (true ? ""Text2"" : "")
                NextLineOfCode();");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesWhileTyping3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"
                PreviousLineOfCode();
                var s3 = $""Text1 { true ? ""Text2"" [|:|] ""Text3
                NextLineOfCode();",
                @"
                PreviousLineOfCode();
                var s3 = $""Text1 { (true ? ""Text2"" : ""Text3)
                NextLineOfCode();");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesWhileTyping4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"
                PreviousLineOfCode();
                var s3 = $""Text1 { true ? ""Text2"" [|:|] ""Text3""
                NextLineOfCode();",
                @"
                PreviousLineOfCode();
                var s3 = $""Text1 { (true ? ""Text2"" : ""Text3"")
                NextLineOfCode();");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesWhileTyping5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"
                PreviousLineOfCode();
                var s3 = $""Text1 { true ? ""Text2"" [|:|] ""Text3"" }
                NextLineOfCode();",
                @"
                PreviousLineOfCode();
                var s3 = $""Text1 { (true ? ""Text2"" : ""Text3"" )}
                NextLineOfCode();");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesWithCS1026PresentBeforeFixIsApplied1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"
                (
                var s3 = $""Text1 { true ? ""Text2"" [|:|] ""Text3"" }
                NextLineOfCode();",
                @"
                (
                var s3 = $""Text1 { (true ? ""Text2"" : ""Text3"" )}
                NextLineOfCode();");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesWithCS1026PresentBeforeFixIsApplied2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"
                PreviousLineOfCode();
                var s3 = $""Text1 { true ? ""Text2"" [|:|] ""Text3"" }
                NextLineOfCode(",
                @"
                PreviousLineOfCode();
                var s3 = $""Text1 { (true ? ""Text2"" : ""Text3"" )}
                NextLineOfCode(");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesWithCS1026PresentBeforeFixIsApplied3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"
                PreviousLineOfCode();
                var s3 = ($""Text1 { true ? ""Text2"" [|:|] ""Text3"" }
                NextLineOfCode();",
                @"
                PreviousLineOfCode();
                var s3 = ($""Text1 { (true ? ""Text2"" : ""Text3"" )}
                NextLineOfCode();");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParenthesesAroundConditionalExpressionInInterpolatedString)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAddParenthesesAddOpeningParenthesisOnly()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                @"var s3 = $""{ true ? 1 [|:|] 2 )}""",
                @"var s3 = $""{ (true ? 1 : 2 )}""");
        }
    }
}
