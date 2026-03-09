// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeGeneration;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Roslyn.Utilities;
using Xunit;
using CS = Microsoft.CodeAnalysis.CSharp;
using VB = Microsoft.CodeAnalysis.VisualBasic;

namespace Microsoft.CodeAnalysis.Editor.UnitTests.MetadataAsSource
{
    public partial class MetadataAsSourceTests : AbstractMetadataAsSourceTests
    {
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "public class C {}";
            var symbolName = "C";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public class [|C|]
{{
    public C();
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Public Class [|C|]
    Public Sub New()
End Class");
        }

        [WorkItem(546241, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546241")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterface()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "public interface I {}";
            var symbolName = "I";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public interface [|I|]
{{
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Public Interface [|I|]
End Interface");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "public class C {}";
            var symbolName = "C..ctor";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public class C
{{
    public [|C|]();
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Public Class C
    Public Sub [|New|]()
End Class");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "public class C { public void Goo() {} }";
            var symbolName = "C.Goo";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public class C
{{
    public C();

    public void [|Goo|]();
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Public Class C
    Public Sub New()

    Public Sub [|Goo|]()
End Class");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "public class C { public string S; }";
            var symbolName = "C.S";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public class C
{{
    public string [|S|];

    public C();
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Public Class C
    Public [|S|] As String

    Public Sub New()
End Class");
        }

        [WorkItem(546240, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546240")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "public class C { public string S { get; protected set; } }";
            var symbolName = "C.S";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public class C
{{
    public C();

    public string [|S|] {{ get; protected set; }}
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Public Class C
    Public Sub New()

    Public Property [|S|] As String
End Class");
        }

        [WorkItem(546194, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546194")]
        [WorkItem(546291, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546291")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEvent()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "using System; public class C { public event Action E; }";
            var symbolName = "C.E";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

using System;

public class C
{{
    public C();

    public event Action [|E|];
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Imports System

Public Class C
    Public Sub New()

    Public Event [|E|] As Action
End Class");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "public class C { protected class D { } }";
            var symbolName = "C+D";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public class C
{{
    public C();

    protected class [|D|]
    {{
        public D();
    }}
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Public Class C
    Public Sub New()

    Protected Class [|D|]
        Public Sub New()
    End Class
End Class");
        }

        [WorkItem(546195, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546195"), WorkItem(546269, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546269")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEnum()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "public enum E { A, B, C }";
            var symbolName = "E";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public enum [|E|]
{{
    A = 0,
    B = 1,
    C = 2
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Public Enum [|E|]
    A = 0
    B = 1
    C = 2
End Enum");
        }

        [WorkItem(546195, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546195"), WorkItem(546269, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546269")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEnumFromField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "public enum E { A, B, C }";
            var symbolName = "E.C";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public enum E
{{
    A = 0,
    B = 1,
    [|C|] = 2
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Public Enum E
    A = 0
    B = 1
    [|C|] = 2
End Enum");
        }

        [WorkItem(546273, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546273")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEnumWithUnderlyingType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "public enum E : short { A = 0, B = 1, C = 2 }";
            var symbolName = "E.C";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public enum E : short
{{
    A = 0,
    B = 1,
    [|C|] = 2
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Public Enum E As Short
    A = 0
    B = 1
    [|C|] = 2
End Enum");
        }

        [WorkItem(650741, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/650741")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEnumWithOverflowingUnderlyingType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "public enum E : ulong { A = 9223372036854775808 }";
            var symbolName = "E.A";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public enum E : ulong
{{
    [|A|] = 9223372036854775808
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Public Enum E As ULong
    [|A|] = 9223372036854775808UL
End Enum");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEnumWithDifferentValues()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "public enum E : short { A = 1, B = 2, C = 3 }";
            var symbolName = "E.C";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public enum E : short
{{
    A = 1,
    B = 2,
    [|C|] = 3
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Public Enum E As Short
    A = 1
    B = 2
    [|C|] = 3
End Enum");
        }

        [WorkItem(546198, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546198")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestTypeInNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "namespace N { public class C {} }";
            var symbolName = "N.C";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

namespace N
{{
    public class [|C|]
    {{
        public C();
    }}
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Namespace N
    Public Class [|C|]
        Public Sub New()
    End Class
End Namespace");
        }

        [WorkItem(546223, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546223")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInlineConstant()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = @"public class C { public const string S = ""Hello mas""; }";
            var symbolName = "C.S";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public class C
{{
    public const string [|S|] = ""Hello mas"";

    public C();
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Public Class C
    Public Const [|S|] As String = ""Hello mas""

    Public Sub New()
End Class");
        }

        [WorkItem(546221, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546221")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInlineTypeOf()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = @"
using System;

public class MyTypeAttribute : Attribute
{
    public MyTypeAttribute(Type type) {}
}

[MyType(typeof(string))]
public class C {}";

            var symbolName = "C";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

[MyType(typeof(string))]
public class [|C|]
{{
    public C();
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

<MyType(GetType(String))>
Public Class [|C|]
    Public Sub New()
End Class");
        }

        [WorkItem(546231, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546231")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNoDefaultConstructorInStructs()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "public struct S {}";
            var symbolName = "S";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public struct [|S|]
{{
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Public Structure [|S|]
End Structure");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestReferenceDefinedType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "public class C { public static C Create() { return new C(); } }";
            var symbolName = "C";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public class [|C|]
{{
    public C();

    public static C Create();
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Public Class [|C|]
    Public Sub New()

    Public Shared Function Create() As C
End Class");
        }

        [WorkItem(546227, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546227")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenericType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "public class G<SomeType> { public SomeType S; }";
            var symbolName = "G`1";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public class [|G|]<SomeType>
{{
    public SomeType S;

    public G();
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Public Class [|G|](Of SomeType)
    Public S As SomeType

    Public Sub New()
End Class");
        }

        [WorkItem(546227, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546227")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestGenericDelegate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "public class C { public delegate void D<SomeType>(SomeType s); }";
            var symbolName = "C+D`1";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public class C
{{
    public C();

    public delegate void [|D|]<SomeType>(SomeType s);
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Public Class C
    Public Sub New()
    Public Delegate Sub [|D|](Of SomeType)(s As SomeType)
End Class");
        }

        [WorkItem(546200, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546200")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = @"
using System;

namespace N
{
    public class WorkingAttribute : Attribute
    {
        public WorkingAttribute(bool working) {}
    }
}

[N.Working(true)]
public class C {}";

            var symbolName = "C";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

using N;

[Working(true)]
public class [|C|]
{{
    public C();
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Imports N

<Working(True)>
Public Class [|C|]
    Public Sub New()
End Class");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSymbolIdMatchesMetadata()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestSymbolIdMatchesMetadataAsync(LanguageNames.CSharp);
            await TestSymbolIdMatchesMetadataAsync(LanguageNames.VisualBasic);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotReusedOnAssemblyDiffers()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestNotReusedOnAssemblyDiffersAsync(LanguageNames.CSharp);
            await TestNotReusedOnAssemblyDiffersAsync(LanguageNames.VisualBasic);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestThrowsOnGenerateNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var namespaceSymbol = CodeGenerationSymbolFactory.CreateNamespaceSymbol("Outerspace");

            using (var context = TestContext.Create())
            {
                await Assert.ThrowsAsync<ArgumentException>(async () =>
                {
                    await context.GenerateSourceAsync(namespaceSymbol);
                });
            }
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestReuseGenerateMemberOfGeneratedType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = "public class C { public bool Is; }";

            using (var context = TestContext.Create(LanguageNames.CSharp, SpecializedCollections.SingletonEnumerable(metadataSource)))
            {
                var a = await context.GenerateSourceAsync("C");
                var b = await context.GenerateSourceAsync("C.Is");
                context.VerifyDocumentReused(a, b);
            }
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestReuseRepeatGeneration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            using (var context = TestContext.Create())
            {
                var a = await context.GenerateSourceAsync();
                var b = await context.GenerateSourceAsync();
                context.VerifyDocumentReused(a, b);
            }
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWorkspaceContextHasReasonableProjectName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            using (var context = TestContext.Create())
            {
                var compilation = await context.DefaultProject.GetCompilationAsync();
                var result = await context.GenerateSourceAsync(compilation.ObjectType);
                var openedDocument = context.GetDocument(result);

                Assert.Equal(openedDocument.Project.AssemblyName, "mscorlib");
                Assert.Equal(openedDocument.Project.Name, "mscorlib");
            }
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestReuseGenerateFromDifferentProject()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            using (var context = TestContext.Create())
            {
                var projectId = ProjectId.CreateNewId();
                var project = context.CurrentSolution.AddProject(projectId, "ProjectB", "ProjectB", LanguageNames.CSharp).GetProject(projectId)
                    .WithMetadataReferences(context.DefaultProject.MetadataReferences)
                    .WithCompilationOptions(new CS.CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

                var a = await context.GenerateSourceAsync(project: context.DefaultProject);
                var b = await context.GenerateSourceAsync(project: project);
                context.VerifyDocumentReused(a, b);
            }
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotReusedGeneratingForDifferentLanguage()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            using (var context = TestContext.Create(LanguageNames.CSharp))
            {
                var projectId = ProjectId.CreateNewId();
                var project = context.CurrentSolution.AddProject(projectId, "ProjectB", "ProjectB", LanguageNames.VisualBasic).GetProject(projectId)
                    .WithMetadataReferences(context.DefaultProject.MetadataReferences)
                    .WithCompilationOptions(new VB.VisualBasicCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

                var a = await context.GenerateSourceAsync(project: context.DefaultProject);
                var b = await context.GenerateSourceAsync(project: project);
                context.VerifyDocumentNotReused(a, b);
            }
        }

        [WorkItem(546311, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/546311")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FormatMetadataAsSource()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            using (var context = TestContext.Create(LanguageNames.CSharp))
            {
                var file = await context.GenerateSourceAsync("System.Console", project: context.DefaultProject);
                var document = context.GetDocument(file);
                await Formatting.Formatter.FormatAsync(document);
            }
        }

        [WorkItem(530829, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530829")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IndexedProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = @"
Public Class C
    Public Property IndexProp(ByVal p1 As Integer) As String
        Get
            Return Nothing
        End Get
        Set(ByVal value As String)

        End Set
    End Property
End Class";
            var expected = $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public class C
{{
    public C();

    public string [|get_IndexProp|](int p1);
    public void set_IndexProp(int p1, string value);
}}";
            var symbolName = "C.get_IndexProp";
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, expected);
        }

        [WorkItem(566688, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/566688")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AttributeReferencingInternalNestedType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = @"using System;
[My(typeof(D))]
public class C
{
    public C() { }

    internal class D { }
}

public class MyAttribute : Attribute
{
    public MyAttribute(Type t) { }
}";

            var expected = $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

[My(typeof(D))]
public class [|C|]
{{
    public C();
}}";
            var symbolName = "C";
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, expected);
        }

        [WorkItem(530978, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530978")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAttributesOnMembers()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = @"using System;

[Obsolete]
public class C
{
    [Obsolete]
    [ThreadStatic]
    public int field1;

    [Obsolete]
    public int prop1 { get; set; }

    [Obsolete]
    public int prop2 { get { return 10; } set {} }

    [Obsolete]
    public void method1() {}

    [Obsolete]
    public C() {}

    [Obsolete]
    ~C() {}

    [Obsolete]
    public int this[int x] { get { return 10; } set {} }

    [Obsolete]
    public event Action event1;

    [Obsolete]
    public event Action event2 { add {} remove {}}

    public void method2([System.Runtime.CompilerServices.CallerMemberName] string name = """") {}

    [Obsolete]
    public static C operator + (C c1, C c2) { return new C(); }
}
";
            var expectedCS = $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

[DefaultMember(""Item"")]
[Obsolete]
public class [|C|]
{{
    [Obsolete]
    [ThreadStatic]
    public int field1;

    [Obsolete]
    public C();

    [Obsolete]
    ~C();

    [Obsolete]
    public int this[int x] {{ get; set; }}

    [Obsolete]
    public int prop1 {{ get; set; }}
    [Obsolete]
    public int prop2 {{ get; set; }}

    [Obsolete]
    public event Action event1;
    [Obsolete]
    public event Action event2;

    [Obsolete]
    public void method1();
    public void method2([CallerMemberName] string name = """");

    [Obsolete]
    public static C operator +(C c1, C c2);
}}";
            var symbolName = "C";
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, expectedCS);

            var expectedVB = $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Imports System
Imports System.Reflection
Imports System.Runtime.CompilerServices

<DefaultMember(""Item"")> <Obsolete>
Public Class [|C|]
    <Obsolete> <ThreadStatic>
    Public field1 As Integer

    <Obsolete>
    Public Sub New()

    <Obsolete>
    Public Property prop1 As Integer
    <Obsolete>
    Public Property prop2 As Integer
    <Obsolete>
    Default Public Property Item(x As Integer) As Integer

    <Obsolete>
    Public Event event1 As Action
    <Obsolete>
    Public Event event2 As Action

    <Obsolete>
    Public Sub method1()
    Public Sub method2(<CallerMemberName> Optional name As String = """")
    <Obsolete>
    Protected Overrides Sub Finalize()

    <Obsolete>
    Public Shared Operator +(c1 As C, c2 As C) As C
End Class";
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, expectedVB);
        }

        [WorkItem(530923, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530923")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEmptyLineBetweenMembers()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = @"using System;

public class C
{
    public int field1;
    public int prop1 { get; set; }
    public int field2;
    public int prop2 { get { return 10; } set {} }
    public void method1() {}
    public C() {}
    public void method2([System.Runtime.CompilerServices.CallerMemberName] string name = """") {}
    ~C() {}
    public int this[int x] { get { return 10; } set {} }
    public event Action event1;
    public static C operator + (C c1, C c2) { return new C(); }
    public event Action event2 { add {} remove {}}
    public static C operator - (C c1, C c2) { return new C(); }
}
";
            var expectedCS = $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

[DefaultMember(""Item"")]
public class [|C|]
{{
    public int field1;
    public int field2;

    public C();

    ~C();

    public int this[int x] {{ get; set; }}

    public int prop1 {{ get; set; }}
    public int prop2 {{ get; set; }}

    public event Action event1;
    public event Action event2;

    public void method1();
    public void method2([CallerMemberName] string name = """");

    public static C operator +(C c1, C c2);
    public static C operator -(C c1, C c2);
}}";
            var symbolName = "C";
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, expectedCS);

            var expectedVB = $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Imports System
Imports System.Reflection
Imports System.Runtime.CompilerServices

<DefaultMember(""Item"")>
Public Class [|C|]
    Public field1 As Integer
    Public field2 As Integer

    Public Sub New()

    Public Property prop1 As Integer
    Public Property prop2 As Integer
    Default Public Property Item(x As Integer) As Integer

    Public Event event1 As Action
    Public Event event2 As Action

    Public Sub method1()
    Public Sub method2(<CallerMemberName> Optional name As String = """")
    Protected Overrides Sub Finalize()

    Public Shared Operator +(c1 As C, c2 As C) As C
    Public Shared Operator -(c1 As C, c2 As C) As C
End Class";
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, expectedVB);
        }

        [WorkItem(728644, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/728644")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEmptyLineBetweenMembers2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var source = @"
using System;

/// <summary>T:IGoo</summary>
public interface IGoo
{
    /// <summary>P:IGoo.Prop1</summary>
    Uri Prop1 { get; set; }
    /// <summary>M:IGoo.Method1</summary>
    Uri Method1();
}
";
            var symbolName = "IGoo";
            var expectedCS = $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

using System;

//
// {FeaturesResources.Summary_colon}
//     T:IGoo
public interface [|IGoo|]
{{
    //
    // {FeaturesResources.Summary_colon}
    //     P:IGoo.Prop1
    Uri Prop1 {{ get; set; }}

    //
    // {FeaturesResources.Summary_colon}
    //     M:IGoo.Method1
    Uri Method1();
}}";
            await GenerateAndVerifySourceAsync(source, symbolName, LanguageNames.CSharp, expectedCS, includeXmlDocComments: true);

            var expectedVB = $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Imports System

'
' {FeaturesResources.Summary_colon}
'     T:IGoo
Public Interface [|IGoo|]
    '
    ' {FeaturesResources.Summary_colon}
    '     P:IGoo.Prop1
    Property Prop1 As Uri

    '
    ' {FeaturesResources.Summary_colon}
    '     M:IGoo.Method1
    Function Method1() As Uri
End Interface";
            await GenerateAndVerifySourceAsync(source, symbolName, LanguageNames.VisualBasic, expectedVB, includeXmlDocComments: true);
        }

        [WorkItem(679114, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/679114"), WorkItem(715013, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/715013")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDefaultValueEnum()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var source = @"
using System.IO;

public class Test
{
    public void goo(FileOptions options = 0) {}
}
";
            var symbolName = "Test";
            var expectedCS = $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

using System.IO;

public class [|Test|]
{{
    public Test();

    public void goo(FileOptions options = FileOptions.None);
}}";
            await GenerateAndVerifySourceAsync(source, symbolName, LanguageNames.CSharp, expectedCS);

            var expectedVB = $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Imports System.IO

Public Class [|Test|]
    Public Sub New()

    Public Sub goo(Optional options As FileOptions = FileOptions.None)
End Class";
            await GenerateAndVerifySourceAsync(source, symbolName, LanguageNames.VisualBasic, expectedVB);
        }

        [WorkItem(651261, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/651261")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNullAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var source = @"
using System;

[Test(null)]
public class TestAttribute : Attribute
{
    public TestAttribute(int[] i)
    {
    }
}";
            var symbolName = "TestAttribute";
            var expectedCS = $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

using System;

[Test(null)]
public class [|TestAttribute|] : Attribute
{{
    public TestAttribute(int[] i);
}}";
            await GenerateAndVerifySourceAsync(source, symbolName, LanguageNames.CSharp, expectedCS);

            var expectedVB = $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Imports System

<Test(Nothing)>
Public Class [|TestAttribute|]
    Inherits Attribute

    Public Sub New(i() As Integer)
End Class";
            await GenerateAndVerifySourceAsync(source, symbolName, LanguageNames.VisualBasic, expectedVB);
        }

        [WorkItem(897006, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/897006")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNavigationViaReducedExtensionMethodCS()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadata = @"using System;
public static class ObjectExtensions
{
    public static void M(this object o, int x) { }
}";
            var sourceWithSymbolReference = @"
class C
{
    void M()
    {
        new object().[|M|](5);
    }
}";
            var expected = $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public static class ObjectExtensions
{{
    public static void [|M|](this object o, int x);
}}";

            using (var context = TestContext.Create(
                LanguageNames.CSharp,
                SpecializedCollections.SingletonEnumerable(metadata),
                includeXmlDocComments: false,
                sourceWithSymbolReference: sourceWithSymbolReference))
            {
                var navigationSymbol = await context.GetNavigationSymbolAsync();
                var metadataAsSourceFile = await context.GenerateSourceAsync(navigationSymbol);
                context.VerifyResult(metadataAsSourceFile, expected);
            }
        }

        [WorkItem(897006, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/897006")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNavigationViaReducedExtensionMethodVB()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadata = @"Imports System.Runtime.CompilerServices
Namespace NS
    Public Module StringExtensions
        <Extension()>
        Public Sub M(ByVal o As String, x As Integer)
        End Sub
    End Module
End Namespace";
            var sourceWithSymbolReference = @"
Imports NS.StringExtensions
Public Module C
    Sub M()
        Dim s = ""Yay""
        s.[|M|](1)
    End Sub
End Module";
            var expected = $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Imports System.Runtime.CompilerServices

Namespace NS
    <Extension>
    Public Module StringExtensions <Extension>
        Public Sub [|M|](o As String, x As Integer)
    End Module
End Namespace";

            using (var context = TestContext.Create(
                LanguageNames.VisualBasic,
                SpecializedCollections.SingletonEnumerable(metadata),
                includeXmlDocComments: false,
                sourceWithSymbolReference: sourceWithSymbolReference))
            {
                var navigationSymbol = await context.GetNavigationSymbolAsync();
                var metadataAsSourceFile = await context.GenerateSourceAsync(navigationSymbol);
                context.VerifyResult(metadataAsSourceFile, expected);
            }
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndexersAndOperators()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = @"public class Program
{
    public int this[int x]
    {
        get
        {
            return 0;
        }
        set
        {

        }
    }

    public static  Program operator + (Program p1, Program p2)
    {
        return new Program();
    }
}";
            var symbolName = "Program";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

using System.Reflection;

[DefaultMember(""Item"")]
public class [|Program|]
{{
    public Program();

    public int this[int x] {{ get; set; }}

    public static Program operator +(Program p1, Program p2);
}}");
            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.VisualBasic, $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Imports System.Reflection

<DefaultMember(""Item"")>
Public Class [|Program|]
    Public Sub New()

    Default Public Property Item(x As Integer) As Integer

    Public Shared Operator +(p1 As Program, p2 As Program) As Program
End Class");
        }

        [WorkItem(15387, "https://github.com/dotnet/roslyn/issues/15387")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestComImport1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = @"
using System.Runtime.InteropServices;

[ComImport]
[Guid(""666A175D-2448-447A-B786-CCC82CBEF156"")]
public interface IComImport
{
    void MOverload();
    void X();
    void MOverload(int i);
    int Prop { get; }
}";
            var symbolName = "IComImport";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

using System.Runtime.InteropServices;

[Guid(""666A175D-2448-447A-B786-CCC82CBEF156"")]
public interface [|IComImport|]
{{
    void MOverload();
    void X();
    void MOverload(int i);

    int Prop {{ get; }}
}}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOptionalParameterWithDefaultLiteral()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadataSource = @"
using System.Threading;

public class C {
    public void M(CancellationToken cancellationToken = default(CancellationToken)) { }
}";
            var symbolName = "C";

            await GenerateAndVerifySourceAsync(metadataSource, symbolName, LanguageNames.CSharp, $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

using System.Threading;

public class [|C|]
{{
    public C();

    public void M(CancellationToken cancellationToken = default);
}}", languageVersion: "CSharp7_1");
        }

        [WorkItem(446567, "https://devdiv.visualstudio.com/DevDiv/_workitems?id=446567")]
        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDocCommentsWithUnixNewLine()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var source = @"
using System;

/// <summary>T:IGoo" + "\n/// ABCDE\n" + @"/// FGHIJK</summary>
public interface IGoo
{
    /// <summary>P:IGoo.Prop1" + "\n/// ABCDE\n" + @"/// FGHIJK</summary>
    Uri Prop1 { get; set; }
    /// <summary>M:IGoo.Method1" + "\n/// ABCDE\n" + @"/// FGHIJK</summary>
    Uri Method1();
}
";
            var symbolName = "IGoo";
            var expectedCS = $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

using System;

//
// {FeaturesResources.Summary_colon}
//     T:IGoo ABCDE FGHIJK
public interface [|IGoo|]
{{
    //
    // {FeaturesResources.Summary_colon}
    //     P:IGoo.Prop1 ABCDE FGHIJK
    Uri Prop1 {{ get; set; }}

    //
    // {FeaturesResources.Summary_colon}
    //     M:IGoo.Method1 ABCDE FGHIJK
    Uri Method1();
}}";
            await GenerateAndVerifySourceAsync(source, symbolName, LanguageNames.CSharp, expectedCS, includeXmlDocComments: true);

            var expectedVB = $@"#Region ""{FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null""
' {CodeAnalysisResources.InMemoryAssembly}
#End Region

Imports System

'
' {FeaturesResources.Summary_colon}
'     T:IGoo ABCDE FGHIJK
Public Interface [|IGoo|]
    '
    ' {FeaturesResources.Summary_colon}
    '     P:IGoo.Prop1 ABCDE FGHIJK
    Property Prop1 As Uri

    '
    ' {FeaturesResources.Summary_colon}
    '     M:IGoo.Method1 ABCDE FGHIJK
    Function Method1() As Uri
End Interface";
            await GenerateAndVerifySourceAsync(source, symbolName, LanguageNames.VisualBasic, expectedVB, includeXmlDocComments: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnmanagedCSharpConstraint_Type()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadata = @"using System;
public class TestType<T> where T : unmanaged
{
}";
            var sourceWithSymbolReference = @"
class C
{
    void M()
    {
        var obj = new [|TestType|]&lt;int&gt;();
    }
}";
            var expected = $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public class [|TestType|]<T> where T : unmanaged
{{
    public TestType();
}}";

            using (var context = TestContext.Create(
                LanguageNames.CSharp,
                SpecializedCollections.SingletonEnumerable(metadata),
                includeXmlDocComments: false,
                languageVersion: "CSharp7_3",
                sourceWithSymbolReference: sourceWithSymbolReference))
            {
                var navigationSymbol = await context.GetNavigationSymbolAsync();
                var metadataAsSourceFile = await context.GenerateSourceAsync(navigationSymbol);
                context.VerifyResult(metadataAsSourceFile, expected);
            }
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnmanagedCSharpConstraint_Method()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadata = @"using System;
public class TestType
{
    public void M<T>() where T : unmanaged
    {
    }
}";
            var sourceWithSymbolReference = @"
class C
{
    void M()
    {
        var obj = new TestType().[|M|]&lt;int&gt;();
    }
}";
            var expected = $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public class TestType
{{
    public TestType();

    public void [|M|]<T>() where T : unmanaged;
}}";

            using (var context = TestContext.Create(
                LanguageNames.CSharp,
                SpecializedCollections.SingletonEnumerable(metadata),
                includeXmlDocComments: false,
                languageVersion: "CSharp7_3",
                sourceWithSymbolReference: sourceWithSymbolReference))
            {
                var navigationSymbol = await context.GetNavigationSymbolAsync();
                var metadataAsSourceFile = await context.GenerateSourceAsync(navigationSymbol);
                context.VerifyResult(metadataAsSourceFile, expected);
            }
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnmanagedCSharpConstraint_Delegate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var metadata = @"using System;
public delegate void D<T>() where T : unmanaged;";
            var sourceWithSymbolReference = @"
class C
{
    void M([|D|]&lt;int&gt; lambda)
    {
    }
}";
            var expected = $@"#region {FeaturesResources.Assembly} ReferencedAssembly, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// {CodeAnalysisResources.InMemoryAssembly}
#endregion

public delegate void [|D|]<T>() where T : unmanaged;";

            using (var context = TestContext.Create(
                LanguageNames.CSharp,
                SpecializedCollections.SingletonEnumerable(metadata),
                includeXmlDocComments: false,
                languageVersion: "CSharp7_3",
                sourceWithSymbolReference: sourceWithSymbolReference))
            {
                var navigationSymbol = await context.GetNavigationSymbolAsync();
                var metadataAsSourceFile = await context.GenerateSourceAsync(navigationSymbol);
                context.VerifyResult(metadataAsSourceFile, expected);
            }
        }
    }
}
