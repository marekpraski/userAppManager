﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace UniwersalnyDesktop
{
    class DBReader
    {
        private SqlConnection dbConnection;
        private QueryData queryData;

        public DBReader(SqlConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }


        public QueryData readFromDB(string sqlQuery)
        {
            queryData = new QueryData();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, dbConnection);
                dbConnection.Open();
                SqlDataReader sqlReader = sqlCommand.ExecuteReader();

                int numberOfColumns = sqlReader.FieldCount;

                while (sqlReader.Read())
                {
                    object[] rowData = new object[numberOfColumns];
                    for (int i = 0; i < numberOfColumns; i++)
                    {
                        rowData[i] = sqlReader.GetValue(i).ToString();
                    }
                    queryData.addQueryData(rowData);
                }

                for (int i = 0; i < sqlReader.FieldCount; i++)
                {
                    queryData.addHeader(sqlReader.GetName(i));
                    queryData.addDataType(sqlReader.GetDataTypeName(i));
                }
                sqlReader.Close();
                sqlCommand.Dispose();
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                MyMessageBox.display(e.Message + "\r\n" + dbConnection.ConnectionString, MessageBoxType.Error);
               
            }

            dbConnection.Close();
            return queryData;
        }       
    }
}
