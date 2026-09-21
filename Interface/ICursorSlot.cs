using System;

public interface ICursorSlot
{
    Item CursorItem { get; }
    bool IsEmpty { get; }
    event Action OnChanged;
}
