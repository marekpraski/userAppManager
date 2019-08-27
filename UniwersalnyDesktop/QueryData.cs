﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniwersalnyDesktop
{
    public class QueryData
    {

        private List<object[]> readData;
        private List<string> headers;
        private List<string> dataTypes;     //typy danych w kolumnach

        public QueryData()
        {
            readData = new List<object[]>();
            headers = new List<string>();
            dataTypes = new List<string>();
        }

        public List<object[]> getQueryData()
        {
            return readData;
        }

        public List<string[]> getQueryDataAsStrings()
        {
            List<string[]> dataAsStrings = new List<string[]>();
            foreach(object[] rowData in readData)
            {
                string[] stringRowData = new string[rowData.Length];
                for(int i=0; i<rowData.Length; i++)
                {
                    string stringItem = rowData[i].ToString();
                    stringRowData[i] = stringItem;
                }
                dataAsStrings.Add(stringRowData);
            }
            return dataAsStrings;
        }

        public List<string> getHeaders()
        {
            return headers;
        }

        public void addQueryData(object[] rowData)
        {
            readData.Add(rowData);
        }        

        public void addHeader(string header)
        {
            headers.Add(header);
        }

        public void addDataType(string dataType)
        {
            dataTypes.Add(dataType);
        }

        public List<string> getDataTypes()
        {
            return dataTypes;
        }

    }
}
