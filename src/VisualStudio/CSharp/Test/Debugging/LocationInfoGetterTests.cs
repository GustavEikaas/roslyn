// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.VisualStudio.LanguageServices.CSharp.Debugging;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Debugging
{
    [UseExportProvider]
    public class LocationInfoGetterTests
    {
        private async Task TestAsync(string markup, string expectedName, int expectedLineOffset, CSharpParseOptions parseOptions = null)
        {
            using (var workspace = TestWorkspace.CreateCSharp(markup, parseOptions))
            {
                var testDocument = workspace.Documents.Single();
                var position = testDocument.CursorPosition.Value;
                var locationInfo = await LocationInfoGetter.GetInfoAsync(
                    workspace.CurrentSolution.Projects.Single().Documents.Single(),
                    position,
                    CancellationToken.None);

                Assert.Equal(expectedName, locationInfo.Name);
                Assert.Equal(expectedLineOffset, locationInfo.LineOffset);
            }
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync("class G$$oo { }", "Goo", 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
        [WorkItem(527668, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/527668"), WorkItem(538415, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/538415")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Class
{
    public static void Meth$$od()
    {
    }
}
", "Class.Method()", 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
        [WorkItem(527668, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/527668")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"namespace Namespace
{
    class Class
    {
        void Method()
        {
        }$$
    }
}", "Namespace.Class.Method()", 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
        [WorkItem(527668, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/527668")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDottedNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"namespace Namespace.Another
{
    class Class
    {
        void Method()
        {
        }$$
    }
}", "Namespace.Another.Class.Method()", 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"namespace Namespace
{
    namespace Another
    {
        class Class
        {
            void Method()
            {
            }$$
        }
    }
}", "Namespace.Another.Class.Method()", 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
        [WorkItem(527668, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/527668")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Outer
{
    class Inner
    {
        void Quux()
        {$$
        }
    }
}", "Outer.Inner.Quux()", 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
        [WorkItem(527668, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/527668")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPropertyGetter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Class
{
    string Property
    {
        get
        {
            return null;$$
        }
    }
}", "Class.Property", 4);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
        [WorkItem(527668, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/527668")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPropertySetter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Class
{
    string Property
    {
        get
        {
            return null;
        }

        set
        {
            string s = $$value;
        }
    }
}", "Class.Property", 9);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
        [WorkItem(538415, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/538415")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Class
{
    int fi$$eld;
}", "Class.field", 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
        [WorkItem(543494, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/543494")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLambdaInFieldInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Class
{
    Action<int> a = b => { in$$t c; };
}", "Class.a", 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
        [WorkItem(543494, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/543494")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMultipleFields()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Class
{
    int a1, a$$2;
}", "Class.a2", 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C1
{
    C1()
    {

    $$}
}
", "C1.C1()", 3);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDestructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C1
{
    ~C1()
    {
    $$}
}
", "C1.~C1()", 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOperator()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"namespace N1
{
    class C1
    {
        public static int operator +(C1 x, C1 y)
        {
            $$return 42;
        }
    }
}
", "N1.C1.+(C1 x, C1 y)", 2); // Old implementation reports "operator +" (rather than "+")...
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConversionOperator()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"namespace N1
{
    class C1
    {
        public static explicit operator N1.C2(N1.C1 x)
        {
            $$return null;
        }
    }
    class C2
    {
    }
}
", "N1.C1.N1.C2(N1.C1 x)", 2); // Old implementation reports "explicit operator N1.C2" (rather than "N1.C2")...
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEvent()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C1
{
    delegate void D1();
    event D1 e1$$;
}
", "C1.e1", 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TextExplicitInterfaceImplementation()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"interface I1
{
    void M1();
}
class C1
{
    void I1.M1()
    {
    $$}
}
", "C1.M1()", 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TextIndexer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C1
{
    C1 this[int x]
    {
        get
        {
            $$return null;
        }
    }
}
", "C1.this[int x]", 4);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestParamsParameter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C1
{
    void M1(params int[] x) { $$ }
}
", "C1.M1(params int[] x)", 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArglistParameter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C1
{
    void M1(__arglist) { $$ }
}
", "C1.M1(__arglist)", 0); // Old implementation does not show "__arglist"...
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRefAndOutParameters()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C1
{
    void M1( ref int x, out int y )
    {
        $$y = x;
    }
}
", "C1.M1( ref int x, out int y )", 2); // Old implementation did not show extra spaces around the parameters...
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOptionalParameters()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C1
{
    void M1(int x =1)
    {
        $$y = x;
    }
}
", "C1.M1(int x =1)", 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExtensionMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"static class C1
{
    static void M1(this int x)
    {
    }$$
}
", "C1.M1(this int x)", 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenericType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C1<T, U>
{
    static void M1() { $$ }
}
", "C1.M1()", 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenericMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C1<T, U>
{
    static void M1<V>() { $$ }
}
", "C1.M1()", 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenericParameters()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C1<T, U>
{
    static void M1<V>(C1<int, V> x, V y) { $$ }
}
", "C1.M1(C1<int, V> x, V y)", 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"{
    class Class
    {
        int a1, a$$2;
    }
}", "Class.a2", 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingNamespaceName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"namespace
{
    class C1
    {
        int M1()
        $${
        }
    }
}", "?.C1.M1()", 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingClassName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"namespace N1
    class 
    {
        int M1()
        $${
        }
    }
}", "N1.M1()", 1); // Old implementation displayed "N1.?.M1", but we don't see a class declaration in the syntax tree...
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingMethodName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"namespace N1
{
    class C1
    {
        static void (ref int x)
        {
        $$}
    }
}", "N1.C1", 4);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingParameterList()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"namespace N1
{
    class C1
    {
        static void M1
        {
        $$}
    }
}", "N1.C1.M1", 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TopLevelField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"$$int f1;
", "f1", 0, new CSharpParseOptions(kind: SourceCodeKind.Script));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TopLevelMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"int M1(int x)
{
$$}
", "M1(int x)", 2, new CSharpParseOptions(kind: SourceCodeKind.Script));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.DebuggingLocationName)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TopLevelStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"

$$System.Console.WriteLine(""Hello"")
", null, 0, new CSharpParseOptions(kind: SourceCodeKind.Script));
        }
    }
}
