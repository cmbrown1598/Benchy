using System.Data;
using Benchy.Framework;

namespace Sample.Benchy.Tests
{
    [BenchmarkFixture(Ignore = false)]
    class ParallelizeBenchmarkTest
    {
        // This code will run in parallel.
        [Benchmark(Parallelize = true, ExecutionCount = 10000)]
        public void Execute()
        {
            var ds = new DataSet("My Dataset");
            ds.Tables.Add(new DataTable("My Table"));
            ds.Tables[0].Columns.Add(new DataColumn("Column1"));
            var row = ds.Tables[0].NewRow();
            row["Column1"] = 43;
            ds.Tables[0].Rows.Add(row);
            ds.Tables[0].AcceptChanges();
            ds.Tables[0].Rows.Clear();
            ds.Tables[0].AcceptChanges();
        }
    }
}
