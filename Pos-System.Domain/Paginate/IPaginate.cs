namespace Pos_System.Domain.Paginate;

public interface IPaginate<TResult>
{
	int Size { get; }
	int Page { get; }
	int Total { get; }
	IList<TResult> Items { get; }
}