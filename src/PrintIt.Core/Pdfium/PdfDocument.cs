﻿using System;
using System.IO;

namespace PrintIt.Core.Pdfium
{
    public abstract class PdfDocument : IDisposable
    {
        private readonly NativeMethods.DocumentHandle _documentHandle;

        internal PdfDocument(NativeMethods.DocumentHandle documentHandle)
        {
            _documentHandle = documentHandle;
        }

        public static MemoryBasedPdfDocument Open(Stream stream, string password = null)
        {
            password = password ?? string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                var buffer = memoryStream.ToArray();
                var documentHandle = NativeMethods.LoadDocument(buffer, buffer.Length, password);
                return new MemoryBasedPdfDocument(documentHandle, buffer);
            }
        }
        
        public virtual void Dispose()
        {
            _documentHandle.Dispose();
        }

        public int PageCount => NativeMethods.GetPageCount(_documentHandle);
    }
}