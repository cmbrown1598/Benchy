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
            _ds.Tables[0].Columns.Add(new DataColumn("Column1"));
        }

        [Teardown]
        public void Teardown()
        {
            _ds = null;
        }

        [Benchmark]
        public void Execute()
        {
            
            var row = _ds.Tables[0].NewRow();
            row["Column1"] = 43;
            _ds.Tables[0].Rows.Add(row);
            _ds.Tables[0].AcceptChanges();
        }
    }
}
