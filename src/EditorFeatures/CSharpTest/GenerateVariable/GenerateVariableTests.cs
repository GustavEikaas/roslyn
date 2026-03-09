// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.CodeAnalysis.CSharp.CodeStyle;
using Microsoft.CodeAnalysis.CSharp.GenerateVariable;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.GenerateVariable
{
    public class GenerateVariableTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        private const int ReadonlyFieldIndex = 1;
        private const int PropertyIndex = 2;
        private const int LocalIndex = 3;

        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (null, new CSharpGenerateVariableCodeFixProvider());

        private readonly CodeStyleOption<bool> onWithInfo = new CodeStyleOption<bool>(true, NotificationOption.Suggestion);

        // specify all options explicitly to override defaults.
        private IDictionary<OptionKey, object> ImplicitTypingEverywhere() => OptionsSet(
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeWherePossible, onWithInfo),
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeWhereApparent, onWithInfo),
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeForIntrinsicTypes, onWithInfo));

        internal IDictionary<OptionKey, object> OptionSet(OptionKey option, object value)
        {
            var options = new Dictionary<OptionKey, object>();
            options.Add(option, value);
            return options;
        }

        protected override ImmutableArray<CodeAction> MassageActions(ImmutableArray<CodeAction> actions)
            => FlattenActions(actions);

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSimpleLowercaseIdentifier1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|goo|];
    }
}",
@"class Class
{
    private object goo;

    void Method()
    {
        goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSimpleLowercaseIdentifier2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|goo|];
    }
}",
@"class Class
{
    private readonly object goo;

    void Method()
    {
        goo;
    }
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTestSimpleLowercaseIdentifier3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|goo|];
    }
}",
@"class Class
{
    public object goo { get; private set; }

    void Method()
    {
        goo;
    }
}",
index: PropertyIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSimpleUppercaseIdentifier1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|Goo|];
    }
}",
@"class Class
{
    public object Goo { get; private set; }

    void Method()
    {
        Goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSimpleUppercaseIdentifier2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|Goo|];
    }
}",
@"class Class
{
    private object Goo;

    void Method()
    {
        Goo;
    }
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSimpleUppercaseIdentifier3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|Goo|];
    }
}",
@"class Class
{
    private readonly object Goo;

    void Method()
    {
        Goo;
    }
}",
index: PropertyIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSimpleRead1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method(int i)
    {
        Method([|goo|]);
    }
}",
@"class Class
{
    private int goo;

    void Method(int i)
    {
        Method(goo);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSimpleWriteCount()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestExactActionSetOfferedAsync(
@"class Class
{
    void Method(int i)
    {
        [|goo|] = 1;
    }
}",
new[] { string.Format(FeaturesResources.Generate_field_1_0, "goo", "Class"), string.Format(FeaturesResources.Generate_property_1_0, "goo", "Class"), string.Format(FeaturesResources.Generate_local_0, "goo") });
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSimpleWrite1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method(int i)
    {
        [|goo|] = 1;
    }
}",
@"class Class
{
    private int goo;

    void Method(int i)
    {
        goo = 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSimpleWrite2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method(int i)
    {
        [|goo|] = 1;
    }
}",
@"class Class
{
    public int goo { get; private set; }

    void Method(int i)
    {
        goo = 1;
    }
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInRef()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method(ref int i)
    {
        Method(ref this.[|goo|]);
    }
}",
@"class Class
{
    private int goo;

    void Method(ref int i)
    {
        Method(ref this.[|goo|]);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyInRef()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System;
class Class
{
    void Method(ref int i)
    {
        Method(ref this.[|goo|]);
    }
}",
@"
using System;
class Class
{
    public ref int goo => throw new NotImplementedException();

    void Method(ref int i)
    {
        Method(ref this.goo);
    }
}", index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyInIn()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System;
class Class
{
    void Method(in int i)
    {
        Method(in this.[|goo|]);
    }
}",
@"
using System;
class Class
{
    public ref readonly int goo => throw new NotImplementedException();

    void Method(in int i)
    {
        Method(in this.goo);
    }
}", index: PropertyIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInRef1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method(ref int i)
    {
        Method(ref [|goo|]);
    }
}",
@"class Class
{
    private int goo;

    void Method(ref int i)
    {
        Method(ref goo);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInOutCodeActionCount()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestExactActionSetOfferedAsync(
@"class Class
{
    void Method(out int i)
    {
        Method(out [|goo|]);
    }
}",
new[] { string.Format(FeaturesResources.Generate_field_1_0, "goo", "Class"), string.Format(FeaturesResources.Generate_local_0, "goo") });
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInOut1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method(out int i)
    {
        Method(out [|goo|]);
    }
}",
@"class Class
{
    private int goo;

    void Method(out int i)
    {
        Method(out goo);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateInStaticMember1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    static void Method()
    {
        [|goo|];
    }
}",
@"class Class
{
    private static object goo;

    static void Method()
    {
        goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateInStaticMember2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    static void Method()
    {
        [|goo|];
    }
}",
@"class Class
{
    private static readonly object goo;

    static void Method()
    {
        goo;
    }
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateInStaticMember3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    static void Method()
    {
        [|goo|];
    }
}",
@"class Class
{
    public static object goo { get; private set; }

    static void Method()
    {
        goo;
    }
}",
index: PropertyIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateOffInstance1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        this.[|goo|];
    }
}",
@"class Class
{
    private object goo;

    void Method()
    {
        this.goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateOffInstance2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        this.[|goo|];
    }
}",
@"class Class
{
    private readonly object goo;

    void Method()
    {
        this.goo;
    }
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateOffInstance3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        this.[|goo|];
    }
}",
@"class Class
{
    public object goo { get; private set; }

    void Method()
    {
        this.goo;
    }
}",
index: PropertyIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateOffWrittenInstance1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        this.[|goo|] = 1;
    }
}",
@"class Class
{
    private int goo;

    void Method()
    {
        this.goo = 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateOffWrittenInstance2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        this.[|goo|] = 1;
    }
}",
@"class Class
{
    public int goo { get; private set; }

    void Method()
    {
        this.goo = 1;
    }
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateOffStatic1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        Class.[|goo|];
    }
}",
@"class Class
{
    private static object goo;

    void Method()
    {
        Class.goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateOffStatic2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        Class.[|goo|];
    }
}",
@"class Class
{
    private static readonly object goo;

    void Method()
    {
        Class.goo;
    }
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateOffStatic3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        Class.[|goo|];
    }
}",
@"class Class
{
    public static object goo { get; private set; }

    void Method()
    {
        Class.goo;
    }
}",
index: PropertyIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateOffWrittenStatic1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        Class.[|goo|] = 1;
    }
}",
@"class Class
{
    private static int goo;

    void Method()
    {
        Class.goo = 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateOffWrittenStatic2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        Class.[|goo|] = 1;
    }
}",
@"class Class
{
    public static int goo { get; private set; }

    void Method()
    {
        Class.goo = 1;
    }
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateInstanceIntoSibling1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        new D().[|goo|];
    }
}

class D
{
}",
@"class Class
{
    void Method()
    {
        new D().goo;
    }
}

class D
{
    internal object goo;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateInstanceIntoOuter1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Outer
{
    class Class
    {
        void Method()
        {
            new Outer().[|goo|];
        }
    }
}",
@"class Outer
{
    private object goo;

    class Class
    {
        void Method()
        {
            new Outer().goo;
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateInstanceIntoDerived1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class : Base
{
    void Method(Base b)
    {
        b.[|goo|];
    }
}

class Base
{
}",
@"class Class : Base
{
    void Method(Base b)
    {
        b.goo;
    }
}

class Base
{
    internal object goo;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateStaticIntoDerived1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class : Base
{
    void Method(Base b)
    {
        Base.[|goo|];
    }
}

class Base
{
}",
@"class Class : Base
{
    void Method(Base b)
    {
        Base.goo;
    }
}

class Base
{
    protected static object goo;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateIntoInterfaceFixCount()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestActionCountAsync(
@"class Class
{
    void Method(I i)
    {
        i.[|goo|];
    }
}

interface I
{
}",
count: 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateIntoInterface1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method(I i)
    {
        i.[|Goo|];
    }
}

interface I
{
}",
@"class Class
{
    void Method(I i)
    {
        i.Goo;
    }
}

interface I
{
    object Goo { get; set; }
}", index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateIntoInterface2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method(I i)
    {
        i.[|Goo|];
    }
}

interface I
{
}",
@"class Class
{
    void Method(I i)
    {
        i.Goo;
    }
}

interface I
{
    object Goo { get; }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateStaticIntoInterfaceMissing()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Class
{
    void Method(I i)
    {
        I.[|Goo|];
    }
}

interface I
{
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateWriteIntoInterfaceFixCount()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestActionCountAsync(
@"class Class
{
    void Method(I i)
    {
        i.[|Goo|] = 1;
    }
}

interface I
{
}",
count: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateWriteIntoInterface1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method(I i)
    {
        i.[|Goo|] = 1;
    }
}

interface I
{
}",
@"class Class
{
    void Method(I i)
    {
        i.Goo = 1;
    }
}

interface I
{
    int Goo { get; set; }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateInGenericType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class<T>
{
    void Method(T t)
    {
        [|goo|] = t;
    }
}",
@"class Class<T>
{
    private T goo;

    void Method(T t)
    {
        goo = t;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateInGenericMethod1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method<T>(T t)
    {
        [|goo|] = t;
    }
}",
@"class Class
{
    private object goo;

    void Method<T>(T t)
    {
        goo = t;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateInGenericMethod2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method<T>(IList<T> t)
    {
        [|goo|] = t;
    }
}",
@"class Class
{
    private IList<object> goo;

    void Method<T>(IList<T> t)
    {
        goo = t;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldBeforeFirstField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    int i;

    void Method()
    {
        [|goo|];
    }
}",
@"class Class
{
    int i;
    private object goo;

    void Method()
    {
        goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldAfterLastField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|goo|];
    }

    int i;
}",
@"class Class
{
    void Method()
    {
        goo;
    }

    int i;
    private object goo;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyAfterLastField1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    int Bar;

    void Method()
    {
        [|Goo|];
    }
}",
@"class Class
{
    int Bar;

    public object Goo { get; private set; }

    void Method()
    {
        Goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyAfterLastField2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|Goo|];
    }

    int Bar;
}",
@"class Class
{
    void Method()
    {
        Goo;
    }

    int Bar;

    public object Goo { get; private set; }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyBeforeFirstProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    int Quux { get; }

    void Method()
    {
        [|Goo|];
    }
}",
@"class Class
{
    public object Goo { get; private set; }
    int Quux { get; }

    void Method()
    {
        Goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyBeforeFirstPropertyEvenWithField1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    int Bar;

    int Quux { get; }

    void Method()
    {
        [|Goo|];
    }
}",
@"class Class
{
    int Bar;

    public object Goo { get; private set; }
    int Quux { get; }

    void Method()
    {
        Goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyAfterLastPropertyEvenWithField2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    int Quux { get; }

    int Bar;

    void Method()
    {
        [|Goo|];
    }
}",
@"class Class
{
    int Quux { get; }
    public object Goo { get; private set; }

    int Bar;

    void Method()
    {
        Goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingInInvocation()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|Goo|]();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingInObjectCreation()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        new [|Goo|]();
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingInTypeDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|A|] a;
    }
}");

            await TestMissingInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|A.B|] a;
    }
}");

            await TestMissingInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|A|].B a;
    }
}");

            await TestMissingInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        A.[|B|] a;
    }
}");

            await TestMissingInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|A.B.C|] a;
    }
}");

            await TestMissingInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|A.B|].C a;
    }
}");

            await TestMissingInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        A.B.[|C|] a;
    }
}");

            await TestMissingInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|A|].B.C a;
    }
}");

            await TestMissingInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        A.[|B|].C a;
    }
}");
        }

        [WorkItem(539336, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539336")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingInAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"[[|A|]]
class Class
{
}");

            await TestMissingInRegularAndScriptAsync(
@"[[|A.B|]]
class Class
{
}");

            await TestMissingInRegularAndScriptAsync(
@"[[|A|].B]
class Class
{
}");

            await TestMissingInRegularAndScriptAsync(
@"[A.[|B|]]
class Class
{
}");

            await TestMissingInRegularAndScriptAsync(
@"[[|A.B.C|]]
class Class
{
}");

            await TestMissingInRegularAndScriptAsync(
@"[[|A.B|].C]
class Class
{
}");

            await TestMissingInRegularAndScriptAsync(
@"[A.B.[|C|]]
class Class
{
}");

            await TestMissingInRegularAndScriptAsync(
@"[[|A|].B.C]
class Class
{
}");

            await TestMissingInRegularAndScriptAsync(
@"[A.B.[|C|]]
class Class
{
}");
        }

        [WorkItem(539340, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539340")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSpansField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestSpansAsync(
@"class C
{
    void M()
    {
        this.[|Goo|] }");

            await TestSpansAsync(
@"class C
{
    void M()
    {
        this.[|Goo|];
    }");

            await TestSpansAsync(
@"class C
{
    void M()
    {
        this.[|Goo|] = 1 }");

            await TestSpansAsync(
@"class C
{
    void M()
    {
        this.[|Goo|] = 1 + 2 }");

            await TestSpansAsync(
@"class C
{
    void M()
    {
        this.[|Goo|] = 1 + 2;
    }");

            await TestSpansAsync(
@"class C
{
    void M()
    {
        this.[|Goo|] += Bar() }");

            await TestSpansAsync(
@"class C
{
    void M()
    {
        this.[|Goo|] += Bar();
    }");
        }

        [WorkItem(539427, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539427")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFromLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method(int i)
    {
        [|goo|] = () => {
            return 2 };
    }
}",
@"using System;

class Class
{
    private Func<int> goo;

    void Method(int i)
    {
        goo = () => {
            return 2 };
    }
}");
        }

        // TODO: Move to TypeInferrer.InferTypes, or something
        [WorkItem(539466, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539466")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateInMethodOverload1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method(int i)
    {
        System.Console.WriteLine([|goo|]);
    }
}",
@"class Class
{
    private bool goo;

    void Method(int i)
    {
        System.Console.WriteLine(goo);
    }
}");
        }

        // TODO: Move to TypeInferrer.InferTypes, or something
        [WorkItem(539466, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539466")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateInMethodOverload2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method(int i)
    {
        System.Console.WriteLine(this.[|goo|]);
    }
}",
@"class Class
{
    private bool goo;

    void Method(int i)
    {
        System.Console.WriteLine(this.goo);
    }
}");
        }

        [WorkItem(539468, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539468")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExplicitProperty1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class : ITest
{
    bool ITest.[|SomeProp|] { get; set; }
}

interface ITest
{
}",
@"class Class : ITest
{
    bool ITest.SomeProp { get; set; }
}

interface ITest
{
    bool SomeProp { get; set; }
}");
        }

        [WorkItem(539468, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539468")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExplicitProperty2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class : ITest
{
    bool ITest.[|SomeProp|] { }
}

interface ITest
{
}",
@"class Class : ITest
{
    bool ITest.SomeProp { }
}

interface ITest
{
    bool SomeProp { get; set; }
}", index: ReadonlyFieldIndex);
        }

        [WorkItem(539468, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539468")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExplicitProperty3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class : ITest
{
    bool ITest.[|SomeProp|] { }
}

interface ITest
{
}",
@"class Class : ITest
{
    bool ITest.SomeProp { }
}

interface ITest
{
    bool SomeProp { get; }
}");
        }

        [WorkItem(539468, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539468")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExplicitProperty4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Class
{
    bool ITest.[|SomeProp|] { }
}

interface ITest
{
}");
        }

        [WorkItem(539468, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539468")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExplicitProperty5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Class : ITest
{
    bool ITest.[|SomeProp|] { }
}

interface ITest
{
    bool SomeProp { get; }
}");
        }

        [WorkItem(539489, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539489")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEscapedName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|@goo|];
    }
}",
@"class Class
{
    private object goo;

    void Method()
    {
        @goo;
    }
}");
        }

        [WorkItem(539489, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539489")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEscapedKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|@int|];
    }
}",
@"class Class
{
    private object @int;

    void Method()
    {
        @int;
    }
}");
        }

        [WorkItem(539529, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539529")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRefLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|test|] = (ref int x) => x = 10;
    }
}",
@"class Class
{
    private object test;

    void Method()
    {
        test = (ref int x) => x = 10;
    }
}");
        }

        [WorkItem(539595, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539595")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnError()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Class
{
    void F<U, V>(U u1, V v1)
    {
        Goo<string, int>([|u1|], u2);
    }
}");
        }

        [WorkItem(539571, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539571")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNameSimplification()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace TestNs
{
    class Program
    {
        class Test
        {
            void Meth()
            {
                Program.[|blah|] = new Test();
            }
        }
    }
}",
@"namespace TestNs
{
    class Program
    {
        private static Test blah;

        class Test
        {
            void Meth()
            {
                Program.blah = new Test();
            }
        }
    }
}");
        }

        [WorkItem(539717, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539717")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPostIncrement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    static void Main(string[] args)
    {
        [|i|]++;
    }
}",
@"class Program
{
    private static int i;

    static void Main(string[] args)
    {
        i++;
    }
}");
        }

        [WorkItem(539717, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539717")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPreDecrement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    static void Main(string[] args)
    {
        --[|i|];
    }
}",
@"class Program
{
    private static int i;

    static void Main(string[] args)
    {
        --i;
    }
}");
        }

        [WorkItem(539738, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539738")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateIntoScript()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"using C;

static class C
{
}

C.[|i|] ++ ;",
@"using C;

static class C
{
    internal static int i;
}

C.i ++ ;",
parseOptions: Options.Script);
        }

        [WorkItem(539558, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539558")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task BugFix5565()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        [|Goo|]#();
    }
}",
@"using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    public static object Goo { get; private set; }

    static void Main(string[] args)
    {
        Goo#();
    }
}");
        }

        [WorkItem(539536, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539536")]
        [Fact(Skip = "Tuples"), Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task BugFix5538()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        new([|goo|])();
    }
}",
@"using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    public static object goo { get; private set; }

    static void Main(string[] args)
    {
        new(goo)();
    }
}",
index: PropertyIndex);
        }

        [WorkItem(539665, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539665")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task BugFix5697()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C { }
class D
{
    void M()
    {
        C.[|P|] = 10;
    }
}
",
@"class C
{
    public static int P { get; internal set; }
}
class D
{
    void M()
    {
        C.P = 10;
    }
}
");
        }

        [WorkItem(539793, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539793")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIncrement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestExactActionSetOfferedAsync(
@"class Program
{
    static void Main()
    {
        [|p|]++;
    }
}",
new[] { string.Format(FeaturesResources.Generate_field_1_0, "p", "Program"), string.Format(FeaturesResources.Generate_property_1_0, "p", "Program"), string.Format(FeaturesResources.Generate_local_0, "p") });

            await TestInRegularAndScriptAsync(
@"class Program
{
    static void Main()
    {
        [|p|]++;
    }
}",
@"class Program
{
    private static int p;

    static void Main()
    {
        p++;
    }
}");
        }

        [WorkItem(539834, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539834")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInGoto()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    static void Main()
    {
        goto [|goo|];
    }
}");
        }

        [WorkItem(539826, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539826")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnLeftOfDot()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    static void Main()
    {
        [|goo|].ToString();
    }
}",
@"class Program
{
    private static object goo;

    static void Main()
    {
        goo.ToString();
    }
}");
        }

        [WorkItem(539840, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539840")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotBeforeAlias()
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
        [|global|]::System.String s;
    }
}");
        }

        [WorkItem(539871, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539871")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnGenericName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C<T>
{
    public delegate void Goo<R>(R r);

    static void M()
    {
        Goo<T> r = [|Goo<T>|];
    }
}");
        }

        [WorkItem(539934, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539934")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnDelegateAddition()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class C
{
    delegate void D();

    void M()
    {
        D d = [|M1|] + M2;
    }
}",
@"class C
{
    private D M1 { get; set; }

    delegate void D();

    void M()
    {
        D d = M1 + M2;
    }
}",
parseOptions: null);
        }

        [WorkItem(539986, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539986")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestReferenceTypeParameter1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C<T>
{
    public void Test()
    {
        C<T> c = A.[|M|];
    }
}

class A
{
}",
@"class C<T>
{
    public void Test()
    {
        C<T> c = A.M;
    }
}

class A
{
    public static C<object> M { get; internal set; }
}");
        }

        [WorkItem(539986, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539986")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestReferenceTypeParameter2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C<T>
{
    public void Test()
    {
        C<T> c = A.[|M|];
    }

    class A
    {
    }
}",
@"class C<T>
{
    public void Test()
    {
        C<T> c = A.M;
    }

    class A
    {
        public static C<T> M { get; internal set; }
    }
}");
        }

        [WorkItem(540159, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/540159")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEmptyIdentifierName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    static void M()
    {
        int i = [|@|] }
}");
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    static void M()
    {
        int i = [|@|]}
}");
        }

        [WorkItem(541194, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541194")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestForeachVar()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        foreach (var v in [|list|])
        {
        }
    }
}",
@"using System.Collections.Generic;

class C
{
    private IEnumerable<object> list;

    void M()
    {
        foreach (var v in list)
        {
        }
    }
}");
        }

        [WorkItem(541265, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541265")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExtensionMethodUsedAsInstance()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"using System;

class C
{
    public static void Main()
    {
        string s = ""Hello"";
        [|f|] = s.ExtensionMethod;
    }
}

public static class MyExtension
{
    public static int ExtensionMethod(this String s)
    {
        return s.Length;
    }
}",
@"using System;

class C
{
    private static Func<int> f;

    public static void Main()
    {
        string s = ""Hello"";
        f = s.ExtensionMethod;
    }
}

public static class MyExtension
{
    public static int ExtensionMethod(this String s)
    {
        return s.Length;
    }
}",
parseOptions: null);
        }

        [WorkItem(541549, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541549")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDelegateInvoke()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class Program
{
    static void Main(string[] args)
    {
        Func<int, int> f = x => x + 1;
        f([|x|]);
    }
}",
@"using System;

class Program
{
    private static int x;

    static void Main(string[] args)
    {
        Func<int, int> f = x => x + 1;
        f(x);
    }
}");
        }

        [WorkItem(541597, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541597")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestComplexAssign1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    static void Main(string[] args)
    {
        [|a|] = a + 10;
    }
}",
@"class Program
{
    private static int a;

    static void Main(string[] args)
    {
        a = a + 10;
    }
}");
        }

        [WorkItem(541597, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541597")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestComplexAssign2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    static void Main(string[] args)
    {
        a = [|a|] + 10;
    }
}",
@"class Program
{
    private static int a;

    static void Main(string[] args)
    {
        a = a + 10;
    }
}");
        }

        [WorkItem(541659, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541659")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTypeNamedVar()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class Program
{
    public static void Main()
    {
        var v = [|p|];
    }
}

class var
{
}",
@"using System;

class Program
{
    private static var p;

    public static void Main()
    {
        var v = p;
    }
}

class var
{
}");
        }

        [WorkItem(541675, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541675")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestStaticExtensionMethodArgument()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class Program
{
    static void Main(string[] args)
    {
        MyExtension.ExMethod([|ss|]);
    }
}

static class MyExtension
{
    public static int ExMethod(this string s)
    {
        return s.Length;
    }
}",
@"using System;

class Program
{
    private static string ss;

    static void Main(string[] args)
    {
        MyExtension.ExMethod(ss);
    }
}

static class MyExtension
{
    public static int ExMethod(this string s)
    {
        return s.Length;
    }
}");
        }

        [WorkItem(539675, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539675")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AddBlankLineBeforeCommentBetweenMembers1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    //method
    static void Main(string[] args)
    {
        [|P|] = 10;
    }
}",
@"class Program
{
    public static int P { get; private set; }

    //method
    static void Main(string[] args)
    {
        P = 10;
    }
}");
        }

        [WorkItem(539675, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539675")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AddBlankLineBeforeCommentBetweenMembers2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    //method
    static void Main(string[] args)
    {
        [|P|] = 10;
    }
}",
@"class Program
{
    private static int P;

    //method
    static void Main(string[] args)
    {
        P = 10;
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(543813, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/543813")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AddBlankLineBetweenMembers1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    static void Main(string[] args)
    {
        [|P|] = 10;
    }
}",
@"class Program
{
    private static int P;

    static void Main(string[] args)
    {
        P = 10;
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(543813, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/543813")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AddBlankLineBetweenMembers2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    static void Main(string[] args)
    {
        [|P|] = 10;
    }
}",
@"class Program
{
    public static int P { get; private set; }

    static void Main(string[] args)
    {
        P = 10;
    }
}",
index: 0);
        }

        [WorkItem(543813, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/543813")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DontAddBlankLineBetweenFields()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    private static int P;

    static void Main(string[] args)
    {
        P = 10;
        [|A|] = 9;
    }
}",
@"class Program
{
    private static int P;
    private static int A;

    static void Main(string[] args)
    {
        P = 10;
        A = 9;
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(543813, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/543813")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DontAddBlankLineBetweenAutoProperties()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    public static int P { get; private set; }

    static void Main(string[] args)
    {
        P = 10;
        [|A|] = 9;
    }
}",
@"class Program
{
    public static int P { get; private set; }
    public static int A { get; private set; }

    static void Main(string[] args)
    {
        P = 10;
        A = 9;
    }
}",
index: 0);
        }

        [WorkItem(539665, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539665")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIntoEmptyClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C { }
class D
{
    void M()
    {
        C.[|P|] = 10;
    }
}",
@"class C
{
    public static int P { get; internal set; }
}
class D
{
    void M()
    {
        C.P = 10;
    }
}");
        }

        [WorkItem(540595, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/540595")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyInScript()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"[|Goo|]",
@"object Goo { get; private set; }

Goo",
parseOptions: Options.Script);
        }

        [WorkItem(542535, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/542535")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstantInParameterValue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string Initial =
@"class C
{   
    const int y = 1 ; 
    public void Goo ( bool x = [|undeclared|] ) { }
} ";

            await TestActionCountAsync(
Initial,
count: 1);

            await TestInRegularAndScriptAsync(
Initial,
@"class C
{   
    const int y = 1 ;
    private const bool undeclared;

    public void Goo ( bool x = undeclared ) { }
} ");
        }

        [WorkItem(542900, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/542900")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFromAttributeNamedArgument1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class ProgramAttribute : Attribute
{
    [Program([|Name|] = 0)]
    static void Main(string[] args)
    {
    }
}",
@"using System;

class ProgramAttribute : Attribute
{
    public int Name { get; set; }

    [Program(Name = 0)]
    static void Main(string[] args)
    {
    }
}");
        }

        [WorkItem(542900, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/542900")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFromAttributeNamedArgument2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class ProgramAttribute : Attribute
{
    [Program([|Name|] = 0)]
    static void Main(string[] args)
    {
    }
}",
@"using System;

class ProgramAttribute : Attribute
{
    public int Name;

    [Program(Name = 0)]
    static void Main(string[] args)
    {
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(541698, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541698")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMinimalAccessibility1_InternalPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Program
{
    public static void Main()
    {
        C c = [|P|];
    }

    private class C
    {
    }
}",
@"class Program
{
    private static C P { get; set; }

    public static void Main()
    {
        C c = P;
    }

    private class C
    {
    }
}",
parseOptions: null);
        }

        [WorkItem(541698, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541698")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMinimalAccessibility2_InternalProtected()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Program
{
    public static void Main()
    {
        C c = [|P|];
    }

    protected class C
    {
    }
}",
@"class Program
{
    protected static C P { get; private set; }

    public static void Main()
    {
        C c = P;
    }

    protected class C
    {
    }
}",
parseOptions: null);
        }

        [WorkItem(541698, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541698")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMinimalAccessibility3_InternalInternal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Program
{
    public static void Main()
    {
        C c = [|P|];
    }

    internal class C
    {
    }
}",
@"class Program
{
    public static C P { get; private set; }

    public static void Main()
    {
        C c = P;
    }

    internal class C
    {
    }
}",
parseOptions: null);
        }

        [WorkItem(541698, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541698")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMinimalAccessibility4_InternalProtectedInternal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Program
{
    public static void Main()
    {
        C c = [|P|];
    }

    protected internal class C
    {
    }
}",
@"class Program
{
    public static C P { get; private set; }

    public static void Main()
    {
        C c = P;
    }

    protected internal class C
    {
    }
}",
parseOptions: null);
        }

        [WorkItem(541698, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541698")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMinimalAccessibility5_InternalPublic()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class Program
{
    public static void Main()
    {
        C c = [|P|];
    }

    public class C
    {
    }
}",
@"class Program
{
    public static C P { get; private set; }

    public static void Main()
    {
        C c = P;
    }

    public class C
    {
    }
}",
parseOptions: null);
        }

        [WorkItem(541698, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541698")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMinimalAccessibility6_PublicInternal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class Program
{
    public static void Main()
    {
        C c = [|P|];
    }

    internal class C
    {
    }
}",
@"public class Program
{
    internal static C P { get; private set; }

    public static void Main()
    {
        C c = P;
    }

    internal class C
    {
    }
}",
parseOptions: null);
        }

        [WorkItem(541698, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541698")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMinimalAccessibility7_PublicProtectedInternal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class Program
{
    public static void Main()
    {
        C c = [|P|];
    }

    protected internal class C
    {
    }
}",
@"public class Program
{
    protected internal static C P { get; private set; }

    public static void Main()
    {
        C c = P;
    }

    protected internal class C
    {
    }
}",
parseOptions: null);
        }

        [WorkItem(541698, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541698")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMinimalAccessibility8_PublicProtected()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class Program
{
    public static void Main()
    {
        C c = [|P|];
    }

    protected class C
    {
    }
}",
@"public class Program
{
    protected static C P { get; private set; }

    public static void Main()
    {
        C c = P;
    }

    protected class C
    {
    }
}",
parseOptions: null);
        }

        [WorkItem(541698, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541698")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMinimalAccessibility9_PublicPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"public class Program
{
    public static void Main()
    {
        C c = [|P|];
    }

    private class C
    {
    }
}",
@"public class Program
{
    private static C P { get; set; }

    public static void Main()
    {
        C c = P;
    }

    private class C
    {
    }
}",
parseOptions: null);
        }

        [WorkItem(541698, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541698")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMinimalAccessibility10_PrivatePrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class outer
{
    private class Program
    {
        public static void Main()
        {
            C c = [|P|];
        }

        private class C
        {
        }
    }
}",
@"class outer
{
    private class Program
    {
        public static C P { get; private set; }

        public static void Main()
        {
            C c = P;
        }

        private class C
        {
        }
    }
}",
parseOptions: null);
        }

        [WorkItem(541698, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541698")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMinimalAccessibility11_PrivateProtected()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class outer
{
    private class Program
    {
        public static void Main()
        {
            C c = [|P|];
        }

        protected class C
        {
        }
    }
}",
@"class outer
{
    private class Program
    {
        public static C P { get; private set; }

        public static void Main()
        {
            C c = P;
        }

        protected class C
        {
        }
    }
}",
parseOptions: null);
        }

        [WorkItem(541698, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541698")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMinimalAccessibility12_PrivateProtectedInternal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class outer
{
    private class Program
    {
        public static void Main()
        {
            C c = [|P|];
        }

        protected internal class C
        {
        }
    }
}",
@"class outer
{
    private class Program
    {
        public static C P { get; private set; }

        public static void Main()
        {
            C c = P;
        }

        protected internal class C
        {
        }
    }
}",
parseOptions: null);
        }

        [WorkItem(541698, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541698")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMinimalAccessibility13_PrivateInternal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class outer
{
    private class Program
    {
        public static void Main()
        {
            C c = [|P|];
        }

        internal class C
        {
        }
    }
}",
@"class outer
{
    private class Program
    {
        public static C P { get; private set; }

        public static void Main()
        {
            C c = P;
        }

        internal class C
        {
        }
    }
}",
parseOptions: null);
        }

        [WorkItem(541698, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541698")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMinimalAccessibility14_ProtectedPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class outer
{
    protected class Program
    {
        public static void Main()
        {
            C c = [|P|];
        }

        private class C
        {
        }
    }
}",
@"class outer
{
    protected class Program
    {
        private static C P { get; set; }

        public static void Main()
        {
            C c = P;
        }

        private class C
        {
        }
    }
}",
parseOptions: null);
        }

        [WorkItem(541698, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541698")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMinimalAccessibility15_ProtectedInternal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class outer
{
    protected class Program
    {
        public static void Main()
        {
            C c = [|P|];
        }

        internal class C
        {
        }
    }
}",
@"class outer
{
    protected class Program
    {
        public static C P { get; private set; }

        public static void Main()
        {
            C c = P;
        }

        internal class C
        {
        }
    }
}",
parseOptions: null);
        }

        [WorkItem(541698, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541698")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMinimalAccessibility16_ProtectedInternalProtected()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class outer
{
    protected internal class Program
    {
        public static void Main()
        {
            C c = [|P|];
        }

        protected class C
        {
        }
    }
}",
@"class outer
{
    protected internal class Program
    {
        protected static C P { get; private set; }

        public static void Main()
        {
            C c = P;
        }

        protected class C
        {
        }
    }
}",
parseOptions: null);
        }

        [WorkItem(541698, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541698")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMinimalAccessibility17_ProtectedInternalInternal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"class outer
{
    protected internal class Program
    {
        public static void Main()
        {
            C c = [|P|];
        }

        internal class C
        {
        }
    }
}",
@"class outer
{
    protected internal class Program
    {
        public static C P { get; private set; }

        public static void Main()
        {
            C c = P;
        }

        internal class C
        {
        }
    }
}",
parseOptions: null);
        }

        [WorkItem(543153, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/543153")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAnonymousObjectInitializer1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var a = new { x = 5 };
        a = new { x = [|HERE|] };
    }
}",
@"class C
{
    private int HERE;

    void M()
    {
        var a = new { x = 5 };
        a = new { x = HERE };
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(543124, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/543124")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNoGenerationIntoAnonymousType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    static void Main(string[] args)
    {
        var v = new { };
        bool b = v.[|Bar|];
    }
}");
        }

        [WorkItem(543543, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/543543")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOfferedForBoundParametersOfOperators()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    public Program(string s)
    {
    }

    static void Main(string[] args)
    {
        Program p = """";
    }

    public static implicit operator Program(string str)
    {
        return new Program([|str|]);
    }
}");
        }

        [WorkItem(544175, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/544175")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnNamedParameterName1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class class1
{
    public void Test()
    {
        Goo([|x|]: x);
    }

    public string Goo(int x)
    {
    }
}");
        }

        [WorkItem(544271, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/544271")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnNamedParameterName2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Goo
{
    public Goo(int a = 42)
    {
    }
}

class DogBed : Goo
{
    public DogBed(int b) : base([|a|]: b)
    {
    }
}");
        }

        [WorkItem(544164, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/544164")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPropertyOnObjectInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Goo
{
}

class Bar
{
    void goo()
    {
        var c = new Goo { [|Gibberish|] = 24 };
    }
}",
@"class Goo
{
    public int Gibberish { get; internal set; }
}

class Bar
{
    void goo()
    {
        var c = new Goo { Gibberish = 24 };
    }
}");
        }

        [WorkItem(13166, "https://github.com/dotnet/roslyn/issues/13166")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPropertyOnNestedObjectInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"public class Inner
{
}

public class Outer
{
    public Inner Inner { get; set; } = new Inner();

    public static Outer X() => new Outer { Inner = { [|InnerValue|] = 5 } };
}",
@"public class Inner
{
    public int InnerValue { get; internal set; }
}

public class Outer
{
    public Inner Inner { get; set; } = new Inner();

    public static Outer X() => new Outer { Inner = { InnerValue = 5 } };
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPropertyOnObjectInitializer1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Goo
{
}

class Bar
{
    void goo()
    {
        var c = new Goo { [|Gibberish|] = Gibberish };
    }
}",
@"class Goo
{
    public object Gibberish { get; internal set; }
}

class Bar
{
    void goo()
    {
        var c = new Goo { Gibberish = Gibberish };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPropertyOnObjectInitializer2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Goo
{
}

class Bar
{
    void goo()
    {
        var c = new Goo { Gibberish = [|Gibberish|] };
    }
}",
@"class Goo
{
}

class Bar
{
    public object Gibberish { get; private set; }

    void goo()
    {
        var c = new Goo { Gibberish = Gibberish };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFieldOnObjectInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Goo
{
}

class Bar
{
    void goo()
    {
        var c = new Goo { [|Gibberish|] = 24 };
    }
}",
@"class Goo
{
    internal int Gibberish;
}

class Bar
{
    void goo()
    {
        var c = new Goo { Gibberish = 24 };
    }
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFieldOnObjectInitializer1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Goo
{
}

class Bar
{
    void goo()
    {
        var c = new Goo { [|Gibberish|] = Gibberish };
    }
}",
@"class Goo
{
    internal object Gibberish;
}

class Bar
{
    void goo()
    {
        var c = new Goo { Gibberish = Gibberish };
    }
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFieldOnObjectInitializer2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Goo
{
}

class Bar
{
    void goo()
    {
        var c = new Goo { Gibberish = [|Gibberish|] };
    }
}",
@"class Goo
{
}

class Bar
{
    private object Gibberish;

    void goo()
    {
        var c = new Goo { Gibberish = Gibberish };
    }
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnlyPropertyAndFieldOfferedForObjectInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestActionCountAsync(
@"class Goo
{
}

class Bar
{
    void goo()
    {
        var c = new Goo { . [|Gibberish|] = 24 };
    }
}",
2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateLocalInObjectInitializerValue()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Goo
{
}

class Bar
{
    void goo()
    {
        var c = new Goo { Gibberish = [|blah|] };
    }
}",
@"class Goo
{
}

class Bar
{
    void goo()
    {
        object blah = null;
        var c = new Goo { Gibberish = blah };
    }
}",
index: LocalIndex);
        }

        [WorkItem(544319, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/544319")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnIncompleteMember1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Class1
{
    Console.[|WriteLine|](); }");
        }

        [WorkItem(544319, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/544319")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnIncompleteMember2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Class1
{ [|WriteLine|]();
}");
        }

        [WorkItem(544319, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/544319")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnIncompleteMember3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

class Class1
{
    [|WriteLine|]
}");
        }

        [WorkItem(544384, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/544384")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPointerType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    static int x;

    unsafe static void F(int* p)
    {
        *p = 1;
    }

    static unsafe void Main(string[] args)
    {
        int[] a = new int[10];
        fixed (int* p2 = &x, int* p3 = ) F(GetP2([|p2|]));
    }

    unsafe private static int* GetP2(int* p2)
    {
        return p2;
    }
}",
@"class Program
{
    static int x;
    private static unsafe int* p2;

    unsafe static void F(int* p)
    {
        *p = 1;
    }

    static unsafe void Main(string[] args)
    {
        int[] a = new int[10];
        fixed (int* p2 = &x, int* p3 = ) F(GetP2(p2));
    }

    unsafe private static int* GetP2(int* p2)
    {
        return p2;
    }
}");
        }

        [WorkItem(544510, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/544510")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnUsingAlias()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using [|S|] = System ; S . Console . WriteLine ( ""hello world"" ) ; ");
        }

        [WorkItem(544907, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/544907")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExpressionTLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
using System.Linq.Expressions;

class C
{
    static void Main()
    {
        Expression<Func<int, int>> e = x => [|Goo|];
    }
}",
@"using System;
using System.Linq.Expressions;

class C
{
    public static int Goo { get; private set; }

    static void Main()
    {
        Expression<Func<int, int>> e = x => Goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNoGenerationIntoEntirelyHiddenType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    void Goo()
    {
        int i = D.[|Bar|];
    }
}

#line hidden
class D
{
}
#line default");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInReturnStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
        return [|goo|];
    }
}",
@"class Program
{
    private object goo;

    void Main()
    {
        return goo;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLocal1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
        Goo([|bar|]);
    }

    static void Goo(int i)
    {
    }
}",
@"class Program
{
    void Main()
    {
        int bar = 0;
        Goo(bar);
    }

    static void Goo(int i)
    {
    }
}",
index: LocalIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLocalMissingForVar()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
        var x = [|var|];
    }");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOutLocal1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
        Goo(out [|bar|]);
    }

    static void Goo(out int i)
    {
    }
}",
@"class Program
{
    void Main()
    {
        int bar;
        Goo(out bar);
    }

    static void Goo(out int i)
    {
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(809542, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/809542")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLocalBeforeComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
#if true
        // Banner Line 1
        // Banner Line 2
        int.TryParse(""123"", out [|local|]);
#endif
    }
}",
@"class Program
{
    void Main()
    {
#if true
        int local;
        // Banner Line 1
        // Banner Line 2
        int.TryParse(""123"", out [|local|]);
#endif
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(809542, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/809542")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLocalAfterComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
#if true
        // Banner Line 1
        // Banner Line 2

        int.TryParse(""123"", out [|local|]);
#endif
    }
}",
@"class Program
{
    void Main()
    {
#if true
        // Banner Line 1
        // Banner Line 2

        int local;
        int.TryParse(""123"", out [|local|]);
#endif
    }
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateIntoVisiblePortion()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

#line hidden
class Program
{
    void Main()
    {
#line default
        Goo(Program.[|X|])
    }
}",
@"using System;

#line hidden
class Program
{
    void Main()
    {
#line default
        Goo(Program.X)
    }

    public static object X { get; private set; }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingWhenNoAvailableRegionToGenerateInto()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

#line hidden
class Program
{
    void Main()
    {
#line default
        Goo(Program.[|X|])


#line hidden
    }
}
#line default");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateLocalAvailableIfBlockIsNotHidden()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

#line hidden
class Program
{
#line default
    void Main()
    {
        Goo([|x|]);
    }
#line hidden
}
#line default",
@"using System;

#line hidden
class Program
{
#line default
    void Main()
    {
        object x = null;
        Goo(x);
    }
#line hidden
}
#line default");
        }

        [WorkItem(545217, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/545217")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateLocalNameSimplification()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    void goo()
    {
        bar([|xyz|]);
    }

    struct sfoo
    {
    }

    void bar(sfoo x)
    {
    }
}",
@"class Program
{
    void goo()
    {
        sfoo xyz = default(sfoo);
        bar(xyz);
    }

    struct sfoo
    {
    }

    void bar(sfoo x)
    {
    }
}",
index: LocalIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestParenthesizedExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
        int v = 1 + ([|k|]);
    }
}",
@"class Program
{
    private int k;

    void Main()
    {
        int v = 1 + (k);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInSelect()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Linq;

class Program
{
    void Main(string[] args)
    {
        var q = from a in args
                select [|v|];
    }
}",
@"using System.Linq;

class Program
{
    private object v;

    void Main(string[] args)
    {
        var q = from a in args
                select v;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInChecked()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
        int[] a = null;
        int[] temp = checked([|goo|]);
    }
}",
@"class Program
{
    private int[] goo;

    void Main()
    {
        int[] a = null;
        int[] temp = checked(goo);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInArrayRankSpecifier()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
        var v = new int[[|k|]];
    }
}",
@"class Program
{
    private int k;

    void Main()
    {
        var v = new int[k];
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInConditional1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    static void Main()
    {
        int i = [|goo|] ? bar : baz;
    }
}",
@"class Program
{
    private static bool goo;

    static void Main()
    {
        int i = goo ? bar : baz;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInConditional2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    static void Main()
    {
        int i = goo ? [|bar|] : baz;
    }
}",
@"class Program
{
    private static int bar;

    static void Main()
    {
        int i = goo ? bar : baz;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInConditional3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    static void Main()
    {
        int i = goo ? bar : [|baz|];
    }
}",
@"class Program
{
    private static int baz;

    static void Main()
    {
        int i = goo ? bar : baz;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInCast()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
        var x = (int)[|y|];
    }
}",
@"class Program
{
    private int y;

    void Main()
    {
        var x = (int)y;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInIf()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
        if ([|goo|])
        {
        }
    }
}",
@"class Program
{
    private bool goo;

    void Main()
    {
        if (goo)
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInSwitch()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
        switch ([|goo|])
        {
        }
    }
}",
@"class Program
{
    private int goo;

    void Main()
    {
        switch (goo)
        {
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
        [|System|].Console.WriteLine(4);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
        [|System.Console|].WriteLine(4);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMissingOnBase()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
        [|base|].ToString();
    }
}");
        }

        [WorkItem(545273, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/545273")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFromAssign1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
        [|undefined|] = 1;
    }
}",
@"class Program
{
    void Main()
    {
        var undefined = 1;
    }
}",
index: PropertyIndex, options: ImplicitTypingEverywhere());
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFuncAssignment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
        [|undefined|] = (x) => 2;
    }
}",
@"class Program
{
    void Main()
    {
        System.Func<object, int> undefined = (x) => 2;
    }
}",
index: PropertyIndex);
        }

        [WorkItem(545273, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/545273")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFromAssign1NotAsVar()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
        [|undefined|] = 1;
    }
}",
@"class Program
{
    void Main()
    {
        int undefined = 1;
    }
}",
index: PropertyIndex);
        }

        [WorkItem(545273, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/545273")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFromAssign2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    void Main()
    {
        [|undefined|] = new { P = ""1"" };
    }
}",
@"class Program
{
    void Main()
    {
        var undefined = new { P = ""1"" };
    }
}",
index: PropertyIndex);
        }

        [WorkItem(545269, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/545269")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateInVenus1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
#line 1 ""goo""
    void Goo()
    {
        this.[|Bar|] = 1;
    }
#line default
#line hidden
}");
        }

        [WorkItem(545269, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/545269")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateInVenus2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
#line 1 ""goo""
    void Goo()
    {
        [|Bar|] = 1;
    }
#line default
#line hidden
}
";
            await TestExactActionSetOfferedAsync(code, new[] { string.Format(FeaturesResources.Generate_local_0, "Bar") });

            await TestInRegularAndScriptAsync(code,
@"
class C
{
#line 1 ""goo""
    void Goo()
    {
        var [|Bar|] = 1;
    }
#line default
#line hidden
}
", options: ImplicitTypingEverywhere());
        }

        [WorkItem(546027, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546027")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyFromAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

[AttributeUsage(AttributeTargets.Class)]
class MyAttrAttribute : Attribute
{
}

[MyAttr(123, [|Version|] = 1)]
class D
{
}",
@"using System;

[AttributeUsage(AttributeTargets.Class)]
class MyAttrAttribute : Attribute
{
    public int Version { get; set; }
}

[MyAttr(123, Version = 1)]
class D
{
}");
        }

        [WorkItem(545232, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/545232")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNewLinePreservationBeforeInsertingLocal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
namespace CSharpDemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const int MEGABYTE = 1024 * 1024;
            Console.WriteLine(MEGABYTE);
 
            Calculate([|multiplier|]);
        }
        static void Calculate(double multiplier = Math.PI)
        {
        }
    }
}
",
@"using System;
namespace CSharpDemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const int MEGABYTE = 1024 * 1024;
            Console.WriteLine(MEGABYTE);

            double multiplier = 0;
            Calculate(multiplier);
        }
        static void Calculate(double multiplier = Math.PI)
        {
        }
    }
}
",
index: LocalIndex);
        }

        [WorkItem(863346, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/863346")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateInGenericMethod_Local()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
class TestClass<T1>
{
    static T TestMethod<T>(T item)
    {
        T t = WrapFunc<T>([|NewLocal|]);
        return t;
    }

    private static T WrapFunc<T>(Func<T1, T> function)
    {
        T1 zoo = default(T1);
        return function(zoo);
    }
}
",
@"using System;
class TestClass<T1>
{
    static T TestMethod<T>(T item)
    {
        Func<T1, T> NewLocal = null;
        T t = WrapFunc<T>(NewLocal);
        return t;
    }

    private static T WrapFunc<T>(Func<T1, T> function)
    {
        T1 zoo = default(T1);
        return function(zoo);
    }
}
",
index: LocalIndex);
        }

        [WorkItem(863346, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/863346")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateInGenericMethod_Property()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
class TestClass<T1>
{
    static T TestMethod<T>(T item)
    {
        T t = WrapFunc<T>([|NewLocal|]);
        return t;
    }

    private static T WrapFunc<T>(Func<T1, T> function)
    {
        T1 zoo = default(T1);
        return function(zoo);
    }
}
",
@"using System;
class TestClass<T1>
{
    public static Func<T1, object> NewLocal { get; private set; }

    static T TestMethod<T>(T item)
    {
        T t = WrapFunc<T>(NewLocal);
        return t;
    }

    private static T WrapFunc<T>(Func<T1, T> function)
    {
        T1 zoo = default(T1);
        return function(zoo);
    }
}
");
        }

        [WorkItem(865067, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/865067")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithYieldReturn()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
using System.Collections.Generic;

class Program
{
    IEnumerable<DayOfWeek> Goo()
    {
        yield return [|abc|];
    }
}",
@"using System;
using System.Collections.Generic;

class Program
{
    private DayOfWeek abc;

    IEnumerable<DayOfWeek> Goo()
    {
        yield return abc;
    }
}");
        }

        [WorkItem(877580, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/877580")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithThrow()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class Program
{
    void Goo()
    {
        throw [|MyExp|];
    }
}",
@"using System;

class Program
{
    private Exception MyExp;

    void Goo()
    {
        throw MyExp;
    }
}", index: ReadonlyFieldIndex);
        }

        [WorkItem(530177, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530177")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnsafeField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|int* a = goo|];
    }
}",
@"class Class
{
    private unsafe int* goo;

    void Method()
    {
        int* a = goo;
    }
}");
        }

        [WorkItem(530177, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530177")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnsafeField2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|int*[] a = goo|];
    }
}",
@"class Class
{
    private unsafe int*[] goo;

    void Method()
    {
        int*[] a = goo;
    }
}");
        }

        [WorkItem(530177, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530177")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnsafeFieldInUnsafeClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"unsafe class Class
{
    void Method()
    {
        [|int* a = goo|];
    }
}",
@"unsafe class Class
{
    private int* goo;

    void Method()
    {
        int* a = goo;
    }
}");
        }

        [WorkItem(530177, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530177")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnsafeFieldInNestedClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"unsafe class Class
{
    class MyClass
    {
        void Method()
        {
            [|int* a = goo|];
        }
    }
}",
@"unsafe class Class
{
    class MyClass
    {
        private int* goo;

        void Method()
        {
            int* a = goo;
        }
    }
}");
        }

        [WorkItem(530177, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530177")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnsafeFieldInNestedClass2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    unsafe class MyClass
    {
        void Method()
        {
            [|int* a = Class.goo|];
        }
    }
}",
@"class Class
{
    private static unsafe int* goo;

    unsafe class MyClass
    {
        void Method()
        {
            int* a = Class.goo;
        }
    }
}");
        }

        [WorkItem(530177, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530177")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnsafeReadOnlyField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|int* a = goo|];
    }
}",
@"class Class
{
    private readonly unsafe int* goo;

    void Method()
    {
        int* a = goo;
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(530177, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530177")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnsafeReadOnlyField2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|int*[] a = goo|];
    }
}",
@"class Class
{
    private readonly unsafe int*[] goo;

    void Method()
    {
        int*[] a = goo;
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(530177, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530177")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnsafeReadOnlyFieldInUnsafeClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"unsafe class Class
{
    void Method()
    {
        [|int* a = goo|];
    }
}",
@"unsafe class Class
{
    private readonly int* goo;

    void Method()
    {
        int* a = goo;
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(530177, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530177")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnsafeReadOnlyFieldInNestedClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"unsafe class Class
{
    class MyClass
    {
        void Method()
        {
            [|int* a = goo|];
        }
    }
}",
@"unsafe class Class
{
    class MyClass
    {
        private readonly int* goo;

        void Method()
        {
            int* a = goo;
        }
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(530177, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530177")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnsafeReadOnlyFieldInNestedClass2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    unsafe class MyClass
    {
        void Method()
        {
            [|int* a = Class.goo|];
        }
    }
}",
@"class Class
{
    private static readonly unsafe int* goo;

    unsafe class MyClass
    {
        void Method()
        {
            int* a = Class.goo;
        }
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(530177, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530177")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnsafeProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|int* a = goo|];
    }
}",
@"class Class
{
    public unsafe int* goo { get; private set; }

    void Method()
    {
        int* a = goo;
    }
}",
index: PropertyIndex);
        }

        [WorkItem(530177, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530177")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnsafeProperty2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|int*[] a = goo|];
    }
}",
@"class Class
{
    public unsafe int*[] goo { get; private set; }

    void Method()
    {
        int*[] a = goo;
    }
}",
index: PropertyIndex);
        }

        [WorkItem(530177, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530177")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnsafePropertyInUnsafeClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"unsafe class Class
{
    void Method()
    {
        [|int* a = goo|];
    }
}",
@"unsafe class Class
{
    public int* goo { get; private set; }

    void Method()
    {
        int* a = goo;
    }
}",
index: PropertyIndex);
        }

        [WorkItem(530177, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530177")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnsafePropertyInNestedClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"unsafe class Class
{
    class MyClass
    {
        void Method()
        {
            [|int* a = goo|];
        }
    }
}",
@"unsafe class Class
{
    class MyClass
    {
        public int* goo { get; private set; }

        void Method()
        {
            int* a = goo;
        }
    }
}",
index: PropertyIndex);
        }

        [WorkItem(530177, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530177")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnsafePropertyInNestedClass2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    unsafe class MyClass
    {
        void Method()
        {
            [|int* a = Class.goo|];
        }
    }
}",
@"class Class
{
    public static unsafe int* goo { get; private set; }

    unsafe class MyClass
    {
        void Method()
        {
            int* a = Class.goo;
        }
    }
}",
index: PropertyIndex);
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|Z|]);
    }
}",
@"class C
{
    public object Z { get; private set; }

    void M()
    {
        var x = nameof(Z);
    }
}");
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|Z|]);
    }
}",
@"class C
{
    private object Z;

    void M()
    {
        var x = nameof(Z);
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfReadonlyField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|Z|]);
    }
}",
@"class C
{
    private readonly object Z;

    void M()
    {
        var x = nameof(Z);
    }
}",
index: PropertyIndex);
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfLocal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|Z|]);
    }
}",
@"class C
{
    void M()
    {
        object Z = null;
        var x = nameof(Z);
    }
}",
index: LocalIndex);
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfProperty2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|Z.X|]);
    }
}",
@"class C
{
    public object Z { get; private set; }

    void M()
    {
        var x = nameof(Z.X);
    }
}");
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfField2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|Z.X|]);
    }
}",
@"class C
{
    private object Z;

    void M()
    {
        var x = nameof(Z.X);
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfReadonlyField2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|Z.X|]);
    }
}",
@"class C
{
    private readonly object Z;

    void M()
    {
        var x = nameof(Z.X);
    }
}",
index: PropertyIndex);
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfLocal2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|Z.X|]);
    }
}",
@"class C
{
    void M()
    {
        object Z = null;
        var x = nameof(Z.X);
    }
}",
index: LocalIndex);
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfProperty3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|Z.X.Y|]);
    }
}",
@"class C
{
    public object Z { get; private set; }

    void M()
    {
        var x = nameof(Z.X.Y);
    }
}");
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfField3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|Z.X.Y|]);
    }
}",
@"class C
{
    private object Z;

    void M()
    {
        var x = nameof(Z.X.Y);
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfReadonlyField3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|Z.X.Y|]);
    }
}",
@"class C
{
    private readonly object Z;

    void M()
    {
        var x = nameof(Z.X.Y);
    }
}",
index: PropertyIndex);
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfLocal3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|Z.X.Y|]);
    }
}",
@"class C
{
    void M()
    {
        object Z = null;
        var x = nameof(Z.X.Y);
    }
}",
index: LocalIndex);
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfMissing()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = [|nameof(1 + 2)|];
    }
}");
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfMissing2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var y = 1 + 2;
        var x = [|nameof(y)|];
    }
}");
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfMissing3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var y = 1 + 2;
        var z = """";
        var x = [|nameof(y, z)|];
    }
}");
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfProperty4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|y|], z);
    }
}",
@"class C
{
    public object y { get; private set; }

    void M()
    {
        var x = nameof(y, z);
    }
}",
index: PropertyIndex);
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfField4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|y|], z);
    }
}",
@"class C
{
    private object y;

    void M()
    {
        var x = nameof(y, z);
    }
}");
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfReadonlyField4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|y|], z);
    }
}",
@"class C
{
    private readonly object y;

    void M()
    {
        var x = nameof(y, z);
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfLocal4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|y|], z);
    }
}",
@"class C
{
    void M()
    {
        object y = null;
        var x = nameof(y, z);
    }
}",
index: LocalIndex);
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfProperty5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|y|]);
    }

    private object nameof(object y)
    {
        return null;
    }
}",
@"class C
{
    public object y { get; private set; }

    void M()
    {
        var x = nameof(y);
    }

    private object nameof(object y)
    {
        return null;
    }
}",
index: PropertyIndex);
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfField5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|y|]);
    }

    private object nameof(object y)
    {
        return null;
    }
}",
@"class C
{
    private object y;

    void M()
    {
        var x = nameof(y);
    }

    private object nameof(object y)
    {
        return null;
    }
}");
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfReadonlyField5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|y|]);
    }

    private object nameof(object y)
    {
        return null;
    }
}",
@"class C
{
    private readonly object y;

    void M()
    {
        var x = nameof(y);
    }

    private object nameof(object y)
    {
        return null;
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(1032176, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1032176")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideNameOfLocal5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void M()
    {
        var x = nameof([|y|]);
    }

    private object nameof(object y)
    {
        return null;
    }
}",
@"class C
{
    void M()
    {
        object y = null;
        var x = nameof(y);
    }

    private object nameof(object y)
    {
        return null;
    }
}",
index: LocalIndex);
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditionalAccessProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void Main(C a)
    {
        C x = a?[|.Instance|];
    }
}",
@"class C
{
    public C Instance { get; private set; }

    void Main(C a)
    {
        C x = a?.Instance;
    }
}");
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditionalAccessField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void Main(C a)
    {
        C x = a?[|.Instance|];
    }
}",
@"class C
{
    private C Instance;

    void Main(C a)
    {
        C x = a?.Instance;
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditionalAccessReadonlyField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void Main(C a)
    {
        C x = a?[|.Instance|];
    }
}",
@"class C
{
    private readonly C Instance;

    void Main(C a)
    {
        C x = a?.Instance;
    }
}",
index: PropertyIndex);
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditionalAccessVarProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void Main(C a)
    {
        var x = a?[|.Instance|];
    }
}",
@"class C
{
    public object Instance { get; private set; }

    void Main(C a)
    {
        var x = a?.Instance;
    }
}");
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditionalAccessVarField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void Main(C a)
    {
        var x = a?[|.Instance|];
    }
}",
@"class C
{
    private object Instance;

    void Main(C a)
    {
        var x = a?.Instance;
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditionalAccessVarReadOnlyField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void Main(C a)
    {
        var x = a?[|.Instance|];
    }
}",
@"class C
{
    private readonly object Instance;

    void Main(C a)
    {
        var x = a?.Instance;
    }
}",
index: PropertyIndex);
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditionalAccessNullableProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void Main(C a)
    {
        int? x = a?[|.B|];
    }
}",
@"class C
{
    public int B { get; private set; }

    void Main(C a)
    {
        int? x = a?.B;
    }
}");
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditionalAccessNullableField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void Main(C a)
    {
        int? x = a?[|.B|];
    }
}",
@"class C
{
    private int B;

    void Main(C a)
    {
        int? x = a?.B;
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditionalAccessNullableReadonlyField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    void Main(C a)
    {
        int? x = a?[|.B|];
    }
}",
@"class C
{
    private readonly int B;

    void Main(C a)
    {
        int? x = a?.B;
    }
}",
index: PropertyIndex);
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyInConditionalAccessExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        C x = a?.B.[|C|];
    }

    public class E
    {
    }
}",
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        C x = a?.B.C;
    }

    public class E
    {
        public C C { get; internal set; }
    }
}");
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyInConditionalAccessExpression2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        int x = a?.B.[|C|];
    }

    public class E
    {
    }
}",
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        int x = a?.B.C;
    }

    public class E
    {
        public int C { get; internal set; }
    }
}");
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyInConditionalAccessExpression3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        int? x = a?.B.[|C|];
    }

    public class E
    {
    }
}",
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        int? x = a?.B.C;
    }

    public class E
    {
        public int C { get; internal set; }
    }
}");
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyInConditionalAccessExpression4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        var x = a?.B.[|C|];
    }

    public class E
    {
    }
}",
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        var x = a?.B.C;
    }

    public class E
    {
        public object C { get; internal set; }
    }
}");
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInConditionalAccessExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        C x = a?.B.[|C|];
    }

    public class E
    {
    }
}",
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        C x = a?.B.C;
    }

    public class E
    {
        internal C C;
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInConditionalAccessExpression2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        int x = a?.B.[|C|];
    }

    public class E
    {
    }
}",
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        int x = a?.B.C;
    }

    public class E
    {
        internal int C;
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInConditionalAccessExpression3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        int? x = a?.B.[|C|];
    }

    public class E
    {
    }
}",
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        int? x = a?.B.C;
    }

    public class E
    {
        internal int C;
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInConditionalAccessExpression4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        var x = a?.B.[|C|];
    }

    public class E
    {
    }
}",
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        var x = a?.B.C;
    }

    public class E
    {
        internal object C;
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateReadonlyFieldInConditionalAccessExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        C x = a?.B.[|C|];
    }

    public class E
    {
    }
}",
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        C x = a?.B.C;
    }

    public class E
    {
        internal readonly C C;
    }
}",
index: PropertyIndex);
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateReadonlyFieldInConditionalAccessExpression2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        int x = a?.B.[|C|];
    }

    public class E
    {
    }
}",
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        int x = a?.B.C;
    }

    public class E
    {
        internal readonly int C;
    }
}",
index: PropertyIndex);
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateReadonlyFieldInConditionalAccessExpression3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        int? x = a?.B.[|C|];
    }

    public class E
    {
    }
}",
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        int? x = a?.B.C;
    }

    public class E
    {
        internal readonly int C;
    }
}",
index: PropertyIndex);
        }

        [WorkItem(1064748, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1064748")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateReadonlyFieldInConditionalAccessExpression4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        var x = a?.B.[|C|];
    }

    public class E
    {
    }
}",
@"class C
{
    public E B { get; private set; }

    void Main(C a)
    {
        var x = a?.B.C;
    }

    public class E
    {
        internal readonly object C;
    }
}",
index: PropertyIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInPropertyInitializers()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    public int MyProperty { get; } = [|y|];
}",
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    private static int y;

    public int MyProperty { get; } = y;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateReadonlyFieldInPropertyInitializers()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    public int MyProperty { get; } = [|y|];
}",
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    private static readonly int y;

    public int MyProperty { get; } = y;
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyInPropertyInitializers()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    public int MyProperty { get; } = [|y|];
}",
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    public static int y { get; private set; }
    public int MyProperty { get; } = y;
}",
index: PropertyIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInExpressionBodyMember()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    public int Y => [|y|];
}",
@"class Program
{
    private int y;

    public int Y => y;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateReadonlyFieldInExpressionBodyMember()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    public int Y => [|y|];
}",
@"class Program
{
    private readonly int y;

    public int Y => y;
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyInExpressionBodyMember()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    public int Y => [|y|];
}",
@"class Program
{
    public int Y => y;

    public int y { get; private set; }
}",
index: PropertyIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInExpressionBodyMember2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public static C operator --(C p) => [|x|];
}",
@"class C
{
    private static C x;

    public static C operator --(C p) => x;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateReadOnlyFieldInExpressionBodyMember2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public static C operator --(C p) => [|x|];
}",
@"class C
{
    private static readonly C x;

    public static C operator --(C p) => x;
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyInExpressionBodyMember2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public static C operator --(C p) => [|x|];
}",
@"class C
{
    public static C x { get; private set; }

    public static C operator --(C p) => x;
}",
index: PropertyIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInExpressionBodyMember3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public static C GetValue(C p) => [|x|];
}",
@"class C
{
    private static C x;

    public static C GetValue(C p) => x;
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateReadOnlyFieldInExpressionBodyMember3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public static C GetValue(C p) => [|x|];
}",
@"class C
{
    private static readonly C x;

    public static C GetValue(C p) => x;
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyInExpressionBodyMember3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    public static C GetValue(C p) => [|x|];
}",
@"class C
{
    public static C x { get; private set; }

    public static C GetValue(C p) => x;
}",
index: PropertyIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInDictionaryInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [[|key|]] = 0 };
    }
}",
@"using System.Collections.Generic;

class Program
{
    private static string key;

    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [key] = 0 };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyInDictionaryInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [""Zero""] = 0, [[|One|]] = 1, [""Two""] = 2 };
    }
}",
@"using System.Collections.Generic;

class Program
{
    public static string One { get; private set; }

    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [""Zero""] = 0, [One] = 1, [""Two""] = 2 };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInDictionaryInitializer2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [""Zero""] = [|i|] };
    }
}",
@"using System.Collections.Generic;

class Program
{
    private static int i;

    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [""Zero""] = i };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateReadOnlyFieldInDictionaryInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [[|key|]] = 0 };
    }
}",
@"using System.Collections.Generic;

class Program
{
    private static readonly string key;

    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [key] = 0 };
    }
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInDictionaryInitializer3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [""Zero""] = 0, [[|One|]] = 1, [""Two""] = 2 };
    }
}",
@"using System.Collections.Generic;

class Program
{
    private static string One;

    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [""Zero""] = 0, [One] = 1, [""Two""] = 2 };
    }
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateReadOnlyFieldInDictionaryInitializer2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [""Zero""] = [|i|] };
    }
}",
@"using System.Collections.Generic;

class Program
{
    private static readonly int i;

    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [""Zero""] = i };
    }
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyInDictionaryInitializer2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [[|key|]] = 0 };
    }
}",
@"using System.Collections.Generic;

class Program
{
    public static string key { get; private set; }

    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [key] = 0 };
    }
}",
index: PropertyIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateReadOnlyFieldInDictionaryInitializer3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [""Zero""] = 0, [[|One|]] = 1, [""Two""] = 2 };
    }
}",
@"using System.Collections.Generic;

class Program
{
    private static readonly string One;

    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [""Zero""] = 0, [One] = 1, [""Two""] = 2 };
    }
}",
index: PropertyIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyInDictionaryInitializer3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [""Zero""] = [|i|] };
    }
}",
@"using System.Collections.Generic;

class Program
{
    public static int i { get; private set; }

    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [""Zero""] = i };
    }
}",
index: PropertyIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateLocalInDictionaryInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [[|key|]] = 0 };
    }
}",
@"using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        string key = null;
        var x = new Dictionary<string, int> { [key] = 0 };
    }
}",
index: LocalIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateLocalInDictionaryInitializer2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [""Zero""] = 0, [[|One|]] = 1, [""Two""] = 2 };
    }
}",
@"using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        string One = null;
        var x = new Dictionary<string, int> { [""Zero""] = 0, [One] = 1, [""Two""] = 2 };
    }
}",
index: LocalIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateLocalInDictionaryInitializer3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var x = new Dictionary<string, int> { [""Zero""] = [|i|] };
    }
}",
@"using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        int i = 0;
        var x = new Dictionary<string, int> { [""Zero""] = i };
    }
}",
index: LocalIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateVariableFromLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class Program
{
    static void Main(string[] args)
    {
        [|goo|] = () => {
            return 0;
        };
    }
}",
@"using System;

class Program
{
    private static Func<int> goo;

    static void Main(string[] args)
    {
        goo = () => {
            return 0;
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateVariableFromLambda2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class Program
{
    static void Main(string[] args)
    {
        [|goo|] = () => {
            return 0;
        };
    }
}",
@"using System;

class Program
{
    public static Func<int> goo { get; private set; }

    static void Main(string[] args)
    {
        goo = () => {
            return 0;
        };
    }
}",
index: ReadonlyFieldIndex);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateVariableFromLambda3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

class Program
{
    static void Main(string[] args)
    {
        [|goo|] = () => {
            return 0;
        };
    }
}",
@"using System;

class Program
{
    static void Main(string[] args)
    {
        Func<int> goo = () =>
        {
            return 0;
        };
    }
}",
index: PropertyIndex);
        }

        [WorkItem(8010, "https://github.com/dotnet/roslyn/issues/8010")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerationFromStaticProperty_Field()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

public class Test
{
    public static int Property1
    {
        get
        {
            return [|_field|];
        }
    }
}",
@"using System;

public class Test
{
    private static int _field;

    public static int Property1
    {
        get
        {
            return _field;
        }
    }
}");
        }

        [WorkItem(8010, "https://github.com/dotnet/roslyn/issues/8010")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerationFromStaticProperty_ReadonlyField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

public class Test
{
    public static int Property1
    {
        get
        {
            return [|_field|];
        }
    }
}",
@"using System;

public class Test
{
    private static readonly int _field;

    public static int Property1
    {
        get
        {
            return _field;
        }
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(8010, "https://github.com/dotnet/roslyn/issues/8010")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerationFromStaticProperty_Property()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

public class Test
{
    public static int Property1
    {
        get
        {
            return [|_field|];
        }
    }
}",
@"using System;

public class Test
{
    public static int Property1
    {
        get
        {
            return _field;
        }
    }

    public static int _field { get; private set; }
}",
index: PropertyIndex);
        }

        [WorkItem(8010, "https://github.com/dotnet/roslyn/issues/8010")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerationFromStaticProperty_Local()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using System;

public class Test
{
    public static int Property1
    {
        get
        {
            return [|_field|];
        }
    }
}",
@"using System;

public class Test
{
    public static int Property1
    {
        get
        {
            int _field = 0;
            return _field;
        }
    }
}",
index: LocalIndex);
        }

        [WorkItem(8358, "https://github.com/dotnet/roslyn/issues/8358")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSameNameAsInstanceVariableInContainingType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Outer
{
    int _field;

    class Inner
    {
        public Inner(int field)
        {
            [|_field|] = field;
        }
    }
}",
@"class Outer
{
    int _field;

    class Inner
    {
        private int _field;

        public Inner(int field)
        {
            _field = field;
        }
    }
}");
        }

        [WorkItem(8358, "https://github.com/dotnet/roslyn/issues/8358")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnStaticWithExistingInstance1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    int _field;

    void M()
    {
        C.[|_field|] = 42;
    }
}");
        }

        [WorkItem(8358, "https://github.com/dotnet/roslyn/issues/8358")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnStaticWithExistingInstance2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    int _field;

    static C()
    {
        [|_field|] = 42;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleRead()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method((int, string) i)
    {
        Method([|tuple|]);
    }
}",
@"class Class
{
    private (int, string) tuple;

    void Method((int, string) i)
    {
        Method(tuple);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleWithOneNameRead()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method((int a, string) i)
    {
        Method([|tuple|]);
    }
}",
@"class Class
{
    private (int a, string) tuple;

    void Method((int a, string) i)
    {
        Method(tuple);
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleWrite()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|tuple|] = (1, ""hello"");
    }
}",
@"class Class
{
    private (int, string) tuple;

    void Method()
    {
        tuple = (1, ""hello"");
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleWithOneNameWrite()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        [|tuple|] = (a: 1, ""hello"");
    }
}",
@"class Class
{
    private (int a, string) tuple;

    void Method()
    {
        tuple = (a: 1, ""hello"");
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleRefReturnProperties()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System;
class C
{
    public void Goo()
    {
        ref int i = ref this.[|Bar|];
    }
}",
@"
using System;
class C
{
    public ref int Bar => throw new NotImplementedException();

    public void Goo()
    {
        ref int i = ref this.Bar;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TupleRefWithField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System;
class C
{
    public void Goo()
    {
        ref int i = ref this.[|bar|];
    }
}",
@"
using System;
class C
{
    private int bar;

    public void Goo()
    {
        ref int i = ref this.bar;
    }
}");
        }

        [WorkItem(17621, "https://github.com/dotnet/roslyn/issues/17621")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithMatchingTypeName1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
using System;

public class Goo
{
    public Goo(String goo)
    {
        [|String|] = goo;
    }
}",
@"
using System;

public class Goo
{
    public Goo(String goo)
    {
        String = goo;
    }

    public string String { get; }
}");
        }

        [WorkItem(17621, "https://github.com/dotnet/roslyn/issues/17621")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithMatchingTypeName2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScript1Async(
@"
using System;

public class Goo
{
    public Goo(String goo)
    {
        [|String|] = goo;
    }
}",
@"
using System;

public class Goo
{
    public Goo(String goo)
    {
        String = goo;
    }

    public string String { get; private set; }
}", index: ReadonlyFieldIndex);
        }

        [WorkItem(18275, "https://github.com/dotnet/roslyn/issues/18275")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
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

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPreferReadOnlyIfAfterReadOnlyAssignment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    private readonly int _goo;

    public Class()
    {
        _goo = 0;
        [|_bar|] = 1;
    }
}",
@"class Class
{
    private readonly int _goo;
    private readonly int _bar;

    public Class()
    {
        _goo = 0;
        _bar = 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPreferReadOnlyIfBeforeReadOnlyAssignment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    private readonly int _goo;

    public Class()
    {
        [|_bar|] = 1;
        _goo = 0;
    }
}",
@"class Class
{
    private readonly int _bar;
    private readonly int _goo;

    public Class()
    {
        _bar = 1;
        _goo = 0;
    }
}");
        }

        [WorkItem(19239, "https://github.com/dotnet/roslyn/issues/19239")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateReadOnlyPropertyInConstructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    public Class()
    {
        [|Bar|] = 1;
    }
}",
@"class Class
{
    public Class()
    {
        Bar = 1;
    }

    public int Bar { get; }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPlaceFieldBasedOnSurroundingStatements()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    private int _goo;
    private int _quux;

    public Class()
    {
        _goo = 0;
        [|_bar|] = 1;
        _quux = 2;
    }
}",
@"class Class
{
    private int _goo;
    private int _bar;
    private int _quux;

    public Class()
    {
        _goo = 0;
        _bar = 1;
        _quux = 2;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPlaceFieldBasedOnSurroundingStatements2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    private int goo;
    private int quux;

    public Class()
    {
        this.goo = 0;
        this.[|bar|] = 1;
        this.quux = 2;
    }
}",
@"class Class
{
    private int goo;
    private int bar;
    private int quux;

    public Class()
    {
        this.goo = 0;
        this.bar = 1;
        this.quux = 2;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPlacePropertyBasedOnSurroundingStatements()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    public int Goo { get; }
    public int Quuz { get; }

    public Class()
    {
        Goo = 0;
        [|Bar|] = 1;
        Quux = 2;
    }
}",
@"class Class
{
    public int Goo { get; }
    public int Bar { get; }
    public int Quuz { get; }

    public Class()
    {
        Goo = 0;
        Bar = 1;
        Quux = 2;
    }
}");
        }

        [WorkItem(19575, "https://github.com/dotnet/roslyn/issues/19575")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotOnGenericCodeParsedAsExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(@"
class C
{
    private void GetEvaluationRuleNames()
    {
        [|IEnumerable|] < Int32 >
        return ImmutableArray.CreateRange();
    }
}");
        }

        [WorkItem(19575, "https://github.com/dotnet/roslyn/issues/19575")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnNonGenericExpressionWithLessThan()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(@"
class C
{
    private void GetEvaluationRuleNames()
    {
        [|IEnumerable|] < Int32
        return ImmutableArray.CreateRange();
    }
}",
@"
class C
{
    public int IEnumerable { get; private set; }

    private void GetEvaluationRuleNames()
    {
        IEnumerable < Int32
        return ImmutableArray.CreateRange();
    }
}");
        }

        [WorkItem(18988, "https://github.com/dotnet/roslyn/issues/18988")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GroupNonReadonlyFieldsTogether()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(@"
class C
{
    public bool isDisposed;

    public readonly int x;
    public readonly int m;

    public C()
    {
        this.[|y|] = 0;
    }
}",
@"
class C
{
    public bool isDisposed;
    private int y;
    public readonly int x;
    public readonly int m;

    public C()
    {
        this.y = 0;
    }
}");
        }

        [WorkItem(18988, "https://github.com/dotnet/roslyn/issues/18988")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GroupReadonlyFieldsTogether()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(@"
class C
{
    public readonly int x;
    public readonly int m;

    public bool isDisposed;

    public C()
    {
        this.[|y|] = 0;
    }
}",
@"
class C
{
    public readonly int x;
    public readonly int m;
    private readonly int y;
    public bool isDisposed;

    public C()
    {
        this.y = 0;
    }
}", index: ReadonlyFieldIndex);
        }

        [WorkItem(20791, "https://github.com/dotnet/roslyn/issues/20791")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithOutOverload1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        Goo(out [|goo|]);
    }

    void Goo(int i) { }
    void Goo(out bool b) { }
}",
@"class Class
{
    private bool goo;

    void Method()
    {
        Goo(out goo);
    }

    void Goo(int i) { }
    void Goo(out bool b) { }
}");
        }

        [WorkItem(20791, "https://github.com/dotnet/roslyn/issues/20791")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithOutOverload2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        Goo([|goo|]);
    }

    void Goo(out bool b) { }
    void Goo(int i) { }
}",
@"class Class
{
    private int goo;

    void Method()
    {
        Goo(goo);
    }

    void Goo(out bool b) { }
    void Goo(int i) { }
}");
        }

        [WorkItem(20791, "https://github.com/dotnet/roslyn/issues/20791")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithRefOverload1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        Goo(ref [|goo|]);
    }

    void Goo(int i) { }
    void Goo(ref bool b) { }
}",
@"class Class
{
    private bool goo;

    void Method()
    {
        Goo(ref goo);
    }

    void Goo(int i) { }
    void Goo(ref bool b) { }
}");
        }

        [WorkItem(20791, "https://github.com/dotnet/roslyn/issues/20791")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithRefOverload2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        Goo([|goo|]);
    }

    void Goo(ref bool b) { }
    void Goo(int i) { }
}",
@"class Class
{
    private int goo;

    void Method()
    {
        Goo(goo);
    }

    void Goo(ref bool b) { }
    void Goo(int i) { }
}");
        }

        [WorkItem(26993, "https://github.com/dotnet/roslyn/issues/26993")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInExpressionBodiedGetter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    public int Property
    {
        get => [|_field|];
    }
}",
@"class Program
{
    private int _field;

    public int Property
    {
        get => _field;
    }
}");
        }

        [WorkItem(26993, "https://github.com/dotnet/roslyn/issues/26993")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInExpressionBodiedGetterWithDifferentAccessibility()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    public int Property
    {
        protected get => [|_field|];
        set => throw new System.NotImplementedException();
    }
}",
@"class Program
{
    private int _field;

    public int Property
    {
        protected get => _field;
        set => throw new System.NotImplementedException();
    }
}");
        }

        [WorkItem(26993, "https://github.com/dotnet/roslyn/issues/26993")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateReadonlyFieldInExpressionBodiedGetter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    public int Property
    {
        get => [|_readonlyField|];
    }
}",
@"class Program
{
    private readonly int _readonlyField;

    public int Property
    {
        get => _readonlyField;
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(26993, "https://github.com/dotnet/roslyn/issues/26993")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyInExpressionBodiedGetter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    public int Property
    {
        get => [|prop|];
    }
}",
@"class Program
{
    public int Property
    {
        get => prop;
    }
    public int prop { get; private set; }
}",
index: PropertyIndex);
        }

        [WorkItem(26993, "https://github.com/dotnet/roslyn/issues/26993")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInExpressionBodiedSetterInferredFromType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    public int Property
    {
        set => [|_field|] = value;
    }
}",
@"class Program
{
    private int _field;

    public int Property
    {
        set => _field = value;
    }
}");
        }

        [WorkItem(26993, "https://github.com/dotnet/roslyn/issues/26993")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInExpressionBodiedLocalFunction()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    public void Method()
    {
        int Local() => [|_field|];
    }
}",
@"class Program
{
    private int _field;

    public void Method()
    {
        int Local() => _field;
    }
}");
        }

        [WorkItem(26993, "https://github.com/dotnet/roslyn/issues/26993")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateReadonlyFieldInExpressionBodiedLocalFunction()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    public void Method()
    {
        int Local() => [|_readonlyField|];
    }
}",
@"class Program
{
    private readonly int _readonlyField;

    public void Method()
    {
        int Local() => _readonlyField;
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(26993, "https://github.com/dotnet/roslyn/issues/26993")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyInExpressionBodiedLocalFunction()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    public void Method()
    {
        int Local() => [|prop|];
    }
}",
@"class Program
{
    public int prop { get; private set; }

    public void Method()
    {
        int Local() => prop;
    }
}",
index: PropertyIndex);
        }

        [WorkItem(26993, "https://github.com/dotnet/roslyn/issues/26993")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInExpressionBodiedLocalFunctionInferredFromType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    public void Method()
    {
        int Local() => [|_field|] = 12;
    }
}",
@"class Program
{
    private int _field;

    public void Method()
    {
        int Local() => _field = 12;
    }
}");
        }

        [WorkItem(26993, "https://github.com/dotnet/roslyn/issues/26993")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInBlockBodiedLocalFunction()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    public void Method()
    {
        int Local()
        {
            return [|_field|];
        }
    }
}",
@"class Program
{
    private int _field;

    public void Method()
    {
        int Local()
        {
            return _field;
        }
    }
}");
        }

        [WorkItem(26993, "https://github.com/dotnet/roslyn/issues/26993")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateReadonlyFieldInBlockBodiedLocalFunction()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    public void Method()
    {
        int Local()
        {
            return [|_readonlyField|];
        }
    }
}",
@"class Program
{
    private readonly int _readonlyField;

    public void Method()
    {
        int Local()
        {
            return _readonlyField;
        }
    }
}",
index: ReadonlyFieldIndex);
        }

        [WorkItem(26993, "https://github.com/dotnet/roslyn/issues/26993")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGeneratePropertyInBlockBodiedLocalFunction()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    public void Method()
    {
        int Local()
        {
            return [|prop|];
        }
    }
}",
@"class Program
{
    public int prop { get; private set; }

    public void Method()
    {
        int Local()
        {
            return prop;
        }
    }
}",
index: PropertyIndex);
        }

        [WorkItem(26993, "https://github.com/dotnet/roslyn/issues/26993")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInBlockBodiedLocalFunctionInferredFromType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Program
{
    public void Method()
    {
        int Local() 
        {
            return [|_field|] = 12;
        }
    }
}",
@"class Program
{
    private int _field;

    public void Method()
    {
        int Local() 
        {
            return _field = 12;
        }
    }
}");
        }

        [WorkItem(26993, "https://github.com/dotnet/roslyn/issues/26993")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInBlockBodiedLocalFunctionInsideLambdaExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System;

class Program
{
    public void Method()
    {
        Action action = () => 
        {
            int Local()
            {
                return [|_field|];
            }
        };
    }
}",
@"
using System;

class Program
{
    private int _field;

    public void Method()
    {
        Action action = () => 
        {
            int Local()
            {
                return _field;
            }
        };
    }
}");
        }

        [WorkItem(26993, "https://github.com/dotnet/roslyn/issues/26993")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenerateFieldInExpressionBodiedLocalFunctionInsideLambdaExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"
using System;

class Program
{
    public void Method()
    {
        Action action = () => 
        {
            int Local() => [|_field|];
        };
    }
}",
@"
using System;

class Program
{
    private int _field;

    public void Method()
    {
        Action action = () => 
        {
            int Local() => _field;
        };
    }
}");
        }

        [WorkItem(26406, "https://github.com/dotnet/roslyn/issues/26406")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIdentifierInsideLock1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        lock ([|goo|])
        {
        }
    }
}",
@"class Class
{
    private object goo;

    void Method()
    {
        lock (goo)
        {
        }
    }
}");
        }

        [WorkItem(26406, "https://github.com/dotnet/roslyn/issues/26406")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIdentifierInsideLock2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        lock ([|goo|])
        {
        }
    }
}",
@"class Class
{
    private readonly object goo;

    void Method()
    {
        lock (goo)
        {
        }
    }
}", index: 1);
        }

        [WorkItem(26406, "https://github.com/dotnet/roslyn/issues/26406")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateVariable)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIdentifierInsideLock3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"class Class
{
    void Method()
    {
        lock ([|goo|])
        {
        }
    }
}",
@"class Class
{
    public object goo { get; private set; }

    void Method()
    {
        lock (goo)
        {
        }
    }
}", index: 2);
        }
    }
}
