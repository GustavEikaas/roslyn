// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.GenerateType;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics.GenerateTypeTests
{
    public partial class GenerateTypeTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        #region SameProject
        #region SameProject_SameFile 
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeDefaultValues()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    void Main()
    {
        [|Goo$$|] f;
    }
}",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"class Program
{
    void Main()
    {
        Goo f;
    }
}

class Goo
{
}",
isNewFile: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeInsideNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    void Main()
    {
        [|A.Goo$$|] f;
    }
}

namespace A
{
}",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"class Program
{
    void Main()
    {
        A.Goo f;
    }
}

namespace A
{
    class Goo
    {
    }
}",
isNewFile: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeInsideQualifiedNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A.B
{
}",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"class Program
{
    void Main()
    {
        A.B.Goo f;
    }
}
namespace A.B
{
    class Goo
    {
    }
}",
isNewFile: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeWithinQualifiedNestedNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    void Main()
    {
        [|A.B.C.Goo$$|] f;
    }
}
namespace A.B
{
    namespace C
    {
    }
}",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"class Program
{
    void Main()
    {
        A.B.C.Goo f;
    }
}
namespace A.B
{
    namespace C
    {
        class Goo
        {
        }
    }
}",
isNewFile: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeWithinNestedQualifiedNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    void Main()
    {
        [|A.B.C.Goo$$|] f;
    }
}
namespace A
{
    namespace B.C
    {
    }
}",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"class Program
{
    void Main()
    {
        A.B.C.Goo f;
    }
}
namespace A
{
    namespace B.C
    {
        class Goo
        {
        }
    }
}",
isNewFile: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeWithConstructorMembers()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var f = new [|$$Goo|](bar: 1, baz: 2);
    }
}",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"class Program
{
    static void Main(string[] args)
    {
        var f = new Goo(bar: 1, baz: 2);
    }
}

class Goo
{
    private int bar;
    private int baz;

    public Goo(int bar, int baz)
    {
        this.bar = bar;
        this.baz = baz;
    }
}",
isNewFile: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeWithBaseTypes()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"using System.Collections.Generic;
class Program
{
    static void Main(string[] args)
    {
        List<int> f = new [|$$Goo|]();
    }
}",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"using System.Collections.Generic;
class Program
{
    static void Main(string[] args)
    {
        List<int> f = new Goo();
    }
}

class Goo : List<int>
{
}",
isNewFile: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeWithPublicInterface()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    void Main()
    {
        [|A.B.C.Goo$$|] f;
    }
}
namespace A
{
    namespace B.C
    {
    }
}",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"class Program
{
    void Main()
    {
        A.B.C.Goo f;
    }
}
namespace A
{
    namespace B.C
    {
        public interface Goo
        {
        }
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeWithInternalStruct()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    void Main()
    {
        [|A.B.C.Goo$$|] f;
    }
}
namespace A
{
    namespace B.C
    {
    }
}",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"class Program
{
    void Main()
    {
        A.B.C.Goo f;
    }
}
namespace A
{
    namespace B.C
    {
        internal struct Goo
        {
        }
    }
}",
accessibility: Accessibility.Internal,
typeKind: TypeKind.Struct,
isNewFile: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeWithDefaultEnum()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A
{
    namespace B
    {
    }
}",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"class Program
{
    void Main()
    {
        A.B.Goo f;
    }
}
namespace A
{
    namespace B
    {
        enum Goo
        {
        }
    }
}",
accessibility: Accessibility.NotApplicable,
typeKind: TypeKind.Enum,
isNewFile: false);
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeWithDefaultEnum_DefaultNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    void Main()
    {
        [|Goo$$|] f;
    }
}",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"using ConsoleApplication;

class Program
{
    void Main()
    {
        Goo f;
    }
}

namespace ConsoleApplication
{
    enum Goo
    {
    }
}",
defaultNamespace: "ConsoleApplication",
accessibility: Accessibility.NotApplicable,
typeKind: TypeKind.Enum,
isNewFile: false);
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeWithDefaultEnum_DefaultNamespace_NotSimpleName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A
{
    namespace B
    {
    }
}",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"class Program
{
    void Main()
    {
        A.B.Goo f;
    }
}
namespace A
{
    namespace B
    {
        enum Goo
        {
        }
    }
}",
defaultNamespace: "ConsoleApplication",
accessibility: Accessibility.NotApplicable,
typeKind: TypeKind.Enum,
isNewFile: false);
        }
        #endregion

        // Working is very similar to the adding to the same file
        #region SameProject_ExistingFile
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeInExistingEmptyFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A.B
{
}
                        </Document>
                        <Document FilePath=""Test2.cs"">

                        </Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace A.B
{
    public interface Goo
    {
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: false,
existingFilename: "Test2.cs");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeInExistingEmptyFile_Usings_Folders()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|Goo$$|] f;
    }
}</Document>
                        <Document Folders= ""outer\inner"" FilePath=""Test2.cs"">

                        </Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace outer.inner
{
    public interface Goo
    {
    }
}",
checkIfUsingsIncluded: true,
expectedTextWithUsings: @"
using outer.inner;

class Program
{
    void Main()
    {
        Goo f;
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: false,
existingFilename: "Test2.cs");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeInExistingEmptyFile_Usings_DefaultNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|Goo$$|] f;
    }
}</Document>
                        <Document FilePath=""Test2.cs"">

                        </Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace ConsoleApplication
{
    public interface Goo
    {
    }
}",
defaultNamespace: "ConsoleApplication",
checkIfUsingsIncluded: true,
expectedTextWithUsings: @"
using ConsoleApplication;

class Program
{
    void Main()
    {
        Goo f;
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: false,
existingFilename: "Test2.cs");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeInExistingEmptyFile_Usings_Folders_DefaultNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|Goo$$|] f;
    }
}</Document>
                        <Document Folders= ""outer\inner"" FilePath=""Test2.cs"">

                        </Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace ConsoleApplication.outer.inner
{
    public interface Goo
    {
    }
}",
defaultNamespace: "ConsoleApplication",
checkIfUsingsIncluded: true,
expectedTextWithUsings: @"
using ConsoleApplication.outer.inner;

class Program
{
    void Main()
    {
        Goo f;
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: false,
existingFilename: "Test2.cs");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeInExistingEmptyFile_NoUsings_Folders_NotSimpleName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A.B
{
}
                        </Document>
                        <Document FilePath=""Test2.cs"" Folders= ""outer\inner"">

                        </Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace A.B
{
    public interface Goo
    {
    }
}",
checkIfUsingsNotIncluded: true,
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: false,
existingFilename: "Test2.cs");
        }
        #endregion

        #region SameProject_NewFile
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeInNewFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A.B
{
}
                        </Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace A.B
{
    public interface Goo
    {
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: true,
newFileFolderContainers: ImmutableArray<string>.Empty,
newFileName: "Test2.cs");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_UsingsNotNeeded_InNewFile_InFolder()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
namespace outer
{
    namespace inner
    {
        class Program
        {
            void Main()
            {
                [|Goo$$|] f;
            }
        }
    }
}
                        </Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace outer.inner
{
    public interface Goo
    {
    }
}",
checkIfUsingsNotIncluded: true,
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: true,
newFileFolderContainers: ImmutableArray.Create("outer", "inner"),
newFileName: "Test2.cs");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_UsingsNeeded_InNewFile_InFolder()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|Goo$$|] f;
    }
}</Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace outer.inner
{
    public interface Goo
    {
    }
}",
checkIfUsingsIncluded: true,
expectedTextWithUsings: @"
using outer.inner;

class Program
{
    void Main()
    {
        Goo f;
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: true,
newFileFolderContainers: ImmutableArray.Create("outer", "inner"),
newFileName: "Test2.cs");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_UsingsNotNeeded_InNewFile_InFolder_NotSimpleName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A.B
{
}
                        </Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace A.B
{
    public interface Goo
    {
    }
}",
checkIfUsingsNotIncluded: true,
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: true,
newFileFolderContainers: ImmutableArray.Create("outer", "inner"),
newFileName: "Test2.cs");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_UsingsNeeded_InNewFile_InFolder_DefaultNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|Goo$$|] f;
    }
}</Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace ConsoleApplication.outer.inner
{
    public interface Goo
    {
    }
}",
defaultNamespace: "ConsoleApplication",
checkIfUsingsIncluded: true,
expectedTextWithUsings: @"
using ConsoleApplication.outer.inner;

class Program
{
    void Main()
    {
        Goo f;
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: true,
newFileFolderContainers: ImmutableArray.Create("outer", "inner"),
newFileName: "Test2.cs");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_UsingsNotNeeded_InNewFile_InFolder_DefaultNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
namespace ConsoleApplication.outer
{
    class Program
    {
        void Main()
        {
            [|Goo$$|] f;
        }
    }
}</Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace ConsoleApplication.outer
{
    public interface Goo
    {
    }
}",
defaultNamespace: "ConsoleApplication",
checkIfUsingsIncluded: true,
expectedTextWithUsings: @"
namespace ConsoleApplication.outer
{
    class Program
    {
        void Main()
        {
            Goo f;
        }
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: true,
newFileFolderContainers: ImmutableArray.Create("outer"),
newFileName: "Test2.cs");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_UsingsNotNeeded_InNewFile_InFolder_DefaultNamespace_NotSimpleName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}

namespace A.B
{
}</Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace A.B
{
    public interface Goo
    {
    }
}",
defaultNamespace: "ConsoleApplication",
checkIfUsingsIncluded: true,
expectedTextWithUsings: @"
class Program
{
    void Main()
    {
        A.B.Goo f;
    }
}

namespace A.B
{
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: true,
newFileFolderContainers: ImmutableArray.Create("outer"),
newFileName: "Test2.cs");
        }

        [WorkItem(898452, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/898452")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_InValidFolderNameNotMadeNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
namespace outer
{
    namespace inner
    {
        class Program
        {
            void Main()
            {
                [|Goo$$|] f;
            }
        }
    }
}</Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
defaultNamespace: "ConsoleApplication",
expected: @"namespace ConsoleApplication
{
    public interface Goo
    {
    }
}",
checkIfUsingsIncluded: true,
expectedTextWithUsings: @"
using ConsoleApplication;

namespace outer
{
    namespace inner
    {
        class Program
        {
            void Main()
            {
                Goo f;
            }
        }
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: true,
areFoldersValidIdentifiers: false,
newFileFolderContainers: ImmutableArray.Create("123", "456"),
newFileName: "Test2.cs");
        }

        #endregion

        #endregion
        #region SameLanguageDifferentProject
        #region SameLanguageDifferentProject_ExistingFile
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoSameLanguageDifferentProjectEmptyFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A.B
{
}
                        </Document>
                    </Project>
                    <Project Language=""C#"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                        <Document FilePath=""Test2.cs"">
                        </Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace A.B
{
    public interface Goo
    {
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: false,
existingFilename: "Test2.cs",
projectName: "Assembly2");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoSameLanguageDifferentProjectExistingFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A.B
{
}
                        </Document>
                    </Project>
                    <Project Language=""C#"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                        <Document Folders=""outer\inner"" FilePath=""Test2.cs"">
namespace A
{
    namespace B
    {
    }
}</Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"
namespace A
{
    namespace B
    {
        public interface Goo
        {
        }
    }
}",
checkIfUsingsNotIncluded: true,
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: false,
existingFilename: "Test2.cs",
projectName: "Assembly2");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoSameLanguageDifferentProjectExistingFile_Usings_Folders()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|Goo$$|] f;
    }
}</Document>
                    </Project>
                    <Project Language=""C#"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                        <Document Folders=""outer\inner"" FilePath=""Test2.cs"">
namespace A
{
    namespace B
    {
    }
}</Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"
namespace A
{
    namespace B
    {
    }
}

namespace outer.inner
{
    public interface Goo
    {
    }
}",
checkIfUsingsIncluded: true,
expectedTextWithUsings: @"
using outer.inner;

class Program
{
    void Main()
    {
        Goo f;
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: false,
existingFilename: "Test2.cs",
projectName: "Assembly2");
        }

        #endregion
        #region SameLanguageDifferentProject_NewFile
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoSameLanguageDifferentProjectNewFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A.B
{
}
                        </Document>
                    </Project>
                    <Project Language=""C#"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace A.B
{
    public interface Goo
    {
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: true,
newFileName: "Test2.cs",
newFileFolderContainers: ImmutableArray<string>.Empty,
projectName: "Assembly2");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoSameLanguageDifferentProjectNewFile_Folders_Usings()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|Goo$$|] f;
    }
}</Document>
                    </Project>
                    <Project Language=""C#"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace outer.inner
{
    public interface Goo
    {
    }
}",
checkIfUsingsIncluded: true,
expectedTextWithUsings: @"
using outer.inner;

class Program
{
    void Main()
    {
        Goo f;
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: true,
newFileName: "Test2.cs",
newFileFolderContainers: ImmutableArray.Create("outer", "inner"),
projectName: "Assembly2");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoSameLanguageDifferentProjectNewFile_Folders_NoUsings_NotSimpleName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A.B
{
}
                        </Document>
                    </Project>
                    <Project Language=""C#"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace A.B
{
    public interface Goo
    {
    }
}",
checkIfUsingsNotIncluded: true,
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: true,
newFileName: "Test2.cs",
newFileFolderContainers: ImmutableArray.Create("outer", "inner"),
projectName: "Assembly2");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoSameLanguageDifferentProjectNewFile_Folders_Usings_DefaultNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|Goo$$|] f;
    }
}</Document>
                    </Project>
                    <Project Language=""C#"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace ConsoleApplication.outer.inner
{
    public interface Goo
    {
    }
}",
defaultNamespace: "ConsoleApplication",
checkIfUsingsIncluded: true,
expectedTextWithUsings: @"
using ConsoleApplication.outer.inner;

class Program
{
    void Main()
    {
        Goo f;
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: true,
newFileName: "Test2.cs",
newFileFolderContainers: ImmutableArray.Create("outer", "inner"),
projectName: "Assembly2");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoSameLanguageDifferentProjectNewFile_Folders_NoUsings_NotSimpleName_DefaultNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A.B
{
}
                        </Document>
                    </Project>
                    <Project Language=""C#"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"namespace A.B
{
    public interface Goo
    {
    }
}",
defaultNamespace: "ConsoleApplication",
checkIfUsingsNotIncluded: true,
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: true,
newFileName: "Test2.cs",
newFileFolderContainers: ImmutableArray.Create("outer", "inner"),
projectName: "Assembly2");
        }
        #endregion
        #endregion
        #region DifferentLanguage
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoDifferentLanguageNewFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A.B
{
}
                        </Document>
                    </Project>
                    <Project Language=""Visual Basic"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"Namespace Global.A.B
    Public Class Goo
    End Class
End Namespace
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: true,
newFileName: "Test2.vb",
newFileFolderContainers: ImmutableArray<string>.Empty,
projectName: "Assembly2");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoDifferentLanguageNewFile_Folders_Usings()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|Goo$$|] f;
    }
}</Document>
                    </Project>
                    <Project Language=""Visual Basic"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"Namespace outer.inner
    Public Class Goo
    End Class
End Namespace
",
checkIfUsingsIncluded: true,
expectedTextWithUsings: @"
using outer.inner;

class Program
{
    void Main()
    {
        Goo f;
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: true,
newFileName: "Test2.vb",
newFileFolderContainers: ImmutableArray.Create("outer", "inner"),
projectName: "Assembly2");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoDifferentLanguageNewFile_Folders_NoUsings_NotSimpleName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A.B
{
}
                        </Document>
                    </Project>
                    <Project Language=""Visual Basic"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"Namespace Global.A.B
    Public Class Goo
    End Class
End Namespace
",
checkIfUsingsNotIncluded: true,
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: true,
newFileName: "Test2.vb",
newFileFolderContainers: ImmutableArray.Create("outer", "inner"),
projectName: "Assembly2");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoDifferentLanguageNewFile_Folders_Usings_RootNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|Goo$$|] f;
    }
}</Document>
                    </Project>
                    <Project Language=""Visual Basic"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                        <CompilationOptions RootNamespace=""BarBaz""/>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"Namespace outer.inner
    Public Class Goo
    End Class
End Namespace
",
checkIfUsingsIncluded: true,
expectedTextWithUsings: @"
using BarBaz.outer.inner;

class Program
{
    void Main()
    {
        Goo f;
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: true,
newFileName: "Test2.vb",
newFileFolderContainers: ImmutableArray.Create("outer", "inner"),
projectName: "Assembly2");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoDifferentLanguageNewFile_Folders_NoUsings_NotSimpleName_RootNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A.B
{
}
                        </Document>
                    </Project>
                    <Project Language=""Visual Basic"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                        <CompilationOptions RootNamespace=""BarBaz""/>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"Namespace Global.A.B
    Public Class Goo
    End Class
End Namespace
",
checkIfUsingsNotIncluded: true,
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: true,
newFileName: "Test2.vb",
newFileFolderContainers: ImmutableArray.Create("outer", "inner"),
projectName: "Assembly2");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoDifferentLanguageNewFile_Folders_NoUsings_NotSimpleName_RootNamespace_ProjectReference()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""Visual Basic"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                        <CompilationOptions RootNamespace=""BarBaz""/>
                        <Document FilePath=""Test2.vb"">
                        Namespace A.B
                            Public Class Goo
                            End Class
                        End Namespace
                        </Document>
                    </Project>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <ProjectReference>Assembly2</ProjectReference>
                        <Document FilePath=""Test1.cs"">
using BarBaz.A;

class Program
{
    void Main()
    {
        [|BarBaz.A.B.Bar$$|] f;
    }
}</Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Bar",
expected: @"Namespace A.B
    Public Class Bar
    End Class
End Namespace
",
defaultNamespace: "ConsoleApplication",
checkIfUsingsNotIncluded: true,
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: true,
newFileName: "Test3.vb",
newFileFolderContainers: ImmutableArray.Create("outer", "inner"),
projectName: "Assembly2");
        }

        [WorkItem(858826, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/858826")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoDifferentLanguageNewFileAdjustFileExtension()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A.B
{
}
                        </Document>
                    </Project>
                    <Project Language=""Visual Basic"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"Namespace Global.A.B
    Public Class Goo
    End Class
End Namespace
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: true,
newFileName: "Test2.vb",
newFileFolderContainers: ImmutableArray<string>.Empty,
projectName: "Assembly2");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoDifferentLanguageExistingEmptyFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A.B
{
}
                        </Document>
                    </Project>
                    <Project Language=""Visual Basic"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                        <Document Folders=""outer\inner"" FilePath=""Test2.vb"">
                        </Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"Namespace Global.A.B
    Public Class Goo
    End Class
End Namespace
",
checkIfUsingsNotIncluded: true,
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: false,
existingFilename: "Test2.vb",
projectName: "Assembly2");
        }

        [WorkItem(850101, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/850101")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoDifferentLanguageExistingEmptyFile_Usings_Folder()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|Goo$$|] f;
    }
}</Document>
                    </Project>
                    <Project Language=""Visual Basic"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                        <Document Folders=""outer\inner"" FilePath=""Test2.vb"">
                        </Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"Namespace outer.inner
    Public Class Goo
    End Class
End Namespace
",
checkIfUsingsIncluded: true,
expectedTextWithUsings: @"
using outer.inner;

class Program
{
    void Main()
    {
        Goo f;
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: false,
existingFilename: "Test2.vb",
projectName: "Assembly2");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoDifferentLanguageExistingNonEmptyFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A.B
{
}
                        </Document>
                    </Project>
                    <Project Language=""Visual Basic"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                        <Document FilePath=""Test2.vb"">
Namespace A
End Namespace
                        </Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"
Namespace A
End Namespace

Namespace Global.A.B
    Public Class Goo
    End Class
End Namespace
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: false,
existingFilename: "Test2.vb",
projectName: "Assembly2");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeIntoDifferentLanguageExistingNonEmptyTargetFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.B.Goo$$|] f;
    }
}
namespace A.B
{
}
                        </Document>
                    </Project>
                    <Project Language=""Visual Basic"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                        <Document FilePath=""Test2.vb"">
Namespace Global
    Namespace A
        Namespace C
        End Namespace
        Namespace B
        End Namespace
    End Namespace
End Namespace</Document>
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"
Namespace Global
    Namespace A
        Namespace C
        End Namespace
        Namespace B
            Public Class Goo
            End Class
        End Namespace
    End Namespace
End Namespace",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: false,
existingFilename: "Test2.vb",
projectName: "Assembly2");
        }

        [WorkItem(861362, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/861362")]
        [WorkItem(869593, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/869593")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateModuleFromCSharpToVisualBasicInTypeContext()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        [|A.Goo$$|].Bar f;
    }
}
namespace A
{
}
                        </Document>
                    </Project>
                    <Project Language=""Visual Basic"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"Namespace Global.A
    Public Module Goo
    End Module
End Namespace
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Module,
isNewFile: true,
newFileName: "Test2.vb",
newFileFolderContainers: ImmutableArray<string>.Empty,
projectName: "Assembly2",
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.Class | TypeKindOptions.Structure | TypeKindOptions.Module));
        }

        #endregion
        #region Bugfix 
        [WorkItem(861462, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/861462")]
        [WorkItem(873066, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/873066")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeWithProperAccessibilityAndTypeKind_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"
public class C : [|$$D|]
{
}",
languageName: LanguageNames.CSharp,
typeName: "D",
expected: @"
public class C : D
{
}

public class D
{
}",
accessibility: Accessibility.Public,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(true, TypeKindOptions.BaseList, false));
        }

        [WorkItem(861462, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/861462")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeWithProperAccessibilityAndTypeKind_2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"public interface CCC : [|$$DDD|]
{
}",
languageName: LanguageNames.CSharp,
typeName: "DDD",
expected: @"public interface CCC : DDD
{
}

public interface DDD
{
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(true, TypeKindOptions.Interface, false));
        }

        [WorkItem(861462, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/861462")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeWithProperAccessibilityAndTypeKind_3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"public struct CCC : [|$$DDD|]
{
}",
languageName: LanguageNames.CSharp,
typeName: "DDD",
expected: @"public struct CCC : DDD
{
}

public interface DDD
{
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Interface,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(true, TypeKindOptions.Interface, false));
        }

        [WorkItem(861362, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/861362")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeInMemberAccessExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var s = [|$$A.B|];
    }
}",
languageName: LanguageNames.CSharp,
typeName: "A",
expected: @"class Program
{
    static void Main(string[] args)
    {
        var s = A.B;
    }
}

public class A
{
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.MemberAccessWithNamespace));
        }

        [WorkItem(861362, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/861362")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeInMemberAccessExpressionInNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var s = [|$$A.B.C|];
    }
}

namespace A
{
}",
languageName: LanguageNames.CSharp,
typeName: "B",
expected: @"class Program
{
    static void Main(string[] args)
    {
        var s = A.B.C;
    }
}

namespace A
{
    public class B
    {
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.MemberAccessWithNamespace));
        }

        [WorkItem(861600, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/861600")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeWithoutEnumForGenericsInMemberAccess()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var s = [|$$Goo<Bar>|].D;
    }
}

class Bar
{
}",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"class Program
{
    static void Main(string[] args)
    {
        var s = Goo<Bar>.D;
    }
}

class Bar
{
}

public class Goo<T>
{
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.Class | TypeKindOptions.Structure));
        }

        [WorkItem(861600, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/861600")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeWithoutEnumForGenericsInNameContext()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        [|$$Goo<Bar>|] baz;
    }
}

internal class Bar
{
}",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"class Program
{
    static void Main(string[] args)
    {
        Goo<Bar> baz;
    }
}

internal class Bar
{
}

public class Goo<T>
{
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.Class | TypeKindOptions.Structure | TypeKindOptions.Interface | TypeKindOptions.Delegate));
        }

        [WorkItem(861600, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/861600")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeInMemberAccessWithNSForModule()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var s = [|Goo.$$Bar|].Baz;
    }
}

namespace Goo
{
}",
languageName: LanguageNames.CSharp,
typeName: "Bar",
expected: @"class Program
{
    static void Main(string[] args)
    {
        var s = Goo.Bar.Baz;
    }
}

namespace Goo
{
    public class Bar
    {
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.MemberAccessWithNamespace));
        }

        [WorkItem(861600, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/861600")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeInMemberAccessWithGlobalNSForModule()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var s = [|$$Bar|].Baz;
    }
}",
languageName: LanguageNames.CSharp,
typeName: "Bar",
expected: @"class Program
{
    static void Main(string[] args)
    {
        var s = Bar.Baz;
    }
}

public class Bar
{
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.MemberAccessWithNamespace));
        }

        [WorkItem(861600, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/861600")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeInMemberAccessWithoutNS()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var s = [|$$Bar|].Baz;
    }
}

namespace Bar
{
}",
languageName: LanguageNames.CSharp,
typeName: "Bar",
isMissing: true);
        }

        [WorkItem(876202, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/876202")]
        [WorkItem(883531, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/883531")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_NoParameterLessConstructorForStruct()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var s = new [|$$Bar|]();
    }
}",
languageName: LanguageNames.CSharp,
typeName: "Bar",
expected: @"class Program
{
    static void Main(string[] args)
    {
        var s = new Bar();
    }
}

public struct Bar
{
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Structure,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.Class | TypeKindOptions.Structure, false));
        }
        #endregion
        #region Delegates
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_ObjectCreationExpression_MethodGroup()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var s2 = new [|$$MyD|](goo);
    }
    static void goo()
    {
    }
}",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"class Program
{
    static void Main(string[] args)
    {
        var s2 = new MyD(goo);
    }
    static void goo()
    {
    }
}

public delegate void MyD();
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.Class | TypeKindOptions.Structure | TypeKindOptions.Delegate));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_ObjectCreationExpression_MethodGroup_Generics()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var s2 = new [|$$MyD|](goo);
    }
    static void goo<T>()
    {
    }
}",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"class Program
{
    static void Main(string[] args)
    {
        var s2 = new MyD(goo);
    }
    static void goo<T>()
    {
    }
}

public delegate void MyD<T>();
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.Class | TypeKindOptions.Structure | TypeKindOptions.Delegate));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_ObjectCreationExpression_Delegate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        MyD1 d = null;
        var s1 = new [|$$MyD2|](d);
    }
    public delegate object MyD1();
}",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"class Program
{
    static void Main(string[] args)
    {
        MyD1 d = null;
        var s1 = new MyD2(d);
    }
    public delegate object MyD1();
}

public delegate object MyD();
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.Class | TypeKindOptions.Structure | TypeKindOptions.Delegate));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_ObjectCreationExpression_Action()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"using System;
class Program
{
    static void Main(string[] args)
    {
        Action<int> action1 = null;
        var s3 = new [|$$MyD|](action1);
    }
}",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"using System;
class Program
{
    static void Main(string[] args)
    {
        Action<int> action1 = null;
        var s3 = new MyD(action1);
    }
}

public delegate void MyD(int obj);
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.Class | TypeKindOptions.Structure | TypeKindOptions.Delegate));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_ObjectCreationExpression_Func()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"using System;
class Program
{
    static void Main(string[] args)
    {
        Func<int> lambda = () => { return 0; };
        var s4 = new [|$$MyD|](lambda);
    }
}",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"using System;
class Program
{
    static void Main(string[] args)
    {
        Func<int> lambda = () => { return 0; };
        var s4 = new MyD(lambda);
    }
}

public delegate int MyD();
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.Class | TypeKindOptions.Structure | TypeKindOptions.Delegate));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_ObjectCreationExpression_ParenLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var s5 = new [|$$MyD|]((int n) => { return n; });
    }
}",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"class Program
{
    static void Main(string[] args)
    {
        var s5 = new MyD((int n) => { return n; });
    }
}

public delegate int MyD(int n);
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.Class | TypeKindOptions.Structure | TypeKindOptions.Delegate));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_ObjectCreationExpression_SimpleLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var s6 = new [|$$MyD|](n => { return n; });
    }
}",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"class Program
{
    static void Main(string[] args)
    {
        var s6 = new MyD(n => { return n; });
    }
}

public delegate void MyD(object n);
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.Class | TypeKindOptions.Structure | TypeKindOptions.Delegate));
        }

        [WorkItem(872935, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/872935")]
        [Fact(Skip = "872935"), Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_ObjectCreationExpression_SimpleLambdaEmpty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var s7 = new [|$$MyD3|](() => { });
    }
}",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"class Program
{
    static void Main(string[] args)
    {
        var s7 = new MyD3(() => { });
    }
}

public delegate void MyD();
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.AllOptions));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_VarDecl_MethodGroup()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        [|$$MyD|] z1 = goo;
    }
    static void goo()
    {
    }
}",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"class Program
{
    static void Main(string[] args)
    {
        MyD z1 = goo;
    }
    static void goo()
    {
    }
}

public delegate void MyD();
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.AllOptions));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_VarDecl_Delegate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        MyD1 temp = null;
        [|$$MyD|] z2 = temp; // Still Error
    }
}

public delegate object MyD1();",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"class Program
{
    static void Main(string[] args)
    {
        MyD1 temp = null;
        MyD z2 = temp; // Still Error
    }
}

public delegate object MyD1();

public delegate object MyD();
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.AllOptions));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_VarDecl_Action()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"using System;
class Program
{
    static void Main(string[] args)
    {
        Action<int, int, int> action2 = null;
        [|$$MyD|] z3 = action2; // Still Error
    }
}",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"using System;
class Program
{
    static void Main(string[] args)
    {
        Action<int, int, int> action2 = null;
        MyD z3 = action2; // Still Error
    }
}

public delegate void MyD(int arg1, int arg2, int arg3);
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.AllOptions));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_VarDecl_Func()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"using System;
class Program
{
    static void Main(string[] args)
    {
        Func<int> lambda2 = () => { return 0; };
        [|$$MyD|] z4 = lambda2; // Still Error
    }
}",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"using System;
class Program
{
    static void Main(string[] args)
    {
        Func<int> lambda2 = () => { return 0; };
        MyD z4 = lambda2; // Still Error
    }
}

public delegate int MyD();
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.AllOptions));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_VarDecl_ParenLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        [|$$MyD|] z5 = (int n) => { return n; };
    }
}
",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"class Program
{
    static void Main(string[] args)
    {
        MyD z5 = (int n) => { return n; };
    }
}

public delegate int MyD(int n);
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.AllOptions));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_VarDecl_SimpleLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        [|$$MyD|] z6 = n => { return n; };
    }
}",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"class Program
{
    static void Main(string[] args)
    {
        MyD z6 = n => { return n; };
    }
}

public delegate void MyD(object n);
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.AllOptions));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_Cast_MethodGroup()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var zz1 = ([|$$MyD|])goo;
    }
    static void goo()
    {
    }
}",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"class Program
{
    static void Main(string[] args)
    {
        var zz1 = (MyD)goo;
    }
    static void goo()
    {
    }
}

public delegate void MyD();
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.AllOptions));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_Cast_Delegate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        MyDDD temp1 = null;
        var zz2 = ([|$$MyD|])temp1; // Still Error
    }
}

public delegate object MyDDD();",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"class Program
{
    static void Main(string[] args)
    {
        MyDDD temp1 = null;
        var zz2 = (MyD)temp1; // Still Error
    }
}

public delegate object MyDDD();

public delegate object MyD();
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.AllOptions));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_Cast_Action()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"using System;
class Program
{
    static void Main(string[] args)
    {
        Action<int, int, int> action3 = null;
        var zz3 = ([|$$MyD|])action3; // Still Error
    }
}",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"using System;
class Program
{
    static void Main(string[] args)
    {
        Action<int, int, int> action3 = null;
        var zz3 = (MyD)action3; // Still Error
    }
}

public delegate void MyD(int arg1, int arg2, int arg3);
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.AllOptions));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_Cast_Func()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"using System;
class Program
{
    static void Main(string[] args)
    {
        Func<int> lambda3 = () => { return 0; };
        var zz4 = ([|$$MyD|])lambda3; // Still Error
    }
}",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"using System;
class Program
{
    static void Main(string[] args)
    {
        Func<int> lambda3 = () => { return 0; };
        var zz4 = (MyD)lambda3; // Still Error
    }
}

public delegate int MyD();
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.AllOptions));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_Cast_ParenLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var zz5 = ([|$$MyD|])((int n) => { return n; });
    }
}
",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"class Program
{
    static void Main(string[] args)
    {
        var zz5 = (MyD)((int n) => { return n; });
    }
}

public delegate int MyD(int n);
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.AllOptions));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_Cast_SimpleLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var zz6 = ([|$$MyD|])(n => { return n; });
    }
}",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"class Program
{
    static void Main(string[] args)
    {
        var zz6 = (MyD)(n => { return n; });
    }
}

public delegate void MyD(object n);
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.AllOptions));
        }

        [WpfFact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateTypeIntoDifferentLanguageNewFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"<Workspace>
                    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"">
                        <Document FilePath=""Test1.cs"">
class Program
{
    void Main()
    {
        var f = ([|A.B.Goo$$|])Main;
    }
}
namespace A.B
{
}
                        </Document>
                    </Project>
                    <Project Language=""Visual Basic"" AssemblyName=""Assembly2"" CommonReferences=""true"">
                    </Project>
                </Workspace>",
languageName: LanguageNames.CSharp,
typeName: "Goo",
expected: @"Namespace Global.A.B
    Public Delegate Sub Goo()
End Namespace
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: true,
newFileName: "Test2.vb",
newFileFolderContainers: ImmutableArray<string>.Empty,
projectName: "Assembly2");
        }

        [WorkItem(860210, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/860210")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_NoInfo()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        [|$$MyD<int>|] d;
    }
}",
languageName: LanguageNames.CSharp,
typeName: "MyD",
expected: @"class Program
{
    static void Main(string[] args)
    {
        MyD<int> d;
    }
}

public delegate void MyD<T>();
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false);
        }
        #endregion 
        #region Dev12Filtering
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_NoEnum_InvocationExpression_0()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var s2 = [|$$B|].C();
    }
}",
languageName: LanguageNames.CSharp,
typeName: "B",
expected: @"class Program
{
    static void Main(string[] args)
    {
        var s2 = B.C();
    }
}

public class B
{
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: false,
assertTypeKindAbsent: new[] { TypeKindOptions.Enum });
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateDelegateType_NoEnum_InvocationExpression_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
        var s2 = [|A.$$B|].C();
    }
}

namespace A
{
}",
languageName: LanguageNames.CSharp,
typeName: "B",
expected: @"class Program
{
    static void Main(string[] args)
    {
        var s2 = A.B.C();
    }
}

namespace A
{
    public class B
    {
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: false,
assertTypeKindAbsent: new[] { TypeKindOptions.Enum });
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_TypeConstraint_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
    }
}

public class F<T> where T : [|$$Bar|] 
{
}
",
languageName: LanguageNames.CSharp,
typeName: "Bar",
expected: @"class Program
{
    static void Main(string[] args)
    {
    }
}

public class F<T> where T : Bar 
{
}

public class Bar
{
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(true, TypeKindOptions.BaseList));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_TypeConstraint_2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
    }
}

class outer
{
    public class F<T> where T : [|$$Bar|] 
    {
    }
}
",
languageName: LanguageNames.CSharp,
typeName: "Bar",
expected: @"class Program
{
    static void Main(string[] args)
    {
    }
}

class outer
{
    public class F<T> where T : Bar 
    {
    }
}

public class Bar
{
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.BaseList));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_TypeConstraint_3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"class Program
{
    static void Main(string[] args)
    {
    }
}

public class outerOuter
{
    public class outer
    {
        public class F<T> where T : [|$$Bar|]
        {
        }
    }
}
",
languageName: LanguageNames.CSharp,
typeName: "Bar",
expected: @"class Program
{
    static void Main(string[] args)
    {
    }
}

public class outerOuter
{
    public class outer
    {
        public class F<T> where T : Bar
        {
        }
    }
}

public class Bar
{
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(true, TypeKindOptions.BaseList));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeWithProperAccessibilityWithNesting_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"
public class B
{
    public class C : [|$$D|]
    {
    }
}",
languageName: LanguageNames.CSharp,
typeName: "D",
expected: @"
public class B
{
    public class C : D
    {
    }
}

public class D
{
}",
accessibility: Accessibility.Public,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(true, TypeKindOptions.BaseList, false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeWithProperAccessibilityWithNesting_2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"
class B
{
    public class C : [|$$D|]
    {
    }
}",
languageName: LanguageNames.CSharp,
typeName: "D",
expected: @"
class B
{
    public class C : D
    {
    }
}

public class D
{
}",
accessibility: Accessibility.Public,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.BaseList, false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateTypeWithProperAccessibilityWithNesting_3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"
class A
{
    public class B
    {
        public class C : [|$$D|]
        {
        }
    }
}",
languageName: LanguageNames.CSharp,
typeName: "D",
expected: @"
class A
{
    public class B
    {
        public class C : D
        {
        }
    }
}

public class D
{
}",
accessibility: Accessibility.Public,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.BaseList, false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_Event_1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"
class A
{
    event [|$$goo|] name1
    {
        add { }
        remove { }
    }
}",
languageName: LanguageNames.CSharp,
typeName: "goo",
expected: @"
class A
{
    event goo name1
    {
        add { }
        remove { }
    }
}

public delegate void goo();
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.Delegate));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_Event_2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"
class A
{
    public event [|$$goo|] name2;
}",
languageName: LanguageNames.CSharp,
typeName: "goo",
expected: @"
class A
{
    public event goo name2;
}

public delegate void goo();
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.Delegate));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_Event_3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"
class A
{
    event [|NS.goo$$|] name1
    {
        add { }
        remove { }
    }
}",
languageName: LanguageNames.CSharp,
typeName: "goo",
expected: @"
class A
{
    event NS.goo name1
    {
        add { }
        remove { }
    }
}

namespace NS
{
    public delegate void goo();
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.Delegate));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_Event_4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"
class A
{
    public event [|NS.goo$$|] name2;
}",
languageName: LanguageNames.CSharp,
typeName: "goo",
expected: @"
class A
{
    public event NS.goo name2;
}

namespace NS
{
    public delegate void goo();
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.Delegate));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_Event_5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"
class A
{
    event [|$$NS.goo.Mydel|] name1
    {
        add { }
        remove { }
    }
}

namespace NS
{
}",
languageName: LanguageNames.CSharp,
typeName: "goo",
expected: @"
class A
{
    event NS.goo.Mydel name1
    {
        add { }
        remove { }
    }
}

namespace NS
{
    public class goo
    {
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.Class | TypeKindOptions.Structure | TypeKindOptions.Module));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_Event_6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"
class A
{
    public event [|$$NS.goo.Mydel|] name2;
}

namespace NS
{
}",
languageName: LanguageNames.CSharp,
typeName: "goo",
expected: @"
class A
{
    public event NS.goo.Mydel name2;
}

namespace NS
{
    public class goo
    {
    }
}",
accessibility: Accessibility.Public,
typeKind: TypeKind.Class,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(false, TypeKindOptions.Class | TypeKindOptions.Structure | TypeKindOptions.Module));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_Event_7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"
public class A
{
    public event [|$$goo|] name1
    {
        add { }
        remove { }
    }
}",
languageName: LanguageNames.CSharp,
typeName: "goo",
expected: @"
public class A
{
    public event goo name1
    {
        add { }
        remove { }
    }
}

public delegate void goo();
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(true, TypeKindOptions.Delegate));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsGenerateType)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task GenerateType_Event_8()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestWithMockedGenerateTypeDialog(
initial: @"
public class outer
{
    public class A
    {
        public event [|$$goo|] name1
        {
            add { }
            remove { }
        }
    }
}",
languageName: LanguageNames.CSharp,
typeName: "goo",
expected: @"
public class outer
{
    public class A
    {
        public event goo name1
        {
            add { }
            remove { }
        }
    }
}

public delegate void goo();
",
accessibility: Accessibility.Public,
typeKind: TypeKind.Delegate,
isNewFile: false,
assertGenerateTypeDialogOptions: new GenerateTypeDialogOptions(true, TypeKindOptions.Delegate));
        }
        #endregion
    }
}
