using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Presentation;

public class AppTitleState
{
    public string Title { get; private set; } = string.Empty;
    public event Action? Changed;

    public void Set(string title)
    {
        if (Title == title) return;
        Title = title;
        Changed?.Invoke();
    }
}
