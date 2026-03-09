// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Recommendations
{
    public class WhenKeywordRecommenderTests : KeywordRecommenderTests
    {
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForCatchClause_AfterCatch()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(AddInsideMethod(
@"try {} catch $$"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForCatchClause_AfterCatchDeclaration1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(AddInsideMethod(
@"try {} catch (Exception) $$"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForCatchClause_AfterCatchDeclaration2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(AddInsideMethod(
@"try {} catch (Exception e) $$"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForCatchClause_AfterCatchDeclarationEmpty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(AddInsideMethod(
@"try {} catch () $$"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForCatchClause_NotAfterTryBlock()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(AddInsideMethod(
@"try {} $$"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForCatchClause_NotAfterFilter1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(AddInsideMethod(
@"try {} catch (Exception e) when $$"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForCatchClause_NotAfterFilter2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(AddInsideMethod(
@"try {} catch (Exception e) when ($$"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForCatchClause_NotAfterFilter3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(AddInsideMethod(
@"try {} catch (Exception e) when (true) $$"));
        }

        [WorkItem(24113, "https://github.com/dotnet/roslyn/issues/24113")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_AfterDeclarationPattern() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(AddInsideMethod(@"switch (1) { case int i $$ }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_AfterDeclarationPattern_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(AddInsideMethod(@"switch (1) { case int i $$ break; }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_AfterDeclarationPattern_BeforeWhen() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(AddInsideMethod(@"switch (1) { case int i $$ when }"));

        [WorkItem(25084, "https://github.com/dotnet/roslyn/issues/25084")]
        [Theory, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
        [InlineData("int.MinValue")]
        [InlineData("1")]
        [InlineData("1 + 1")]
        [InlineData("true ? 1 : 1")]
        [InlineData("(1 + )")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_AfterExpression(string expression) =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(AddInsideMethod($@"switch (1) {{ case {expression} $$ }}"));

        [Theory, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
        [InlineData("int.MinValue")]
        [InlineData("1")]
        [InlineData("1 + 1")]
        [InlineData("true ? 1 : 1")]
        [InlineData("(1 + )")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_AfterExpression_BeforeBreak(string expression) =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(AddInsideMethod($@"switch (1) {{ case {expression} $$ break; }}"));

        [Theory, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
        [InlineData("int.MinValue")]
        [InlineData("1")]
        [InlineData("1 + 1")]
        [InlineData("true ? 1 : 1")]
        [InlineData("(1 + )")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_AfterExpression_BeforeWhen(string expression) =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(AddInsideMethod($@"switch (1) {{ case {expression} $$ when }}"));

        [Theory, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
        [InlineData("int.")]
        [InlineData("1 +")]
        [InlineData("true ?")]
        [InlineData("true ? 1")]
        [InlineData("true ? 1 :")]
        [InlineData("(1")]
        [InlineData("(1 + 1")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_NotAfterIncompleteExpression(string expression) =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod($@"switch (1) {{ case {expression} $$ }}"));

        [Theory, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
        [InlineData("int.")]
        [InlineData("1 +")]
        [InlineData("true ?")]
        [InlineData("true ? 1")]
        [InlineData("true ? 1 :")]
        [InlineData("(1")]
        [InlineData("(1 + 1")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_NotAfterIncompleteExpression_BeforeBreak(string expression) =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod($@"switch (1) {{ case {expression} $$ break; }}"));

        [Theory, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
        [InlineData("int.")]
        [InlineData("1 +")]
        [InlineData("true ?")]
        [InlineData("true ? 1")]
        [InlineData("true ? 1 :")]
        [InlineData("(1")]
        [InlineData("(1 + 1")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_NotAfterIncompleteExpression_BeforeWhen(string expression) =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod($@"switch (1) {{ case {expression} $$ when }}"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_NotInsideExpression() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (1) { case (1 + 1 $$) }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_NotInsideExpression_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (1) { case (1 + 1 $$) break; }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_NotInsideExpression_BeforeWhen() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (1) { case (1 + 1 $$) when }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_NotAfterCase() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (1) { case $$ }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_NotAfterCase_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (1) { case $$ break; }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_NotAfterCase_BeforeWhen() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (1) { case $$ when }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_NotAfterDefault() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (1) { default $$ }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_NotAfterDefault_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (1) { default $$ break; }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_NotAfterWhen() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (1) { case 1 when $$ }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_NotAfterWhen_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (1) { case 1 when $$ break; }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_NotAfterColon() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (1) { case 1: $$ }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_NotAfterColon_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (1) { case 1: $$ break; }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_NotInEmptySwitchStatement() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (1) { $$ }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterPredefinedType() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (new object()) { case int $$ }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterPredefinedType_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (new object()) { case int $$ break; }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterPredefinedType_BeforeWhen() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (new object()) { case int $$ when }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterGenericType() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (new object()) { case Dictionary<string, int> $$ }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterGenericType_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (new object()) { case Dictionary<string, int> $$ break; }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterGenericType_BeforeWhen() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (new object()) { case Dictionary<string, int> $$ when }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterCustomType() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(@"
class SyntaxNode { }
class C
{
    void M() { switch (new object()) { case SyntaxNode $$ } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterCustomType_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(@"
class SyntaxNode { }
class C
{
    void M() { switch (new object()) { case SyntaxNode $$ break; } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterCustomType_BeforeWhen() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(@"
class SyntaxNode { }
class C
{
    void M() { switch (new object()) { case SyntaxNode $$ when } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterTypeAlias() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(@"
using Type = System.String;
class C
{
    void M() { switch (new object()) { case Type $$ } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterTypeAlias_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(@"
using Type = System.String;
class C
{
    void M() { switch (new object()) { case Type $$ break; } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterTypeAlias_BeforeWhen() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(@"
using Type = System.String;
class C
{
    void M() { switch (new object()) { case Type $$ when } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterOverloadedTypeName() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
    await VerifyAbsenceAsync(@"
class ValueTuple { }
class ValueTuple<T> { }
class C
{
    void M() { switch (new object()) { case ValueTuple $$ } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterOverloadedTypeName_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(@"
class ValueTuple { }
class ValueTuple<T> { }
class C
{
    void M() { switch (new object()) { case ValueTuple $$ break; } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterOverloadedTypeName_BeforeWhen() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(@"
class ValueTuple { }
class ValueTuple<T> { }
class C
{
    void M() { switch (new object()) { case ValueTuple $$ when } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterColorColor() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(@"
class Color { }
class C
{
    const Color Color = null;
    void M() { switch (new object()) { case Color $$ } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterColorColor_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(@"
class Color { }
class C
{
    const Color Color = null;
    void M() { switch (new object()) { case Color $$ break; } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterColorColor_BeforeWhen() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(@"
class Color { }
class C
{
    const Color Color = null;
    void M() { switch (new object()) { case Color $$ when } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterOverloadedTypeNameColorColor() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
    await VerifyKeywordAsync(@"
class Color<T> { }
class Color { }
class C
{
    const Color Color = null;
    void M() { switch (new object()) { case Color $$ } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterOverloadedTypeNameColorColor_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(@"
class Color<T> { }
class Color { }
class C
{
    const Color Color = null;
    void M() { switch (new object()) { case Color $$ break; } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterOverloadedTypeNameColorColor_BeforeWhen() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(@"
class Color<T> { }
class Color { }
class C
{
    const Color Color = null;
    void M() { switch (new object()) { case Color $$ when } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterLocalConstant() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(AddInsideMethod(@"const object c = null; switch (new object()) { case c $$ }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterLocalConstant_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(AddInsideMethod(@"const object c = null; switch (new object()) { case c $$ break; }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterLocalConstant_BeforeWhen() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(AddInsideMethod(@"const object c = null; switch (new object()) { case c $$ when }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterUnknownName() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(AddInsideMethod(@"switch (new object()) { case unknown $$ }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterUnknownName_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(AddInsideMethod(@"switch (new object()) { case unknown $$ break; }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterUnknownName_BeforeWhen() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(AddInsideMethod(@"switch (new object()) { case unknown $$ when }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterVar() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (new object()) { case var $$ }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterVar_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (new object()) { case var $$ break; }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterVar_BeforeWhen() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(AddInsideMethod(@"switch (new object()) { case var $$ when }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterClassVar() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(@"
class var { }
class C
{
    void M() { switch (new object()) { case var $$ } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterClassVar_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(@"
class var { }
class C
{
    void M() { switch (new object()) { case var $$ break; } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_NotAfterClassVar_BeforeWhen() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyAbsenceAsync(@"
class var { }
class C
{
    void M() { switch (new object()) { case var $$ when } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterLocalConstantVar() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(AddInsideMethod(@"const object var = null; switch (new object()) { case var $$ }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterLocalConstantVar_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(AddInsideMethod(@"const object var = null; switch (new object()) { case var $$ break; }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterLocalConstantVar_BeforeWhen() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(AddInsideMethod(@"const object var = null; switch (new object()) { case var $$ when }"));

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterClassAndLocalConstantVar() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            await VerifyKeywordAsync(@"
class var { }
class C
{
    void M() { const object var = null; switch (new object()) { case var $$ } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterClassAndLocalConstantVar_BeforeBreak() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
    await VerifyKeywordAsync(@"
class var { }
class C
{
    void M() { const object var = null; switch (new object()) { case var $$ break; } }
}");

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForSwitchCase_SemanticCheck_AfterClassAndLocalConstantVar_BeforeWhen() =>
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
    await VerifyKeywordAsync(@"
class var { }
class C
{
    void M() { const object var = null; switch (new object()) { case var $$ when } }
}");
    }
}
