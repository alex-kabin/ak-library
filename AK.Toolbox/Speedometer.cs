using System;
using System.Diagnostics;
using System.Threading;

namespace AK.Toolbox
{
	public class Speedometer
	{
		public struct State
		{
			internal State(Speedometer speedometer)
			{
				_total = speedometer._total;
				_current = speedometer._current;
				_totalPerInterval = speedometer._current - speedometer._previous;
				_elapsedTime = speedometer._stopwatch.Elapsed;
				_interval = _elapsedTime - speedometer._lastRefreshTime;
				_progressRatio = (_total.HasValue && _total > 0) ? _current / _total : (double?)null;

				if(_total.HasValue && _current > 0)
					_remainingTime = new TimeSpan((long)(_total.Value/_current*_elapsedTime.Ticks));
				else
					_remainingTime = null;
			}

			private readonly double? _total;
			public double? Total { get { return _total; } }
			
			private readonly double _current;
			public double Current { get { return _current; } }
			
			private readonly double _totalPerInterval;
			public double TotalPerInterval { get { return _totalPerInterval; } }
			
			private readonly TimeSpan _interval;
			public TimeSpan Interval { get { return _interval; } }
			
			private readonly TimeSpan _elapsedTime;
			public TimeSpan ElapsedTime { get { return _elapsedTime; } }
			
			private readonly TimeSpan? _remainingTime;
			public TimeSpan? RemainingTime { get { return _remainingTime; } }

			private readonly double? _progressRatio;
			public double? ProgressRatio { get { return _progressRatio; } }
		}

		private readonly Stopwatch _stopwatch = new Stopwatch();
		private readonly object _locker = new object();

		private double? _total;
		private double _current;
		private double _previous;
		private TimeSpan _lastRefreshTime;
		private bool _updated;
		
		public Speedometer(double? total = null)
		{
			_total = total;
		}

		public void Start(double current = 0.0)
		{
			lock (_locker)
			{
				if(_stopwatch.IsRunning)
					throw new InvalidOperationException("Already started");
				
				_current = current;
				_previous = current;
				_updated = false;
				_lastRefreshTime = TimeSpan.Zero;
				_stopwatch.Start();
			}
		}

		public void Update(double value = 1.0)
		{
			lock (_locker)
			{
				_current += value;
				_updated = true;
			}
		}

		public void Stop()
		{
			lock (_locker)
			{
				_stopwatch.Stop();
			}
		}

		public State QueryState()
		{
			lock (_locker)
			{
//				if(!_stopwatch.IsRunning)
//					throw new InvalidOperationException("Can't provide state while stopped");

				var state = new State(this);

				if (_updated)
				{
					_lastRefreshTime = _stopwatch.Elapsed;
					_previous = _current;
					_updated = false;
				}

				return state;
			}
		}
	}
}