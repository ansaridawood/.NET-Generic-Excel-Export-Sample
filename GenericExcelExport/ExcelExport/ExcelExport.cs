﻿using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GenericExcelExport.ExcelExport
{

    public abstract class ExcelExport : IExcelExport
    {
        protected string _sheetName;
        protected string _fileName;
        protected List<string> _headers;
        protected List<string> _type;
        protected IWorkbook _workbook;
        protected ISheet _sheet;

        /// <summary>
        /// Common Code for the Export
        /// It creates Workbook, Sheet, Generate Header Cells and returns HttpResponseMessage
        /// </summary>
        /// <typeparam name="T">Generic Class Type</typeparam>
        /// <param name="exportData">Data to be exported</param>
        /// <param name="fileName">Export File Name</param>
        /// <param name="sheetName">First Sheet Name</param>
        /// <returns></returns>
        public HttpResponseMessage Export<T>(List<T> exportData, string fileName,
            bool appendDateTimeInFileName = false,
            string sheetName = Utility.DEFAULT_SHEET_NAME)
        {
            _sheetName = sheetName;

            _fileName = appendDateTimeInFileName
                ? $"{fileName}_{DateTime.Now.ToString(Utility.DEFAULT_FILE_DATETIME)}"
                : fileName;

            #region Generation of Workbook, Sheet and General Configuration
            _workbook = new XSSFWorkbook();
            _sheet = _workbook.CreateSheet(_sheetName);

            var headerStyle = _workbook.CreateCellStyle();
            var headerFont = _workbook.CreateFont();
            headerFont.IsBold = true;
            headerStyle.SetFont(headerFont);
            #endregion

            WriteData(exportData);

            #region Generating Header Cells
            var header = _sheet.CreateRow(0);
            for (var i = 0; i < _headers.Count; i++)
            {
                var cell = header.CreateCell(i);
                cell.SetCellValue(_headers[i]);
                cell.CellStyle = headerStyle;
                // It's heavy, it slows down your Excel if you have large data                
                _sheet.AutoSizeColumn(i);
            }
            #endregion

            #region Generating and Returning Stream for Excel
            using (var memoryStream = new MemoryStream())
            {
                _workbook.Write(memoryStream);
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(memoryStream.ToArray())
                };

                response.Content.Headers.ContentType = new MediaTypeHeaderValue(Utility.EXCEL_MEDIA_TYPE);
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(Utility.DISPOSITION_TYPE_ATTACHMENT)
                {
                    FileName = $"{_fileName}.xlsx"
                };

                return response;
            }
            #endregion
        }

        /// <summary>
        /// Generic Definition to handle all types of List
        /// Overrride this function to provide your own implementation
        /// </summary>
        /// <param name="exportData"></param>
        public abstract void WriteData<T>(List<T> exportData);
    }


}