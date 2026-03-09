// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;
using static Microsoft.CodeAnalysis.Editor.UnitTests.Classification.FormattedClassifications;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Classification
{
    [Trait(Traits.Feature, Traits.Features.Classification)]
    public partial class SyntacticClassifierTests
    {
        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_IfTrue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if true
#endif";
            await TestInMethodAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Keyword("true"),
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_IfTrueWithComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if true //Goo
#endif";
            await TestInMethodAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Keyword("true"),
                Comment("//Goo"),
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_IfFalse()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if false
#endif";
            await TestInMethodAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Keyword("false"),
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_IfGOO()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if GOO
#endif";
            await TestInMethodAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Identifier("GOO"),
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_IfNotTrue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if !true
#endif";
            await TestInMethodAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Operators.Exclamation,
                Keyword("true"),
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_IfNotFalse()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if !false
#endif";
            await TestInMethodAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Operators.Exclamation,
                Keyword("false"),
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_IfNotGOO()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if !GOO
#endif";
            await TestInMethodAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Operators.Exclamation,
                Identifier("GOO"),
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_IfTrueWithParens()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if (true)
#endif";
            await TestInMethodAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Punctuation.OpenParen,
                Keyword("true"),
                Punctuation.CloseParen,
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_IfFalseWithParens()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if (false)
#endif";
            await TestInMethodAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Punctuation.OpenParen,
                Keyword("false"),
                Punctuation.CloseParen,
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_IfGOOWithParens()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if (GOO)
#endif";
            await TestInMethodAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Punctuation.OpenParen,
                Identifier("GOO"),
                Punctuation.CloseParen,
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_IfOrExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if GOO || BAR
#endif";

            await TestInMethodAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Identifier("GOO"),
                Operators.BarBar,
                Identifier("BAR"),
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_IfAndExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if GOO && BAR
#endif";

            await TestInMethodAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Identifier("GOO"),
                Operators.AmpersandAmpersand,
                Identifier("BAR"),
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_IfOrAndExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if GOO || BAR && BAZ
#endif";

            await TestInMethodAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Identifier("GOO"),
                Operators.BarBar,
                Identifier("BAR"),
                Operators.AmpersandAmpersand,
                Identifier("BAZ"),
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_IfOrExpressionWithParens()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if (GOO || BAR)
#endif";

            await TestInMethodAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Punctuation.OpenParen,
                Identifier("GOO"),
                Operators.BarBar,
                Identifier("BAR"),
                Punctuation.CloseParen,
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_IfAndExpressionWithParens()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if (GOO && BAR)
#endif";

            await TestInMethodAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Punctuation.OpenParen,
                Identifier("GOO"),
                Operators.AmpersandAmpersand,
                Identifier("BAR"),
                Punctuation.CloseParen,
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_IfOrAndExpressionWithParens()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if GOO || (BAR && BAZ)
#endif";

            await TestInMethodAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Identifier("GOO"),
                Operators.BarBar,
                Punctuation.OpenParen,
                Identifier("BAR"),
                Operators.AmpersandAmpersand,
                Identifier("BAZ"),
                Punctuation.CloseParen,
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_If1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync("#if goo",
                PPKeyword("#"),
                PPKeyword("if"),
                Identifier("goo"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_If2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(" #if goo",
                PPKeyword("#"),
                PPKeyword("if"),
                Identifier("goo"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_If3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if goo
#endif";
            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Identifier("goo"),
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_If4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if
#endif";
            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_If5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if
aoeu
aoeu
#endif";
            var start = code.IndexOf("#endif", StringComparison.Ordinal);
            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Inactive(@"aoeu
aoeu
"), PPKeyword("#"),
     PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_If6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if
#else
aeu";
            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                PPKeyword("#"),
                PPKeyword("else"),
                Identifier("aeu"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_If7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if
#else
#endif
aeu";
            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                PPKeyword("#"),
                PPKeyword("else"),
                PPKeyword("#"),
                PPKeyword("endif"),
                Identifier("aeu"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_If8()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if
#else
aoeu
aoeu
aou
#endif
aeu";
            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                PPKeyword("#"),
                PPKeyword("else"),
                Identifier("aoeu"),
                Field("aoeu"),
                Identifier("aou"),
                PPKeyword("#"),
                PPKeyword("endif"),
                Field("aeu"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_If9()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if //Goo1
#else //Goo2
aoeu
aoeu
aou
#endif //Goo3
aeu";
            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Comment("//Goo1"),
                PPKeyword("#"),
                PPKeyword("else"),
                Comment("//Goo2"),
                Identifier("aoeu"),
                Field("aoeu"),
                Identifier("aou"),
                PPKeyword("#"),
                PPKeyword("endif"),
                Comment("//Goo3"),
                Field("aeu"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_Region1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync("#region Goo",
                PPKeyword("#"),
                PPKeyword("region"),
                PPText("Goo"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_Region2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync("   #region goo",
                PPKeyword("#"),
                PPKeyword("region"),
                PPText("goo"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_EndRegion1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync("#endregion",
                PPKeyword("#"),
                PPKeyword("endregion"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_EndRegion2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync("   #endregion",
                PPKeyword("#"),
                PPKeyword("endregion"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_EndRegion3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync("#endregion adsf",
                PPKeyword("#"),
                PPKeyword("endregion"),
                PPText("adsf"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_EndRegion4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync("   #endregion adsf",
                PPKeyword("#"),
                PPKeyword("endregion"),
                PPText("adsf"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_RegionEndRegion1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"#region
#endregion",
                PPKeyword("#"),
                PPKeyword("region"),
                PPKeyword("#"),
                PPKeyword("endregion"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_CommentAfterRegion1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"#region adsf //comment
#endregion",
                PPKeyword("#"),
                PPKeyword("region"),
                PPText("adsf //comment"),
                PPKeyword("#"),
                PPKeyword("endregion"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_CommentAfterRegion2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"#region //comment
#endregion",
                PPKeyword("#"),
                PPKeyword("region"),
                PPText("//comment"),
                PPKeyword("#"),
                PPKeyword("endregion"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_CommentAfterEndRegion1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"#region
#endregion adsf //comment",
                PPKeyword("#"),
                PPKeyword("region"),
                PPKeyword("#"),
                PPKeyword("endregion"),
                PPText("adsf //comment"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_CommentAfterEndRegion2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"#region
#endregion //comment",
                PPKeyword("#"),
                PPKeyword("region"),
                PPKeyword("#"),
                PPKeyword("endregion"),
                Comment("//comment"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_DeclarationDirectives()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"#define A
#undef B",
                PPKeyword("#"),
                PPKeyword("define"),
                Identifier("A"),
                PPKeyword("#"),
                PPKeyword("undef"),
                Identifier("B"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_IfElseEndIfDirectives()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code =
@"#if true
#elif DEBUG
#else
#endif";
            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("if"),
                Keyword("true"),
                PPKeyword("#"),
                PPKeyword("elif"),
                Identifier("DEBUG"),
                PPKeyword("#"),
                PPKeyword("else"),
                PPKeyword("#"),
                PPKeyword("endif"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_DefineDirective()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#define GOO";
            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("define"),
                Identifier("GOO"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_DefineDirectiveWithCommentAndNoName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#define //Goo";
            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("define"),
                Comment("//Goo"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_DefineDirectiveWithComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#define GOO //Goo";
            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("define"),
                Identifier("GOO"),
                Comment("//Goo"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_UndefDirectives()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#undef GOO";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("undef"),
                Identifier("GOO"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_UndefDirectiveWithCommentAndNoName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#undef //Goo";
            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("undef"),
                Comment("//Goo"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_UndefDirectiveWithComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#undef GOO //Goo";
            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("undef"),
                Identifier("GOO"),
                Comment("//Goo"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_ErrorDirective()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#error GOO";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("error"),
                PPText("GOO"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_ErrorDirectiveWithComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#error GOO //Goo";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("error"),
                PPText("GOO //Goo"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_WarningDirective()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#warning GOO";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("warning"),
                PPText("GOO"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_WarningDirectiveWithComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#warning GOO //Goo";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("warning"),
                PPText("GOO //Goo"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_LineHidden()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#line hidden";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("line"),
                PPKeyword("hidden"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_LineHiddenWithComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#line hidden //Goo";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("line"),
                PPKeyword("hidden"),
                Comment("//Goo"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_LineDefault()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#line default";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("line"),
                PPKeyword("default"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_LineDefaultWithComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#line default //Goo";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("line"),
                PPKeyword("default"),
                Comment("//Goo"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_LineNumber()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#line 100";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("line"),
                Number("100"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_LineNumberWithComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#line 100 //Goo";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("line"),
                Number("100"),
                Comment("//Goo"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_LineNumberWithFilename()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#line 100 ""C:\Goo""";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("line"),
                Number("100"),
                String("\"C:\\Goo\""));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_LineNumberWithFilenameAndComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#line 100 ""C:\Goo"" //Goo";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("line"),
                Number("100"),
                String("\"C:\\Goo\""),
                Comment("//Goo"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_PragmaChecksum1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"#pragma checksum stuff",
                PPKeyword("#"),
                PPKeyword("pragma"),
                PPKeyword("checksum"),
                PPText("stuff"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_PragmaChecksum2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"#pragma checksum ""file.txt"" ""{00000000-0000-0000-0000-000000000000}"" ""2453""",
                PPKeyword("#"),
                PPKeyword("pragma"),
                PPKeyword("checksum"),
                String("\"file.txt\""),
                String("\"{00000000-0000-0000-0000-000000000000}\""),
                String("\"2453\""));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_PragmaChecksum3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"#pragma checksum ""file.txt"" ""{00000000-0000-0000-0000-000000000000}"" ""2453"" // Goo",
                PPKeyword("#"),
                PPKeyword("pragma"),
                PPKeyword("checksum"),
                String("\"file.txt\""),
                String("\"{00000000-0000-0000-0000-000000000000}\""),
                String("\"2453\""),
                Comment("// Goo"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_PragmaWarningDisableOne()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#pragma warning disable 100";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("pragma"),
                PPKeyword("warning"),
                PPKeyword("disable"),
                Number("100"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_PragmaWarningDisableOneWithComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#pragma warning disable 100 //Goo";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("pragma"),
                PPKeyword("warning"),
                PPKeyword("disable"),
                Number("100"),
                Comment("//Goo"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_PragmaWarningRestoreOne()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#pragma warning restore 100";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("pragma"),
                PPKeyword("warning"),
                PPKeyword("restore"),
                Number("100"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_PragmaWarningRestoreOneWithComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#pragma warning restore 100 //Goo";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("pragma"),
                PPKeyword("warning"),
                PPKeyword("restore"),
                Number("100"),
                Comment("//Goo"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_PragmaWarningDisableTwo()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#pragma warning disable 100, 101";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("pragma"),
                PPKeyword("warning"),
                PPKeyword("disable"),
                Number("100"),
                Punctuation.Comma,
                Number("101"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_PragmaWarningRestoreTwo()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#pragma warning restore 100, 101";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("pragma"),
                PPKeyword("warning"),
                PPKeyword("restore"),
                Number("100"),
                Punctuation.Comma,
                Number("101"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_PragmaWarningDisableThree()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#pragma warning disable 100, 101, 102";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("pragma"),
                PPKeyword("warning"),
                PPKeyword("disable"),
                Number("100"),
                Punctuation.Comma,
                Number("101"),
                Punctuation.Comma,
                Number("102"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Classification)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PP_PragmaWarningRestoreThree()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"#pragma warning restore 100, 101, 102";

            await TestAsync(code,
                PPKeyword("#"),
                PPKeyword("pragma"),
                PPKeyword("warning"),
                PPKeyword("restore"),
                Number("100"),
                Punctuation.Comma,
                Number("101"),
                Punctuation.Comma,
                Number("102"));
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DiscardInOutDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                code: @"M2(out var _);",
                expected: Classifications(Identifier("M2"), Punctuation.OpenParen, Keyword("out"), Identifier("var"),
                    Identifier("_"), Punctuation.CloseParen, Punctuation.Semicolon));
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DiscardInCasePattern()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                code: @"switch (1) { case int _: }",
                expected: Classifications(Keyword("switch"), Punctuation.OpenParen, Number("1"), Punctuation.CloseParen,
                    Punctuation.OpenCurly, Keyword("case"), Keyword("int"), Identifier("_"), Punctuation.Colon, Punctuation.CloseCurly));
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DiscardInDeconstruction()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                code: @"var (x, _) = (1, 2);",
                expected: Classifications(Identifier("var"), Punctuation.OpenParen, Identifier("x"), Punctuation.Comma,
                    Identifier("_"), Punctuation.CloseParen, Operators.Equals, Punctuation.OpenParen, Number("1"),
                    Punctuation.Comma, Number("2"), Punctuation.CloseParen, Punctuation.Semicolon));
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DiscardInDeconstruction2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                code: @"(var _, var _) = (1, 2);",
                expected: Classifications(Punctuation.OpenParen, Identifier("var"), Identifier("_"), Punctuation.Comma,
                    Identifier("var"), Identifier("_"), Punctuation.CloseParen, Operators.Equals, Punctuation.OpenParen,
                    Number("1"), Punctuation.Comma, Number("2"), Punctuation.CloseParen, Punctuation.Semicolon));
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ShortDiscardInDeconstruction()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                code: @"int x; (_, x) = (1, 2);",
                expected: Classifications(Keyword("int"), Local("x"), Punctuation.Semicolon, Punctuation.OpenParen,
                    Identifier("_"), Punctuation.Comma, Identifier("x"), Punctuation.CloseParen, Operators.Equals,
                    Punctuation.OpenParen, Number("1"), Punctuation.Comma, Number("2"), Punctuation.CloseParen,
                    Punctuation.Semicolon));
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ShortDiscardInOutDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                code: @"M2(out _);",
                expected: Classifications(Identifier("M2"), Punctuation.OpenParen, Keyword("out"), Identifier("_"), Punctuation.CloseParen,
                    Punctuation.Semicolon));
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ShortDiscardInAssignment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
                code: @"_ = 1;",
                expected: Classifications(Identifier("_"), Operators.Equals, Number("1"), Punctuation.Semicolon));
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task UnderscoreInAssignment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(code: @"int _; _ = 1;" ,
                expected: Classifications(Keyword("int"), Local("_"), Punctuation.Semicolon, Identifier("_"), Operators.Equals,
                    Number("1"), Punctuation.Semicolon));
        }
    }
}
