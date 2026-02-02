using System.Collections.Generic;

namespace inzibackend.DataExporting;

public interface IExcelColumnSelectionInput
{
    List<string> SelectedColumns { get; set; }
}

