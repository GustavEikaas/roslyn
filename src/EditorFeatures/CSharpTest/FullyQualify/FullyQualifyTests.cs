// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.CodeFixes.FullyQualify;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.FullyQualify
{
    public class FullyQualifyTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (null, new CSharpFullyQualifyCodeFixProvider());

        protected override ImmutableArray<CodeAction> MassageActions(ImmutableArray<CodeAction> actions)
            => FlattenActions(actions);

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTypeFromMultipleNamespaces1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    [|IDictionary|] Method()
    {
        Goo();
    }
}",
@"class Class
{
    System.Collections.IDictionary Method()
    {
        Goo();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTypeFromMultipleNamespaces2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    [|IDictionary|] Method()
    {
        Goo();
    }
}",
@"class Class
{
    System.Collections.Generic.IDictionary Method()
    {
        Goo();
    }
}",
index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenericWithNoArgs()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    [|List|] Method()
    {
        Goo();
    }
}",
@"class Class
{
    System.Collections.Generic.List Method()
    {
        Goo();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenericWithCorrectArgs()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    [|List<int>|] Method()
    {
        Goo();
    }
}",
@"class Class
{
    System.Collections.Generic.List<int> Method()
    {
        Goo();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSmartTagDisplayText()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestSmartTagTextAsync(
@"class Class
{
    [|List<int>|] Method()
    {
        Goo();
    }
}",
"System.Collections.Generic.List");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenericWithWrongArgs()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Class
{
    [|List<int, string>|] Method()
    {
        Goo();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnVar1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"namespace N
{
    class var { }
}

class C
{
    void M()
    {
        [|var|]
    }
}
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnVar2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"namespace N
{
    class Bar { }
}

class C
{
    void M()
    {
        [|var|]
    }
}
");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenericInLocalDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Goo()
    {
        [|List<int>|] a = new List<int>();
    }
}",
@"class Class
{
    void Goo()
    {
        System.Collections.Generic.List<int> a = new List<int>();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenericItemType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Class
{
    List<[|Int32|]> l;
}",
@"using System.Collections.Generic;

class Class
{
    List<System.Int32> l;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateWithExistingUsings()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class Class
{
    [|List<int>|] Method()
    {
        Goo();
    }
}",
@"using System;

class Class
{
    System.Collections.Generic.List<int> Method()
    {
        Goo();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateInNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace N
{
    class Class
    {
        [|List<int>|] Method()
        {
            Goo();
        }
    }
}",
@"namespace N
{
    class Class
    {
        System.Collections.Generic.List<int> Method()
        {
            Goo();
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateInNamespaceWithUsings()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace N
{
    using System;

    class Class
    {
        [|List<int>|] Method()
        {
            Goo();
        }
    }
}",
@"namespace N
{
    using System;

    class Class
    {
        System.Collections.Generic.List<int> Method()
        {
            Goo();
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExistingUsing()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestActionCountAsync(
@"using System.Collections.Generic;

class Class
{
    [|IDictionary|] Method()
    {
        Goo();
    }
}",
count: 1);

            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Class
{
    [|IDictionary|] Method()
    {
        Goo();
    }
}",
@"using System.Collections.Generic;

class Class
{
    System.Collections.IDictionary Method()
    {
        Goo();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingIfUniquelyBound()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Class
{
    [|String|] Method()
    {
        Goo();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingIfUniquelyBoundGeneric()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Class
{
    [|List<int>|] Method()
    {
        Goo();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnEnum()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Goo()
    {
        var a = [|Colors|].Red;
    }
}

namespace A
{
    enum Colors
    {
        Red,
        Green,
        Blue
    }
}",
@"class Class
{
    void Goo()
    {
        var a = A.Colors.Red;
    }
}

namespace A
{
    enum Colors
    {
        Red,
        Green,
        Blue
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnClassInheritance()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class : [|Class2|]
{
}

namespace A
{
    class Class2
    {
    }
}",
@"class Class : A.Class2
{
}

namespace A
{
    class Class2
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnImplementedInterface()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class : [|IGoo|]
{
}

namespace A
{
    interface IGoo
    {
    }
}",
@"class Class : A.IGoo
{
}

namespace A
{
    interface IGoo
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAllInBaseList()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class : [|IGoo|], Class2
{
}

namespace A
{
    class Class2
    {
    }
}

namespace B
{
    interface IGoo
    {
    }
}",
@"class Class : B.IGoo, Class2
{
}

namespace A
{
    class Class2
    {
    }
}

namespace B
{
    interface IGoo
    {
    }
}");

            await TestInRegularAndScriptAsync(
@"class Class : B.IGoo, [|Class2|]
{
}

namespace A
{
    class Class2
    {
    }
}

namespace B
{
    interface IGoo
    {
    }
}",
@"class Class : B.IGoo, A.Class2
{
}

namespace A
{
    class Class2
    {
    }
}

namespace B
{
    interface IGoo
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAttributeUnexpanded()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"[[|Obsolete|]]
class Class
{
}",
@"[System.Obsolete]
class Class
{
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAttributeExpanded()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"[[|ObsoleteAttribute|]]
class Class
{
}",
@"[System.ObsoleteAttribute]
class Class
{
}");
        }

        [WorkItem(527360, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/527360")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExtensionMethods()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Goo
{
    void Bar()
    {
        var values = new List<int>() { 1, 2, 3 };
        values.[|Where|](i => i > 1);
    }
}");
        }

        [WorkItem(538018, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/538018")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterNew()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Goo()
    {
        List<int> l;
        l = new [|List<int>|]();
    }
}",
@"class Class
{
    void Goo()
    {
        List<int> l;
        l = new System.Collections.Generic.List<int>();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestArgumentsInMethodCall()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Test()
    {
        Console.WriteLine([|DateTime|].Today);
    }
}",
@"class Class
{
    void Test()
    {
        Console.WriteLine(System.DateTime.Today);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCallSiteArgs()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Test([|DateTime|] dt)
    {
    }
}",
@"class Class
{
    void Test(System.DateTime dt)
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUsePartialClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace A
{
    public class Class
    {
        [|PClass|] c;
    }
}

namespace B
{
    public partial class PClass
    {
    }
}",
@"namespace A
{
    public class Class
    {
        B.PClass c;
    }
}

namespace B
{
    public partial class PClass
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenericClassInNestedNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace A
{
    namespace B
    {
        class GenericClass<T>
        {
        }
    }
}

namespace C
{
    class Class
    {
        [|GenericClass<int>|] c;
    }
}",
@"namespace A
{
    namespace B
    {
        class GenericClass<T>
        {
        }
    }
}

namespace C
{
    class Class
    {
        A.B.GenericClass<int> c;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestBeforeStaticMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Test()
    {
        [|Math|].Sqrt();
    }",
@"class Class
{
    void Test()
    {
        System.Math.Sqrt();
    }");
        }

        [WorkItem(538136, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/538136")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestBeforeNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace A
{
    class Class
    {
        [|C|].Test t;
    }
}

namespace B
{
    namespace C
    {
        class Test
        {
        }
    }
}",
@"namespace A
{
    class Class
    {
        B.C.Test t;
    }
}

namespace B
{
    namespace C
    {
        class Test
        {
        }
    }
}");
        }

        [WorkItem(527395, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/527395")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSimpleNameWithLeadingTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class { void Test() { /*goo*/[|Int32|] i; } }",
@"class Class { void Test() { /*goo*/System.Int32 i; } }");
        }

        [WorkItem(527395, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/527395")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenericNameWithLeadingTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class { void Test() { /*goo*/[|List<int>|] l; } }",
@"class Class { void Test() { /*goo*/System.Collections.Generic.List<int> l; } }");
        }

        [WorkItem(538740, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/538740")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFullyQualifyTypeName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"public class Program
{
    public class Inner
    {
    }
}

class Test
{
    [|Inner|] i;
}",
@"public class Program
{
    public class Inner
    {
    }
}

class Test
{
    Program.Inner i;
}");
        }

        [WorkItem(26887, "https://github.com/dotnet/roslyn/issues/26887")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFullyQualifyUnboundIdentifier3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"public class Program
{
    public class Inner
    {
    }
}

class Test
{
    public [|Inner|] Name
}",
@"public class Program
{
    public class Inner
    {
    }
}

class Test
{
    public Program.Inner Name
}");
        }

        [WorkItem(538740, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/538740")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFullyQualifyTypeName_NotForGenericType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program<T>
{
    public class Inner
    {
    }
}

class Test
{
    [|Inner|] i;
}");
        }

        [WorkItem(538764, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/538764")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFullyQualifyThroughAlias()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using Alias = System;

class C
{
    [|Int32|] i;
}",
@"using Alias = System;

class C
{
    Alias.Int32 i;
}");
        }

        [WorkItem(538763, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/538763")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFullyQualifyPrioritizeTypesOverNamespaces1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace Outer
{
    namespace C
    {
        class C
        {
        }
    }
}

class Test
{
    [|C|] c;
}",
@"namespace Outer
{
    namespace C
    {
        class C
        {
        }
    }
}

class Test
{
    Outer.C.C c;
}");
        }

        [WorkItem(538763, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/538763")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFullyQualifyPrioritizeTypesOverNamespaces2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace Outer
{
    namespace C
    {
        class C
        {
        }
    }
}

class Test
{
    [|C|] c;
}",
@"namespace Outer
{
    namespace C
    {
        class C
        {
        }
    }
}

class Test
{
    Outer.C c;
}",
index: 1);
        }

        [WorkItem(539853, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539853")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task BugFix5950()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"using System.Console; WriteLine([|Expression|].Constant(123));",
@"using System.Console; WriteLine(System.Linq.Expressions.Expression.Constant(123));",
parseOptions: GetScriptOptions());
        }

        [WorkItem(540318, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/540318")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterAlias()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        System::[|Console|] :: WriteLine(""TEST"");
    }
}");
        }

        [WorkItem(540942, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/540942")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnIncompleteStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;
using System.IO;

class C
{
    static void Main(string[] args)
    {
        [|Path|] }
}");
        }

        [WorkItem(542643, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/542643")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAssemblyAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"[assembly: [|InternalsVisibleTo|](""Project"")]",
@"[assembly: System.Runtime.CompilerServices.InternalsVisibleTo(""Project"")]");
        }

        [WorkItem(543388, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/543388")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnAliasName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using [|GIBBERISH|] = Goo.GIBBERISH;

class Program
{
    static void Main(string[] args)
    {
        GIBBERISH x;
    }
}

namespace Goo
{
    public class GIBBERISH
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnAttributeOverloadResolutionError()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System.Runtime.InteropServices;

class M
{
    [[|DllImport|]()]
    static extern int? My();
}");
        }

        [WorkItem(544950, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/544950")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnAbstractConstructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System.IO;

class Program
{
    static void Main(string[] args)
    {
        var s = new [|Stream|]();
    }
}");
        }

        [WorkItem(545774, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/545774")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var input = @"[ assembly : [|Guid|] ( ""9ed54f84-a89d-4fcd-a854-44251e925f09"" ) ] ";
            await TestActionCountAsync(input, 1);

            await TestInRegularAndScriptAsync(
input,
@"[ assembly : System.Runtime.InteropServices.Guid( ""9ed54f84-a89d-4fcd-a854-44251e925f09"" ) ] ");
        }

        [WorkItem(546027, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546027")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyFromAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

[AttributeUsage(AttributeTargets.Class)]
class MyAttrAttribute : Attribute
{
}

[MyAttr(123, [|Version|] = 1)]
class D
{
}");
        }

        [WorkItem(775448, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/775448")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ShouldTriggerOnCS0308()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            // CS0308: The non-generic type 'A' cannot be used with type arguments
            await TestInRegularAndScriptAsync(
@"using System.Collections;

class Test
{
    static void Main(string[] args)
    {
        [|IEnumerable<int>|] f;
    }
}",
@"using System.Collections;

class Test
{
    static void Main(string[] args)
    {
        System.Collections.Generic.IEnumerable<int> f;
    }
}");
        }

        [WorkItem(947579, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/947579")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AmbiguousTypeFix()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using n1;
using n2;

class B
{
    void M1()
    {
        [|var a = new A();|]
    }
}

namespace n1
{
    class A
    {
    }
}

namespace n2
{
    class A
    {
    }
}",
@"using n1;
using n2;

class B
{
    void M1()
    {
        var a = new n1.A();
    }
}

namespace n1
{
    class A
    {
    }
}

namespace n2
{
    class A
    {
    }
}");
        }

        [WorkItem(995857, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/995857")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NonPublicNamespaces()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace MS.Internal.Xaml
{
    private class A
    {
    }
}

namespace System.Xaml
{
    public class A
    {
    }
}

public class Program
{
    static void M()
    {
        [|Xaml|]
    }
}",
@"namespace MS.Internal.Xaml
{
    private class A
    {
    }
}

namespace System.Xaml
{
    public class A
    {
    }
}

public class Program
{
    static void M()
    {
        System.Xaml
    }
}");

            await TestInRegularAndScriptAsync(
@"namespace MS.Internal.Xaml
{
    public class A
    {
    }
}

namespace System.Xaml
{
    public class A
    {
    }
}

public class Program
{
    static void M()
    {
        [|Xaml|]
    }
}",
@"namespace MS.Internal.Xaml
{
    public class A
    {
    }
}

namespace System.Xaml
{
    public class A
    {
    }
}

public class Program
{
    static void M()
    {
        MS.Internal.Xaml
    }
}", index: 1);
        }

        [WorkItem(11071, "https://github.com/dotnet/roslyn/issues/11071")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AmbiguousFixOrdering()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using n1;
using n2;

[[|Inner|].C]
class B
{
}

namespace n1
{
    namespace Inner
    {
    }
}

namespace n2
{
    namespace Inner
    {
        class CAttribute
        {
        }
    }
}",
@"using n1;
using n2;

[n2.Inner.C]
class B
{
}

namespace n1
{
    namespace Inner
    {
    }
}

namespace n2
{
    namespace Inner
    {
        class CAttribute
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleTest()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    ([|IDictionary|], string) Method()
    {
        Goo();
    }
}",
@"class Class
{
    (System.Collections.IDictionary, string) Method()
    {
        Goo();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleWithOneName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    ([|IDictionary|] a, string) Method()
    {
        Goo();
    }
}",
@"class Class
{
    (System.Collections.IDictionary a, string) Method()
    {
        Goo();
    }
}");
        }

        [WorkItem(18275, "https://github.com/dotnet/roslyn/issues/18275")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestContextualKeyword1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
namespace N
{
    class nameof
    {
    }
}

class C
{
    void M()
    {
        [|nameof|]
    }
}");
        }

        [WorkItem(18623, "https://github.com/dotnet/roslyn/issues/18623")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDoNotQualifyToTheSameTypeToFixWrongArity()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"
using System.Collections.Generic;

class Program : [|IReadOnlyCollection|]
{
}");
        }

        [WorkItem(19575, "https://github.com/dotnet/roslyn/issues/19575")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsFullyQualify)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNoNonGenericsWithGenericCodeParsedAsExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    private void GetEvaluationRuleNames()
    {
        [|IEnumerable|] < Int32 >
        return ImmutableArray.CreateRange();
    }
}";
            await TestActionCountAsync(code, count: 1);

            await TestInRegularAndScriptAsync(
code,
@"
class C
{
    private void GetEvaluationRuleNames()
    {
        System.Collections.Generic.IEnumerable < Int32 >
        return ImmutableArray.CreateRange();
    }
}");
        }
    }
}
