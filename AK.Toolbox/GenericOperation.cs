using System.Threading.Tasks;

namespace AK.Toolbox
{
	public class GenericOperation<TResult> : IOperation<TResult>
	{
		private readonly Speedometer _speedometer;

		public GenericOperation(Task<TResult> task, Speedometer speedometer = null)
		{
			Task = task;
			_speedometer = speedometer;
		}

		public Task<TResult> Task
		{
			get; private set;
		}
		
		public bool SupportsSpeedometer
		{
			get { return _speedometer != null; }
		}
		
		public Speedometer.State QuerySpeed()
		{
			if (_speedometer != null)
				return _speedometer.QueryState();
			else
				return default(Speedometer.State);
		}
	}
}