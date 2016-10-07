// Decompiled with JetBrains decompiler
// Type: sbsit.utils.DBF
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace sbsit.utils
{
  public static class DBF
  {
    public static void save_dbf(DataTable tbl, string full_file_name, DBF.dbf_field[] fields)
    {
      OleDbConnection connection = new OleDbConnection("Provider=VFPOLEDB.1;Data Source=" + Path.GetDirectoryName(full_file_name) + ";Password=;Collating Sequence=MACHINE;Extended Properties=dBASE III");
      connection.Open();
      string str1 = Path.GetFileNameWithoutExtension(full_file_name).Replace(' ', '_');
      using (OleDbCommand oleDbCommand1 = new OleDbCommand("create table " + str1 + "(", connection))
      {
        foreach (DBF.dbf_field field in fields)
        {
          OleDbCommand oleDbCommand2 = oleDbCommand1;
          oleDbCommand2.CommandText = oleDbCommand2.CommandText + field.Name + " " + field.DataType + ",";
        }
        oleDbCommand1.CommandText = oleDbCommand1.CommandText.Remove(oleDbCommand1.CommandText.Length - 1);
        oleDbCommand1.CommandText += ");";
        try
        {
          oleDbCommand1.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.ToString());
          return;
        }
      }
      using (OleDbCommand oleDbCommand1 = new OleDbCommand("insert into " + str1 + " (", connection))
      {
        string empty = string.Empty;
        foreach (DBF.dbf_field field in fields)
        {
          OleDbCommand oleDbCommand2 = oleDbCommand1;
          string str2 = oleDbCommand2.CommandText + field.Name + ",";
          oleDbCommand2.CommandText = str2;
          empty += "?,";
          oleDbCommand1.Parameters.Add(field.Name, field.ParamType);
        }
        oleDbCommand1.CommandText = oleDbCommand1.CommandText.Remove(oleDbCommand1.CommandText.Length - 1);
        OleDbCommand oleDbCommand3 = oleDbCommand1;
        string str3 = oleDbCommand3.CommandText + ") values (" + empty.Remove(empty.Length - 1) + ");";
        oleDbCommand3.CommandText = str3;
        foreach (DataRow row in (InternalDataCollectionBase) tbl.Rows)
        {
          foreach (DBF.dbf_field field in fields)
            oleDbCommand1.Parameters[field.Name].Value = row[field.Name];
          try
          {
            oleDbCommand1.ExecuteNonQuery();
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show(ex.ToString());
            return;
          }
        }
      }
      connection.Close();
      connection.Dispose();
      GC.Collect();
    }

    public static void DataTableSaveToDBF(DataTable DT, string Folder, string tblname)
    {
      if (tblname.ToLower().EndsWith(".dbf"))
        tblname = tblname.Remove(tblname.Length - 3);
      File.Delete(Folder + "\\" + tblname + ".DBF");
      FileStream fileStream = new FileStream(Folder + "\\" + tblname + ".DBF", FileMode.Create);
      byte[] buffer1 = new byte[4]{ (byte) 3, (byte) 99, (byte) 4, (byte) 4 };
      fileStream.Write(buffer1, 0, buffer1.Length);
      byte[] buffer2 = new byte[4]{ (byte) (DT.Rows.Count % 16777216 % 65536 % 256), (byte) (DT.Rows.Count % 16777216 % 65536 / 256), (byte) (DT.Rows.Count % 16777216 / 65536), (byte) (DT.Rows.Count / 16777216) };
      fileStream.Write(buffer2, 0, buffer2.Length);
      int num1 = (DT.Columns.Count + 1) * 32 + 1;
      byte[] buffer3 = new byte[2]{ (byte) (num1 % 256), (byte) (num1 / 256) };
      fileStream.Write(buffer3, 0, buffer3.Length);
      string[] strArray1 = new string[DT.Columns.Count];
      string[] strArray2 = new string[DT.Columns.Count];
      byte[] numArray1 = new byte[DT.Columns.Count];
      byte[] numArray2 = new byte[DT.Columns.Count];
      int num2 = 1;
      foreach (DataColumn column in (InternalDataCollectionBase) DT.Columns)
      {
        string upper = column.ColumnName.ToUpper();
        while (upper.Length < 10)
          upper += "\0";
        strArray1[column.Ordinal] = upper.Substring(0, 10) + "\0";
        strArray2[column.Ordinal] = "C";
        numArray1[column.Ordinal] = (byte) 50;
        numArray2[column.Ordinal] = (byte) 0;
        string s = column.DataType.ToString();
        // ISSUE: reference to a compiler-generated method
        uint stringHash = PrivateImplementationDetails.ComputeStringHash(s);
        if (stringHash <= 1741144581U)
        {
          if (stringHash <= 875577056U)
          {
            if ((int) stringHash != 347085918)
            {
              if ((int) stringHash != 848225627)
              {
                if ((int) stringHash == 875577056 && s == "System.UInt64")
                {
                  strArray2[column.Ordinal] = "N";
                  numArray1[column.Ordinal] = (byte) 21;
                }
              }
              else if (s == "System.Double")
              {
                strArray2[column.Ordinal] = "F";
                numArray1[column.Ordinal] = (byte) 38;
                numArray2[column.Ordinal] = (byte) 5;
              }
            }
            else if (s == "System.Boolean")
            {
              strArray2[column.Ordinal] = "L";
              numArray1[column.Ordinal] = (byte) 1;
            }
          }
          else if (stringHash <= 1541528931U)
          {
            if ((int) stringHash != 942540437)
            {
              if ((int) stringHash == 1541528931 && s == "System.DateTime")
              {
                strArray2[column.Ordinal] = "D";
                numArray1[column.Ordinal] = (byte) 8;
              }
            }
            else if (s == "System.UInt16")
            {
              strArray2[column.Ordinal] = "N";
              numArray1[column.Ordinal] = (byte) 6;
            }
          }
          else if ((int) stringHash != 1697786220)
          {
            if ((int) stringHash == 1741144581 && s == "System.Decimal")
            {
              strArray2[column.Ordinal] = "N";
              numArray1[column.Ordinal] = (byte) 38;
              numArray2[column.Ordinal] = (byte) 5;
            }
          }
          else if (s == "System.Int16")
          {
            strArray2[column.Ordinal] = "N";
            numArray1[column.Ordinal] = (byte) 6;
          }
        }
        else if (stringHash <= 2747029693U)
        {
          if ((int) stringHash != 1764058053)
          {
            if ((int) stringHash != -2109583554)
            {
              if ((int) stringHash == -1547937603 && s == "System.SByte")
              {
                strArray2[column.Ordinal] = "N";
                numArray1[column.Ordinal] = (byte) 6;
              }
            }
            else if (s == "System.Single")
            {
              strArray2[column.Ordinal] = "F";
              numArray1[column.Ordinal] = (byte) 38;
              numArray2[column.Ordinal] = (byte) 5;
            }
          }
          else if (s == "System.Int64")
          {
            strArray2[column.Ordinal] = "N";
            numArray1[column.Ordinal] = (byte) 21;
          }
        }
        else if (stringHash <= 3291009739U)
        {
          if ((int) stringHash != -1215022916)
          {
            if ((int) stringHash == -1003957557 && s == "System.UInt32")
            {
              strArray2[column.Ordinal] = "N";
              numArray1[column.Ordinal] = (byte) 11;
            }
          }
          else if (s == "System.Byte")
          {
            strArray2[column.Ordinal] = "N";
            numArray1[column.Ordinal] = (byte) 1;
          }
        }
        else if ((int) stringHash != -114490822)
        {
          if ((int) stringHash == -93602905 && s == "System.String")
          {
            DataTable dataTable = DT.Copy();
            dataTable.Columns.Add("StringLengthMathColumn", System.Type.GetType("System.Int32"), "LEN(" + column.ColumnName + ")");
            DataRow[] dataRowArray = dataTable.Select("", "StringLengthMathColumn DESC");
            if ((uint) dataRowArray.Length > 0U)
            {
              if (dataRowArray[0]["StringLengthMathColumn"].ToString() != "")
              {
                int num3 = (int) dataRowArray[0]["StringLengthMathColumn"];
                numArray1[column.Ordinal] = num3 <= (int) byte.MaxValue ? (byte) num3 : byte.MaxValue;
              }
              if ((int) numArray1[column.Ordinal] == 0)
                numArray1[column.Ordinal] = (byte) 1;
            }
          }
        }
        else if (s == "System.Int32")
        {
          strArray2[column.Ordinal] = "N";
          numArray1[column.Ordinal] = (byte) 11;
        }
        num2 += (int) numArray1[column.Ordinal];
      }
      byte[] buffer4 = new byte[2]{ (byte) (num2 % 256), (byte) (num2 / 256) };
      fileStream.Write(buffer4, 0, buffer4.Length);
      for (int index = 0; index < 20; ++index)
        fileStream.WriteByte((byte) 0);
      foreach (DataColumn column in (InternalDataCollectionBase) DT.Columns)
      {
        byte[] bytes = Encoding.Default.GetBytes(strArray1[column.Ordinal]);
        fileStream.Write(bytes, 0, bytes.Length);
        byte[] numArray3 = new byte[5];
        numArray3[0] = Encoding.ASCII.GetBytes(strArray2[column.Ordinal])[0];
        byte[] buffer5 = numArray3;
        fileStream.Write(buffer5, 0, buffer5.Length);
        byte[] buffer6 = new byte[2]{ numArray1[column.Ordinal], numArray2[column.Ordinal] };
        fileStream.Write(buffer6, 0, buffer6.Length);
        byte[] buffer7 = new byte[14];
        fileStream.Write(buffer7, 0, buffer7.Length);
      }
      fileStream.WriteByte((byte) 13);
      DateTimeFormatInfo dateTimeFormat = new CultureInfo("en-US", false).DateTimeFormat;
      NumberFormatInfo numberFormat = new CultureInfo("en-US", false).NumberFormat;
      string str1 = "";
      while (str1.Length < (int) byte.MaxValue)
        str1 += " ";
      foreach (DataRow row in (InternalDataCollectionBase) DT.Rows)
      {
        fileStream.WriteByte((byte) 32);
        foreach (DataColumn column in (InternalDataCollectionBase) DT.Columns)
        {
          string s = row[column].ToString();
          if (s != "")
          {
            string str2 = strArray2[column.Ordinal];
            s = str2 == "L" ? bool.Parse(s).ToString() : (str2 == "N" ? Decimal.Parse(s).ToString((IFormatProvider) numberFormat) : (str2 == "F" ? float.Parse(s).ToString((IFormatProvider) numberFormat) : (str2 == "D" ? DateTime.Parse(s).ToString("yyyyMMdd", (IFormatProvider) dateTimeFormat) : s.Trim() + str1)));
          }
          else if (strArray2[column.Ordinal] == "C" || strArray2[column.Ordinal] == "D")
            s = str1;
          while (s.Length < (int) numArray1[column.Ordinal])
            s += "\0";
          byte[] bytes = Encoding.GetEncoding(866).GetBytes(s.Substring(0, (int) numArray1[column.Ordinal]));
          fileStream.Write(bytes, 0, bytes.Length);
        }
      }
      fileStream.WriteByte((byte) 26);
      fileStream.Close();
    }

    public class dbf_field
    {
      public string Name;
      public string DataType;

      public OleDbType ParamType
      {
        get
        {
          OleDbType oleDbType = OleDbType.VarChar;
          string str = this.DataType.ToUpper().Substring(0, 1);
          if (!(str == "C"))
          {
            if (!(str == "L"))
            {
              if (!(str == "I"))
              {
                if (!(str == "N"))
                {
                  if (str == "D")
                    oleDbType = OleDbType.Date;
                }
                else
                  oleDbType = OleDbType.Numeric;
              }
              else
                oleDbType = OleDbType.Integer;
            }
            else
              oleDbType = OleDbType.Boolean;
          }
          else
            oleDbType = OleDbType.VarChar;
          return oleDbType;
        }
      }

      public dbf_field(string name, string data_type)
      {
        this.Name = name;
        this.DataType = data_type;
      }
    }
  }
}
