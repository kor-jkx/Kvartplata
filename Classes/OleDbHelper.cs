// Decompiled with JetBrains decompiler
// Type: Kvartplata.Classes.OleDbHelper
// Assembly: Kvartplata, Version=1.16.3.10, Culture=neutral, PublicKeyToken=null
// MVID: 07D514F7-94DF-4C8B-8A8A-7DDC3C754113
// Assembly location: S:\soft-work\kor-jkx\kvartplata\Kvartplata.exe

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace Kvartplata.Classes
{
  public sealed class OleDbHelper
  {
    private OleDbHelper()
    {
    }

    private static void AttachParameters(OleDbCommand command, IEnumerable<OleDbParameter> commandParameters)
    {
      if (command == null)
        throw new ArgumentNullException("command");
      if (commandParameters == null)
        return;
      foreach (OleDbParameter commandParameter in commandParameters)
      {
        if (commandParameter != null)
        {
          if ((commandParameter.Direction == ParameterDirection.InputOutput || commandParameter.Direction == ParameterDirection.Input) && commandParameter.Value == null)
            commandParameter.Value = (object) DBNull.Value;
          command.Parameters.Add(commandParameter);
        }
      }
    }

    private static void AssignParameterValues(IEnumerable<OleDbParameter> commandParameters, DataRow dataRow)
    {
      if (commandParameters == null || dataRow == null)
        return;
      int num = 0;
      foreach (OleDbParameter commandParameter in commandParameters)
      {
        if (commandParameter.ParameterName == null || commandParameter.ParameterName.Length <= 1)
          throw new Exception(string.Format("Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.", (object) num, (object) commandParameter.ParameterName));
        if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
          commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
        ++num;
      }
    }

    private static void AssignParameterValues(OleDbParameter[] commandParameters, object[] parameterValues)
    {
      if (commandParameters == null || parameterValues == null)
        return;
      if (commandParameters.Length != parameterValues.Length)
        throw new ArgumentException("Parameter count does not match Parameter Value count.");
      int index = 0;
      for (int length = commandParameters.Length; index < length; ++index)
      {
        if (parameterValues[index] is IDbDataParameter)
        {
          IDbDataParameter parameterValue = (IDbDataParameter) parameterValues[index];
          if (parameterValue.Value == null)
            commandParameters[index].Value = (object) DBNull.Value;
          else
            commandParameters[index].Value = parameterValue.Value;
        }
        else if (parameterValues[index] == null)
          commandParameters[index].Value = (object) DBNull.Value;
        else
          commandParameters[index].Value = parameterValues[index];
      }
    }

    private static void PrepareCommand(OleDbCommand command, OleDbConnection connection, OleDbTransaction transaction, CommandType commandType, string commandText, int executeTimeout, OleDbParameter[] commandParameters, out bool mustCloseConnection)
    {
      if (command == null)
        throw new ArgumentNullException("command");
      if (commandText == null || commandText.Length == 0)
        throw new ArgumentNullException("commandText");
      if (connection.State != ConnectionState.Open)
      {
        mustCloseConnection = true;
        connection.Open();
      }
      else
        mustCloseConnection = false;
      command.Connection = connection;
      command.CommandText = commandText;
      command.CommandTimeout = executeTimeout;
      if (transaction != null)
      {
        if (transaction.Connection == null)
          throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        command.Transaction = transaction;
      }
      command.CommandType = commandType;
      if (commandParameters == null)
        return;
      OleDbHelper.AttachParameters(command, (IEnumerable<OleDbParameter>) commandParameters);
    }

    public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, int executeTimeout)
    {
      return OleDbHelper.ExecuteNonQuery(connectionString, commandType, commandText, executeTimeout, (OleDbParameter[]) null);
    }

    public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, int executeTimeout, params OleDbParameter[] commandParameters)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      using (OleDbConnection connection = new OleDbConnection(connectionString))
      {
        connection.Open();
        return OleDbHelper.ExecuteNonQuery(connection, commandType, commandText, executeTimeout, commandParameters);
      }
    }

    public static int ExecuteNonQuery(string connectionString, string spName, int executeTimeout, params object[] parameterValues)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (parameterValues == null || (uint) parameterValues.Length <= 0U)
        return OleDbHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connectionString, spName);
      OleDbHelper.AssignParameterValues(spParameterSet, parameterValues);
      return OleDbHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static int ExecuteNonQuery(OleDbConnection connection, CommandType commandType, string commandText, int executeTimeout)
    {
      return OleDbHelper.ExecuteNonQuery(connection, commandType, commandText, executeTimeout, (OleDbParameter[]) null);
    }

    public static int ExecuteNonQuery(OleDbConnection connection, CommandType commandType, string commandText, int executeTimeout, params OleDbParameter[] commandParameters)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      OleDbCommand command = new OleDbCommand();
      bool mustCloseConnection = false;
      OleDbHelper.PrepareCommand(command, connection, (OleDbTransaction) null, commandType, commandText, executeTimeout, commandParameters, out mustCloseConnection);
      int num = command.ExecuteNonQuery();
      command.Parameters.Clear();
      if (mustCloseConnection)
        connection.Close();
      return num;
    }

    public static int ExecuteNonQuery(OleDbConnection connection, string spName, int executeTimeout, params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (parameterValues == null || (uint) parameterValues.Length <= 0U)
        return OleDbHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connection, spName);
      OleDbHelper.AssignParameterValues(spParameterSet, parameterValues);
      return OleDbHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static int ExecuteNonQuery(OleDbTransaction transaction, CommandType commandType, string commandText, int executeTimeout)
    {
      return OleDbHelper.ExecuteNonQuery(transaction, commandType, commandText, executeTimeout, (OleDbParameter[]) null);
    }

    public static int ExecuteNonQuery(OleDbTransaction transaction, CommandType commandType, string commandText, int executeTimeout, params OleDbParameter[] commandParameters)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      OleDbCommand command = new OleDbCommand();
      bool mustCloseConnection = false;
      OleDbHelper.PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, executeTimeout, commandParameters, out mustCloseConnection);
      int num = command.ExecuteNonQuery();
      command.Parameters.Clear();
      return num;
    }

    public static int ExecuteNonQuery(OleDbTransaction transaction, string spName, int executeTimeout, params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (parameterValues == null || (uint) parameterValues.Length <= 0U)
        return OleDbHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      OleDbHelper.AssignParameterValues(spParameterSet, parameterValues);
      return OleDbHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, int executeTimeout)
    {
      return OleDbHelper.ExecuteDataset(connectionString, commandType, commandText, executeTimeout, (OleDbParameter[]) null);
    }

    public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, int executeTimeout, params OleDbParameter[] commandParameters)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      using (OleDbConnection connection = new OleDbConnection(connectionString))
      {
        connection.Open();
        return OleDbHelper.ExecuteDataset(connection, commandType, commandText, executeTimeout, commandParameters);
      }
    }

    public static DataSet ExecuteDataset(string connectionString, string spName, int executeTimeout, params object[] parameterValues)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (parameterValues == null || (uint) parameterValues.Length <= 0U)
        return OleDbHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connectionString, spName);
      OleDbHelper.AssignParameterValues(spParameterSet, parameterValues);
      return OleDbHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static DataSet ExecuteDataset(OleDbConnection connection, CommandType commandType, string commandText, int executeTimeout)
    {
      return OleDbHelper.ExecuteDataset(connection, commandType, commandText, executeTimeout, (OleDbParameter[]) null);
    }

    public static DataSet ExecuteDataset(OleDbConnection connection, CommandType commandType, string commandText, int executeTimeout, params OleDbParameter[] commandParameters)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      OleDbCommand oleDbCommand = new OleDbCommand();
      bool mustCloseConnection = false;
      OleDbHelper.PrepareCommand(oleDbCommand, connection, (OleDbTransaction) null, commandType, commandText, executeTimeout, commandParameters, out mustCloseConnection);
      using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
      {
        DataSet dataSet = new DataSet();
        oleDbDataAdapter.Fill(dataSet);
        oleDbCommand.Parameters.Clear();
        if (mustCloseConnection)
          connection.Close();
        return dataSet;
      }
    }

    public static DataSet ExecuteDataset(OleDbConnection connection, string spName, int executeTimeout, params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (parameterValues == null || (uint) parameterValues.Length <= 0U)
        return OleDbHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connection, spName);
      OleDbHelper.AssignParameterValues(spParameterSet, parameterValues);
      return OleDbHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static DataSet ExecuteDataset(OleDbTransaction transaction, CommandType commandType, string commandText, int executeTimeout)
    {
      return OleDbHelper.ExecuteDataset(transaction, commandType, commandText, executeTimeout, (OleDbParameter[]) null);
    }

    public static DataSet ExecuteDataset(OleDbTransaction transaction, CommandType commandType, string commandText, int executeTimeout, params OleDbParameter[] commandParameters)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      OleDbCommand oleDbCommand = new OleDbCommand();
      bool mustCloseConnection = false;
      OleDbHelper.PrepareCommand(oleDbCommand, transaction.Connection, transaction, commandType, commandText, executeTimeout, commandParameters, out mustCloseConnection);
      using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
      {
        DataSet dataSet = new DataSet();
        oleDbDataAdapter.Fill(dataSet);
        oleDbCommand.Parameters.Clear();
        return dataSet;
      }
    }

    public static DataSet ExecuteDataset(OleDbTransaction transaction, string spName, int executeTimeout, params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (parameterValues == null || (uint) parameterValues.Length <= 0U)
        return OleDbHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      OleDbHelper.AssignParameterValues(spParameterSet, parameterValues);
      return OleDbHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    private static OleDbDataReader ExecuteReader(OleDbConnection connection, OleDbTransaction transaction, CommandType commandType, string commandText, int executeTimeout, OleDbParameter[] commandParameters, OleDbHelper.OleDbConnectionOwnership connectionOwnership)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      bool mustCloseConnection = false;
      OleDbCommand command = new OleDbCommand();
      try
      {
        OleDbHelper.PrepareCommand(command, connection, transaction, commandType, commandText, executeTimeout, commandParameters, out mustCloseConnection);
        OleDbDataReader oleDbDataReader = connectionOwnership != OleDbHelper.OleDbConnectionOwnership.External ? command.ExecuteReader(CommandBehavior.CloseConnection) : command.ExecuteReader();
        bool flag = true;
        foreach (DbParameter parameter in (DbParameterCollection) command.Parameters)
        {
          if (parameter.Direction != ParameterDirection.Input)
            flag = false;
        }
        if (flag)
          command.Parameters.Clear();
        return oleDbDataReader;
      }
      catch
      {
        if (mustCloseConnection)
          connection.Close();
        throw;
      }
    }

    public static OleDbDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, int executeTimeout)
    {
      return OleDbHelper.ExecuteReader(connectionString, commandType, commandText, executeTimeout, (OleDbParameter[]) null);
    }

    public static OleDbDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, int executeTimeout, params OleDbParameter[] commandParameters)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      OleDbConnection connection = (OleDbConnection) null;
      try
      {
        connection = new OleDbConnection(connectionString);
        connection.Open();
        return OleDbHelper.ExecuteReader(connection, (OleDbTransaction) null, commandType, commandText, executeTimeout, commandParameters, OleDbHelper.OleDbConnectionOwnership.Internal);
      }
      catch
      {
        if (connection != null)
          connection.Close();
        throw;
      }
    }

    public static OleDbDataReader ExecuteReader(string connectionString, string spName, int executeTimeout, params object[] parameterValues)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (parameterValues == null || (uint) parameterValues.Length <= 0U)
        return OleDbHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connectionString, spName);
      OleDbHelper.AssignParameterValues(spParameterSet, parameterValues);
      return OleDbHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static OleDbDataReader ExecuteReader(OleDbConnection connection, CommandType commandType, string commandText, int executeTimeout)
    {
      return OleDbHelper.ExecuteReader(connection, commandType, commandText, executeTimeout, (OleDbParameter[]) null);
    }

    public static OleDbDataReader ExecuteReader(OleDbConnection connection, CommandType commandType, string commandText, int executeTimeout, params OleDbParameter[] commandParameters)
    {
      return OleDbHelper.ExecuteReader(connection, (OleDbTransaction) null, commandType, commandText, executeTimeout, commandParameters, OleDbHelper.OleDbConnectionOwnership.External);
    }

    public static OleDbDataReader ExecuteReader(OleDbConnection connection, string spName, int executeTimeout, params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (parameterValues == null || (uint) parameterValues.Length <= 0U)
        return OleDbHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connection, spName);
      OleDbHelper.AssignParameterValues(spParameterSet, parameterValues);
      return OleDbHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static OleDbDataReader ExecuteReader(OleDbTransaction transaction, CommandType commandType, string commandText, int executeTimeout)
    {
      return OleDbHelper.ExecuteReader(transaction, commandType, commandText, executeTimeout, (OleDbParameter[]) null);
    }

    public static OleDbDataReader ExecuteReader(OleDbTransaction transaction, CommandType commandType, string commandText, int executeTimeout, params OleDbParameter[] commandParameters)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      return OleDbHelper.ExecuteReader(transaction.Connection, transaction, commandType, commandText, executeTimeout, commandParameters, OleDbHelper.OleDbConnectionOwnership.External);
    }

    public static OleDbDataReader ExecuteReader(OleDbTransaction transaction, string spName, int executeTimeout, params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (parameterValues == null || (uint) parameterValues.Length <= 0U)
        return OleDbHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      OleDbHelper.AssignParameterValues(spParameterSet, parameterValues);
      return OleDbHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, int executeTimeout)
    {
      return OleDbHelper.ExecuteScalar(connectionString, commandType, commandText, executeTimeout, (OleDbParameter[]) null);
    }

    public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, int executeTimeout, params OleDbParameter[] commandParameters)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      using (OleDbConnection connection = new OleDbConnection(connectionString))
      {
        connection.Open();
        return OleDbHelper.ExecuteScalar(connection, commandType, commandText, executeTimeout, commandParameters);
      }
    }

    public static object ExecuteScalar(string connectionString, string spName, int executeTimeout, params object[] parameterValues)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (parameterValues == null || (uint) parameterValues.Length <= 0U)
        return OleDbHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connectionString, spName);
      OleDbHelper.AssignParameterValues(spParameterSet, parameterValues);
      return OleDbHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static object ExecuteScalar(OleDbConnection connection, CommandType commandType, string commandText, int executeTimeout)
    {
      return OleDbHelper.ExecuteScalar(connection, commandType, commandText, executeTimeout, (OleDbParameter[]) null);
    }

    public static object ExecuteScalar(OleDbConnection connection, CommandType commandType, string commandText, int executeTimeout, params OleDbParameter[] commandParameters)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      OleDbCommand command = new OleDbCommand();
      bool mustCloseConnection = false;
      OleDbHelper.PrepareCommand(command, connection, (OleDbTransaction) null, commandType, commandText, executeTimeout, commandParameters, out mustCloseConnection);
      object obj = command.ExecuteScalar();
      command.Parameters.Clear();
      if (mustCloseConnection)
        connection.Close();
      return obj;
    }

    public static object ExecuteScalar(OleDbConnection connection, string spName, int executeTimeout, params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (parameterValues == null || (uint) parameterValues.Length <= 0U)
        return OleDbHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connection, spName);
      OleDbHelper.AssignParameterValues(spParameterSet, parameterValues);
      return OleDbHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static object ExecuteScalar(OleDbTransaction transaction, CommandType commandType, string commandText, int executeTimeout)
    {
      return OleDbHelper.ExecuteScalar(transaction, commandType, commandText, executeTimeout, (OleDbParameter[]) null);
    }

    public static object ExecuteScalar(OleDbTransaction transaction, CommandType commandType, string commandText, int executeTimeout, params OleDbParameter[] commandParameters)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      OleDbCommand command = new OleDbCommand();
      bool mustCloseConnection = false;
      OleDbHelper.PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, executeTimeout, commandParameters, out mustCloseConnection);
      object obj = command.ExecuteScalar();
      command.Parameters.Clear();
      return obj;
    }

    public static object ExecuteScalar(OleDbTransaction transaction, string spName, int executeTimeout, params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (parameterValues == null || (uint) parameterValues.Length <= 0U)
        return OleDbHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      OleDbHelper.AssignParameterValues(spParameterSet, parameterValues);
      return OleDbHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static void FillDataset(string connectionString, CommandType commandType, string commandText, int executeTimeout, DataSet dataSet, string[] tableNames)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      if (dataSet == null)
        throw new ArgumentNullException("dataSet");
      using (OleDbConnection connection = new OleDbConnection(connectionString))
      {
        connection.Open();
        OleDbHelper.FillDataset(connection, commandType, commandText, executeTimeout, dataSet, tableNames);
      }
    }

    public static void FillDataset(string connectionString, CommandType commandType, string commandText, int executeTimeout, DataSet dataSet, string[] tableNames, params OleDbParameter[] commandParameters)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      if (dataSet == null)
        throw new ArgumentNullException("dataSet");
      using (OleDbConnection connection = new OleDbConnection(connectionString))
      {
        connection.Open();
        OleDbHelper.FillDataset(connection, commandType, commandText, executeTimeout, dataSet, tableNames, commandParameters);
      }
    }

    public static void FillDataset(string connectionString, string spName, int executeTimeout, DataSet dataSet, string[] tableNames, params object[] parameterValues)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      if (dataSet == null)
        throw new ArgumentNullException("dataSet");
      using (OleDbConnection connection = new OleDbConnection(connectionString))
      {
        connection.Open();
        OleDbHelper.FillDataset(connection, spName, executeTimeout, dataSet, tableNames, parameterValues);
      }
    }

    public static void FillDataset(OleDbConnection connection, CommandType commandType, string commandText, int executeTimeout, DataSet dataSet, string[] tableNames)
    {
      OleDbHelper.FillDataset(connection, commandType, commandText, executeTimeout, dataSet, tableNames, (OleDbParameter[]) null);
    }

    public static void FillDataset(OleDbConnection connection, CommandType commandType, string commandText, int executeTimeout, DataSet dataSet, string[] tableNames, params OleDbParameter[] commandParameters)
    {
      OleDbHelper.FillDataset(connection, (OleDbTransaction) null, commandType, commandText, executeTimeout, dataSet, tableNames, commandParameters);
    }

    public static void FillDataset(OleDbConnection connection, string spName, int executeTimeout, DataSet dataSet, string[] tableNames, params object[] parameterValues)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (dataSet == null)
        throw new ArgumentNullException("dataSet");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (parameterValues != null && (uint) parameterValues.Length > 0U)
      {
        OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connection, spName);
        OleDbHelper.AssignParameterValues(spParameterSet, parameterValues);
        OleDbHelper.FillDataset(connection, CommandType.StoredProcedure, spName, executeTimeout, dataSet, tableNames, spParameterSet);
      }
      else
        OleDbHelper.FillDataset(connection, CommandType.StoredProcedure, spName, executeTimeout, dataSet, tableNames);
    }

    public static void FillDataset(OleDbTransaction transaction, CommandType commandType, string commandText, int executeTimeout, DataSet dataSet, string[] tableNames)
    {
      OleDbHelper.FillDataset(transaction, commandType, commandText, executeTimeout, dataSet, tableNames, (OleDbParameter[]) null);
    }

    public static void FillDataset(OleDbTransaction transaction, CommandType commandType, string commandText, int executeTimeout, DataSet dataSet, string[] tableNames, params OleDbParameter[] commandParameters)
    {
      OleDbHelper.FillDataset(transaction.Connection, transaction, commandType, commandText, executeTimeout, dataSet, tableNames, commandParameters);
    }

    public static void FillDataset(OleDbTransaction transaction, string spName, int executeTimeout, DataSet dataSet, string[] tableNames, params object[] parameterValues)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if (dataSet == null)
        throw new ArgumentNullException("dataSet");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (parameterValues != null && (uint) parameterValues.Length > 0U)
      {
        OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
        OleDbHelper.AssignParameterValues(spParameterSet, parameterValues);
        OleDbHelper.FillDataset(transaction, CommandType.StoredProcedure, spName, executeTimeout, dataSet, tableNames, spParameterSet);
      }
      else
        OleDbHelper.FillDataset(transaction, CommandType.StoredProcedure, spName, executeTimeout, dataSet, tableNames);
    }

    private static void FillDataset(OleDbConnection connection, OleDbTransaction transaction, CommandType commandType, string commandText, int executeTimeout, DataSet dataSet, string[] tableNames, params OleDbParameter[] commandParameters)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (dataSet == null)
        throw new ArgumentNullException("dataSet");
      OleDbCommand oleDbCommand = new OleDbCommand();
      bool mustCloseConnection = false;
      OleDbHelper.PrepareCommand(oleDbCommand, connection, transaction, commandType, commandText, executeTimeout, commandParameters, out mustCloseConnection);
      using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(oleDbCommand))
      {
        if (tableNames != null && (uint) tableNames.Length > 0U)
        {
          string sourceTable = "Table";
          for (int index = 0; index < tableNames.Length; ++index)
          {
            if (tableNames[index] == null || tableNames[index].Length == 0)
              throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
            oleDbDataAdapter.TableMappings.Add(sourceTable, tableNames[index]);
            sourceTable += (index + 1).ToString();
          }
        }
        oleDbDataAdapter.Fill(dataSet);
        oleDbCommand.Parameters.Clear();
      }
      if (!mustCloseConnection)
        return;
      connection.Close();
    }

    public static void UpdateDataset(OleDbCommand insertCommand, OleDbCommand deleteCommand, OleDbCommand updateCommand, DataSet dataSet, string tableName)
    {
      if (insertCommand == null)
        throw new ArgumentNullException("insertCommand");
      if (deleteCommand == null)
        throw new ArgumentNullException("deleteCommand");
      if (updateCommand == null)
        throw new ArgumentNullException("updateCommand");
      if (tableName == null || tableName.Length == 0)
        throw new ArgumentNullException("tableName");
      using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter())
      {
        oleDbDataAdapter.UpdateCommand = updateCommand;
        oleDbDataAdapter.InsertCommand = insertCommand;
        oleDbDataAdapter.DeleteCommand = deleteCommand;
        oleDbDataAdapter.Update(dataSet, tableName);
        dataSet.AcceptChanges();
      }
    }

    public static OleDbCommand CreateCommand(OleDbConnection connection, string spName, params string[] sourceColumns)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      OleDbCommand command = new OleDbCommand(spName, connection);
      command.CommandType = CommandType.StoredProcedure;
      if (sourceColumns != null && (uint) sourceColumns.Length > 0U)
      {
        OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connection, spName);
        for (int index = 0; index < sourceColumns.Length; ++index)
          spParameterSet[index].SourceColumn = sourceColumns[index];
        OleDbHelper.AttachParameters(command, (IEnumerable<OleDbParameter>) spParameterSet);
      }
      return command;
    }

    public static int ExecuteNonQueryTypedParams(string connectionString, string spName, int executeTimeout, DataRow dataRow)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (dataRow == null || (uint) dataRow.ItemArray.Length <= 0U)
        return OleDbHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connectionString, spName);
      OleDbHelper.AssignParameterValues((IEnumerable<OleDbParameter>) spParameterSet, dataRow);
      return OleDbHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static int ExecuteNonQueryTypedParams(OleDbConnection connection, string spName, int executeTimeout, DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (dataRow == null || (uint) dataRow.ItemArray.Length <= 0U)
        return OleDbHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connection, spName);
      OleDbHelper.AssignParameterValues((IEnumerable<OleDbParameter>) spParameterSet, dataRow);
      return OleDbHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static int ExecuteNonQueryTypedParams(OleDbTransaction transaction, string spName, int executeTimeout, DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (dataRow == null || (uint) dataRow.ItemArray.Length <= 0U)
        return OleDbHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      OleDbHelper.AssignParameterValues((IEnumerable<OleDbParameter>) spParameterSet, dataRow);
      return OleDbHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static DataSet ExecuteDatasetTypedParams(string connectionString, string spName, int executeTimeout, DataRow dataRow)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (dataRow == null || (uint) dataRow.ItemArray.Length <= 0U)
        return OleDbHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connectionString, spName);
      OleDbHelper.AssignParameterValues((IEnumerable<OleDbParameter>) spParameterSet, dataRow);
      return OleDbHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static DataSet ExecuteDatasetTypedParams(OleDbConnection connection, string spName, int executeTimeout, DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (dataRow == null || (uint) dataRow.ItemArray.Length <= 0U)
        return OleDbHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connection, spName);
      OleDbHelper.AssignParameterValues((IEnumerable<OleDbParameter>) spParameterSet, dataRow);
      return OleDbHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static DataSet ExecuteDatasetTypedParams(OleDbTransaction transaction, string spName, int executeTimeout, DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (dataRow == null || (uint) dataRow.ItemArray.Length <= 0U)
        return OleDbHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      OleDbHelper.AssignParameterValues((IEnumerable<OleDbParameter>) spParameterSet, dataRow);
      return OleDbHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static OleDbDataReader ExecuteReaderTypedParams(string connectionString, string spName, int executeTimeout, DataRow dataRow)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (dataRow == null || (uint) dataRow.ItemArray.Length <= 0U)
        return OleDbHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connectionString, spName);
      OleDbHelper.AssignParameterValues((IEnumerable<OleDbParameter>) spParameterSet, dataRow);
      return OleDbHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static OleDbDataReader ExecuteReaderTypedParams(OleDbConnection connection, string spName, int executeTimeout, DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (dataRow == null || (uint) dataRow.ItemArray.Length <= 0U)
        return OleDbHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connection, spName);
      OleDbHelper.AssignParameterValues((IEnumerable<OleDbParameter>) spParameterSet, dataRow);
      return OleDbHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static OleDbDataReader ExecuteReaderTypedParams(OleDbTransaction transaction, string spName, int executeTimeout, DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (dataRow == null || (uint) dataRow.ItemArray.Length <= 0U)
        return OleDbHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      OleDbHelper.AssignParameterValues((IEnumerable<OleDbParameter>) spParameterSet, dataRow);
      return OleDbHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static object ExecuteScalarTypedParams(string connectionString, string spName, int executeTimeout, DataRow dataRow)
    {
      if (connectionString == null || connectionString.Length == 0)
        throw new ArgumentNullException("connectionString");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (dataRow == null || (uint) dataRow.ItemArray.Length <= 0U)
        return OleDbHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connectionString, spName);
      OleDbHelper.AssignParameterValues((IEnumerable<OleDbParameter>) spParameterSet, dataRow);
      return OleDbHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static object ExecuteScalarTypedParams(OleDbConnection connection, string spName, int executeTimeout, DataRow dataRow)
    {
      if (connection == null)
        throw new ArgumentNullException("connection");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (dataRow == null || (uint) dataRow.ItemArray.Length <= 0U)
        return OleDbHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(connection, spName);
      OleDbHelper.AssignParameterValues((IEnumerable<OleDbParameter>) spParameterSet, dataRow);
      return OleDbHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    public static object ExecuteScalarTypedParams(OleDbTransaction transaction, string spName, int executeTimeout, DataRow dataRow)
    {
      if (transaction == null)
        throw new ArgumentNullException("transaction");
      if (transaction != null && transaction.Connection == null)
        throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
      if (spName == null || spName.Length == 0)
        throw new ArgumentNullException("spName");
      if (dataRow == null || (uint) dataRow.ItemArray.Length <= 0U)
        return OleDbHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, executeTimeout);
      OleDbParameter[] spParameterSet = OleDbHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
      OleDbHelper.AssignParameterValues((IEnumerable<OleDbParameter>) spParameterSet, dataRow);
      return OleDbHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, executeTimeout, spParameterSet);
    }

    private enum OleDbConnectionOwnership
    {
      Internal,
      External,
    }
  }
}
