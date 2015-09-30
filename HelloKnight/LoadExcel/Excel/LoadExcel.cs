//using Excel = Microsoft.Office.Interop.Excel;
//using System.Reflection;
using System;
using System.Data;
using System.Data.OleDb;
//using System.Windows.Forms;

class CLoadExcel
{
    //Excel.Application xls;
    //Excel._Workbook wbook;
    //Excel._Worksheet wSheet;
    // 엑셀 파일 읽기 위해 설정 [4/22/2012 JK]


    public void Initialize()
    {

    }

    public void CleanUp()
    {
        // dispose used objects
        m_DataTable.Dispose();
        m_DataAdapter.Dispose();
        m_DBCommand.Dispose();

        m_ExcelConnection.Close();
        m_ExcelConnection.Dispose();
    }

    public void ReleaseSheet()
    {
        // dispose used objects
        m_DataTable.Dispose();
        m_DataAdapter.Dispose();
        m_DBCommand.Dispose();
    }
    // 엑셀 데이터 로드 [4/22/2012 JK]
    public void LoadExcel(string p_strFileName)
    {
        p_strFileName = Environment.CurrentDirectory + "\\" + p_strFileName;            // 경로 설정
        string strConnect = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source = " + p_strFileName + "; Extended Properties = Excel 12.0";
        m_ExcelConnection = new OleDbConnection(strConnect);
        m_ExcelConnection.Open();
    }
    public void ActiveSheet(string p_strSheetName)
    {
        string strSQL = "SELECT * FROM [" + p_strSheetName + "$]";
        m_DBCommand = new OleDbCommand(strSQL, m_ExcelConnection);
        m_DataAdapter = new OleDbDataAdapter(m_DBCommand);

        m_DataTable = new DataTable();
        m_DataAdapter.Fill(m_DataTable);
    }

    public string GetData(int p_iX, int p_iY)
    {
        if (p_iY < m_DataTable.Rows.Count)
        {
            return m_DataTable.Rows[p_iY][p_iX].ToString();    // 인덱스 얻기
        }
        else
        {
            return "";
        }
    }

    private OleDbConnection m_ExcelConnection;
    private OleDbCommand m_DBCommand;
    private OleDbDataAdapter m_DataAdapter;
    private DataTable m_DataTable;

}
