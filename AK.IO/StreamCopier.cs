using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AK.Essentials;
using AK.Essentials.Extensions;

namespace AK.IO
{
	public class StreamCopier
	{
		private const int SPEED_MEASURE_INTERVAL_IN_MILLISECONDS = 5;
		public struct CopyProgress
		{
			public long? TotalBytes { get; set; }
			public long BytesCopied { get; set; }
			public double BytesPerSecond { get; set; }
			public TimeSpan TimeElapsed { get; set; }
			public TimeSpan? TimeLeft { get; set; }
		}

		public enum FinishStatus
		{
			Completed,
			Canceled,
			Faulted
		}

		public struct FinishResult
		{
			public FinishStatus Status { get; set; }
			public Exception Error { get; set; }
		}

		private Stream _sourceStream;
		private Stream _destinationStream;
		private CancellationToken _cancellationToken;
		private long _sourceLength;
		private byte[] _buffer;

		public int BufferSize { get; set; }

		public StreamCopier() { }

		private long _bytesCopiedCounter;
		private long _bytesCopiedInIntervalCounter;
		
		private Stopwatch _stopwatch;
		private Stopwatch _intervalStopwatch;

		private CopyProgress PrepareProgress()
		{
			return new CopyProgress
			       	{
			       		BytesCopied = _bytesCopiedCounter,
			       		BytesPerSecond = _bytesCopiedInIntervalCounter/_intervalStopwatch.Elapsed.TotalSeconds,
			       		TimeElapsed = _stopwatch.Elapsed,
			       		TotalBytes = _sourceLength > 0 ? _sourceLength : (long?)null,
			       		TimeLeft =
			       			_sourceLength > 0
			       				? TimeSpan.FromMilliseconds((double)_sourceLength/_bytesCopiedInIntervalCounter/_intervalStopwatch.ElapsedMilliseconds)
			       				: (TimeSpan?)null
			       	};
		}

		public void Copy(Stream source, Stream destination, CancellationToken cancellationToken)
		{
			if(_sourceStream != null)
				throw new InvalidOperationException("Copy process is already running");

			if(source == null)
				throw new ArgumentNullException("source");
			if (destination == null)
				throw new ArgumentNullException("destination");

			_sourceStream = source;
			_destinationStream = destination;
			_cancellationToken = cancellationToken;

			if(!_sourceStream.CanRead)
				throw new InvalidOperationException("Source stream should support reading");

			if (!_destinationStream.CanWrite)
				throw new InvalidOperationException("Destination stream should support writing");

			_sourceLength = -1;
			try
			{
				_sourceLength = _sourceStream.Length;
			}
			catch (NotSupportedException)
			{
			}

			_buffer = new byte[BufferSize];
			
			_stopwatch = new Stopwatch();
			_stopwatch.Start();

			_intervalStopwatch = new Stopwatch();
			_intervalStopwatch.Start();

			ReadNextChunk();
		}

		private void ReadNextChunk()
		{
			_sourceStream.BeginRead(_buffer, 0, BufferSize, ar =>
			                                                	{
																					int bytesRead;
																					try
																					{
																						bytesRead = _sourceStream.EndRead(ar);
																					}
																					catch (Exception ex)
																					{
																						OnFinished(FinishStatus.Faulted, ex);
																						return;
																					}

																					if (_cancellationToken.IsCancellationRequested)
																					{
																						OnFinished(FinishStatus.Canceled, null);
																						return;
																					}
																					
																					WriteNextChunk(bytesRead);
			                                                	}, null);
		}

		private void WriteNextChunk(int size)
		{
			if (size > 0)
			{
				_destinationStream.BeginWrite(_buffer, 0, size, ar =>
				                                                	{
																						try
																						{
																							_destinationStream.EndWrite(ar);
																						}
																						catch (Exception ex)
																						{
																							OnFinished(FinishStatus.Faulted, ex);
																							return;
																						}

																						if (_cancellationToken.IsCancellationRequested)
																						{
																							OnFinished(FinishStatus.Canceled, null);
																							return;
																						}

																						_bytesCopiedCounter += size;

				                                                		if (_intervalStopwatch.ElapsedMilliseconds < SPEED_MEASURE_INTERVAL_IN_MILLISECONDS)
				                                                			_bytesCopiedInIntervalCounter += size;
				                                                		else
				                                                		{
				                                                			_bytesCopiedInIntervalCounter = 0;
				                                                			_intervalStopwatch.Restart();
				                                                		}

																						ReportProgress();

				                                                		ReadNextChunk();
				                                                	}, null);
			}
			else
			{
				OnFinished(FinishStatus.Completed, null);
			}
		}


		public EventHandler<ValueEventArgs<CopyProgress>> Progress;

		private void ReportProgress()
		{
			var progress = PrepareProgress();
			Progress.Fire(this, new ValueEventArgs<CopyProgress>(progress));
		}

		public EventHandler<ValueEventArgs<FinishResult>> Finished;

		private void OnFinished(FinishStatus status, Exception error)
		{
			_sourceStream = null;
			_destinationStream = null;
			_stopwatch.Stop();
			_intervalStopwatch.Stop();

			Finished.Fire(this, new ValueEventArgs<FinishResult>(new FinishResult() { Status = status, Error = error }));
		}
	}
}