// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.CodeRefactorings.InvertIf;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.CodeRefactorings.InvertIf
{
    public class InvertIfTests : AbstractCSharpCodeActionTest
    {
        private async Task TestFixOneAsync(
            string initial,
            string expected)
        {
            await TestInRegularAndScriptAsync(CreateTreeText(initial), CreateTreeText(expected));
        }

        protected override CodeRefactoringProvider CreateCodeRefactoringProvider(Workspace workspace, TestParameters parameters)
            => new CSharpInvertIfCodeRefactoringProvider();

        private string CreateTreeText(string initial)
        {
            return
@"class A
{
    bool a = true;
    bool b = true;
    bool c = true;
    bool d = true;

    void Goo()
    {
" + initial + @"
    }
}";
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_Identifier()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a) { a(); } else { b(); }",
@"if (!a) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_IdentifierWithTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if /*0*/(/*1*/a/*2*/)/*3*/ { a(); } else { b(); }",
@"if /*0*/(/*1*/!a/*2*/)/*3*/ { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_NotIdentifier()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (!a) { a(); } else { b(); }",
@"if (a) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_NotIdentifierWithTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if /*0*/(/*1*/!/*1b*/a/*2*/)/*3*/ { a(); } else { b(); }",
@"if /*0*/(/*1*/a/*2*/)/*3*/ { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_EqualsEquals()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a == b) { a(); } else { b(); }",
@"if (a != b) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_NotEquals()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a != b) { a(); } else { b(); }",
@"if (a == b) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_GreaterThan()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a > b) { a(); } else { b(); }",
@"if (a <= b) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_GreaterThanEquals()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a >= b) { a(); } else { b(); }",
@"if (a < b) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_LessThan()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a < b) { a(); } else { b(); }",
@"if (a >= b) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_LessThanEquals()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a <= b) { a(); } else { b(); }",
@"if (a > b) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_DoubleParentheses()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if ((a)) { a(); } else { b(); }",
@"if (!a) { b(); } else { a(); }");
        }

        [WpfFact(Skip = "https://github.com/dotnet/roslyn/issues/26427"), Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_DoubleParenthesesWithInnerTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if ((/*1*/a/*2*/)) { a(); } else { b(); }",
@"if (/*1*/!a/*2*/) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_DoubleParenthesesWithMiddleTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (/*1*/(a)/*2*/) { a(); } else { b(); }",
@"if (/*1*/!a/*2*/) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_DoubleParenthesesWithOutsideTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if /*before*/((a))/*after*/ { a(); } else { b(); }",
@"if /*before*/(!a)/*after*/ { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_Is()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a is Goo) { a(); } else { b(); }",
@"if (!(a is Goo)) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_MethodCall()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a.Goo()) { a(); } else { b(); }",
@"if (!a.Goo()) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_Or()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a || b) { a(); } else { b(); }",
@"if (!a && !b) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_Or2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (!a || !b) { a(); } else { b(); }",
@"if (a && b) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_Or3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (!a || b) { a(); } else { b(); }",
@"if (a && !b) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_Or4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a | b) { a(); } else { b(); }",
@"if (!a & !b) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_And()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a && b) { a(); } else { b(); }",
@"if (!a || !b) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_And2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (!a && !b) { a(); } else { b(); }",
@"if (a || b) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_And3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (!a && b) { a(); } else { b(); }",
@"if (a || !b) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_And4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a & b) { a(); } else { b(); }",
@"if (!a | !b) { b(); } else { a(); }");
        }


        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_ParenthesizeAndForPrecedence()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a && b || c) { a(); } else { b(); }",
@"if ((!a || !b) && !c) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_Plus()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a + b) { a(); } else { b(); }",
@"if (!(a + b)) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_True()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (true) { a(); } else { b(); }",
@"if (false) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_TrueWithTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (/*1*/true/*2*/) { a(); } else { b(); }",
@"if (/*1*/false/*2*/) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_False()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (false) { a(); } else { b(); }",
@"if (true) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_OtherLiteralExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (literalexpression) { a(); } else { b(); }",
@"if (!literalexpression) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_TrueAndFalse()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (true && false) { a(); } else { b(); }",
@"if (false || true) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_NoCurlyBraces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a) a(); else b();",
@"if (!a) b(); else a();");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_CurlyBracesOnIf()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a) { a(); } else b();",
@"if (!a) b(); else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_CurlyBracesOnElse()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a) a(); else { b(); }",
@"if (!a) { b(); } else a();");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_IfElseIf()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a) { a(); } else if (b) { b(); }",
@"if (!a) { if (b) { b(); } } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_IfElseIfElse()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (a) { a(); } else if (b) { b(); } else { c(); }",
@"if (!a) { if (b) { b(); } else { c(); } } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_CompoundConditional()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if (((a == b) && (c != d)) || ((e < f) && (!g))) { a(); } else { b(); }",
@"if ((a != b || c == d) && (e >= f || g)) { b(); } else { a(); }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_Trivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"[||]if /*1*/ (a) /*2*/ { /*3*/ a() /*4*/; /*5*/ } /*6*/ else if /*7*/ (b) /*8*/ { /*9*/ b(); /*10*/ } /*11*/ else /*12*/ { /*13*/ c(); /*14*/} /*15*/",
@"if /*1*/ (!a) /*2*/ { if /*7*/ (b) /*8*/ { /*9*/ b(); /*10*/ } /*11*/ else /*12*/ { /*13*/ c(); /*14*/} /*6*/ } else { /*3*/ a() /*4*/; /*5*/ } /*15*/");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestKeepTriviaWithinExpression_BrokenCode()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class A
{
    void Goo()
    {
        [||]if (a ||
        b &&
        c < // comment
        d)
        {
            a();
        }
        else
        {
            b();
        }
    }
}",
@"class A
{
    void Goo()
    {
        if (!a &&
        (!b ||
        c >= // comment
        d))
        {
            b();
        }
        else
        {
            a();
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestKeepTriviaWithinExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class A
{
    void Goo()
    {
        bool a = true;
        bool b = true;
        bool c = true;
        bool d = true;

        [||]if (a ||
        b &&
        c < // comment
        d)
        {
            a();
        }
        else
        {
            b();
        }
    }
}",
@"class A
{
    void Goo()
    {
        bool a = true;
        bool b = true;
        bool c = true;
        bool d = true;

        if (!a &&
        (!b ||
        c >= // comment
        d))
        {
            b();
        }
        else
        {
            a();
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMultiline_IfElseIfElse()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class A
{
    void Goo()
    {
        [||]if (a)
        {
            a();
        }
        else if (b)
        {
            b();
        }
        else
        {
            c();
        }
    }
}",
@"class A
{
    void Goo()
    {
        if (!a)
        {
            if (b)
            {
                b();
            }
            else
            {
                c();
            }
        }
        else
        {
            a();
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMultiline_IfElse()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class A
{
    void Goo()
    {
        [||]if (foo) 
            bar();
        else
            if (baz)
                Quux();
    }
}",
@"class A
{
    void Goo()
    {
        if (!foo)
        {
            if (baz)
                Quux();
        }
        else
            bar();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMultiline_OpenCloseBracesSameLine()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class A
{
    void Goo()
    {
        [||]if (foo) {
           x();
           x();
        } else {
           y();
           y();
        }
    }
}",
@"class A
{
    void Goo()
    {
        if (!foo) {
           y();
           y();
        } else {
           x();
           x();
        }
    }
}");
        }
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMultiline_Trivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class A
{
    void Goo()
    { /*1*/
        [||]if (a) /*2*/
        { /*3*/
            /*4*/
            goo(); /*5*/
            /*6*/
        } /*7*/
        else if (b) /*8*/
        { /*9*/
            /*10*/
            goo(); /*11*/
            /*12*/
        } /*13*/
        else /*14*/
        { /*15*/
            /*16*/
            goo(); /*17*/
            /*18*/
        } /*19*/
        /*20*/
    }
}",
@"class A
{
    void Goo()
    { /*1*/
        if (!a) /*2*/
        {
            if (b) /*8*/
            { /*9*/
              /*10*/
                goo(); /*11*/
                       /*12*/
            } /*13*/
            else /*14*/
            { /*15*/
              /*16*/
                goo(); /*17*/
                       /*18*/
            } /*19*/
        }
        else
        { /*3*/
            /*4*/
            goo(); /*5*/
            /*6*/
        } /*7*/
        /*20*/
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnNonEmptySpan()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    void F()
    {
        [|if (a)
        {
            a();
        }
        else
        {
            b();
        }|]
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOverlapsHiddenPosition1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    void F()
    {
#line hidden
        [||]if (a)
        {
            a();
        }
        else
        {
            b();
        }
#line default
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOverlapsHiddenPosition2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    void F()
    {
        [||]if (a)
        {
#line hidden
            a();
#line default
        }
        else
        {
            b();
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOverlapsHiddenPosition3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    void F()
    {
        [||]if (a)
        {
            a();
        }
        else
        {
#line hidden
            b();
#line default
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOverlapsHiddenPosition4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    void F()
    {
        [||]if (a)
        {
#line hidden
            a();
        }
        else
        {
            b();
#line default
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOverlapsHiddenPosition5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    void F()
    {
        [||]if (a)
        {
            a();
#line hidden
        }
        else
        {
#line default
            b();
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOverlapsHiddenPosition6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
#line hidden
class C 
{
    void F()
    {
#line default
        [||]if (a)
        {
            a();
        }
        else
        {
            b();
        }
    }
}",

@"
#line hidden
class C 
{
    void F()
    {
#line default
        if (!a)
        {
            b();
        }
        else
        {
            a();
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOverlapsHiddenPosition7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
#line hidden
class C 
{
    void F()
    {
#line default
        [||]if (a)
        {
            a();
        }
        else
        {
            b();
        }
#line hidden
    }
}
#line default",

@"
#line hidden
class C 
{
    void F()
    {
#line default
        if (!a)
        {
            b();
        }
        else
        {
            a();
        }
#line hidden
    }
}
#line default");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_SimplifyToLengthEqualsZero()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"string x; [||]if (x.Length > 0) { GreaterThanZero(); } else { EqualsZero(); } } } ",
@"string x; if (x.Length == 0) { EqualsZero(); } else { GreaterThanZero(); } } } ");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_SimplifyToLengthEqualsZero2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"string[] x; [||]if (x.Length > 0) { GreaterThanZero(); } else { EqualsZero(); } } } ",
@"string[] x; if (x.Length == 0) { EqualsZero(); } else { GreaterThanZero(); } } } ");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_SimplifyToLengthEqualsZero3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"string x; [||]if (x.Length > 0x0) { a(); } else { b(); } } } ",
@"string x; if (x.Length == 0x0) { b(); } else { a(); } } } ");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_SimplifyToLengthEqualsZero4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"string x; [||]if (0 < x.Length) { a(); } else { b(); } } } ",
@"string x; if (0 == x.Length) { b(); } else { a(); } } } ");
        }

        [WorkItem(545986, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/545986")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_SimplifyToEqualsZero1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"byte x = 1; [||]if (0 < x) { a(); } else { b(); } } } ",
@"byte x = 1; if (0 == x) { b(); } else { a(); } } } ");
        }

        [WorkItem(545986, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/545986")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_SimplifyToEqualsZero2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"ushort x = 1; [||]if (0 < x) { a(); } else { b(); } } } ",
@"ushort x = 1; if (0 == x) { b(); } else { a(); } } } ");
        }

        [WorkItem(545986, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/545986")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_SimplifyToEqualsZero3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"uint x = 1; [||]if (0 < x) { a(); } else { b(); } } } ",
@"uint x = 1; if (0 == x) { b(); } else { a(); } } } ");
        }

        [WorkItem(545986, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/545986")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_SimplifyToEqualsZero4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"ulong x = 1; [||]if (x > 0) { a(); } else { b(); } } } ",
@"ulong x = 1; if (x == 0) { b(); } else { a(); } } } ");
        }

        [WorkItem(545986, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/545986")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_SimplifyToNotEqualsZero1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"ulong x = 1; [||]if (0 == x) { a(); } else { b(); } } } ",
@"ulong x = 1; if (0 != x) { b(); } else { a(); } } } ");
        }

        [WorkItem(545986, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/545986")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_SimplifyToNotEqualsZero2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"ulong x = 1; [||]if (x == 0) { a(); } else { b(); } } } ",
@"ulong x = 1; if (x != 0) { b(); } else { a(); } } } ");
        }

        [WorkItem(530505, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530505")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_SimplifyLongLengthEqualsZero()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"string[] x; [||]if (x.LongLength > 0) { GreaterThanZero(); } else { EqualsZero(); } } } ",
@"string[] x; if (x.LongLength == 0) { EqualsZero(); } else { GreaterThanZero(); } } } ");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_DoesNotSimplifyToLengthEqualsZero()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"string x; [||]if (x.Length >= 0) { a(); } else { b(); } } } ",
@"string x; if (x.Length < 0) { b(); } else { a(); } } } ");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsInvertIf)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSingleLine_DoesNotSimplifyToLengthEqualsZero2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestFixOneAsync(
@"string x; [||]if (x.Length > 0.0f) { GreaterThanZero(); } else { EqualsZero(); } } } ",
@"string x; if (x.Length <= 0.0f) { EqualsZero(); } else { GreaterThanZero(); } } } ");
        }
    }
}
