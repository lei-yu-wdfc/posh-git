using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Wonga.QA.Framework;

namespace Wonga.QA.WebTool
{
    public class TableMaker
    {
        public TableRow tbRow(string name, string value)
        {

            TableCell cell_first = new TableCell();
            cell_first.Text = name;
            TableCell cell_second = new TableCell();
            cell_second.Text = value;
            TableRow row = new TableRow();
            row.Cells.Add(cell_first);
            row.Cells.Add(cell_second);
            return row;
        }
    }
}