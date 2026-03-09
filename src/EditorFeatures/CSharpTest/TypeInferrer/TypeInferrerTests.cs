// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editor.UnitTests.TypeInferrer;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.LanguageServices;
using Microsoft.CodeAnalysis.Shared.Extensions;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.TypeInferrer
{
    public partial class TypeInferrerTests : TypeInferrerTestBase<CSharpTestWorkspaceFixture>
    {
        public TypeInferrerTests(CSharpTestWorkspaceFixture workspaceFixture) : base(workspaceFixture)
        {
        }

        protected override async Task TestWorkerAsync(Document document, TextSpan textSpan, string expectedType, bool useNodeStartPosition)
        {
            var root = await document.GetSyntaxRootAsync();
            var node = FindExpressionSyntaxFromSpan(root, textSpan);
            var typeInference = document.GetLanguageService<ITypeInferenceService>();

            var inferredType = useNodeStartPosition
                ? typeInference.InferType(await document.GetSemanticModelForSpanAsync(new TextSpan(node?.SpanStart ?? textSpan.Start, 0), CancellationToken.None), node?.SpanStart ?? textSpan.Start, objectAsDefault: true, cancellationToken: CancellationToken.None)
                : typeInference.InferType(await document.GetSemanticModelForSpanAsync(node?.Span ?? textSpan, CancellationToken.None), node, objectAsDefault: true, cancellationToken: CancellationToken.None);
            var typeSyntax = inferredType.GenerateTypeSyntax().NormalizeWhitespace();
            Assert.Equal(expectedType, typeSyntax.ToString());
        }

        private async Task TestInClassAsync(string text, string expectedType)
        {
            text = @"class C
{
    $
}".Replace("$", text);
            await TestAsync(text, expectedType);
        }

        private async Task TestInMethodAsync(string text, string expectedType, bool testNode = true, bool testPosition = true)
        {
            text = @"class C
{
    void M()
    {
        $
    }
}".Replace("$", text);
            await TestAsync(text, expectedType, testNode: testNode, testPosition: testPosition);
        }

        private ExpressionSyntax FindExpressionSyntaxFromSpan(SyntaxNode root, TextSpan textSpan)
        {
            var token = root.FindToken(textSpan.Start);
            var currentNode = token.Parent;
            while (currentNode != null)
            {
                if (currentNode is ExpressionSyntax result && result.Span == textSpan)
                {
                    return result;
                }

                currentNode = currentNode.Parent;
            }

            return null;
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditional1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            // We do not support position inference here as we're before the ? and we only look
            // backwards to infer a type here.
            await TestInMethodAsync(
@"var q = [|Goo()|] ? 1 : 2;", "global::System.Boolean",
                testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditional2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"var q = a ? [|Goo()|] : 2;", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditional3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"var q = a ? """" : [|Goo()|];", "global::System.String");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestVariableDeclarator1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"int q = [|Goo()|];", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestVariableDeclarator2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"var q = [|Goo()|];", "global::System.Object");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCoalesce1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"var q = [|Goo()|] ?? 1;", "global::System.Int32?", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCoalesce2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"bool? b;
var q = b ?? [|Goo()|];", "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCoalesce3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"string s;
var q = s ?? [|Goo()|];", "global::System.String");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCoalesce4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"var q = [|Goo()|] ?? string.Empty;", "global::System.String", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestBinaryExpression1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"string s;
var q = s + [|Goo()|];", "global::System.String");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestBinaryExpression2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"var s;
var q = s || [|Goo()|];", "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestBinaryOperator1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"var q = x << [|Goo()|];", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestBinaryOperator2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"var q = x >> [|Goo()|];", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAssignmentOperator3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"var q <<= [|Goo()|];", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAssignmentOperator4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"var q >>= [|Goo()|];", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOverloadedConditionalLogicalOperatorsInferBool()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"using System;

class C
{
    public static C operator &(C c, C d)
    {
        return null;
    }

    public static bool operator true(C c)
    {
        return true;
    }

    public static bool operator false(C c)
    {
        return false;
    }

    static void Main(string[] args)
    {
        var c = new C() && [|Goo()|];
    }
}", "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditionalLogicalOrOperatorAlwaysInfersBool()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        var x = a || [|7|];
    }
}";
            await TestAsync(text, "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditionalLogicalAndOperatorAlwaysInfersBool()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        var x = a && [|7|];
    }
}";
            await TestAsync(text, "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalOrOperatorInference1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        var x = [|a|] | true;
    }
}";
            await TestAsync(text, "global::System.Boolean", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalOrOperatorInference2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        var x = [|a|] | b | c || d;
    }
}";
            await TestAsync(text, "global::System.Boolean", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalOrOperatorInference3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        var x = a | b | [|c|] || d;
    }
}";
            await TestAsync(text, "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalOrOperatorInference4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        var x = Goo([|a|] | b);
    }
    static object Goo(Program p)
    {
        return p;
    }
}";
            await TestAsync(text, "Program", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalOrOperatorInference5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        var x = Goo([|a|] | b);
    }
    static object Goo(bool p)
    {
        return p;
    }
}";
            await TestAsync(text, "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalOrOperatorInference6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        if (([|x|] | y) != 0) {}
    }
}";
            await TestAsync(text, "global::System.Int32", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalOrOperatorInference7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        if ([|x|] | y) {}
    }
}";
            await TestAsync(text, "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalAndOperatorInference1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        var x = [|a|] & true;
    }
}";
            await TestAsync(text, "global::System.Boolean", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalAndOperatorInference2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        var x = [|a|] & b & c && d;
    }
}";
            await TestAsync(text, "global::System.Boolean", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalAndOperatorInference3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        var x = a & b & [|c|] && d;
    }
}";
            await TestAsync(text, "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalAndOperatorInference4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        var x = Goo([|a|] & b);
    }
    static object Goo(Program p)
    {
        return p;
    }
}";
            await TestAsync(text, "Program", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalAndOperatorInference5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        var x = Goo([|a|] & b);
    }
    static object Goo(bool p)
    {
        return p;
    }
}";
            await TestAsync(text, "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalAndOperatorInference6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        if (([|x|] & y) != 0) {}
    }
}";
            await TestAsync(text, "global::System.Int32", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalAndOperatorInference7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        if ([|x|] & y) {}
    }
}";
            await TestAsync(text, "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalXorOperatorInference1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        var x = [|a|] ^ true;
    }
}";
            await TestAsync(text, "global::System.Boolean", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalXorOperatorInference2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        var x = [|a|] ^ b ^ c && d;
    }
}";
            await TestAsync(text, "global::System.Boolean", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalXorOperatorInference3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        var x = a ^ b ^ [|c|] && d;
    }
}";
            await TestAsync(text, "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalXorOperatorInference4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        var x = Goo([|a|] ^ b);
    }
    static object Goo(Program p)
    {
        return p;
    }
}";
            await TestAsync(text, "Program", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalXorOperatorInference5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        var x = Goo([|a|] ^ b);
    }
    static object Goo(bool p)
    {
        return p;
    }
}";
            await TestAsync(text, "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalXorOperatorInference6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        if (([|x|] ^ y) != 0) {}
    }
}";
            await TestAsync(text, "global::System.Int32", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalXorOperatorInference7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        if ([|x|] ^ y) {}
    }
}";
            await TestAsync(text, "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalOrEqualsOperatorInference1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        if ([|x|] |= y) {}
    }
}";
            await TestAsync(text, "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalOrEqualsOperatorInference2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        int z = [|x|] |= y;
    }
}";
            await TestAsync(text, "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalAndEqualsOperatorInference1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        if ([|x|] &= y) {}
    }
}";
            await TestAsync(text, "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalAndEqualsOperatorInference2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        int z = [|x|] &= y;
    }
}";
            await TestAsync(text, "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalXorEqualsOperatorInference1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        if ([|x|] ^= y) {}
    }
}";
            await TestAsync(text, "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617633, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617633")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLogicalXorEqualsOperatorInference2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"using System;
class C
{
    static void Main(string[] args)
    {
        int z = [|x|] ^= y;
    }
}";
            await TestAsync(text, "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestReturn1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInClassAsync(
@"int M()
{
    return [|Goo()|];
}", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestReturn2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"return [|Goo()|];", "void");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestReturn3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInClassAsync(
@"int Property
{
    get
    {
        return [|Goo()|];
    }
}", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(827897, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/827897")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestYieldReturn()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup =
@"using System.Collections.Generic;

class Program
{
    IEnumerable<int> M()
    {
        yield return [|abc|]
    }
}";
            await TestAsync(markup, "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestReturnInLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"System.Func<string, int> f = s =>
{
    return [|Goo()|];
};", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"System.Func<string, int> f = s => [|Goo()|];", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestThrow()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"throw [|Goo()|];", "global::System.Exception");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCatch()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync("try { } catch ([|Goo|] ex) { }", "global::System.Exception");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIf()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(@"if ([|Goo()|]) { }", "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWhile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(@"while ([|Goo()|]) { }", "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDo()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(@"do { } while ([|Goo()|])", "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFor1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"for (int i = 0; [|Goo()|];

i++) { }", "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFor2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(@"for (string i = [|Goo()|]; ; ) { }", "global::System.String");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFor3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(@"for (var i = [|Goo()|]; ; ) { }", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUsing1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(@"using ([|Goo()|]) { }", "global::System.IDisposable");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUsing2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(@"using (int i = [|Goo()|]) { }", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUsing3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(@"using (var v = [|Goo()|]) { }", "global::System.IDisposable");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForEach()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(@"foreach (int v in [|Goo()|]) { }", "global::System.Collections.Generic.IEnumerable<global::System.Int32>");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPrefixExpression1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"var q = +[|Goo()|];", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPrefixExpression2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"var q = -[|Goo()|];", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPrefixExpression3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"var q = ~[|Goo()|];", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPrefixExpression4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"var q = ![|Goo()|];", "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPrefixExpression5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"var q = System.DayOfWeek.Monday & ~[|Goo()|];", "global::System.DayOfWeek");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArrayRankSpecifier()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"var q = new string[[|Goo()|]];", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSwitch1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(@"switch ([|Goo()|]) { }", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSwitch2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(@"switch ([|Goo()|]) { default: }", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSwitch3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(@"switch ([|Goo()|]) { case ""a"": }", "global::System.String");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodCall1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"Bar([|Goo()|]);", "global::System.Object");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodCall2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInClassAsync(
@"void M()
{
    Bar([|Goo()|]);
}

void Bar(int i);", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodCall3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInClassAsync(
@"void M()
{
    Bar([|Goo()|]);
}

void Bar();", "global::System.Object");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodCall4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInClassAsync(
@"void M()
{
    Bar([|Goo()|]);
}

void Bar(int i, string s);", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethodCall5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInClassAsync(
@"void M()
{
    Bar(s: [|Goo()|]);
}

void Bar(int i, string s);", "global::System.String");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructorCall1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"new C([|Goo()|]);", "global::System.Object");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructorCall2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInClassAsync(
@"void M()
{
    new C([|Goo()|]);
} C(int i)
{
}", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructorCall3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInClassAsync(
@"void M()
{
    new C([|Goo()|]);
} C()
{
}", "global::System.Object");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructorCall4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInClassAsync(
@"void M()
{
    new C([|Goo()|]);
} C(int i, string s)
{
}", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructorCall5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInClassAsync(
@"void M()
{
    new C(s: [|Goo()|]);
} C(int i, string s)
{
}", "global::System.String");
        }

        [WorkItem(858112, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/858112")]
        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestThisConstructorInitializer1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class MyClass
{
    public MyClass(int x) : this([|test|])
    {
    }
}", "global::System.Int32");
        }

        [WorkItem(858112, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/858112")]
        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestThisConstructorInitializer2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class MyClass
{
    public MyClass(int x, string y) : this(5, [|test|])
    {
    }
}", "global::System.String");
        }

        [WorkItem(858112, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/858112")]
        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestBaseConstructorInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class B
{
    public B(int x)
    {
    }
}

class D : B
{
    public D() : base([|test|])
    {
    }
}", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndexAccess1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"string[] i;

i[[|Goo()|]];", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndexerCall1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(@"this[[|Goo()|]];", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndexerCall2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            // Update this when binding of indexers is working.
            await TestInClassAsync(
@"void M()
{
    this[[|Goo()|]];
}

int this[int i] { get; }", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndexerCall3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            // Update this when binding of indexers is working.
            await TestInClassAsync(
@"void M()
{
    this[[|Goo()|]];
}

int this[int i, string s] { get; }", "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndexerCall5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInClassAsync(
@"void M()
{
    this[s: [|Goo()|]];
}

int this[int i, string s] { get; }", "global::System.String");
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArrayInitializerInImplicitArrayCreationSimple()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System.Collections.Generic;

class C
{
  void M()
  {
       var a = new[] { 1, [|2|] };
  }
}";

            await TestAsync(text, "global::System.Int32");
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArrayInitializerInImplicitArrayCreation1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System.Collections.Generic;

class C
{
  void M()
  {
       var a = new[] { Bar(), [|Goo()|] };
  }

  int Bar() { return 1; }
  int Goo() { return 2; }
}";

            await TestAsync(text, "global::System.Int32");
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArrayInitializerInImplicitArrayCreation2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System.Collections.Generic;

class C
{
  void M()
  {
       var a = new[] { Bar(), [|Goo()|] };
  }

  int Bar() { return 1; }
}";

            await TestAsync(text, "global::System.Int32");
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArrayInitializerInImplicitArrayCreation3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System.Collections.Generic;

class C
{
  void M()
  {
       var a = new[] { Bar(), [|Goo()|] };
  }
}";

            await TestAsync(text, "global::System.Object");
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArrayInitializerInEqualsValueClauseSimple()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System.Collections.Generic;

class C
{
  void M()
  {
       int[] a = { 1, [|2|] };
  }
}";

            await TestAsync(text, "global::System.Int32");
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArrayInitializerInEqualsValueClause()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System.Collections.Generic;

class C
{
  void M()
  {
       int[] a = { Bar(), [|Goo()|] };
  }

  int Bar() { return 1; }
}";

            await TestAsync(text, "global::System.Int32");
        }

        [Fact]
        [WorkItem(529480, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/529480")]
        [Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCollectionInitializer1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System.Collections.Generic;

class C
{
  void M()
  {
    new List<int>() { [|Goo()|] };
  }
}";

            await TestAsync(text, "global::System.Int32");
        }

        [Fact]
        [WorkItem(529480, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/529480")]
        [Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCollectionInitializer2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"
using System.Collections.Generic;

class C
{
  void M()
  {
    new Dictionary<int,string>() { { [|Goo()|], """" } };
  }
}";

            await TestAsync(text, "global::System.Int32");
        }

        [Fact]
        [WorkItem(529480, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/529480")]
        [Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCollectionInitializer3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"
using System.Collections.Generic;

class C
{
  void M()
  {
    new Dictionary<int,string>() { { 0, [|Goo()|] } };
  }
}";

            await TestAsync(text, "global::System.String");
        }

        [Fact]
        [WorkItem(529480, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/529480")]
        [Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCustomCollectionInitializerAddMethod1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"class C : System.Collections.IEnumerable
{
    void M()
    {
        var x = new C() { [|a|] };
    }

    void Add(int i) { }
    void Add(string s, bool b) { }

    public System.Collections.IEnumerator GetEnumerator()
    {
        throw new System.NotImplementedException();
    }
}";

            await TestAsync(text, "global::System.Int32", testPosition: false);
        }

        [Fact]
        [WorkItem(529480, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/529480")]
        [Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCustomCollectionInitializerAddMethod2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"class C : System.Collections.IEnumerable
{
    void M()
    {
        var x = new C() { { ""test"", [|b|] } };
    }

    void Add(int i) { }
    void Add(string s, bool b) { }

    public System.Collections.IEnumerator GetEnumerator()
    {
        throw new System.NotImplementedException();
    }
}";

            await TestAsync(text, "global::System.Boolean");
        }

        [Fact]
        [WorkItem(529480, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/529480")]
        [Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCustomCollectionInitializerAddMethod3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"class C : System.Collections.IEnumerable
{
    void M()
    {
        var x = new C() { { [|s|], true } };
    }

    void Add(int i) { }
    void Add(string s, bool b) { }

    public System.Collections.IEnumerator GetEnumerator()
    {
        throw new System.NotImplementedException();
    }
}";

            await TestAsync(text, "global::System.String");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArrayInference1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"
class A
{
    void Goo()
    {
        A[] x = new [|C|][] { };
    }
}";

            await TestAsync(text, "global::A", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArrayInference1_Position()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"
class A
{
    void Goo()
    {
        A[] x = new [|C|][] { };
    }
}";

            await TestAsync(text, "global::A[]", testNode: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArrayInference2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"
class A
{
    void Goo()
    {
        A[][] x = new [|C|][][] { };
    }
}";

            await TestAsync(text, "global::A", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArrayInference2_Position()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"
class A
{
    void Goo()
    {
        A[][] x = new [|C|][][] { };
    }
}";

            await TestAsync(text, "global::A[][]", testNode: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArrayInference3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"
class A
{
    void Goo()
    {
        A[][] x = new [|C|][] { };
    }
}";

            await TestAsync(text, "global::A[]", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArrayInference3_Position()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"
class A
{
    void Goo()
    {
        A[][] x = new [|C|][] { };
    }
}";

            await TestAsync(text, "global::A[][]", testNode: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArrayInference4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"
using System;
class A
{
    void Goo()
    {
        Func<int, int>[] x = new Func<int, int>[] { [|Bar()|] };
    }
}";

            await TestAsync(text, "global::System.Func<global::System.Int32, global::System.Int32>");
        }

        [WorkItem(538993, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/538993")]
        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideLambda2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System;
class C
{
  void M()
  {
    Func<int,int> f = i => [|here|]
  }
}";

            await TestAsync(text, "global::System.Int32");
        }

        [WorkItem(539813, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539813")]
        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPointer1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"class C
{
  void M(int* i)
  {
    var q = i[[|Goo()|]];
  }
}";

            await TestAsync(text, "global::System.Int32");
        }

        [WorkItem(539813, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539813")]
        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDynamic1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"class C
{
  void M(dynamic i)
  {
    var q = i[[|Goo()|]];
  }
}";

            await TestAsync(text, "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestChecked1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"class C
{
  void M()
  {
    string q = checked([|Goo()|]);
  }
}";

            await TestAsync(text, "global::System.String");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(553584, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/553584")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAwaitTaskOfT()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System.Threading.Tasks;
class C
{
  void M()
  {
    int x = await [|Goo()|];
  }
}";

            await TestAsync(text, "global::System.Threading.Tasks.Task<global::System.Int32>");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(553584, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/553584")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAwaitTaskOfTaskOfT()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System.Threading.Tasks;
class C
{
  void M()
  {
    Task<int> x = await [|Goo()|];
  }
}";

            await TestAsync(text, "global::System.Threading.Tasks.Task<global::System.Threading.Tasks.Task<global::System.Int32>>");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(553584, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/553584")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAwaitTask()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System.Threading.Tasks;
class C
{
  void M()
  {
    await [|Goo()|];
  }
}";

            await TestAsync(text, "global::System.Threading.Tasks.Task");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617622, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617622")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLockStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"class C
{
  void M()
  {
    lock([|Goo()|])
    {
    }
  }
}";

            await TestAsync(text, "global::System.Object");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(617622, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/617622")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAwaitExpressionInLockStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"class C
{
  async void M()
  {
    lock(await [|Goo()|])
    {
    }
  }
}";

            await TestAsync(text, "global::System.Threading.Tasks.Task<global::System.Object>");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(827897, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/827897")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestReturnFromAsyncTaskOfT()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup =
@"using System.Threading.Tasks;
class Program
{
    async Task<int> M()
    {
        await Task.Delay(1);
        return [|ab|]
    }
}";
            await TestAsync(markup, "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(853840, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/853840")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAttributeArguments1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup =
@"[A([|dd|], ee, Y = ff)]
class AAttribute : System.Attribute
{
    public int X;
    public string Y;

    public AAttribute(System.DayOfWeek a, double b)
    {

    }
}";
            await TestAsync(markup, "global::System.DayOfWeek");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(853840, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/853840")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAttributeArguments2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup =
@"[A(dd, [|ee|], Y = ff)]
class AAttribute : System.Attribute
{
    public int X;
    public string Y;

    public AAttribute(System.DayOfWeek a, double b)
    {

    }
}";
            await TestAsync(markup, "global::System.Double");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(853840, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/853840")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAttributeArguments3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup =
@"[A(dd, ee, Y = [|ff|])]
class AAttribute : System.Attribute
{
    public int X;
    public string Y;

    public AAttribute(System.DayOfWeek a, double b)
    {

    }
}";
            await TestAsync(markup, "global::System.String");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(757111, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/757111")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestReturnStatementWithinDelegateWithinAMethodCall()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System;

class Program
{
    delegate string A(int i);

    static void Main(string[] args)
    {
        B(delegate(int i) { return [|M()|]; });
    }

    private static void B(A a)
    {
    }
}";

            await TestAsync(text, "global::System.String");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(994388, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/994388")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCatchFilterClause()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"
try
{ }
catch (Exception) if ([|M()|])
}";
            await TestInMethodAsync(text, "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(994388, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/994388")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCatchFilterClause1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"
try
{ }
catch (Exception) if ([|M|])
}";
            await TestInMethodAsync(text, "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(994388, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/994388")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCatchFilterClause2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"
try
{ }
catch (Exception) if ([|M|].N)
}";
            await TestInMethodAsync(text, "global::System.Object", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(643, "https://github.com/dotnet/roslyn/issues/643")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAwaitExpressionWithChainingMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System;
using System.Threading.Tasks;

class C
{
    static async void T()
    {
        bool x = await [|M()|].ConfigureAwait(false);
    }
}";
            await TestAsync(text, "global::System.Threading.Tasks.Task<global::System.Boolean>", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(643, "https://github.com/dotnet/roslyn/issues/643")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAwaitExpressionWithChainingMethod2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System;
using System.Threading.Tasks;

class C
{
    static async void T()
    {
        bool x = await [|M|].ContinueWith(a => { return true; }).ContinueWith(a => { return false; });
    }
}";
            await TestAsync(text, "global::System.Threading.Tasks.Task<global::System.Object>", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(4233, "https://github.com/dotnet/roslyn/issues/4233")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAwaitExpressionWithGenericMethod1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System.Threading.Tasks;

public class C
{
    private async void M()
    {
        bool merged = await X([|Test()|]);
    }

    private async Task<T> X<T>(T t) { return t; }
}";
            await TestAsync(text, "global::System.Boolean", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(4233, "https://github.com/dotnet/roslyn/issues/4233")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAwaitExpressionWithGenericMethod2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System.Threading.Tasks;

public class C
{
    private async void M()
    {
        bool merged = await Task.Run(() => [|Test()|]);;
    }

    private async Task<T> X<T>(T t) { return t; }
}";
            await TestAsync(text, "global::System.Boolean");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(4483, "https://github.com/dotnet/roslyn/issues/4483")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNullCoalescingOperator1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
    @"class C
{
    void M()
    {
        object z = [|a|]?? null;
    }
}";
            await TestAsync(text, "global::System.Object");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(4483, "https://github.com/dotnet/roslyn/issues/4483")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNullCoalescingOperator2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
    @"class C
{
    void M()
    {
        object z = [|a|] ?? b ?? c;
    }
}";
            await TestAsync(text, "global::System.Object");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(4483, "https://github.com/dotnet/roslyn/issues/4483")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNullCoalescingOperator3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
    @"class C
{
    void M()
    {
        object z = a ?? [|b|] ?? c;
    }
}";
            await TestAsync(text, "global::System.Object");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(5126, "https://github.com/dotnet/roslyn/issues/5126")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSelectLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
    @"using System.Collections.Generic;
using System.Linq;

class C
{
    void M(IEnumerable<string> args)
    {
        args = args.Select(a =>[||])
    }
}";
            await TestAsync(text, "global::System.Object", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(5126, "https://github.com/dotnet/roslyn/issues/5126")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSelectLambda2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
    @"using System.Collections.Generic;
using System.Linq;

class C
{
    void M(IEnumerable<string> args)
    {
        args = args.Select(a =>[|b|])
    }
}";
            await TestAsync(text, "global::System.String", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(1903, "https://github.com/dotnet/roslyn/issues/1903")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSelectLambda3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System.Collections.Generic;
using System.Linq;

class A { }
class B { }
class C
{
    IEnumerable<B> GetB(IEnumerable<A> a)
    {
        return a.Select(i => [|Goo(i)|]);
    }
}";
            await TestAsync(text, "global::B");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(4486, "https://github.com/dotnet/roslyn/issues/4486")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestReturnInAsyncLambda1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
    @"using System;
using System.IO;
using System.Threading.Tasks;

public class C
{
    public async void M()
    {
        Func<Task<int>> t2 = async () => { return [|a|]; };
    }
}";
            await TestAsync(text, "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(4486, "https://github.com/dotnet/roslyn/issues/4486")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestReturnInAsyncLambda2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
    @"using System;
using System.IO;
using System.Threading.Tasks;

public class C
{
    public async void M()
    {
        Func<Task<int>> t2 = async delegate () { return [|a|]; };
    }
}";
            await TestAsync(text, "global::System.Int32");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(6765, "https://github.com/dotnet/roslyn/issues/6765")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDefaultStatement1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
    @"class C
{
    static void Main(string[] args)
    {
        System.ConsoleModifiers c = default([||])
    }
}";
            await TestAsync(text, "global::System.ConsoleModifiers", testNode: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
        [WorkItem(6765, "https://github.com/dotnet/roslyn/issues/6765")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDefaultStatement2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
    @"class C
{
    static void Goo(System.ConsoleModifiers arg)
    {
        Goo(default([||])
    }
}";
            await TestAsync(text, "global::System.ConsoleModifiers", testNode: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWhereCall()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
    @"
using System.Collections.Generic;
class C
{
    void Goo()
    {
        [|ints|].Where(i => i > 10);
    }
}";
            await TestAsync(text, "global::System.Collections.Generic.IEnumerable<global::System.Int32>", testPosition: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWhereCall2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
    @"
using System.Collections.Generic;
class C
{
    void Goo()
    {
        [|ints|].Where(i => null);
    }
}";
            await TestAsync(text, "global::System.Collections.Generic.IEnumerable<global::System.Object>", testPosition: false);
        }

        [WorkItem(12755, "https://github.com/dotnet/roslyn/issues/12755")]
        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestObjectCreationBeforeArrayIndexing()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"using System;
class C
{
  void M()
  {
        int[] array;
        C p = new [||]
        array[4] = 4;
  }
}";

            await TestAsync(text, "global::C", testNode: false);
        }

        [WorkItem(15468, "https://github.com/dotnet/roslyn/issues/15468")]
        [WorkItem(25305, "https://github.com/dotnet/roslyn/issues/25305")]
        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDeconstruction()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"[|(int i, _)|] =", "(global::System.Int32 i, global::System.Object _)", testPosition: false);
        }

        [WorkItem(15468, "https://github.com/dotnet/roslyn/issues/15468")]
        [WorkItem(25305, "https://github.com/dotnet/roslyn/issues/25305")]
        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDeconstruction2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInMethodAsync(
@"(int i, _) =  [||]", "(global::System.Int32 i, global::System.Object _)", testNode: false);
        }

        [WorkItem(13402, "https://github.com/dotnet/roslyn/issues/13402")]
        [Fact, Trait(Traits.Feature, Traits.Features.TypeInferenceService)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestObjectCreationBeforeBlock()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text =
@"class Program
{
    static void Main(string[] args)
    {
        Program p = new [||] 
        { }
    }
}";

            await TestAsync(text, "global::Program", testNode: false);
        }
    }
}
