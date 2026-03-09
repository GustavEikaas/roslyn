// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.MakeFieldReadonly;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.MakeFieldReadonly
{
    public class MakeFieldReadonlyTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (new CSharpMakeFieldReadonlyDiagnosticAnalyzer(), new CSharpMakeFieldReadonlyCodeFixProvider());

        [Theory, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
        [InlineData("public")]
        [InlineData("internal")]
        [InlineData("protected")]
        [InlineData("protected internal")]
        [InlineData("private protected")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NonPrivateField(string accessibility)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
$@"class MyClass
{{
    {accessibility} int[| _goo |];
}}");
        }
        
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldIsEvent()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class MyClass
{
    private event System.EventHandler [|Goo|];
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldIsReadonly()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class MyClass
{
    private readonly int [|_goo|];
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldIsConst()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class MyClass
{
    private const int [|_goo|];
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldNotAssigned()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];
}",
@"class MyClass
{
    private readonly int _goo;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldNotAssigned_Struct()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"struct MyStruct
{
    private int [|_goo|];
}",
@"struct MyStruct
{
    private readonly int _goo;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInline()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|] = 0;
}",
@"class MyClass
{
    private readonly int _goo = 0;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MultipleFieldsAssignedInline_AllCanBeReadonly()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|] = 0, _bar = 0;
}",
@"class MyClass
{
    private readonly int _goo = 0;
    private int _bar = 0;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ThreeFieldsAssignedInline_AllCanBeReadonly_SeparatesAllAndKeepsThemInOrder()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class MyClass
{
    private int _goo = 0, [|_bar|] = 0, _fizz = 0;
}",
@"class MyClass
{
    private int _goo = 0;
    private readonly int _bar = 0;
    private int _fizz = 0;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MultipleFieldsAssignedInline_OneIsAssignedInMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class MyClass
{
    private int _goo = 0, [|_bar|] = 0;
    Goo()
    {
        _goo = 0;
    }
}",
@"class MyClass
{
    private int _goo = 0;
    private readonly int _bar = 0;

    Goo()
    {
        _goo = 0;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MultipleFieldsAssignedInline_NoInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|], _bar = 0;
}",
@"class MyClass
{
    private readonly int _goo;
    private int _bar = 0;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInCtor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];
    MyClass()
    {
        _goo = 0;
    }
}",
@"class MyClass
{
    private readonly int _goo;
    MyClass()
    {
        _goo = 0;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInSimpleLambdaInCtor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"public class MyClass
{
    private int [|_goo|];
    public MyClass()
    {
        this.E = x => this._goo = 0;
    }

    public Action<int> E;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInLambdaInCtor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"public class MyClass
{
    private int [|_goo|];
    public MyClass()
    {
        this.E += (_, __) => this._goo = 0;
    }

    public event EventHandler E;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInLambdaWithBlockInCtor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"public class MyClass
{
    private int [|_goo|];
    public MyClass()
    {
        this.E += (_, __) => { this._goo = 0; }
    }

    public event EventHandler E;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInAnonymousFunctionInCtor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"public class MyClass
{
    private int [|_goo|];
    public MyClass()
    {
        this.E = delegate { this._goo = 0; };
    }

    public Action<int> E;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInLocalFunctionExpressionBodyInCtor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"public class MyClass
{
    private int [|_goo|];
    public MyClass()
    {
        void LocalFunction() => this._goo = 0;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInLocalFunctionBlockBodyInCtor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"public class MyClass
{
    private int [|_goo|];
    public MyClass()
    {
        void LocalFunction() { this._goo = 0; }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInCtor_DifferentInstance()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];
    MyClass()
    {
        var goo = new MyClass();
        goo._goo = 0;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInCtor_DifferentInstance_ObjectInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];
    MyClass()
    {
        var goo = new MyClass { _goo = 0 };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInCtor_QualifiedWithThis()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];
    MyClass()
    {
        this._goo = 0;
    }
}",
@"class MyClass
{
    private readonly int _goo;
    MyClass()
    {
        this._goo = 0;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldReturnedInProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];
    int Goo
    {
        get { return _goo; }
    }
}",
@"class MyClass
{
    private readonly int _goo;
    int Goo
    {
        get { return _goo; }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];
    int Goo
    {
        get { return _goo; }
        set { _goo = value; }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];
    int Goo()
    {
        _goo = 0;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInNestedTypeConstructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];

    class Derived : MyClass
    {
        Derived()
        {
            _goo = 1;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInNestedTypeMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];

    class Derived : MyClass
    {
        void Method()
        {
            _goo = 1;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task VariableAssignedToFieldInMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];
    int Goo()
    {
        var i = _goo;
    }
}",
@"class MyClass
{
    private readonly int _goo;
    int Goo()
    {
        var i = _goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInMethodWithCompoundOperator()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|] = 0;
    int Goo(int value)
    {
        _goo += value;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldUsedWithPostfixIncrement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|] = 0;
    int Goo(int value)
    {
        _goo++;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldUsedWithPrefixDecrement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|] = 0;
    int Goo(int value)
    {
        --_goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AssignedInPartialClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"partial class MyClass
{
    private int [|_goo|];
}
partial class MyClass
{
    void SetGoo()
    {
        _goo = 0;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PassedAsParameter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];
    void Goo()
    {
        Bar(_goo);
    }
    void Bar(int goo)
    {
    }
}",
@"class MyClass
{
    private readonly int _goo;
    void Goo()
    {
        Bar(_goo);
    }
    void Bar(int goo)
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PassedAsOutParameter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];
    void Goo()
    {
        int.TryParse(""123"", out _goo);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PassedAsRefParameter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];
    void Goo()
    {
        Bar(ref _goo);
    }
    void Bar(ref int goo)
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PassedAsOutParameterInCtor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];
    MyClass()
    {
        int.TryParse(""123"", out _goo);
    }
}",
@"class MyClass
{
    private readonly int _goo;
    MyClass()
    {
        int.TryParse(""123"", out _goo);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PassedAsRefParameterInCtor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];
    MyClass()
    {
        Bar(ref _goo);
    }
    void Bar(ref int goo)
    {
    }
}",
@"class MyClass
{
    private readonly int _goo;
    MyClass()
    {
        Bar(ref _goo);
    }
    void Bar(ref int goo)
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StaticFieldAssignedInStaticCtor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class MyClass
{
    private static int [|_goo|];
    static MyClass()
    {
        _goo = 0;
    }
}",
@"class MyClass
{
    private static readonly int _goo;
    static MyClass()
    {
        _goo = 0;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StaticFieldAssignedInNonStaticCtor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class MyClass
{
    private static int [|_goo|];
    MyClass()
    {
        _goo = 0;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldTypeIsMutableStruct()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"struct MyStruct
{
    private int _goo;
}
class MyClass
{
    private MyStruct [|_goo|];
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldTypeIsCustomImmutableStruct()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"struct MyStruct
{
    private readonly int _goo;
    private const int _bar = 0;
    private static int _fizz;
}
class MyClass
{
    private MyStruct [|_goo|];
}",
@"struct MyStruct
{
    private readonly int _goo;
    private const int _bar = 0;
    private static int _fizz;
}
class MyClass
{
    private readonly MyStruct _goo;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FixAll()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class MyClass
{
    private int {|FixAllInDocument:_goo|} = 0, _bar = 0;
    private int _x = 0, _y = 0, _z = 0;
    private int _fizz = 0;

    void Method() { _z = 1; }
}",
@"class MyClass
{
    private readonly int _goo = 0, _bar = 0;
    private readonly int _x = 0;
    private readonly int _y = 0;
    private int _z = 0;
    private readonly int _fizz = 0;

    void Method() { _z = 1; }
}");
        }

        // Remove this test when https://github.com/dotnet/roslyn/issues/25652 is fixed
        [Fact]
        [Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FixAllDoesNotSupportPartial()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"  partial struct MyClass
    {
        private static Func<int, bool> {|FixAllInDocument:_test1|} = x => x > 0;
        private static Func<int, bool> _test2 = x => x < 0;

        private static Func<int, bool> _test3 = x =>
        {
            return x == 0;
        };

        private static Func<int, bool> _test4 = x =>
        {
            return x != 0;
        };
    }

    partial struct MyClass { }");
        }

        [Fact(Skip = "Partial types not yet supported: https://github.com/dotnet/roslyn/issues/25652")]
        [Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FixAll2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"  partial struct MyClass
    {
        private static Func<int, bool> {|FixAllInDocument:_test1|} = x => x > 0;
        private static Func<int, bool> _test2 = x => x < 0;

        private static Func<int, bool> _test3 = x =>
        {
            return x == 0;
        };

        private static Func<int, bool> _test4 = x =>
        {
            return x != 0;
        };
    }

    partial struct MyClass { }",
@"  partial struct MyClass
    {
        private static readonly Func<int, bool> _test1 = x => x > 0;
        private static readonly Func<int, bool> _test2 = x => x < 0;

        private static readonly Func<int, bool> _test3 = x =>
        {
            return x == 0;
        };

        private static readonly Func<int, bool> _test4 = x =>
        {
            return x != 0;
        };
    }

    partial struct MyClass { }");
        }

        [WorkItem(26262, "https://github.com/dotnet/roslyn/issues/26262")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInCtor_InParens()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];
    MyClass()
    {
        (_goo) = 0;
    }
}",
@"class MyClass
{
    private readonly int _goo;
    MyClass()
    {
        (_goo) = 0;
    }
}");
        }

        [WorkItem(26262, "https://github.com/dotnet/roslyn/issues/26262")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInCtor_QualifiedWithThis_InParens()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class MyClass
{
    private int [|_goo|];
    MyClass()
    {
        (this._goo) = 0;
    }
}",
@"class MyClass
{
    private readonly int _goo;
    MyClass()
    {
        (this._goo) = 0;
    }
}");
        }

        [WorkItem(26264, "https://github.com/dotnet/roslyn/issues/26264")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInMethod_InDeconstruction()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    [|int i;|]
    int j;

    void M()
    {
        (i, j) = (1, 2);
    }
}");
        }

        [WorkItem(26264, "https://github.com/dotnet/roslyn/issues/26264")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInMethod_InDeconstruction_InParens()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    [|int i;|]
    int j;

    void M()
    {
        ((i, j), j) = ((1, 2), 3);
    }
}");
        }

        [WorkItem(26264, "https://github.com/dotnet/roslyn/issues/26264")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldAssignedInMethod_InDeconstruction_WithThis_InParens()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"class C
{
    [|int i;|]
    int j;

    void M()
    {
        ((this.i, j), j) = (1, 2);
    }
}");
        }

        [WorkItem(26264, "https://github.com/dotnet/roslyn/issues/26264")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsMakeFieldReadonly)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldUsedInTupleExpressionOnRight()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    [|int i;|]
    int j;

    void M()
    {
        (j, j) = (i, i);
    }
}",
@"class C
{
    readonly int i;
    int j;

    void M()
    {
        (j, j) = (i, i);
    }
}");
        }
    }
}
