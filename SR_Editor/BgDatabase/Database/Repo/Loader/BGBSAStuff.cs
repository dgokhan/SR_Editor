//ORIGINAL code from https://github.com/gwiazdorrr/BetterStreamingAssets

// Better Streaming Assets, Piotr Gwiazdowski <gwiazdorrr+github at gmail.com>, 2017
// Bits below are copied from or inspired by System.IO.Compression.dll; leaving comments from
// original source code and attaching license

// The MIT License(MIT)
//
// Copyright(c) .NET Foundation and Contributors
//
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;

namespace BansheeGz.BGDatabase
{
    internal struct ZipGenericExtraField
    {
        private const int SizeOfHeader = 4;

        private ushort _tag;
        private ushort _size;
        private byte[] _data;

        public ushort Tag => _tag;

        // returns size of data, not of the entire block
        public ushort Size => _size;

        public byte[] Data => _data;

        // shouldn't ever read the byte at position endExtraField
        // assumes we are positioned at the beginning of an extra field subfield
        public static bool TryReadBlock(BinaryReader reader, long endExtraField, out ZipGenericExtraField field)
        {
            field = new ZipGenericExtraField();

            // not enough bytes to read tag + size
            if (endExtraField - reader.BaseStream.Position < 4) return false;

            field._tag = reader.ReadUInt16();
            field._size = reader.ReadUInt16();

            // not enough bytes to read the data
            if (endExtraField - reader.BaseStream.Position < field._size) return false;

            field._data = reader.ReadBytes(field._size);
            return true;
        }
    }

    internal struct Zip64ExtraField
    {
        // Size is size of the record not including the tag or size fields
        // If the extra field is going in the local header, it cannot include only
        // one of uncompressed/compressed size

        public const int OffsetToFirstField = 4;
        private const ushort TagConstant = 1;

        private ushort _size;
        private long? _uncompressedSize;
        private long? _compressedSize;
        private long? _localHeaderOffset;
        private int? _startDiskNumber;


        public long? UncompressedSize
        {
            get => _uncompressedSize;
            set
            {
                _uncompressedSize = value;
                UpdateSize();
            }
        }

        public long? CompressedSize
        {
            get => _compressedSize;
            set
            {
                _compressedSize = value;
                UpdateSize();
            }
        }

        public long? LocalHeaderOffset
        {
            get => _localHeaderOffset;
            set
            {
                _localHeaderOffset = value;
                UpdateSize();
            }
        }

        public int? StartDiskNumber => _startDiskNumber;

        private void UpdateSize()
        {
            _size = 0;
            if (_uncompressedSize != null) _size += 8;
            if (_compressedSize != null) _size += 8;
            if (_localHeaderOffset != null) _size += 8;
            if (_startDiskNumber != null) _size += 4;
        }

        // There is a small chance that something very weird could happen here. The code calling into this function
        // will ask for a value from the extra field if the field was masked with FF's. It's theoretically possible
        // that a field was FF's legitimately, and the writer didn't decide to write the corresponding extra field.
        // Also, at the same time, other fields were masked with FF's to indicate looking in the zip64 record.
        // Then, the search for the zip64 record will fail because the expected size is wrong,
        // and a nulled out Zip64ExtraField will be returned. Thus, even though there was Zip64 data,
        // it will not be used. It is questionable whether this situation is possible to detect

        // unlike the other functions that have try-pattern semantics, these functions always return a
        // Zip64ExtraField. If a Zip64 extra field actually doesn't exist, all of the fields in the
        // returned struct will be null
        //
        // If there are more than one Zip64 extra fields, we take the first one that has the expected size
        //
        public static Zip64ExtraField GetJustZip64Block(Stream extraFieldStream,
            bool readUncompressedSize, bool readCompressedSize,
            bool readLocalHeaderOffset, bool readStartDiskNumber)
        {
            Zip64ExtraField zip64Field;
            using (var reader = new BinaryReader(extraFieldStream))
            {
                ZipGenericExtraField currentExtraField;
                while (ZipGenericExtraField.TryReadBlock(reader, extraFieldStream.Length, out currentExtraField))
                    if (TryGetZip64BlockFromGenericExtraField(currentExtraField, readUncompressedSize,
                        readCompressedSize, readLocalHeaderOffset, readStartDiskNumber, out zip64Field))
                        return zip64Field;
            }

            zip64Field = new Zip64ExtraField();

            zip64Field._compressedSize = null;
            zip64Field._uncompressedSize = null;
            zip64Field._localHeaderOffset = null;
            zip64Field._startDiskNumber = null;

            return zip64Field;
        }

        private static bool TryGetZip64BlockFromGenericExtraField(ZipGenericExtraField extraField,
            bool readUncompressedSize, bool readCompressedSize,
            bool readLocalHeaderOffset, bool readStartDiskNumber,
            out Zip64ExtraField zip64Block)
        {
            zip64Block = new Zip64ExtraField();

            zip64Block._compressedSize = null;
            zip64Block._uncompressedSize = null;
            zip64Block._localHeaderOffset = null;
            zip64Block._startDiskNumber = null;

            if (extraField.Tag != TagConstant) return false;

            // this pattern needed because nested using blocks trigger CA2202
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream(extraField.Data);
                using (var reader = new BinaryReader(ms))
                {
                    ms = null;

                    zip64Block._size = extraField.Size;

                    ushort expectedSize = 0;

                    if (readUncompressedSize) expectedSize += 8;
                    if (readCompressedSize) expectedSize += 8;
                    if (readLocalHeaderOffset) expectedSize += 8;
                    if (readStartDiskNumber) expectedSize += 4;

                    // if it is not the expected size, perhaps there is another extra field that matches
                    if (expectedSize != zip64Block._size) return false;

                    if (readUncompressedSize) zip64Block._uncompressedSize = reader.ReadInt64();
                    if (readCompressedSize) zip64Block._compressedSize = reader.ReadInt64();
                    if (readLocalHeaderOffset) zip64Block._localHeaderOffset = reader.ReadInt64();
                    if (readStartDiskNumber) zip64Block._startDiskNumber = reader.ReadInt32();

                    // original values are unsigned, so implies value is too big to fit in signed integer
                    if (zip64Block._uncompressedSize < 0) throw new ZipArchiveException("FieldTooBigUncompressedSize");
                    if (zip64Block._compressedSize < 0) throw new ZipArchiveException("FieldTooBigCompressedSize");
                    if (zip64Block._localHeaderOffset < 0) throw new ZipArchiveException("FieldTooBigLocalHeaderOffset");
                    if (zip64Block._startDiskNumber < 0) throw new ZipArchiveException("FieldTooBigStartDiskNumber");

                    return true;
                }
            }
            finally
            {
                ms?.Dispose();
            }
        }
    }

    internal struct Zip64EndOfCentralDirectoryLocator
    {
        public const uint SignatureConstant = 0x07064B50;
        public const int SizeOfBlockWithoutSignature = 16;

        public uint NumberOfDiskWithZip64EOCD;
        public ulong OffsetOfZip64EOCD;
        public uint TotalNumberOfDisks;

        public static bool TryReadBlock(BinaryReader reader, out Zip64EndOfCentralDirectoryLocator zip64EOCDLocator)
        {
            zip64EOCDLocator = new Zip64EndOfCentralDirectoryLocator();

            if (reader.ReadUInt32() != SignatureConstant) return false;

            zip64EOCDLocator.NumberOfDiskWithZip64EOCD = reader.ReadUInt32();
            zip64EOCDLocator.OffsetOfZip64EOCD = reader.ReadUInt64();
            zip64EOCDLocator.TotalNumberOfDisks = reader.ReadUInt32();
            return true;
        }
    }

    internal struct Zip64EndOfCentralDirectoryRecord
    {
        private const uint SignatureConstant = 0x06064B50;
        private const ulong NormalSize = 0x2C; // the size of the data excluding the size/signature fields if no extra data included

        public ulong SizeOfThisRecord;
        public ushort VersionMadeBy;
        public ushort VersionNeededToExtract;
        public uint NumberOfThisDisk;
        public uint NumberOfDiskWithStartOfCD;
        public ulong NumberOfEntriesOnThisDisk;
        public ulong NumberOfEntriesTotal;
        public ulong SizeOfCentralDirectory;
        public ulong OffsetOfCentralDirectory;

        public static bool TryReadBlock(BinaryReader reader, out Zip64EndOfCentralDirectoryRecord zip64EOCDRecord)
        {
            zip64EOCDRecord = new Zip64EndOfCentralDirectoryRecord();

            if (reader.ReadUInt32() != SignatureConstant) return false;

            zip64EOCDRecord.SizeOfThisRecord = reader.ReadUInt64();
            zip64EOCDRecord.VersionMadeBy = reader.ReadUInt16();
            zip64EOCDRecord.VersionNeededToExtract = reader.ReadUInt16();
            zip64EOCDRecord.NumberOfThisDisk = reader.ReadUInt32();
            zip64EOCDRecord.NumberOfDiskWithStartOfCD = reader.ReadUInt32();
            zip64EOCDRecord.NumberOfEntriesOnThisDisk = reader.ReadUInt64();
            zip64EOCDRecord.NumberOfEntriesTotal = reader.ReadUInt64();
            zip64EOCDRecord.SizeOfCentralDirectory = reader.ReadUInt64();
            zip64EOCDRecord.OffsetOfCentralDirectory = reader.ReadUInt64();

            return true;
        }
    }

    internal struct ZipLocalFileHeader
    {
        public const uint DataDescriptorSignature = 0x08074B50;
        public const uint SignatureConstant = 0x04034B50;
        public const int OffsetToCrcFromHeaderStart = 14;
        public const int OffsetToBitFlagFromHeaderStart = 6;
        public const int SizeOfLocalHeader = 30;


        // will not throw end of stream exception
        public static bool TrySkipBlock(BinaryReader reader)
        {
            const int OffsetToFilenameLength = 22; // from the point after the signature

            if (reader.ReadUInt32() != SignatureConstant) return false;


            if (reader.BaseStream.Length < reader.BaseStream.Position + OffsetToFilenameLength) return false;

            reader.BaseStream.Seek(OffsetToFilenameLength, SeekOrigin.Current);

            var filenameLength = reader.ReadUInt16();
            var extraFieldLength = reader.ReadUInt16();

            if (reader.BaseStream.Length < reader.BaseStream.Position + filenameLength + extraFieldLength) return false;

            reader.BaseStream.Seek(filenameLength + extraFieldLength, SeekOrigin.Current);

            return true;
        }
    }

    internal struct ZipCentralDirectoryFileHeader
    {
        public const uint SignatureConstant = 0x02014B50;
        public byte VersionMadeByCompatibility;
        public byte VersionMadeBySpecification;
        public ushort VersionNeededToExtract;
        public ushort GeneralPurposeBitFlag;
        public ushort CompressionMethod;
        public uint LastModified; // convert this on the fly
        public uint Crc32;
        public long CompressedSize;
        public long UncompressedSize;
        public ushort FilenameLength;
        public ushort ExtraFieldLength;
        public ushort FileCommentLength;
        public int DiskNumberStart;
        public ushort InternalFileAttributes;
        public uint ExternalFileAttributes;
        public long RelativeOffsetOfLocalHeader;

        public byte[] Filename;
        public byte[] FileComment;
        public List<ZipGenericExtraField> ExtraFields;

        // if saveExtraFieldsAndComments is false, FileComment and ExtraFields will be null
        // in either case, the zip64 extra field info will be incorporated into other fields
        public static bool TryReadBlock(BinaryReader reader, out ZipCentralDirectoryFileHeader header)
        {
            header = new ZipCentralDirectoryFileHeader();

            if (reader.ReadUInt32() != SignatureConstant) return false;
            header.VersionMadeBySpecification = reader.ReadByte();
            header.VersionMadeByCompatibility = reader.ReadByte();
            header.VersionNeededToExtract = reader.ReadUInt16();
            header.GeneralPurposeBitFlag = reader.ReadUInt16();
            header.CompressionMethod = reader.ReadUInt16();
            header.LastModified = reader.ReadUInt32();
            header.Crc32 = reader.ReadUInt32();
            var compressedSizeSmall = reader.ReadUInt32();
            var uncompressedSizeSmall = reader.ReadUInt32();
            header.FilenameLength = reader.ReadUInt16();
            header.ExtraFieldLength = reader.ReadUInt16();
            header.FileCommentLength = reader.ReadUInt16();
            var diskNumberStartSmall = reader.ReadUInt16();
            header.InternalFileAttributes = reader.ReadUInt16();
            header.ExternalFileAttributes = reader.ReadUInt32();
            var relativeOffsetOfLocalHeaderSmall = reader.ReadUInt32();

            header.Filename = reader.ReadBytes(header.FilenameLength);

            var uncompressedSizeInZip64 = uncompressedSizeSmall == ZipHelper.Mask32Bit;
            var compressedSizeInZip64 = compressedSizeSmall == ZipHelper.Mask32Bit;
            var relativeOffsetInZip64 = relativeOffsetOfLocalHeaderSmall == ZipHelper.Mask32Bit;
            var diskNumberStartInZip64 = diskNumberStartSmall == ZipHelper.Mask16Bit;

            Zip64ExtraField zip64;

            var endExtraFields = reader.BaseStream.Position + header.ExtraFieldLength;
            using (Stream str = new SubReadOnlyStream(reader.BaseStream, reader.BaseStream.Position, header.ExtraFieldLength, true))
            {
                header.ExtraFields = null;
                zip64 = Zip64ExtraField.GetJustZip64Block(str,
                    uncompressedSizeInZip64, compressedSizeInZip64,
                    relativeOffsetInZip64, diskNumberStartInZip64);
            }

            // There are zip files that have malformed ExtraField blocks in which GetJustZip64Block() silently bails out without reading all the way to the end
            // of the ExtraField block. Thus we must force the stream's position to the proper place.
            reader.BaseStream.AdvanceToPosition(endExtraFields);

            reader.BaseStream.Position += header.FileCommentLength;
            header.FileComment = null;

            header.UncompressedSize = zip64.UncompressedSize == null
                ? uncompressedSizeSmall
                : zip64.UncompressedSize.Value;
            header.CompressedSize = zip64.CompressedSize == null
                ? compressedSizeSmall
                : zip64.CompressedSize.Value;
            header.RelativeOffsetOfLocalHeader = zip64.LocalHeaderOffset == null
                ? relativeOffsetOfLocalHeaderSmall
                : zip64.LocalHeaderOffset.Value;
            header.DiskNumberStart = zip64.StartDiskNumber == null
                ? diskNumberStartSmall
                : zip64.StartDiskNumber.Value;

            return true;
        }
    }

    internal struct ZipEndOfCentralDirectoryBlock
    {
        public const uint SignatureConstant = 0x06054B50;
        public const int SizeOfBlockWithoutSignature = 18;
        public uint Signature;
        public ushort NumberOfThisDisk;
        public ushort NumberOfTheDiskWithTheStartOfTheCentralDirectory;
        public ushort NumberOfEntriesInTheCentralDirectoryOnThisDisk;
        public ushort NumberOfEntriesInTheCentralDirectory;
        public uint SizeOfCentralDirectory;
        public uint OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber;
        public byte[] ArchiveComment;


        public static bool TryReadBlock(BinaryReader reader, out ZipEndOfCentralDirectoryBlock eocdBlock)
        {
            eocdBlock = new ZipEndOfCentralDirectoryBlock();
            if (reader.ReadUInt32() != SignatureConstant) return false;

            eocdBlock.Signature = SignatureConstant;
            eocdBlock.NumberOfThisDisk = reader.ReadUInt16();
            eocdBlock.NumberOfTheDiskWithTheStartOfTheCentralDirectory = reader.ReadUInt16();
            eocdBlock.NumberOfEntriesInTheCentralDirectoryOnThisDisk = reader.ReadUInt16();
            eocdBlock.NumberOfEntriesInTheCentralDirectory = reader.ReadUInt16();
            eocdBlock.SizeOfCentralDirectory = reader.ReadUInt32();
            eocdBlock.OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber = reader.ReadUInt32();

            var commentLength = reader.ReadUInt16();
            eocdBlock.ArchiveComment = reader.ReadBytes(commentLength);

            return true;
        }
    }

    internal static class ZipHelper
    {
        internal const uint Mask32Bit = 0xFFFFFFFF;
        internal const ushort Mask16Bit = 0xFFFF;

        private const int BackwardsSeekingBufferSize = 32;


        /// <summary>
        /// Reads exactly bytesToRead out of stream, unless it is out of bytes
        /// </summary>
        internal static void ReadBytes(Stream stream, byte[] buffer, int bytesToRead)
        {
            var bytesLeftToRead = bytesToRead;

            var totalBytesRead = 0;

            while (bytesLeftToRead > 0)
            {
                var bytesRead = stream.Read(buffer, totalBytesRead, bytesLeftToRead);
                if (bytesRead == 0) throw new IOException();

                totalBytesRead += bytesRead;
                bytesLeftToRead -= bytesRead;
            }
        }


        // assumes all bytes of signatureToFind are non zero, looks backwards from current position in stream,
        // if the signature is found then returns true and positions stream at first byte of signature
        // if the signature is not found, returns false
        internal static bool SeekBackwardsToSignature(Stream stream, uint signatureToFind)
        {
            var bufferPointer = 0;
            uint currentSignature = 0;
            var buffer = new byte[BackwardsSeekingBufferSize];

            var outOfBytes = false;
            var signatureFound = false;

            while (!signatureFound && !outOfBytes)
            {
                outOfBytes = SeekBackwardsAndRead(stream, buffer, out bufferPointer);

//                    Debug.Assert(bufferPointer < buffer.Length);

                while (bufferPointer >= 0 && !signatureFound)
                {
                    currentSignature = (currentSignature << 8) | (uint)buffer[bufferPointer];
                    if (currentSignature == signatureToFind) signatureFound = true;
                    else bufferPointer--;
                }
            }

            if (!signatureFound) return false;
            else
            {
                stream.Seek(bufferPointer, SeekOrigin.Current);
                return true;
            }
        }

        // Skip to a further position downstream (without relying on the stream being seekable)
        internal static void AdvanceToPosition(this Stream stream, long position)
        {
            var numBytesLeft = position - stream.Position;
//                Debug.Assert(numBytesLeft >= 0);
            while (numBytesLeft != 0)
            {
                const int throwAwayBufferSize = 64;
                var numBytesToSkip = numBytesLeft > throwAwayBufferSize ? throwAwayBufferSize : (int)numBytesLeft;
                var numBytesActuallySkipped = stream.Read(new byte[throwAwayBufferSize], 0, numBytesToSkip);
                if (numBytesActuallySkipped == 0) throw new IOException();
                numBytesLeft -= numBytesActuallySkipped;
            }
        }

        // Returns true if we are out of bytes
        private static bool SeekBackwardsAndRead(Stream stream, byte[] buffer, out int bufferPointer)
        {
            if (stream.Position >= buffer.Length)
            {
                stream.Seek(-buffer.Length, SeekOrigin.Current);
                ReadBytes(stream, buffer, buffer.Length);
                stream.Seek(-buffer.Length, SeekOrigin.Current);
                bufferPointer = buffer.Length - 1;
                return false;
            }
            else
            {
                var bytesToRead = (int)stream.Position;
                stream.Seek(0, SeekOrigin.Begin);
                ReadBytes(stream, buffer, bytesToRead);
                stream.Seek(0, SeekOrigin.Begin);
                bufferPointer = bytesToRead - 1;
                return true;
            }
        }
    }

    public class ZipArchiveException : Exception
    {
        public ZipArchiveException(string msg) : base(msg)
        {
        }

        public ZipArchiveException(string msg, Exception inner)
            : base(msg, inner)
        {
        }
    }

    public static class ZipArchiveUtils
    {
        public static void ReadEndOfCentralDirectory(Stream stream, BinaryReader reader, out long expectedNumberOfEntries, out long centralDirectoryStart)
        {
            try
            {
                // this seeks to the start of the end of central directory record
                stream.Seek(-ZipEndOfCentralDirectoryBlock.SizeOfBlockWithoutSignature, SeekOrigin.End);
                if (!ZipHelper.SeekBackwardsToSignature(stream, ZipEndOfCentralDirectoryBlock.SignatureConstant)) throw new ZipArchiveException("SignatureConstant");

                var eocdStart = stream.Position;

                // read the EOCD
                ZipEndOfCentralDirectoryBlock eocd;
                var eocdProper = ZipEndOfCentralDirectoryBlock.TryReadBlock(reader, out eocd);
//                    Debug.Assert(eocdProper); // we just found this using the signature finder, so it should be okay

                if (eocd.NumberOfThisDisk != eocd.NumberOfTheDiskWithTheStartOfTheCentralDirectory) throw new ZipArchiveException("SplitSpanned");

                centralDirectoryStart = eocd.OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber;
                if (eocd.NumberOfEntriesInTheCentralDirectory != eocd.NumberOfEntriesInTheCentralDirectoryOnThisDisk) throw new ZipArchiveException("SplitSpanned");
                expectedNumberOfEntries = eocd.NumberOfEntriesInTheCentralDirectory;


                // only bother looking for zip64 EOCD stuff if we suspect it is needed because some value is FFFFFFFFF
                // because these are the only two values we need, we only worry about these
                // if we don't find the zip64 EOCD, we just give up and try to use the original values
                if (eocd.NumberOfThisDisk == ZipHelper.Mask16Bit ||
                    eocd.OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber == ZipHelper.Mask32Bit ||
                    eocd.NumberOfEntriesInTheCentralDirectory == ZipHelper.Mask16Bit)
                {
                    // we need to look for zip 64 EOCD stuff
                    // seek to the zip 64 EOCD locator
                    stream.Seek(eocdStart - Zip64EndOfCentralDirectoryLocator.SizeOfBlockWithoutSignature, SeekOrigin.Begin);
                    // if we don't find it, assume it doesn't exist and use data from normal eocd
                    if (ZipHelper.SeekBackwardsToSignature(stream, Zip64EndOfCentralDirectoryLocator.SignatureConstant))
                    {
                        // use locator to get to Zip64EOCD
                        Zip64EndOfCentralDirectoryLocator locator;
                        var zip64eocdLocatorProper = Zip64EndOfCentralDirectoryLocator.TryReadBlock(reader, out locator);
//                            Debug.Assert(zip64eocdLocatorProper); // we just found this using the signature finder, so it should be okay

                        if (locator.OffsetOfZip64EOCD > long.MaxValue) throw new ZipArchiveException("FieldTooBigOffsetToZip64EOCD");
                        var zip64EOCDOffset = (long)locator.OffsetOfZip64EOCD;

                        stream.Seek(zip64EOCDOffset, SeekOrigin.Begin);

                        // read Zip64EOCD
                        Zip64EndOfCentralDirectoryRecord record;
                        if (!Zip64EndOfCentralDirectoryRecord.TryReadBlock(reader, out record)) throw new ZipArchiveException("Zip64EOCDNotWhereExpected");

                        if (record.NumberOfEntriesTotal > long.MaxValue) throw new ZipArchiveException("FieldTooBigNumEntries");
                        if (record.OffsetOfCentralDirectory > long.MaxValue) throw new ZipArchiveException("FieldTooBigOffsetToCD");
                        if (record.NumberOfEntriesTotal != record.NumberOfEntriesOnThisDisk) throw new ZipArchiveException("SplitSpanned");

                        expectedNumberOfEntries = (long)record.NumberOfEntriesTotal;
                        centralDirectoryStart = (long)record.OffsetOfCentralDirectory;
                    }
                }

                if (centralDirectoryStart > stream.Length) throw new ZipArchiveException("FieldTooBigOffsetToCD");
            }
            catch (EndOfStreamException ex)
            {
                throw new ZipArchiveException("CDCorrupt", ex);
            }
            catch (IOException ex)
            {
                throw new ZipArchiveException("CDCorrupt", ex);
            }
        }
    }

    internal class SubReadOnlyStream : Stream
    {
        private readonly long m_offset;
        private readonly bool m_leaveOpen;

        private long? m_length;
        private Stream m_actualStream;
        private long m_position;

        public SubReadOnlyStream(Stream actualStream, bool leaveOpen = false)
        {
            m_actualStream = actualStream ?? throw new ArgumentNullException("superStream");
            m_leaveOpen = leaveOpen;
        }

        public SubReadOnlyStream(Stream actualStream, long offset, long length, bool leaveOpen = false)
            : this(actualStream, leaveOpen)
        {
            if (offset < 0) throw new ArgumentOutOfRangeException("offset");

            if (length < 0) throw new ArgumentOutOfRangeException("length");

//            Debug.Assert(offset <= actualStream.Length);
//            Debug.Assert(actualStream.Length >= length);
//            Debug.Assert(offset + length <= actualStream.Length);

            m_offset = offset;
            m_position = offset;
            m_length = length;
        }

        public override long Length
        {
            get
            {
                ThrowIfDisposed();

                if (!m_length.HasValue) m_length = m_actualStream.Length - m_offset;

                return m_length.Value;
                ;
            }
        }

        public override long Position
        {
            get
            {
                ThrowIfDisposed();
                return m_position - m_offset;
            }
            set
            {
                ThrowIfDisposed();
                throw new NotSupportedException();
            }
        }

        public override bool CanRead => m_actualStream.CanRead;

        public override bool CanSeek => m_actualStream.CanSeek;

        public override bool CanWrite => false;

        public override int Read(byte[] buffer, int offset, int count)
        {
            ThrowIfCantRead();
            ThrowIfDisposed();

            if (m_actualStream.Position != m_position) m_actualStream.Seek(m_position, SeekOrigin.Begin);

            if (m_length.HasValue)
            {
                var endPosition = m_offset + m_length.Value;
                if (m_position + count > endPosition) count = (int)(endPosition - m_position);
            }

            var bytesRead = m_actualStream.Read(buffer, offset, count);
            m_position += bytesRead;
            return bytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            ThrowIfDisposed();

            if (origin == SeekOrigin.Begin) m_position = m_actualStream.Seek(m_offset + offset, SeekOrigin.Begin);
            else if (origin == SeekOrigin.End) m_position = m_actualStream.Seek(m_offset + Length + offset, SeekOrigin.End);
            else m_position = m_actualStream.Seek(offset, SeekOrigin.Current);

            return m_position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        // Close the stream for reading.  Note that this does NOT close the superStream (since
        // the substream is just 'a chunk' of the super-stream
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                if (m_actualStream != null)
                {
                    if (!m_leaveOpen) m_actualStream.Dispose();

                    m_actualStream = null;
                }


            base.Dispose(disposing);
        }

        private void ThrowIfDisposed()
        {
            if (m_actualStream == null) throw new ObjectDisposedException(GetType().ToString(), "");
        }

        private void ThrowIfCantRead()
        {
            if (!CanRead) throw new NotSupportedException();
        }
    }
}