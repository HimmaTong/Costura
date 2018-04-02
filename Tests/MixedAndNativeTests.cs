﻿using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Writers;
using Fody;
using NUnit.Framework;

[TestFixture]
public class MixedAndNativeTests : BaseCosturaTest
{
    protected override string Suffix => "MixedAndNative";

    [OneTimeSetUp]
    public void CreateAssembly()
    {
            CreateIsolatedAssemblyCopy("ExeToProcessWithNative",
                "<Costura Unmanaged32Assemblies='AssemblyToReferenceMixed' />",
                new[] { "AssemblyToReferenceMixed.dll" });
    }

    [SetUp]
    public void Setup()
    {
            LoadAssemblyIntoAppDomain();
    }

    [Test, Category("Code")]
    public void Native()
    {
        var instance1 = assembly.GetInstance("ClassToTest");
        Assert.AreEqual("Hello", instance1.NativeFoo());
    }

    [Test, Category("Code")]
    public void Mixed()
    {
        var instance1 = assembly.GetInstance("ClassToTest");
        Assert.AreEqual("Hello", instance1.MixedFoo());
    }

    [Test, Category("Code")]
    public void MixedPInvoke()
    {
        var instance1 = assembly.GetInstance("ClassToTest");
        Assert.AreEqual("Hello", instance1.MixedFooPInvoke());
    }

    [Test, Category("IL")]
    public void TemplateHasCorrectSymbols()
    {
        using (ApprovalResults.ForScenario(Suffix))
        {
            var text = Ildasm.Decompile(afterAssemblyPath, "Costura.AssemblyLoader");
            Approvals.Verify(text);
        }
    }
}