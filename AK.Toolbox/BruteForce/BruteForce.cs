using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AK.Toolbox.BruteForce
{
	public class BruteForce
	{
		private readonly ISpecimenGenerator _generator;
		private readonly Speedometer _speedometer;

		public BruteForce(ISpecimenGenerator generator)
		{
			_generator = generator;
			_speedometer = new Speedometer(generator.Total);
		}

		private ParallelQuery<Specimen> PrepareBaseQuery(CancellationToken cancellationToken)
		{
			return
				_generator.Generate()
					.AsParallel().WithCancellation(cancellationToken).WithMergeOptions(ParallelMergeOptions.NotBuffered)
					.Select(s =>
					        	{
					        		_speedometer.Update();
					        		return s;
					        	})
					.Where(s => s.IsValid());
		}

		public IOperation<Specimen[]> FindAll(CancellationToken cancellationToken)
		{
			return new GenericOperation<Specimen[]>(
				Task<Specimen[]>.Factory.StartNew(() =>
				                                  	{
				                                  		_speedometer.Start();
				                                  		try
				                                  		{
				                                  			return PrepareBaseQuery(cancellationToken).Where(s => s.Matches()).ToArray();
				                                  		}
				                                  		finally
				                                  		{
				                                  			_speedometer.Stop();
				                                  		}
				                                  	}, cancellationToken),
				_speedometer);
		}

		public IOperation<Specimen> FindFirst(CancellationToken cancellationToken)
		{
			return new GenericOperation<Specimen>(
				Task<Specimen>.Factory.StartNew(() =>
				                                	{
				                                		_speedometer.Start();
				                                		try
				                                		{
				                                			return PrepareBaseQuery(cancellationToken)
				                                				.FirstOrDefault(s => s.Matches());
				                                		}
				                                		finally
				                                		{
				                                			_speedometer.Stop();
				                                		}
				                                	}, cancellationToken),
				_speedometer);
		}
	}
}