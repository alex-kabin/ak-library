namespace AK.Toolbox.BruteForce
{
	public abstract class Specimen
	{
		public virtual bool IsValid()
		{
			return true;
		}

		public abstract bool Matches();
	}
}