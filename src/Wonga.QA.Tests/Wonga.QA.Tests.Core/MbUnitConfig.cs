using System;
using Gallio.Framework.Pattern;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;

[AssemblyFixture]
public class MbUnitConfig
{
    [FixtureSetUp]
    public void Setup()
    {
        SetDegreeOfParallelism();
    }

    private void SetDegreeOfParallelism()
    {
        switch(Config.AUT)
        {
            case(AUT.Ca):
                TestAssemblyExecutionParameters.DegreeOfParallelism = Environment.ProcessorCount;
                break;
            case (AUT.Wb):
                TestAssemblyExecutionParameters.DegreeOfParallelism = 1;
                break;
			case (AUT.Za):
				TestAssemblyExecutionParameters.DegreeOfParallelism = 6;
        		break;
            default:
                TestAssemblyExecutionParameters.DegreeOfParallelism = Environment.ProcessorCount;
                break;
        }
    }
}