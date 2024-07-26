namespace Koishibot.Core.Services.TwitchEventSubNew.Misc;

public class LimitedSizeHashSet<TType, TKeyType>
{
	private readonly HashSet<TKeyType> _hashSet = new();
	private readonly LinkedList<TType> _list = new();
	private readonly int _maxSize;
	private readonly Func<TType, TKeyType> _keySelector;

	public LimitedSizeHashSet(int maxSize, Func<TType, TKeyType> keySelector)
	{
		_maxSize = maxSize;
		_keySelector = keySelector;
	}

	public void Add(TType item)
	{
		if (_hashSet.Count >= _maxSize)
		{
			var oldestNode = _list.First;
			_hashSet.Remove(_keySelector(oldestNode.Value));
			_list.RemoveFirst();
		}

		if (_hashSet.Add(_keySelector(item)))
		{
			_list.AddLast(item);
		}
	}

	public TType LastItem()
	{
		return _list.Last.Value;
	}

	public TType Peek()
	{
		return _list.First.Value;
	}

	public bool Contains(TKeyType item)
	{
		return _hashSet.Contains(item);
	}
}