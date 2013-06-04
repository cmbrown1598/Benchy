using System.Data;
using Benchy;

namespace Sample.Benchy.Tests
{
    [BenchmarkFixture]
    class ExampleBenchmarkTest
    {
        private DataSet _ds;


        [Setup]
        public void Setup()
        {
            _ds = new DataSet("My Dataset");
            _ds.Tables.Add(new DataTable("My Table"));
            _ds.Tables.Add(new DataTable("My second table"));
            _ds.Tables[0].Columns.Add(new DataColumn("Column1"));
            _ds.Tables[1].Columns.Add(new DataColumn("Column1"));
        }

        [Teardown]
        public void Teardown()
        {
            _ds = null;
        }

        [Benchmark]
        public void Execute()
        {
            var a = 100 - 1;
        }
    }
}
