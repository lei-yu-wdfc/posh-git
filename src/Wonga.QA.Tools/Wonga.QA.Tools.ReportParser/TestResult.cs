using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Tools.ReportParser
{
    public enum TestOutcome
    {
        Passed,
        Failed,
        Ignored,
        Pending,
        Inconclusive,
        UnknownOutcome
    }

    public enum TestKind
    {
        Fixture,
        Test,
        Unknown
    }

    public class TestResult : IEquatable<TestResult>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public TestOutcome Outcome { get; set; }
        public string DebugTrace { get; set; }
        public string StackTrace { get; set; }
        public string WarningsTrace { get; set; }
        public int AssertCount { get; set; }
        public double Duration { get; set; }
        public Dictionary<string, List<string>> Metadata { get; set; }
        public TestKind TestKind
        {
            get
            {
                var result = TestKind.Unknown;
                var testKindText = GetFirstMetadataValue("TestKind");
                Enum.TryParse(testKindText, true, out result);
                return result;
            }
        }

        public List<TestResult> Children { get; set; }

        public TestResult()
        {
            Outcome = TestOutcome.UnknownOutcome;
            Metadata = new Dictionary<string, List<string>>();
            Children = new List<TestResult>();
        }

        public bool Equals(TestResult other)
        {

            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Id.Equals(other.Id);
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public override int GetHashCode()
        {

            //Get hash code for the Name field if it is not null.
            int hashTestResultId = Id == null ? 0 : Id.GetHashCode();

            //Calculate the hash code for the product.
            return hashTestResultId;
        }

        private string GetFirstMetadataValue(string key)
        {
            List<string> testKindText;
            Metadata.TryGetValue("TestKind", out testKindText);
            if (testKindText == null || testKindText.Count == 0)
                return null;
            return testKindText[0];
        }

        private List<string> GetAllMetadataValues(string key)
        {
            List<string> testKindText = new List<string>();
            Metadata.TryGetValue("TestKind", out testKindText);
            return testKindText;
        }
    }
}
