// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.OleDbHelperParameterCache
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace Kvartplata.Classes
{
  public sealed class OleDbHelperParameterCache
  {
    private static readonly Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

    private OleDbHelperParameterCache()
    {
    }

    private static OleDbParameter[] DiscoverSpParameterSet(OleDbConnection connection, string spName, bool includeReturnValueParameter)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      OleDbCommand command = new OleDbCommand(spName, connection);
      command.CommandType = CommandType.StoredProcedure;
      connection.Open();
      OleDbCommandBuilder.DeriveParameters(command);
      connection.Close();
      if (!includeReturnValueParameter)
        command.Parameters.RemoveAt(0);
      OleDbParameter[] array = new OleDbParameter[command.Parameters.Count];
      command.Parameters.CopyTo(array, 0);
      foreach (DbParameter dbParameter in array)
        dbParameter.Value = (object) DBNull.Value;
      return array;
    }

    private static OleDbParameter[] CloneParameters(OleDbParameter[] originalParameters)
    {
      OleDbParameter[] oleDbParameterArray = new OleDbParameter[originalParameters.Length];
      int index = 0;
      for (int length = originalParameters.Length; index < length; ++index)
        oleDbParameterArray[index] = (OleDbParameter) ((ICloneable) originalParameters[index]).Clone();
      return oleDbParameterArray;
    }

    public static void CacheParameterSet(string connectionString, string commandText, params OleDbParameter[] commandParameters)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      if (commandText == null || commandText.Length == 0)
        throw new ArgumentNullException("commandText");
      string str = connectionString + ":" + commandText;
      OleDbHelperParameterCache.paramCache[(object) str] = (object) commandParameters;
    }

    public static OleDbParameter[] GetCachedParameterSet(string connectionString, string commandText)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      if (commandText == null || commandText.Length == 0)
        throw new ArgumentNullException("commandText");
      string str = connectionString + ":" + commandText;
      OleDbParameter[] originalParameters = OleDbHelperParameterCache.paramCache[(object) str] as OleDbParameter[];
      if (originalParameters == null)
        return (OleDbParameter[]) null;
      return OleDbHelperParameterCache.CloneParameters(originalParameters);
    }

    public static OleDbParameter[] GetSpParameterSet(string connectionString, string spName)
    {
      return OleDbHelperParameterCache.GetSpParameterSet(connectionString, spName, false);
    }

    public static OleDbParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      using (OleDbConnection connection = new OleDbConnection(connectionString))
        return OleDbHelperParameterCache.GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
    }

    public static OleDbParameter[] GetSpParameterSet(OleDbConnection connection, string spName)
    {
      return OleDbHelperParameterCache.GetSpParameterSet(connection, spName, false);
    }

    internal static OleDbParameter[] GetSpParameterSet(OleDbConnection connection, string spName, bool includeReturnValueParameter)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      using (OleDbConnection connection1 = (OleDbConnection) ((ICloneable) connection).Clone())
        return OleDbHelperParameterCache.GetSpParameterSetInternal(connection1, spName, includeReturnValueParameter);
    }

    private static OleDbParameter[] GetSpParameterSetInternal(OleDbConnection connection, string spName, bool includeReturnValueParameter)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      string str = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");
      OleDbParameter[] originalParameters = OleDbHelperParameterCache.paramCache[(object) str] as OleDbParameter[];
      if (originalParameters == null)
      {
        OleDbParameter[] oleDbParameterArray = OleDbHelperParameterCache.DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
        OleDbHelperParameterCache.paramCache[(object) str] = (object) oleDbParameterArray;
        originalParameters = oleDbParameterArray;
      }
      return OleDbHelperParameterCache.CloneParameters(originalParameters);
    }
  }
}
