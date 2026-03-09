// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp.Completion.SuggestionMode;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Completion.CompletionProviders
{
    public class SuggestionModeCompletionProviderTests : AbstractCSharpCompletionProviderTests
    {
        public SuggestionModeCompletionProviderTests(CSharpTestWorkspaceFixture workspaceFixture) 
            : base(workspaceFixture)
        {
        }

        internal override CompletionProvider CreateCompletionProvider()
            => new CSharpSuggestionModeCompletionProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterFirstExplicitArgument()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            // The right-hand-side parses like a possible deconstruction or tuple type
            await VerifyBuilderAsync(AddInsideMethod(@"Func<int, int, int> f = (int x, i $$"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterFirstImplicitArgument()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            // The right-hand-side parses like a possible deconstruction or tuple type
            await VerifyBuilderAsync(AddInsideMethod(@"Func<int, int, int> f = (x, i $$"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterFirstImplicitArgumentInMethodCall()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"class c
{
    private void bar(Func<int, int, bool> f) { }
    
    private void goo()
    {
        bar((x, i $$
    }
}
";
            // The right-hand-side parses like a possible deconstruction or tuple type
            await VerifyBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterFirstExplicitArgumentInMethodCall()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"class c
{
    private void bar(Func<int, int, bool> f) { }
    
    private void goo()
    {
        bar((int x, i $$
    }
}
";
            // Could be a deconstruction expression
            await VerifyBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DelegateTypeExpected1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;

class c
{
    private void bar(Func<int, int, bool> f) { }
    
    private void goo()
    {
        bar($$
    }
}
";
            await VerifyBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DelegateTypeExpected2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyBuilderAsync(AddUsingDirectives("using System;", AddInsideMethod(@"Func<int, int, int> f = $$")));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ObjectInitializerDelegateType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    public Func<int> myfunc { get; set; }
}

class a
{
    void goo()
    {
        var b = new Program() { myfunc = $$
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [Fact, WorkItem(817145, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/817145"), Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExplicitArrayInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;

class a
{
    void goo()
    {
        Func<int, int>[] myfunc = new Func<int, int>[] { $$;
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ImplicitArrayInitializerUnknownType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;

class a
{
    void goo()
    {
        var a = new [] { $$;
    }
}";
            await VerifyNotBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ImplicitArrayInitializerKnownDelegateType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;

class a
{
    void goo()
    {
        var a = new [] { x => 2 * x, $$
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TernaryOperatorUnknownType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;

class a
{
    void goo()
    {
        var a = true ? $$
    }
}";
            await VerifyNotBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TernaryOperatorKnownDelegateType1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;

class a
{
    void goo()
    {
        var a = true ? x => x * 2 : $$
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TernaryOperatorKnownDelegateType2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;

class a
{
    void goo()
    {
        Func<int, int> a = true ? $$
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OverloadTakesADelegate1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;

class a
{
    void goo(int a) { }
    void goo(Func<int, int> a) { }

    void bar()
    {
        this.goo($$
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OverloadTakesDelegate2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;

class a
{
    void goo(int i, int a) { }
    void goo(int i, Func<int, int> a) { }

    void bar()
    {
        this.goo(1, $$
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExplicitCastToDelegate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;

class a
{

    void bar()
    {
        (Func<int, int>) ($$
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
        [WorkItem(860580, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/860580")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ReturnStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;

class a
{
    Func<int, int> bar()
    {
        return $$
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task BuilderInAnonymousType1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;

class a
{
    int bar()
    {
        var q = new {$$
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task BuilderInAnonymousType2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;

class a
{
    int bar()
    {
        var q = new {$$ 1, 2 };
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task BuilderInAnonymousType3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
class a
{
    int bar()
    {
        var q = new {Name = 1, $$ };
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task BuilderInFromClause()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
using System.Linq;

class a
{
    int bar()
    {
        var q = from $$
    }
}";
            await VerifyBuilderAsync(markup.ToString());
        }

        [WorkItem(823968, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/823968")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task BuilderInJoinClause()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
using System.Linq;
using System.Collections.Generic;

class a
{
    int bar()
    {
        var list = new List<int>();
        var q = from a in list
                join $$
    }
}";
            await VerifyBuilderAsync(markup.ToString());
        }

        [WorkItem(544290, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/544290")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ParenthesizedLambdaArgument()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
class Program
{
    static void Main(string[] args)
    {
        Console.CancelKeyPress += new ConsoleCancelEventHandler((a$$, e) => { });
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [WorkItem(544379, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/544379")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IncompleteParenthesizedLambdaArgument()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
class Program
{
    static void Main(string[] args)
    {
        Console.CancelKeyPress += new ConsoleCancelEventHandler((a$$
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [WorkItem(544379, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/544379")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IncompleteNestedParenthesizedLambdaArgument()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
class Program
{
    static void Main(string[] args)
    {
        Console.CancelKeyPress += new ConsoleCancelEventHandler(((a$$
    }
}";
            await VerifyNotBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ParenthesizedExpressionInVarDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
class Program
{
    static void Main(string[] args)
    {
        var x = (a$$
    }
}";
            await VerifyNotBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
        [WorkItem(24432, "https://github.com/dotnet/roslyn/issues/24432")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInObjectCreation()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
class Program
{
    static void Main()
    {
        Program x = new P$$
    }
}";
            await VerifyNotBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
        [WorkItem(24432, "https://github.com/dotnet/roslyn/issues/24432")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInArrayCreation()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
class Program
{
    static void Main()
    {
        Program[] x = new $$
    }
}";
            await VerifyNotBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
        [WorkItem(24432, "https://github.com/dotnet/roslyn/issues/24432")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInArrayCreation2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
class Program
{
    static void Main()
    {
        Program[] x = new Pr$$
    }
}";
            await VerifyNotBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleExpressionInVarDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
class Program
{
    static void Main(string[] args)
    {
        var x = (a$$, b)
    }
}";
            await VerifyNotBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleExpressionInVarDeclaration2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
class Program
{
    static void Main(string[] args)
    {
        var x = (a, b$$)
    }
}";
            await VerifyNotBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IncompleteLambdaInActionDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
class Program
{
    static void Main(string[] args)
    {
        System.Action x = (a$$, b)
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleWithNamesInActionDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
class Program
{
    static void Main(string[] args)
    {
        System.Action x = (a$$, b: b)
    }
}";
            await VerifyNotBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleWithNamesInActionDeclaration2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
class Program
{
    static void Main(string[] args)
    {
        System.Action x = (a: a, b$$)
    }
}";
            await VerifyNotBuilderAsync(markup);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleWithNamesInVarDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
class Program
{
    static void Main(string[] args)
    {
        var x = (a: a, b$$)
    }
}";
            await VerifyNotBuilderAsync(markup);
        }

        [WorkItem(546363, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546363")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task BuilderForLinqExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
using System.Linq.Expressions;
 
public class Class
{
    public void Goo(Expression<Action<int>> arg)
    {
        Goo($$
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [WorkItem(546363, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546363")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotInTypeParameter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
using System.Linq.Expressions;
 
public class Class
{
    public void Goo(Expression<Action<int>> arg)
    {
        Enumerable.Empty<$$
    }
}";
            await VerifyNotBuilderAsync(markup);
        }

        [WorkItem(611477, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/611477")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExtensionMethodFaultTolerance()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
 
namespace Outer
{
    public struct ImmutableArray<T> : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }
 
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
 
    public static class ReadOnlyArrayExtensions
    {
        public static ImmutableArray<TResult> Select<T, TResult>(this ImmutableArray<T> array, Func<T, TResult> selector)
        {
            throw new NotImplementedException();
        }
    }
 
    namespace Inner
    {
        class Program
        {
            static void Main(string[] args)
            {
                args.Select($$
            }
        }
    }
}
";
            await VerifyBuilderAsync(markup);
        }

        [WorkItem(834609, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/834609")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task LambdaWithAutomaticBraceCompletion()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
using System;
 
public class Class
{
    public void Goo()
    {
        EventHandler h = (s$$)
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [WorkItem(858112, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/858112")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ThisConstructorInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
class X 
{ 
    X(Func<X> x) : this($$) { } 
}";
            await VerifyBuilderAsync(markup);
        }

        [WorkItem(858112, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/858112")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task BaseConstructorInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;
class B
{
    public B(Func<B> x) {}
}

class D : B 
{ 
    D() : base($$) { } 
}";
            await VerifyBuilderAsync(markup);
        }

        [WorkItem(887842, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/887842")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PreprocessorExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"class C
{
#if $$
}";
            await VerifyBuilderAsync(markup);
        }

        [WorkItem(967254, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/967254")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ImplicitArrayInitializerAfterNew()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"using System;

class a
{
    void goo()
    {
        int[] a = new $$;
    }
}";
            await VerifyNotBuilderAsync(markup);
        }

        [WorkItem(7213, "https://github.com/dotnet/roslyn/issues/7213")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NamespaceDeclaration_Unqualified()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"namespace $$";
            await VerifyBuilderAsync(markup);
        }

        [WorkItem(7213, "https://github.com/dotnet/roslyn/issues/7213")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NamespaceDeclaration_Qualified()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"namespace A.$$";
            await VerifyBuilderAsync(markup);
        }

        [WorkItem(7213, "https://github.com/dotnet/roslyn/issues/7213")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PartialClassName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"partial class $$";
            await VerifyBuilderAsync(markup);
        }

        [WorkItem(7213, "https://github.com/dotnet/roslyn/issues/7213")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PartialStructName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"partial struct $$";
            await VerifyBuilderAsync(markup);
        }

        [WorkItem(7213, "https://github.com/dotnet/roslyn/issues/7213")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PartialInterfaceName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"partial interface $$";
            await VerifyBuilderAsync(markup);
        }

        [WorkItem(12818, "https://github.com/dotnet/roslyn/issues/12818")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task UnwrapParamsArray()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
class C {
    C(params Action<int>[] a) {
        new C($$
    }
}";
            await VerifyBuilderAsync(markup);
        }

        [WorkItem(12818, "https://github.com/dotnet/roslyn/issues/12818")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DoNotUnwrapRegularArray()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
class C {
    C(Action<int>[] a) {
        new C($$
    }
}";
            await VerifyNotBuilderAsync(markup);
        }

        [WorkItem(15443, "https://github.com/dotnet/roslyn/issues/15443")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotBuilderWhenDelegateInferredRightOfDotInInvocation()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C {
	Action a = Task.$$
}";
            await VerifyNotBuilderAsync(markup);
        }

        [WorkItem(15443, "https://github.com/dotnet/roslyn/issues/15443")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotBuilderInTypeArgument()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
namespace ConsoleApplication1
{
    class Program
    {
        class N { }
        static void Main(string[] args)
        {
            Program.N n = Load<Program.$$
        }

        static T Load<T>() => default(T);
    }
}";
            await VerifyNotBuilderAsync(markup);
        }

        [WorkItem(16176, "https://github.com/dotnet/roslyn/issues/16176")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotBuilderForLambdaAfterNew()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C {
	Action a = new $$
}";
            await VerifyNotBuilderAsync(markup);
        }

        [WorkItem(20937, "https://github.com/dotnet/roslyn/issues/20937")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AsyncLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
using System.Threading.Tasks;
class Program
{
    public void B(Func<int, int, Task<int>> f) { }

    void A()
    {
        B(async($$";

            await VerifyBuilderAsync(markup);
        }

        [WorkItem(20937, "https://github.com/dotnet/roslyn/issues/20937")]
        [Fact, Trait(Traits.Feature, Traits.Features.Completion)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AsyncLambdaAfterComma()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
using System;
using System.Threading.Tasks;
class Program
{
    public void B(Func<int, int, Task<int>> f) { }

    void A()
    {
        B(async(p1, $$";

            await VerifyBuilderAsync(markup);
        }

        private async Task VerifyNotBuilderAsync(string markup)
        {
            await VerifyWorkerAsync(markup, isBuilder: false);
        }

        private async Task VerifyBuilderAsync(string markup)
        {
            await VerifyWorkerAsync(markup, isBuilder: true);
        }

        private async Task VerifyWorkerAsync(string markup, bool isBuilder)
        {
            MarkupTestFile.GetPosition(markup, out var code, out int position);

            using (var workspaceFixture = new CSharpTestWorkspaceFixture())
            {
                var document1 = workspaceFixture.UpdateDocument(code, SourceCodeKind.Regular);
                await CheckResultsAsync(document1, position, isBuilder);

                if (await CanUseSpeculativeSemanticModelAsync(document1, position))
                {
                    var document2 = workspaceFixture.UpdateDocument(code, SourceCodeKind.Regular, cleanBeforeUpdate: false);
                    await CheckResultsAsync(document2, position, isBuilder);
                }

                workspaceFixture.DisposeAfterTest();
            }
        }

        private async Task CheckResultsAsync(Document document, int position, bool isBuilder)
        {
            var triggerInfos = new List<CompletionTrigger>();
            triggerInfos.Add(CompletionTrigger.CreateInsertionTrigger('a'));
            triggerInfos.Add(CompletionTrigger.Invoke);
            triggerInfos.Add(CompletionTrigger.CreateDeletionTrigger('z'));

            var service = GetCompletionService(document.Project.Solution.Workspace);

            foreach (var triggerInfo in triggerInfos)
            {
                var completionList = await service.GetContextAsync(
                    service.ExclusiveProviders?[0], document, position, triggerInfo,
                    options: null, cancellationToken: CancellationToken.None);

                if (isBuilder)
                {
                    Assert.NotNull(completionList);
                    Assert.True(completionList.SuggestionModeItem != null, "Expecting a suggestion mode, but none was present");
                }
                else
                {
                    if (completionList != null)
                    {
                        Assert.True(completionList.SuggestionModeItem == null, "group.Builder == " + (completionList.SuggestionModeItem != null ? completionList.SuggestionModeItem.DisplayText : "null"));
                    }
                }
            }
        }
    }
}
