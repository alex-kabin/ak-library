using System.Threading.Tasks;

namespace AK.Toolbox
{
	public interface IOperation<TResult>
	{
		Task<TResult> Task { get; }
		bool SupportsSpeedometer { get; }
		Speedometer.State QuerySpeed();
	}
}