using SFML.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UIFramework.Text;
public static class WriteQueue
{
    private static readonly ConcurrentDictionary<UIText, string> drawQueue = new();

    /// <summary>
    /// Adds a drawing action with its target drawable object to the queue.
    /// </summary>
    /// <param name="action">A tuple containing the drawing action and the drawable target.</param>
    public static void EnqueueDraw(UIText textElement, string text)
    {
        drawQueue[textElement] = text;
    }

    /// <summary>
    /// Выполняет все отложенные обновления текста.
    /// </summary>
    public static void ExecuteAll()
    {
        foreach (var kvp in drawQueue)
        {
            kvp.Key.SetTextAsync(kvp.Value);
        }
        drawQueue.Clear();
    }
}
